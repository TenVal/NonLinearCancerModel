using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotLinearCancerModel.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для LinearModelView.xaml
    /// </summary>
    public partial class LinearModelView : UserControl
    {
        public struct ParametersModel
        {
            public float l1;
            public float b;
            public float e;
            public float d;
            public float l3;
            public float u;
            public float days;
            public float xBrain;
            public float yBrain;
            public float treatment;
            public float bEnd;
            public float bStep;

            public ParametersModel(
                float l1,
                float b,
                float e,
                float d,
                float l3,
                float u,
                float days,
                float xBrain,
                float yBrain,
                float treatment,
                float bEnd,
                float bStep)
            {
                this.l1 = l1;
                this.b = b;
                this.e = e;
                this.d = d;
                this.l3 = l3;
                this.u = u;
                this.days = days;
                this.xBrain = xBrain;
                this.yBrain = yBrain;
                this.treatment = treatment;
                this.bEnd = bEnd;
                this.bStep = bStep;
            }
        }
        public LinearModelView()
        {
            InitializeComponent();
        }


        private void unpressedButton()
        {
            CalculateMin.Content = "Calculate";
            CalculateMin.IsEnabled = true;
            SolidColorBrush brushForUnpressedButton = new SolidColorBrush(Colors.White);
            CalculateMin.Foreground = brushForUnpressedButton;
            PercentProgressBarCalculate.Visibility = Visibility.Collapsed;
            ProgressBarCalculate.Visibility = Visibility.Collapsed;
            ProgressBarCalculate.Value = 0;
        }


        private void worker_RunWorkerComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done Calculate min!");
            CalculateMin.Content = "Find min";
            CalculateMin.IsEnabled = true;
            SolidColorBrush brushForUnpressedButton = new SolidColorBrush(Colors.White);
            CalculateMin.Foreground = brushForUnpressedButton;
            PercentProgressBarCalculate.Visibility = Visibility.Collapsed;
            ProgressBarCalculate.Visibility = Visibility.Collapsed;
            ProgressBarCalculate.Value = 0;
        }


        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarCalculate.Value = e.ProgressPercentage;
        }


        private void workerMin_Calculate(object sender, DoWorkEventArgs e)
        {
            ParametersModel paramsModel = (ParametersModel)e.Argument;
            var worker = sender as BackgroundWorker;
            worker.ReportProgress(0, String.Format("Processing Iteration 1."));
            DataCancer modelData = new DataCancer();

            int numberPatients = 10;
            float valueOfDivisionProgressBar = 100 / numberPatients;

            float l1 = paramsModel.l1;
            float b = paramsModel.b;
            float eSpeed = paramsModel.e;
            float d = paramsModel.d;
            float l3 = paramsModel.l3;
            float u = paramsModel.u;
            float days = paramsModel.days;
            float xBrain = paramsModel.xBrain;
            float yBrain = paramsModel.yBrain;
            float treatment = paramsModel.treatment;
            float bEnd = paramsModel.bEnd;
            float bStep = paramsModel.bStep;

            float stepScale = 1;

            float stepForMethodRK = 0.1f;

            float tMax;

            SystemOfEquation systemEq;
            MethodDiffEquation methodDiff;

            float bForFindMin = b;
            // Counting for everyone patient and find required by min difference between predict data and actual data
            for (int i = 0; i < numberPatients; i++)
            {
                //Storage to keep data about matched value
                Dictionary<string, List<float>> cancerValuesParameters = new Dictionary<string, List<float>>()
                {
                    {"l1" , new List<float>() },
                    {"b" , new List<float>() },
                    {"e" , new List<float>() },
                    {"d" , new List<float>() },
                    {"l3" , new List<float>() },
                    {"u" , new List<float>() },
                    {"days" , new List<float>()},
                    {"xBrain" , new List<float>() },
                    {"yBrain" , new List<float>() },
                    {"treatment" , new List<float>() },
                    {"BStep", new List<float>() },
                    {"Difference" , new List<float>()},
                };
                List<List<float>> listAllValuesT = new List<List<float>>();
                List<List<float>> listAllValuesVolume = new List<List<float>>();

                float tStart = modelData.Patients[i]["Diameter"][0][0];
                float tEnd = modelData.Patients[i]["Diameter"][0][modelData.Patients[i]["Diameter"][0].Count - 1];
                tMax = tEnd - tStart;

                float x0 = 1;
                float y0 = 1;
                float z0 = 0;

                //tMax /= 30;
                do
                {
                    // add to list every parameter
                    cancerValuesParameters["l1"].Add(l1);
                    cancerValuesParameters["b"].Add(b);
                    cancerValuesParameters["e"].Add(eSpeed);
                    cancerValuesParameters["d"].Add(d);
                    cancerValuesParameters["l3"].Add(l3);
                    cancerValuesParameters["u"].Add(u);
                    cancerValuesParameters["days"].Add(days);
                    cancerValuesParameters["xBrain"].Add(xBrain);
                    cancerValuesParameters["yBrain"].Add(yBrain);
                    cancerValuesParameters["treatment"].Add(treatment);
                    cancerValuesParameters["BStep"].Add(bStep);

                    systemEq = new SystemOfEquation(l1, b, d, eSpeed, l3, u);
                    methodDiff = new MethodDiffEquation();

                    List<List<float>> values = new List<List<float>>();
                    try
                    {
                        values = methodDiff.RK3D(tStart, x0, y0, z0, tEnd, stepForMethodRK, systemEq);
                    }
                    catch (Exception eGet)
                    {
                        MessageBox.Show($"{eGet.Message}");
                        unpressedButton();
                    }

                    // Data for time-volume plot

                    listAllValuesT.Add(values[0]);
                    listAllValuesVolume.Add(values[1]);

                    // find difference between modelData and diffusionModelData and add to list
                    cancerValuesParameters["Difference"].Add(Math.Abs(
                        values[1][values[1].Count - 1] -
                        modelData.Patients[i]["Volume"][1][modelData.Patients[i]["Volume"][1].Count - 1]));

                    bForFindMin += bStep;
                }
                while (bForFindMin <= bEnd);


                bForFindMin = b;
                // Find required speed and others parameters
                float minDifference = cancerValuesParameters["Difference"].Min();
                int indexMinDifference = cancerValuesParameters["Difference"].IndexOf(cancerValuesParameters["Difference"].Min());
                float requiredB = cancerValuesParameters["b"][indexMinDifference];

                List<float> requiredTValue = listAllValuesT[indexMinDifference];
                List<float> requiredVolume = listAllValuesVolume[indexMinDifference];

                float differenceT = requiredTValue[0] - (tStart);
                //float differenceT = requiredTValue[0] - (tStart / 30);
                for (int itemTValues = 0; itemTValues < requiredTValue.Count; itemTValues++)
                {
                    requiredTValue[itemTValues] = requiredTValue[itemTValues] - differenceT;
                }
                Debug.WriteLine("requiredTValue\t" + requiredTValue[0].ToString() + "\ntStart\t" + tStart);

                //now save ml
                float differencePoints = (requiredVolume[0] * stepScale) - modelData.Patients[i]["Volume"][1][0];
                for (int itemPointsValues = 0; itemPointsValues < requiredVolume.Count; itemPointsValues++)
                {
                    //requiredNumberPointsVolume[itemPointsValues] = requiredNumberPointsVolume[itemPointsValues] * stepScale - differencePoints;
                    requiredVolume[itemPointsValues] = requiredVolume[itemPointsValues] / 1000 - differencePoints;
                }
                Debug.WriteLine("requiredVolume\t" + requiredVolume[0].ToString() + "\nmodelData\t" + modelData.Patients[i]["Volume"][1][0].ToString());

                // prepare data about parameters of cancer for writing into files
                Dictionary<string, float> requiredCancerValuesParameters = new Dictionary<string, float>()
                {

                    {"l1" ,  cancerValuesParameters["l1"][indexMinDifference] },
                    {"b" , cancerValuesParameters["b"][indexMinDifference] },
                    {"e" , cancerValuesParameters["e"][indexMinDifference] },
                    {"l3" , cancerValuesParameters["l3"][indexMinDifference] },
                    {"u" , cancerValuesParameters["u"][indexMinDifference] },
                    {"days" , cancerValuesParameters["days"][indexMinDifference] },
                    {"xBrain" , cancerValuesParameters["xBrain"][indexMinDifference]},
                    {"yBrain" , cancerValuesParameters["yBrain"][indexMinDifference] },
                    {"treatment" , cancerValuesParameters["treatment"][indexMinDifference] },
                    {"BStep" , cancerValuesParameters["BStep"][indexMinDifference] },
                    {"TMax" , requiredTValue.Last()},
                    {"Difference" , cancerValuesParameters["Difference"][indexMinDifference]},
                };

                // write every data about modeling to files
                // Write time-value data to file
                string pathWriteValueToFile = @"dataTumor\PredictData\PersonalPatient\Volume\timeValue\txt\";
                ActionDataFile.writeTimeValueToFile("Volume", i, requiredTValue.ToArray(), requiredVolume.ToArray(), pathWriteValueToFile);
                // write params of modeling to file
                ActionDataFile.writeParametersToFile(type: "Volume", number: i, cancerParameters: requiredCancerValuesParameters);

                worker.ReportProgress((i + 1) * (int)valueOfDivisionProgressBar, String.Format("Processing Iteration {0}", i + 1));
            }

            worker.ReportProgress(100, "Done Calculate min!");
            
        }


        private void CalculateMin_Click(object sender, RoutedEventArgs e)
        {
            PercentProgressBarCalculate.Visibility = Visibility.Visible;
            ProgressBarCalculate.Visibility = Visibility.Visible;
            CalculateMin.Content = "Calculate...";
            SolidColorBrush brushForPressedButton = new SolidColorBrush(Colors.Black);
            CalculateMin.Foreground = brushForPressedButton;
            CalculateMin.IsEnabled = false;

            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += worker_RunWorkerComplited;
            worker.WorkerReportsProgress = true;
            worker.DoWork += workerMin_Calculate;
            worker.ProgressChanged += worker_ProgressChanged;

            float treatment = 0;
            if(CheckBoxTreatment.IsChecked == true)
            {
                treatment = 1;
            }
            try
            {
                ParametersModel paramsModel = new ParametersModel(
                                    float.Parse(TextBoxL1.Text),
                                    float.Parse(TextBoxB.Text),
                                    float.Parse(TextBoxE.Text),
                                    float.Parse(TextBoxD.Text),
                                    float.Parse(TextBoxL3.Text),
                                    float.Parse(TextBoxU.Text),
                                    float.Parse(TextBoxDays.Text),
                                    float.Parse(TextBoxXBrain.Text),
                                    float.Parse(TextBoxYBrain.Text),
                                    treatment,
                                    float.Parse(TextBoxBEnd.Text),
                                    float.Parse(TextBoxBStep.Text));
                worker.RunWorkerAsync(paramsModel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                unpressedButton();
            }
            
        }



        private void CalculateOne_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
