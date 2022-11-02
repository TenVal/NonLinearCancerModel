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
using System.Globalization;
using System.IO;

namespace NotLinearCancerModel.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для CalculateOneView.xaml
    /// </summary>
    public partial class CalculateOneView : UserControl
    {
        public struct ParametersCancerForOneCalculate
        {
            public float length;
            public float RightX;
            public float h;
            public float d;
            public float k;
            public float speed;
            public float angleXY;
            public float angleZ;
            public float alpha;
            public float tMax;
            public int numberPatient;

            public ParametersCancerForOneCalculate(
                float length,
                float RightX,
                float h,
                float d,
                float k,
                float speed,
                float angleXY,
                float angleZ,
                float alpha,
                float tMax,
                int numberPatient)
            {
                this.length = length;
                this.RightX = RightX;
                this.h = h;
                this.d = d;
                this.k = k;
                this.speed = speed;
                this.angleXY = angleXY;
                this.angleZ = angleZ;
                this.alpha = alpha;
                this.tMax = tMax;
                this.numberPatient = numberPatient;
            }
        }


        public CalculateOneView()
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

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            /*change style pressed and calculated button*/
            PercentProgressBarCalculate.Visibility = Visibility.Visible;
            ProgressBarCalculate.Visibility = Visibility.Visible;
            Calculate.Content = "Calculate...";
            SolidColorBrush brushForPressedButton = new SolidColorBrush(Colors.Black);
            Calculate.Foreground = brushForPressedButton;
            Calculate.IsEnabled = false;
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += worker_RunWorkerComplited;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_Calculate;
            worker.ProgressChanged += worker_ProgressChanged;

            /*create structer to save parameters from input box*/
            ParametersCancerForOneCalculate paramsCancer;
            int numberPatient = 0;
            if (ComboBoxChoosePatient.SelectedItem != null)
            {
                numberPatient = int.Parse(ComboBoxChoosePatient.Text, CultureInfo.InvariantCulture);
            }
            try
            {
                paramsCancer = new ParametersCancerForOneCalculate(
                                    float.Parse(TextBoxLength.Text),
                                    float.Parse(TextBoxLength.Text),
                                    float.Parse(TextBoxH.Text),
                                    float.Parse(TextBoxD.Text),
                                    float.Parse(TextBoxK.Text),
                                    float.Parse(TextBoxSpeed.Text),
                                    float.Parse(TextBoxAngleXY.Text),
                                    float.Parse(TextBoxAngleZ.Text),
                                    float.Parse(TextBoxAlpha.Text),
                                    float.Parse(TextBoxTMax.Text),
                                    numberPatient);
                worker.RunWorkerAsync(paramsCancer);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(String.Format("Please input correct parameters!\n{ex}", ex));
            }
            catch (FormatException ex)
            {
                MessageBox.Show(String.Format("Please input correct parameters!\n{ex}", ex));
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(String.Format("Please don't go beyond the limits\n{ex}", ex));
            }
        }


        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarCalculate.Value = e.ProgressPercentage;
        }


        private void worker_Calculate(object sender, DoWorkEventArgs e)
        {
            DataCancer modelData = new DataCancer();
            ParametersCancerForOneCalculate paramsCancer = (ParametersCancerForOneCalculate)e.Argument;
            var worker = sender as BackgroundWorker;
            worker.ReportProgress(0, String.Format("Processing ..."));

            float length = paramsCancer.length;
            float RightX = paramsCancer.RightX;
            float h = paramsCancer.h;
            float d = paramsCancer.d;
            float k = paramsCancer.k;
            float speed = paramsCancer.speed;
            float angleXY = paramsCancer.angleXY;
            float angleZ = paramsCancer.angleZ;
            float alpha = paramsCancer.alpha;
            float tMax = paramsCancer.tMax;
            float stepScale = (h / 10) * (h / 10) * (h / 10);
            int numberPatient = paramsCancer.numberPatient;
            float tStart = 0;
            float tEnd = 0;
            if (numberPatient != 0)
            {
                numberPatient--;
                tStart = modelData.Patients[numberPatient]["Diameter"][0][0];
                tEnd = modelData.Patients[numberPatient]["Diameter"][0][modelData.Patients[numberPatient]["Diameter"][0].Count - 1];
                tMax = tEnd - tStart;
                numberPatient++;
            }
            tMax /= 30;
            string path = @"dataTumor\PredictData\Any\";

            C c = new C(speed, angleXY, angleZ);
            Q q = new Q(0);
            D dF = new D(speed, d);

            worker.ReportProgress(10, String.Format("Processing ..."));
            MethodDiffusion diffusion = new MethodDiffusion(dF, c, q, alpha);
            worker.ReportProgress(20, String.Format("Processing ..."));

            int N = (int)(length / h);
            double[,,] valuesP = new double[N, N, N];
            diffusion.getValues(tMax, h, k, length, valuesP);

            worker.ReportProgress(60, String.Format("Processing ..."));

            // Data for time-volume plot
            float[] numberPointsVolume = new float[diffusion.NumberPointsVolume.Count];
            Array.Copy(diffusion.NumberPointsVolume.ToArray(), numberPointsVolume, numberPointsVolume.Length);
            float[] tValues = new float[diffusion.TValues.Count];
            Array.Copy(diffusion.TValues.ToArray(), tValues, tValues.Length);


            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"\nModel data 1 {modelData.Patients[i]["Volume"][1][0]}");
            }

            if (numberPatient != 0)
            {
                //Refactoring data to compare with Experimental; change numberPatient to extract data
                numberPatient--;
                
                float differenceT = tValues[0] - (tStart / 30);
                for (int itemTValues = 0; itemTValues < tValues.Length; itemTValues++)
                {
                    tValues[itemTValues] = tValues[itemTValues] - differenceT;
                }

                float differencePoints = (numberPointsVolume[0] * stepScale) - modelData.Patients[numberPatient]["Volume"][1][0];
                for (int itemPointsValues = 0; itemPointsValues < numberPointsVolume.Length; itemPointsValues++)
                {
                    numberPointsVolume[itemPointsValues] = numberPointsVolume[itemPointsValues] * stepScale - differencePoints;
                }
                numberPatient++;
            }
            else
            {
                for (int itemPointsValues = 0; itemPointsValues < numberPointsVolume.Length; itemPointsValues++)
                {
                    numberPointsVolume[itemPointsValues] = numberPointsVolume[itemPointsValues] * stepScale;
                }
            }

            Dictionary<string, float> CancerValuesParameters = new Dictionary<string, float>()
            {
                {"Length" ,  length },
                {"H" , h },
                {"D" , d },
                {"K" , k },
                {"Speed" , speed},
                {"AngleXY" , angleXY },
                {"AngleZ" , angleZ },
                {"Alpha", alpha },
                {"TMax" , tMax },
                {"numberPatient", numberPatient }
            };

            // copy old data to compare predict and last in the future
            ActionDataFile.copyAllFiles(path + @"Volume\timeValue\txt\" + numberPatient.ToString() + "Volume.txt", path + @"Volume\timeValue\txt\" + numberPatient.ToString() + "VolumeOld.txt");
            // write every data about modeling to files
            ActionDataFile.writeDataToFile("Volume", numberPatient-1, valuesP, path);
            // Write time-value data to file
            ActionDataFile.writeTimeValueToFile("Volume", numberPatient-1, tValues, numberPointsVolume, path);
            // write params of modeling to file
            ActionDataFile.writeParametersToFile(type: "Volume", number: numberPatient-1, cancerParameters: CancerValuesParameters, pathToSave: path);
            worker.ReportProgress(100, String.Format("Done Calculate!"));
        }

        private void worker_RunWorkerComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done Calculate!");
            Calculate.Content = "Calculate";
            Calculate.IsEnabled = true;
            PercentProgressBarCalculate.Visibility = Visibility.Collapsed;
            ProgressBarCalculate.Visibility = Visibility.Collapsed;
            SolidColorBrush brushForPressedButton = new SolidColorBrush(Colors.White);
            Calculate.Foreground = brushForPressedButton;
            ProgressBarCalculate.Value = 0;
        }


        private void ImportParams_Click(object sender, RoutedEventArgs e)
        {
            int number = 1;
            string type = "Volume";
            if (ComboBoxChoosePatient.SelectedItem != null)
            {
                TextBlock selectedItem = (TextBlock)ComboBoxChoosePatient.SelectedItem;
                
                try
                {
                    number = int.Parse(selectedItem.Text.ToString());
                }
                catch (System.NullReferenceException ex)
                {
                    ComboBoxChoosePatient.SelectedIndex = 0;
                    MessageBox.Show(String.Format("Please choose patient at the list!\n{ex}", ex));
                }
                string pathToRead = @"dataTumor\PredictData\PersonalPatients\" + type + @"\txt\params\" + number.ToString() + @"Params.txt";
                Dictionary<string, float> cancerParams = ActionDataFile.getParametersFromFile(type, number, pathToRead);

                TextBoxLength.Text = cancerParams["Length"].ToString();
                TextBoxH.Text = cancerParams["H"].ToString();
                TextBoxD.Text = cancerParams["D"].ToString();
                TextBoxK.Text = cancerParams["K"].ToString();
                TextBoxSpeed.Text = cancerParams["Speed"].ToString();
                TextBoxAngleXY.Text = cancerParams["AngleXY"].ToString();
                TextBoxAngleZ.Text = cancerParams["AngleZ"].ToString();
                TextBoxAlpha.Text = cancerParams["Alpha"].ToString();
                TextBoxTMax.Text = cancerParams["TMax"].ToString();
            }
            else
            {
                MessageBox.Show(String.Format("Please choose patient at the list!"));
            }            
        }
    }
}
