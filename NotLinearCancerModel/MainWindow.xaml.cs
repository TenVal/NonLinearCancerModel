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
        private int numberPatientOutputPlotFindMin;
        private int numberPatientOutputPlotOne;
        private const string type = "Volume";
        public struct ParamsForSavePlot
        {
            public bool radioButtonChecked;
            public string pathPythonInterpreter;
            public int numberPatientSavePlot;

            public ParamsForSavePlot(
                bool radioButtonChecked,
                string pathPythonInterpreter,
                int numberPatientSavePlot)
            {
                this.radioButtonChecked = radioButtonChecked;
                this.pathPythonInterpreter = pathPythonInterpreter;
                this.numberPatientSavePlot = numberPatientSavePlot;
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
            numberPatientOutputPlotFindMin = 1;
            numberPatientOutputPlotOne = 0;
            string pathImgVolume;
            string pathImgTimeVolume;
            string pathParameters;

            if (RadioButtonFindMin.IsChecked == true)
            {
                try
                {
                    numberPatientOutputPlotFindMin = int.Parse(TextBoxPatientNumberPlot.Text, CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show($"Please, input correct data (number patient)!\n{ex}");
                }
                pathImgVolume = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\img\" + numberPatientOutputPlotFindMin.ToString() + type + @".png";
                pathImgTimeVolume = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\timeValue\img\" + numberPatientOutputPlotFindMin.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\txt\params\" + numberPatientOutputPlotFindMin.ToString() + "Params.txt";
            }
            else
            {
                pathImgVolume = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\img\" + numberPatientOutputPlotFindMin.ToString() + type + @".png";
                pathImgTimeVolume = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\timeValue\img\" + numberPatientOutputPlotFindMin.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\txt\params\" + numberPatientOutputPlotFindMin.ToString() + "Params.txt";
            }
            string textLabelParams = "Cancer\tParameters:\n";
            Dictionary<string, float> cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotFindMin, pathParameters);

            foreach (var keyValueCancer in cancerParameters)
            {
                textLabelParams += (keyValueCancer.Key + "\t");
                textLabelParams += (keyValueCancer.Value.ToString() + "\n");
            }
            textBoxCancerParameters.Text = textLabelParams;

            // Output images (plots)
            BitmapImage bmpVolume = new BitmapImage();
            bmpVolume.BeginInit();
            bmpVolume.UriSource = new Uri(pathImgVolume, UriKind.Relative);
            bmpVolume.EndInit();
            BitmapImage bmpTimeVolume = new BitmapImage();
            bmpTimeVolume.BeginInit();
            bmpTimeVolume.UriSource = new Uri(pathImgTimeVolume, UriKind.Relative);
            bmpTimeVolume.EndInit();
            Image1.Stretch = Stretch.Fill;
            Image1.Source = bmpVolume;
            Image2.Stretch = Stretch.Fill;
            Image2.Source = bmpTimeVolume;
        }


        private void ButtonGoBackImg_Click(object sender, RoutedEventArgs e)
        {
            string pathImgVolume;
            string pathImgTimeVolume;
            string pathParameters;
            string type = "Volume";
            Dictionary<string, float> cancerParameters;
            string textLabelParams;

            if (RadioButtonFindMin.IsChecked == true)
            {
                numberPatientOutputPlotFindMin--;
                if (numberPatientOutputPlotFindMin < 1)
                {
                    numberPatientOutputPlotFindMin = 10;
                }
                pathImgVolume = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\img\" + numberPatientOutputPlotFindMin.ToString() + type + @".png";
                pathImgTimeVolume = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\timeValue\img\" + numberPatientOutputPlotFindMin.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\txt\params\" + numberPatientOutputPlotFindMin.ToString() + "Params.txt";
                cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotFindMin, pathParameters);
                textLabelParams = String.Format("Cancer\tParameters {0}:\n", numberPatientOutputPlotFindMin);
            }
            else
            {
                numberPatientOutputPlotOne--;
                if (numberPatientOutputPlotOne < 0)
                {
                    numberPatientOutputPlotOne = 10;
                }
                pathImgVolume = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\img\" + numberPatientOutputPlotOne.ToString() + type + @".png";
                pathImgTimeVolume = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\timeValue\img\" + numberPatientOutputPlotOne.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\txt\params\" + numberPatientOutputPlotOne.ToString() + "Params.txt";
                cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotOne, pathParameters);
                textLabelParams = String.Format("Cancer\tParameters {0}:\n", numberPatientOutputPlotOne);
            }

            foreach (var keyValueCancer in cancerParameters)
            {
                textLabelParams += (keyValueCancer.Key + "\t");
                textLabelParams += (keyValueCancer.Value.ToString() + "\n");
            }
            textBoxCancerParameters.Text = textLabelParams;

            // Output images (plots)
            BitmapImage bmpVolume = new BitmapImage();
            bmpVolume.BeginInit();
            bmpVolume.UriSource = new Uri(pathImgVolume, UriKind.Relative);
            bmpVolume.EndInit();
            BitmapImage bmpTimeVolume = new BitmapImage();
            bmpTimeVolume.BeginInit();
            bmpTimeVolume.UriSource = new Uri(pathImgTimeVolume, UriKind.Relative);
            bmpTimeVolume.EndInit();
            Image1.Stretch = Stretch.Fill;
            Image1.Source = bmpVolume;
            Image2.Stretch = Stretch.Fill;
            Image2.Source = bmpTimeVolume;
        }


        private void ButtonGoNextImg_Click(object sender, RoutedEventArgs e)
        {
            string pathImgVolume;
            string pathImgTimeVolume;
            string pathParameters;
            string type = "Volume";
            string textLabelParams;
            Dictionary<string, float> cancerParameters;

            if (RadioButtonFindMin.IsChecked == true)
            {
                numberPatientOutputPlotFindMin++;
                if (numberPatientOutputPlotFindMin > 10)
                {
                    numberPatientOutputPlotFindMin = 1;
                }
                pathImgVolume = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\img\" + numberPatientOutputPlotFindMin.ToString() + type + @".png";
                pathImgTimeVolume = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\timeValue\img\" + numberPatientOutputPlotFindMin.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\txt\params\" + numberPatientOutputPlotFindMin.ToString() + "Params.txt";
                cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotFindMin, pathParameters);
                textLabelParams = String.Format("Cancer\tParameters {0}:\n", numberPatientOutputPlotFindMin);
            }
            else
            {
                numberPatientOutputPlotOne++;
                if (numberPatientOutputPlotOne > 10)
                {
                    numberPatientOutputPlotOne = 1;
                }
                pathImgVolume = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\img\" + numberPatientOutputPlotOne.ToString() + type + @".png";
                pathImgTimeVolume = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\timeValue\img\" + numberPatientOutputPlotOne.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\txt\params\" + numberPatientOutputPlotOne.ToString() + "Params.txt";
                cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotOne, pathParameters);
                textLabelParams = String.Format("Cancer\tParameters {0}:\n", numberPatientOutputPlotOne);
            }

            foreach (var keyValueCancer in cancerParameters)
            {
                textLabelParams += (keyValueCancer.Key + "\t");
                textLabelParams += (keyValueCancer.Value.ToString() + "\n");
            }
            textBoxCancerParameters.Text = textLabelParams;

            // Output images (plots)
            BitmapImage bmpVolume = new BitmapImage();
            bmpVolume.BeginInit();
            bmpVolume.UriSource = new Uri(pathImgVolume, UriKind.Relative);
            bmpVolume.EndInit();
            BitmapImage bmpTimeVolume = new BitmapImage();
            bmpTimeVolume.BeginInit();
            bmpTimeVolume.UriSource = new Uri(pathImgTimeVolume, UriKind.Relative);
            bmpTimeVolume.EndInit();
            Image1.Stretch = Stretch.Fill;
            Image1.Source = bmpVolume;
            Image2.Stretch = Stretch.Fill;
            Image2.Source = bmpTimeVolume;
        }


        private void RadioButtonFindMin_Checked(object sender, RoutedEventArgs e)
        {
            /*TextBoxPatientNumberPlot.Text = "";
            TextBoxPatientNumberPlot.IsReadOnly = false;
            TextBoxPatientNumberPlot.Visibility = Visibility.Visible;
            LabelPatientNumberPlot.Visibility = Visibility.Visible;*/
        }


        private void RadioButtonWithoutFindMin_Checked(object sender, RoutedEventArgs e)
        {
            /*TextBoxPatientNumberPlot.Text = "There is no any definite patient";
            TextBoxPatientNumberPlot.IsReadOnly = true;
            TextBoxPatientNumberPlot.Visibility = Visibility.Collapsed;
            LabelPatientNumberPlot.Visibility = Visibility.Collapsed;*/
        }


        private void SavePlots_Click(object sender, RoutedEventArgs e)
        {
            SavePlots.Content = "Saving...";
            SavePlots.IsEnabled = false;
            SolidColorBrush brushForPressedButton = new SolidColorBrush(Colors.Black);
            SavePlots.Foreground = brushForPressedButton;

            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += worker_RunWorkerComplited;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_SavePlots;
            worker.ProgressChanged += worker_ProgressChanged;
            ParamsForSavePlot paramsForSavePlot;
            int numberPatientSavePlot = 0;
            if (RadioButtonWithoutFindMin.IsChecked == true)
            {
                if (TextBoxPatientNumberPlot.Text.Trim() == "")
                {
                    numberPatientSavePlot = 0;
                    LabelPatientNumberPlot.Content += "\nSaved non-patient data (any, number - 0)";
                }
                else
                {
                    try
                    {
                        numberPatientSavePlot = int.Parse(TextBoxPatientNumberPlot.Text, CultureInfo.InvariantCulture);
                    }
                    catch (ArgumentNullException ex)
                    {
                        MessageBox.Show(String.Format("Please input correct parameters!\n{0}", ex));
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show(String.Format("Please input correct parameters!\n{0}", ex));
                    }
                    catch (OverflowException ex)
                    {
                        MessageBox.Show(String.Format("Please don't go beyond the limits\n{0}", ex));
                    }
                }
            }
            paramsForSavePlot = new ParamsForSavePlot(
                (bool)RadioButtonFindMin.IsChecked,
                TextBoxPythonInterpreter.Text.ToString().Trim(),
                numberPatientSavePlot);
            worker.RunWorkerAsync(paramsForSavePlot);
        }


        private void worker_SavePlots(object sender, DoWorkEventArgs e)
        {
            ParamsForSavePlot paramsForSavePlot = (ParamsForSavePlot)e.Argument;
            var worker = sender as BackgroundWorker;

            worker.ReportProgress(0);
            string scriptPython;
            if (paramsForSavePlot.radioButtonChecked == true)
            {
                scriptPython = @"..\..\..\CancerVolumePlot.py";
            }
            else
            {
                scriptPython = @"..\..\..\OneCancerVolumePlot.py";
            }

            // Create Process start info
            var psi = new ProcessStartInfo();

            // Provide Script
            string pathPythonInterpreter = paramsForSavePlot.pathPythonInterpreter.Trim();
            // checking current path to python interpreter
            if (pathPythonInterpreter == "Please, input your path to python interpreter" || pathPythonInterpreter == "")
            {
                pathPythonInterpreter = @"..\..\..\env\Scripts\python.exe";
            }
            worker.ReportProgress(20);
            psi.FileName = pathPythonInterpreter;
          
            //Provide Arguments
            var numberPatientToSavePlot = paramsForSavePlot.numberPatientSavePlot.ToString();

            psi.Arguments = $"\"{scriptPython}\" \"{numberPatientToSavePlot}\" \"{type}\"";

            // Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            worker.ReportProgress(30);
            // Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            worker.ReportProgress(100);
            // Display outut
            Debug.WriteLine("ERRORS python:");
            Debug.WriteLine(errors);
            Debug.WriteLine("Results python:");
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
            SavePlots.IsEnabled = true;
            SolidColorBrush brushForPressedButton = new SolidColorBrush(Colors.White);
            SavePlots.Foreground = brushForPressedButton;
            LabelPatientNumberPlot.Content = "Patient number plot";
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
