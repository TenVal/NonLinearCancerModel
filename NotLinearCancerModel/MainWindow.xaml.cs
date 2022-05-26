using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;

namespace NotLinearCancerModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numberPatientForOutputPlots;
        public struct ParamsForSavePlot
        {
            public bool radioButtonChecked;
            public string pathPythonInterpreter;

            public ParamsForSavePlot(
                bool radioButtonChecked,
                string pathPythonInterpreter)
            {
                this.radioButtonChecked = radioButtonChecked;
                this.pathPythonInterpreter = pathPythonInterpreter;
            }
        }


        public MainWindow()
        {
            InitializeComponent();
        }


        private void ShowPlots_Click(object sender, RoutedEventArgs e)
        {
            Image1.Source = null;
            Image2.Source = null;
            numberPatientForOutputPlots = 1;
            string pathImg1;
            string pathImg2;
            string pathParameters;
            string type = "Volume";
            if (RadioButtonFindMin.IsChecked == true)
            {
                try
                {
                    numberPatientForOutputPlots = int.Parse(TextBoxPatientNumberPlot.Text, CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show($"Please, input correct data (number patient)!\n{ex}");
                }
                pathImg1 = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathImg2 = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\timeValue\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\txt\params\" + numberPatientForOutputPlots.ToString() + "Params.txt";
            }
            else
            {
                numberPatientForOutputPlots = 1;
                pathImg1 = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathImg2 = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\timeValue\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\txt\params\" + numberPatientForOutputPlots.ToString() + "Params.txt";
            }
            string textLabelParams = "Cancer\tParameters:\n";
            Dictionary<string, float> cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientForOutputPlots, pathParameters);
            
            foreach (var keyValueCancer in cancerParameters)
            {
                textLabelParams += (keyValueCancer.Key + "\t");
                textLabelParams += (keyValueCancer.Value.ToString() + "\n");
            }
            textBoxCancerParameters.Text = textLabelParams;

            // Output images (plots)
            BitmapImage bmp1 = new BitmapImage();
            bmp1.BeginInit();
            bmp1.UriSource = new Uri(pathImg1, UriKind.Relative);
            bmp1.EndInit();
            BitmapImage bmp2 = new BitmapImage();
            bmp2.BeginInit();
            bmp2.UriSource = new Uri(pathImg2, UriKind.Relative);
            bmp2.EndInit();
            Image1.Stretch = Stretch.Fill;
            Image1.Source = bmp1;
            Image2.Stretch = Stretch.Fill;
            Image2.Source = bmp2;
        }


        private void ButtonGoBackImg_Click(object sender, RoutedEventArgs e)
        {
            string pathImg1;
            string pathImg2;
            string pathParameters;
            string type = "Volume";
            if (RadioButtonFindMin.IsChecked == true)
            {
                numberPatientForOutputPlots--;
                if (numberPatientForOutputPlots < 1)
                {
                    numberPatientForOutputPlots = 10;
                }
                pathImg1 = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathImg2 = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\timeValue\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\txt\params\" + numberPatientForOutputPlots.ToString() + "Params.txt";
            }
            else
            {
                numberPatientForOutputPlots = 1;
                pathImg1 = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathImg2 = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\timeValue\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\txt\params\" + numberPatientForOutputPlots.ToString() + "Params.txt";
            }

            string textLabelParams = "Cancer\tParameters:\n";
            Dictionary<string, float> cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientForOutputPlots, pathParameters);

            foreach (var keyValueCancer in cancerParameters)
            {
                textLabelParams += (keyValueCancer.Key + "\t");
                textLabelParams += (keyValueCancer.Value.ToString() + "\n");
            }
            textBoxCancerParameters.Text = textLabelParams;

            // Output images (plots)
            BitmapImage bmp1 = new BitmapImage();
            bmp1.BeginInit();
            bmp1.UriSource = new Uri(pathImg1, UriKind.Relative);
            bmp1.EndInit();
            BitmapImage bmp2 = new BitmapImage();
            bmp2.BeginInit();
            bmp2.UriSource = new Uri(pathImg2, UriKind.Relative);
            bmp2.EndInit();
            Image1.Stretch = Stretch.Fill;
            Image1.Source = bmp1;
            Image2.Stretch = Stretch.Fill;
            Image2.Source = bmp2;
        }


        private void ButtonGoNextImg_Click(object sender, RoutedEventArgs e)
        {
            string pathImg1;
            string pathImg2;
            string pathParameters;
            string type = "Volume";
            if (RadioButtonFindMin.IsChecked == true)
            {
                numberPatientForOutputPlots++;
                if (numberPatientForOutputPlots > 10)
                {
                    numberPatientForOutputPlots = 1;
                }
                pathImg1 = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathImg2 = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\timeValue\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\txt\params\" + numberPatientForOutputPlots.ToString() + "Params.txt";
            }
            else
            {
                numberPatientForOutputPlots = 1;
                pathImg1 = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathImg2 = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\timeValue\img\" + numberPatientForOutputPlots.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\txt\params\" + numberPatientForOutputPlots.ToString() + "Params.txt";
            }

            string textLabelParams = "Cancer\t\tParameters:\n";
            Dictionary<string, float> cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientForOutputPlots, pathParameters);

            foreach (var keyValueCancer in cancerParameters)
            {
                textLabelParams += (keyValueCancer.Key + "\t\t");
                textLabelParams += (keyValueCancer.Value.ToString() + "\n");
            }
            textBoxCancerParameters.Text = textLabelParams;

            // Output images (plots)
            BitmapImage bmp1 = new BitmapImage();
            bmp1.BeginInit();
            bmp1.UriSource = new Uri(pathImg1, UriKind.Relative);
            bmp1.EndInit();
            BitmapImage bmp2 = new BitmapImage();
            bmp2.BeginInit();
            bmp2.UriSource = new Uri(pathImg2, UriKind.Relative);
            bmp2.EndInit();
            Image1.Stretch = Stretch.Fill;
            Image1.Source = bmp1;
            Image2.Stretch = Stretch.Fill;
            Image2.Source = bmp2;
        }


        private void RadioButtonFindMin_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxPatientNumberPlot.Text = "";
            TextBoxPatientNumberPlot.IsReadOnly = false;
            TextBoxPatientNumberPlot.Visibility = Visibility.Visible;
            LabelPatientNumberPlot.Visibility = Visibility.Visible;
        }


        private void RadioButtonWithoutFindMin_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxPatientNumberPlot.Text = "There is no any definite patient";
            TextBoxPatientNumberPlot.IsReadOnly = true;
            TextBoxPatientNumberPlot.Visibility = Visibility.Collapsed;
            LabelPatientNumberPlot.Visibility = Visibility.Collapsed;
        }


        private void SavePlots_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += worker_RunWorkerComplited;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_SavePlots;
            worker.ProgressChanged += worker_ProgressChanged;
            ParamsForSavePlot paramsForSavePlot;
            try
            {
                paramsForSavePlot = new ParamsForSavePlot(
                    (bool)RadioButtonFindMin.IsChecked,
                    TextBoxPythonInterpreter.Text.ToString().Trim());
                worker.RunWorkerAsync(paramsForSavePlot);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(String.Format("Please input correct parameters!\n{ex}"));
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
        

        private void worker_SavePlots(object sender, DoWorkEventArgs e)
        {
            ParamsForSavePlot paramsForSavePlot = (ParamsForSavePlot)e.Argument;
            var worker = sender as BackgroundWorker;

            worker.ReportProgress(0);
            string scriptPython;
            if (paramsForSavePlot.radioButtonChecked == true)
            {
                scriptPython = @"CancerVolumePlot.py";
            }
            else
            {
                scriptPython = @"OneCancerVolumePlot.py";
            }
            // Create Process start info
            var psi = new ProcessStartInfo();

            string pathPython = paramsForSavePlot.pathPythonInterpreter.Trim();
            // checking current path to python interpreter
            if (pathPython == "Please, input your path to python interpreter" || pathPython == "")
            {
                pathPython = @"..\..\..\env\Scripts\python.exe";
            }

            psi.FileName = pathPython;

            // Provide Scripts and Arguments
            var var1 = "";
            var var2 = "";

            psi.Arguments = $"\"{scriptPython}\"";
            // Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            // Display outut
            Debug.WriteLine("ERRORS:");
            Debug.WriteLine(errors);
            Debug.WriteLine("Results:");
            Debug.WriteLine(results);
            if (errors == "")
            {
                MessageBox.Show($"Success save img!\nResults:\t{results}");
            }
            else
            {
                MessageBox.Show($"There were some errors!\nErrors:\t{errors}");
            }
            worker.ReportProgress(100);
        }


        private void worker_RunWorkerComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            SavePlots.Content = "Save Plots";
        }


        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SavePlots.Content = "Saving...";
        }

        private void ButtonShowTotal_Click(object sender, RoutedEventArgs e)
        {
            string type = @"Volume";
            string pathImg = @"..\..\..\dataTumor\PredictData\Total\" + type + @"\img\All.png";

            textBoxCancerParameters.Text = "";

            // Output images (plots)
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(pathImg, UriKind.Relative);
            bmp.EndInit();

            Image1.Stretch = Stretch.Fill;
            Image1.Source = bmp;
            Image2.Source = null;
        }
    }
}
