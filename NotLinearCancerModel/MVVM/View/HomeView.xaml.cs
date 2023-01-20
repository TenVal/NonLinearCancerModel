using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public struct ParametersCancer
        {
            public float length;
            public float RightX;
            public float h;
            public float d;
            public float k;
            public float SpeedEnd;
            public float SpeedStep;
            public float speed;
            public float angleXY;
            public float angleZ;
            public float resistance;

            public ParametersCancer(
                float length,
                float RightX,
                float h,
                float d,
                float k,
                float SpeedEnd,
                float SpeedStep,
                float speed,
                float angleXY,
                float angleZ,
                float resistance)
            {
                this.length = length;
                this.RightX = RightX;
                this.h = h;
                this.d = d;
                this.k = k;
                this.SpeedEnd = SpeedEnd;
                this.SpeedStep = SpeedStep;
                this.speed = speed;
                this.angleXY = angleXY;
                this.angleZ = angleZ;
                this.resistance = resistance;
            }
        }


        public HomeView()
        {
            InitializeComponent();
        }

        internal ViewModel.MainViewModel MainViewModel
        {
            get => default;
            set
            {
            }
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
            worker.RunWorkerCompleted += workerMin_RunWorkerComplited;
            worker.WorkerReportsProgress = true;
            worker.DoWork += workerMin_Calculate;
            worker.ProgressChanged += workerMin_ProgressChanged;
            
            try
            {
                ParametersCancer paramsCancer = new ParametersCancer(
                                    float.Parse(TextBoxLength.Text),
                                    float.Parse(TextBoxLength.Text),
                                    float.Parse(TextBoxH.Text),
                                    float.Parse(TextBoxD.Text),
                                    float.Parse(TextBoxK.Text),
                                    float.Parse(TextBoxSpeedEnd.Text),
                                    float.Parse(TextBoxSpeedStep.Text),
                                    float.Parse(TextBoxSpeedStart.Text),
                                    float.Parse(TextBoxAngleXY.Text),
                                    float.Parse(TextBoxAngleZ.Text),
                                    float.Parse(TextBoxAlpha.Text));
                worker.RunWorkerAsync(paramsCancer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }   
        }

        private void workerMin_RunWorkerComplited(object sender, RunWorkerCompletedEventArgs e)
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

        private void workerMin_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarCalculate.Value = e.ProgressPercentage;
        }

        private void workerMin_Calculate(object sender, DoWorkEventArgs e)
        {
            ParametersCancer paramsCancer = (ParametersCancer)e.Argument;
            var worker = sender as BackgroundWorker;
            worker.ReportProgress(0, String.Format("Processing Iteration 1."));
            DataCancer modelData = new DataCancer();

            int numberPatients = 10;
            float valueOfDivisionProgressBar = 100 / numberPatients;

            float length = paramsCancer.length;
            float RightX = paramsCancer.RightX;
            float h = paramsCancer.h;
            float d = paramsCancer.d;
            Debug.WriteLine("D\t" + d);
            float k = paramsCancer.k;
            float SpeedEnd = paramsCancer.SpeedEnd;
            float SpeedStep = paramsCancer.SpeedStep;
            float speed = paramsCancer.speed;
            float angleXY = paramsCancer.angleXY;
            float angleZ = paramsCancer.angleZ;
            float alpha = paramsCancer.resistance;
            int N = (int)(length / h);
            float stepScale = (h / 10) * (h / 10) * (h / 10);

            float tMax;

            C c = new C(speed, angleXY, angleZ);
            Q q = new Q(0);
            MethodDiffusion diffusion;

            float speedForFindMin = speed;
            // Counting for everyone patient and find required by min difference between predict data and actual data
            for (int i = 0; i < numberPatients; i++)
            {
                //Storage to keep data about matched value
                Dictionary<string, List<float>> cancerValuesParameters = new Dictionary<string, List<float>>()
                {
                    {"Length" , new List<float>() },
                    {"H" , new List<float>() },
                    {"D" , new List<float>() },
                    {"K" , new List<float>() },
                    {"SpeedEnd" , new List<float>() },
                    {"SpeedStep" , new List<float>() },
                    {"Speed" , new List<float>()},
                    {"AngleXY" , new List<float>() },
                    {"AngleZ" , new List<float>() },
                    {"Resistance" , new List<float>() },
                    {"Difference" , new List<float>()},
                };
                List<double[,,]> listAllValuesP = new List<double[,,]>();
                List<float[]> listAllValuesT = new List<float[]>();
                List<float[]> listAllValuesNumberPointsVolume = new List<float[]>();

                float tStart = modelData.Patients[i]["Diameter"][0][0];
                float tEnd = modelData.Patients[i]["Diameter"][0][modelData.Patients[i]["Diameter"][0].Count - 1];
                tMax = tEnd - tStart;

                tMax /= 30;
                do
                {
                    // add to list every parameter
                    cancerValuesParameters["Length"].Add(length);
                    cancerValuesParameters["H"].Add(h);
                    cancerValuesParameters["D"].Add(d);
                    //Debug.WriteLine("add D\t" + d);
                    cancerValuesParameters["K"].Add(k);
                    cancerValuesParameters["SpeedEnd"].Add(SpeedEnd);
                    cancerValuesParameters["SpeedStep"].Add(SpeedStep);
                    cancerValuesParameters["Speed"].Add(speedForFindMin);
                    cancerValuesParameters["AngleXY"].Add(angleXY);
                    cancerValuesParameters["AngleZ"].Add(angleZ);
                    cancerValuesParameters["Resistance"].Add(alpha);

                    D dF = new D(speedForFindMin, d);
                    diffusion = new MethodDiffusion(dF, c, q, alpha);

                    double[,,] valuesP = new double[N, N, N];
                    diffusion.getValues(tMax, h, k, length, valuesP);

                    // Data for time-volume plot
                    float[] numberPointsVolume = new float[diffusion.NumberPointsVolume.Count];
                    Array.Copy(diffusion.NumberPointsVolume.ToArray(), numberPointsVolume, numberPointsVolume.Length);
                    float[] tValues = new float[diffusion.TValues.Count];
                    Array.Copy(diffusion.TValues.ToArray(), tValues, tValues.Length);
                    /*var valuesP = diffusion.getValues(tMax, h, k, length);
                    Array.Copy(diffusion.getValues(tMax, h, k, length), valuesP, valuesP.GetLength(0) + valuesP.GetLength(1) + valuesP.GetLength(2));*/
                    listAllValuesP.Add(valuesP);
                    listAllValuesT.Add(tValues);
                    listAllValuesNumberPointsVolume.Add(numberPointsVolume);

                    // find difference between modelData and diffusionModelData and add to list
                    cancerValuesParameters["Difference"].Add(Math.Abs(
                        diffusion.NumberPointsVolume[diffusion.NumberPointsVolume.Count - 1] * stepScale -
                        modelData.Patients[i]["Volume"][1][modelData.Patients[i]["Volume"][1].Count - 1]));

                    speedForFindMin += SpeedStep;
                }
                while (speedForFindMin <= SpeedEnd);

                
                speedForFindMin = speed;
                // Find required speed and others parameters
                float minDifference = cancerValuesParameters["Difference"].Min();
                int indexMinDifference = cancerValuesParameters["Difference"].IndexOf(cancerValuesParameters["Difference"].Min());
                float requiredSpeed = cancerValuesParameters["Speed"][indexMinDifference];

                double[,,] requiredValuesP = listAllValuesP[indexMinDifference];
                float[] requiredTValue = listAllValuesT[indexMinDifference];
                float[] requiredNumberPointsVolume = listAllValuesNumberPointsVolume[indexMinDifference];

                float differenceT = requiredTValue[0] - (tStart / 30);
                for (int itemTValues = 0; itemTValues < requiredTValue.Length; itemTValues++)
                {
                    requiredTValue[itemTValues] = requiredTValue[itemTValues] - differenceT;
                }
                Debug.WriteLine("requiredTValue\t" + requiredTValue[0].ToString() + "\ntStart\t" + tStart);

                //now save ml
                float differencePoints = (requiredNumberPointsVolume[0] * stepScale) - modelData.Patients[i]["Volume"][1][0];
                for (int itemPointsValues = 0; itemPointsValues < requiredNumberPointsVolume.Length; itemPointsValues++)
                {
                    //requiredNumberPointsVolume[itemPointsValues] = requiredNumberPointsVolume[itemPointsValues] * stepScale - differencePoints;
                    requiredNumberPointsVolume[itemPointsValues] = requiredNumberPointsVolume[itemPointsValues] / 1000 - differencePoints;
                }
                Debug.WriteLine("requiredNumberPointsVolume\t" + requiredNumberPointsVolume[0].ToString() + "\nmodelData\t" + modelData.Patients[i]["Volume"][1][0].ToString());

                // prepare data about parameters of cancer for writing into files
                Dictionary<string, float> requiredCancerValuesParameters = new Dictionary<string, float>()
                {
                    {"Length" ,  cancerValuesParameters["Length"][indexMinDifference] },
                    {"H" , cancerValuesParameters["H"][indexMinDifference] },
                    {"D" , cancerValuesParameters["D"][indexMinDifference] },
                    {"K" , cancerValuesParameters["K"][indexMinDifference] },
                    {"SpeedEnd" , cancerValuesParameters["SpeedEnd"][indexMinDifference] },
                    {"SpeedStep" , cancerValuesParameters["SpeedStep"][indexMinDifference] },
                    {"Speed" , cancerValuesParameters["Speed"][indexMinDifference]},
                    {"AngleXY" , cancerValuesParameters["AngleXY"][indexMinDifference] },
                    {"AngleZ" , cancerValuesParameters["AngleZ"][indexMinDifference] },
                    {"Alpha" , cancerValuesParameters["Resistance"][indexMinDifference] },
                    {"TMax" , requiredTValue.Last()},
                    {"Difference" , cancerValuesParameters["Difference"][indexMinDifference]},
                };

                // write every data about modeling to files
                ActionDataFile.writeDataToFile("Volume", i, requiredValuesP);
                // Write time-value data to file
                ActionDataFile.writeTimeValueToFile("Volume", i, requiredTValue, requiredNumberPointsVolume);
                // write params of modeling to file
                float[] paramsForCancer = { requiredSpeed, d, k };
                ActionDataFile.writeParametersToFile(type: "Volume", number: i, cancerParameters: requiredCancerValuesParameters);

                worker.ReportProgress((i + 1) * (int)valueOfDivisionProgressBar, String.Format("Processing Iteration {0}", i + 1));
            }

            worker.ReportProgress(100, "Done Calculate min!");
        }
    }
}