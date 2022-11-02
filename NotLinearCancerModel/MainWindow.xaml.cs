﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;


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
            // Copy
            string pathFrom = @"..\..\..\";
            string pathToEnv = @"env";
            string pathToEnvDataTumor = @"dataTumor";
            string pathToActionDataFile = @"ActionDataFile.py";
            string pathToCancerVolumePlot = @"CancerVolumePlot.py";
            string pathToOneCancerVolumePlot = @"OneCancerVolumePlot.py";
            if (Directory.Exists(pathToEnv) == false)
            {
                // This path is a directory
                ActionDataFile.copyDir(pathFrom + pathToEnv, pathToEnv);
            }
            if (Directory.Exists(pathToEnvDataTumor) == false)
            {
                // This path is a directory
                ActionDataFile.copyDir(pathFrom + pathToEnvDataTumor, pathToEnvDataTumor);
            }
            if (File.Exists(pathToActionDataFile) == false)
            {
                // This path is a file
                File.Copy(pathFrom + pathToActionDataFile, pathToActionDataFile);
            }
            if (File.Exists(pathToCancerVolumePlot) == false)
            {
                // This path is a file
                File.Copy(pathFrom + pathToCancerVolumePlot, pathToCancerVolumePlot);
            }
            if (File.Exists(pathToOneCancerVolumePlot) == false)
            {
                // This path is a file
                File.Copy(pathFrom + pathToOneCancerVolumePlot, pathToOneCancerVolumePlot);
            }

        }


        private int outputImages(string pathImgVolume, string pathImgTimeVolume)
        {
            // Output images (plots)
            Image1.Source = null;
            Image2.Source = null;
            Image1.InvalidateMeasure();
            Image1.InvalidateArrange();
            Image1.InvalidateVisual();
            Image1.UpdateLayout();
            Image2.InvalidateMeasure();
            Image2.InvalidateArrange();
            Image2.InvalidateVisual();
            Image2.UpdateLayout();

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

            return 0;
        }


        private int outputParameters(Dictionary<string, float> cancerParameters, string textLabelParams)
        {
            foreach (var keyValueCancer in cancerParameters)
            {
                textLabelParams += (keyValueCancer.Key + "\t");
                textLabelParams += (keyValueCancer.Value.ToString() + "\n");
            }
            textBoxCancerParameters.Text = textLabelParams;
            return 0;
        }


        private string[] getPathsImages(int number, string typeMode)
        {
            string[] pathsToImages = new string[3];
            pathsToImages[0] = String.Format(@"dataTumor\PredictData\{0}\{1}\img\{2}{3}.png", typeMode, type, number, type);
            pathsToImages[1] = String.Format(@"dataTumor\PredictData\{0}\{1}\timeValue\img\{2}{3}.png", typeMode, type, number, type);
            pathsToImages[2] = String.Format(@"dataTumor\PredictData\{0}\{1}\txt\params\{2}Params.txt", typeMode, type, number);
            return pathsToImages;
        }


        private string getTextParams(int number, string typeMode)
        {
            string textLabelParams = String.Format("Cancer\tParameters {0}:\n", number);
            if (typeMode == "Any")
            {
                textLabelParams = String.Format("{0}Cancer\tParameters {1}:\n", typeMode, number);
            }
            return textLabelParams;
        }


        private Dictionary<string, string> getInfOutputImages(int number, string typeMode)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string[] pathsToImages = new string[3];
            pathsToImages = getPathsImages(number, typeMode);

            result.Add("pathImgVolume", pathsToImages[0]);
            result.Add("pathImgTimeVolume", pathsToImages[1]);
            result.Add("pathParameters", pathsToImages[2]);
            result.Add("textLabelParams", getTextParams(number, typeMode));

            return result;
        }


        private void ShowPlots_Click(object sender, RoutedEventArgs e)
        {
            numberPatientOutputPlotFindMin = 1;
            numberPatientOutputPlotOne = 0;
            Dictionary<string, string> paths;

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
                paths = getInfOutputImages(numberPatientOutputPlotFindMin, typeMode: "PersonalPatients");
            }
            else
            {
                paths = getInfOutputImages(numberPatientOutputPlotOne, typeMode: "Any");
            }
            Dictionary<string, float> cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotFindMin, paths["pathParameters"]);
            
            // Output Parameters
            outputParameters(cancerParameters, paths["textLabelParams"]);

            // Output images (plots)
            outputImages(paths["pathImgVolume"], paths["pathImgTimeVolume"]);
        }


        private void ButtonGoBackImg_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, float> cancerParameters;
            Dictionary<string, string> paths;

            if (RadioButtonFindMin.IsChecked == true)
            {
                numberPatientOutputPlotFindMin--;
                if (numberPatientOutputPlotFindMin < 1)
                {
                    numberPatientOutputPlotFindMin = 10;
                }
                paths = getInfOutputImages(numberPatientOutputPlotFindMin, typeMode: "PersonalPatients");
                cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotFindMin, paths["pathParameters"]);

            }
            else
            {
                numberPatientOutputPlotOne--;
                if (numberPatientOutputPlotOne < 0)
                {
                    numberPatientOutputPlotOne = 10;
                }
                paths = getInfOutputImages(numberPatientOutputPlotOne, typeMode: "Any");
                cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotOne, paths["pathParameters"]);

            }
            // Output Parameters
            outputParameters(cancerParameters, paths["textLabelParams"]);

            // Output images (plots)
            outputImages(paths["pathImgVolume"] , paths["pathImgTimeVolume"]);
        }


        private void ButtonGoNextImg_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, float> cancerParameters;
            Dictionary<string, string> paths;

            if (RadioButtonFindMin.IsChecked == true)
            {
                numberPatientOutputPlotFindMin++;
                if (numberPatientOutputPlotFindMin > 10)
                {
                    numberPatientOutputPlotFindMin = 1;
                }
                paths = getInfOutputImages(numberPatientOutputPlotFindMin, typeMode: "PersonalPatients");
                cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotFindMin, paths["pathParameters"]);
            }
            else
            {
                numberPatientOutputPlotOne++;
                if (numberPatientOutputPlotOne > 10)
                {
                    numberPatientOutputPlotOne = 1;
                }
                paths = getInfOutputImages(numberPatientOutputPlotOne, typeMode: "Any");
                cancerParameters = ActionDataFile.getParametersFromFile(type, numberPatientOutputPlotOne, paths["pathParameters"]);
            }
            // Output Parameters
            outputParameters(cancerParameters, paths["textLabelParams"]);

            // Output images (plots)
            outputImages(paths["pathImgVolume"], paths["pathImgTimeVolume"]);
        }


        private void RadioButtonFindMin_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Patient Number Plot";
        }


        private void RadioButtonWithoutFindMin_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Input number patient to save plots\nOutput it";
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
            string typeMode = "";
            worker.ReportProgress(0);
            string scriptPython;
            if (paramsForSavePlot.radioButtonChecked == true)
            {
                typeMode = "PersonalPatients";
                scriptPython = @"CancerVolumePlot.py";

            }
            else
            {
                typeMode = "Any";
                scriptPython = @"OneCancerVolumePlot.py";
                /*if (System.IO.File.Exists(String.Format(@"..\..\..\dataTumor\PredictData\{0}\{1}\img\{2}{3}.png", typeMode, type, paramsForSavePlot.numberPatientSavePlot, type)))
                    System.IO.File.Delete("c:\\t.jpg");*/
            }
            String pathAssembley = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Debug.WriteLine(String.Format("\npathAssembley - {0}", pathAssembley));
            // Create Process start info
            var psi = new ProcessStartInfo();

            // Provide Script
            string pathPythonInterpreter = paramsForSavePlot.pathPythonInterpreter.Trim();
            // checking current path to python interpreter
            if (pathPythonInterpreter == "Please, input your path to python interpreter" || pathPythonInterpreter == "")
            {
                pathPythonInterpreter = @"env\Scripts\python.exe";
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
            // Copy data directory
            //File.Copy(@"C:\dir1\1.txt", @"C:\dir2\2.txt", true);

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
            Image1.Source = null;
            Image2.Source = null;
            string type = @"Volume";
            string pathImg = @"dataTumor\PredictData\Total\" + type + @"\img\All.png";

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
