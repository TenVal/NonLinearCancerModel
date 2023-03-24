using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Cache;

namespace NotLinearCancerModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _numberPatientOutputPlotFindMin;
        private int numberPatientOutputPlotOne;
        private string type = "Volume";

        public class ParamsForSavePlot
        {
            private int _radioButtonChecked;
            private string _pathPythonInterpreter;
            private int _numberPatientSavePlot;

            protected internal int RadioButtonChecked
            {
                get { return _radioButtonChecked; }
            }

            protected internal string PathPythonInterpreter
            {
                get { return _pathPythonInterpreter; }
            }

            protected internal int NumberPatientSavePlot
            {
                get { return _numberPatientSavePlot; }
            }

            public ParamsForSavePlot(
                int radioButtonChecked,
                string pathPythonInterpreter,
                int numberPatientSavePlot)
            {
                this._radioButtonChecked = radioButtonChecked;
                this._pathPythonInterpreter = pathPythonInterpreter;
                this._numberPatientSavePlot = numberPatientSavePlot;
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

            // This path is a file
            File.Delete(pathToActionDataFile);
            File.Copy(pathFrom + pathToActionDataFile, pathToActionDataFile);
            // This path is a file
            File.Delete(pathToCancerVolumePlot);
            File.Copy(pathFrom + pathToCancerVolumePlot, pathToCancerVolumePlot);
            // This path is a file
            File.Delete(pathToOneCancerVolumePlot);
            File.Copy(pathFrom + pathToOneCancerVolumePlot, pathToOneCancerVolumePlot);
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
            bmpVolume.CacheOption = BitmapCacheOption.None;
            bmpVolume.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            bmpVolume.CacheOption = BitmapCacheOption.OnLoad;
            bmpVolume.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmpVolume.UriSource = new Uri(pathImgVolume, UriKind.Relative);
            bmpVolume.EndInit();

            BitmapImage bmpTimeVolume = new BitmapImage();
            bmpTimeVolume.BeginInit();
            bmpTimeVolume.CacheOption = BitmapCacheOption.None;
            bmpTimeVolume.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            bmpTimeVolume.CacheOption = BitmapCacheOption.OnLoad;
            bmpTimeVolume.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
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
            _numberPatientOutputPlotFindMin = 1;
            numberPatientOutputPlotOne = 0;
            Dictionary<string, string> paths;

            if (RadioButtonFindMin.IsChecked == true || RadioButtonTemperatureFunction.IsChecked == true)
            {
                try
                {
                    _numberPatientOutputPlotFindMin = int.Parse(TextBoxPatientNumberPlot.Text, CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show($"Please, input correct data (number patient)!\n{ex}");
                }
                paths = getInfOutputImages(_numberPatientOutputPlotFindMin, typeMode: "PersonalPatients");
            }
            else
            {
                paths = getInfOutputImages(numberPatientOutputPlotOne, typeMode: "Any");
            }
            if (RadioButtonTemperatureFunction.IsChecked == true)
            {
                string typeTemperature = "Temperature";
                paths["pathImgVolume"] = String.Format(@"dataTumor\PredictData\{0}\{1}\timeValue\img\{2}{3}.png", "PersonalPatients", type, _numberPatientOutputPlotFindMin, typeTemperature);
            }
            Dictionary<string, float> cancerParameters = ActionDataFile.getParametersFromFile(type, _numberPatientOutputPlotFindMin, paths["pathParameters"]);
            
            // Output Parameters
            outputParameters(cancerParameters, paths["textLabelParams"]);

            // Output images (plots)
            outputImages(paths["pathImgVolume"], paths["pathImgTimeVolume"]);
        }


        private void ButtonGoBackImg_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, float> cancerParameters;
            Dictionary<string, string> paths;

            if (RadioButtonFindMin.IsChecked == true || RadioButtonTemperatureFunction.IsChecked == true)
            {
                _numberPatientOutputPlotFindMin--;
                if (_numberPatientOutputPlotFindMin < 1)
                {
                    _numberPatientOutputPlotFindMin = 10;
                }
                paths = getInfOutputImages(_numberPatientOutputPlotFindMin, typeMode: "PersonalPatients");
                cancerParameters = ActionDataFile.getParametersFromFile(type, _numberPatientOutputPlotFindMin, paths["pathParameters"]);

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
            if (RadioButtonTemperatureFunction.IsChecked == true)
            {
                string typeTemperature = "Temperature";
                paths["pathImgVolume"] = String.Format(@"dataTumor\PredictData\{0}\{1}\timeValue\img\{2}{3}.png", "PersonalPatients", type, _numberPatientOutputPlotFindMin, typeTemperature);
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

            if (RadioButtonFindMin.IsChecked == true || RadioButtonTemperatureFunction.IsChecked == true)
            {
                _numberPatientOutputPlotFindMin++;
                if (_numberPatientOutputPlotFindMin > 10)
                {
                    _numberPatientOutputPlotFindMin = 1;
                }
                paths = getInfOutputImages(_numberPatientOutputPlotFindMin, typeMode: "PersonalPatients");
                cancerParameters = ActionDataFile.getParametersFromFile(type, _numberPatientOutputPlotFindMin, paths["pathParameters"]);
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
            if (RadioButtonTemperatureFunction.IsChecked == true)
            {
                string typeTemperature = "Temperature";
                paths["pathImgVolume"] = String.Format(@"dataTumor\PredictData\{0}\{1}\timeValue\img\{2}{3}.png", "PersonalPatients", type, _numberPatientOutputPlotFindMin, typeTemperature);
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
            Image1.Visibility = Visibility.Visible;
            ImageBrain1.Visibility = Visibility.Collapsed;
            ImageBrain2.Visibility = Visibility.Collapsed;
            ImageBrain3.Visibility = Visibility.Collapsed;
            ImageBrain1.Visibility = Visibility.Collapsed;
        }
        

        private void RadioButtonLinearModel_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Input number patient to save plots\nOutput it";
            Image1.Visibility = Visibility.Collapsed;
            ImageBrain1.Visibility = Visibility.Visible;
            ImageBrain2.Visibility = Visibility.Visible;
            ImageBrain3.Visibility = Visibility.Visible;
            ImageBrain1.Visibility = Visibility.Visible;
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
            int radioButtonChecked = 0;
            if (RadioButtonWithoutFindMin.IsChecked == true)
            {
                radioButtonChecked = 1;
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
            else if(RadioButtonFindMin.IsChecked == true)
            {
                radioButtonChecked = 0;
            }
            else if(RadioButtonTemperatureFunction.IsChecked == true)
            {
                radioButtonChecked = 2;
            }
            paramsForSavePlot = new ParamsForSavePlot(
                radioButtonChecked,
                TextBoxPythonInterpreter.Text.ToString().Trim(),
                numberPatientSavePlot);
            worker.RunWorkerAsync(paramsForSavePlot);
        }


        private void worker_SavePlots(object sender, DoWorkEventArgs e)
        {
            ParamsForSavePlot paramsForSavePlot = (ParamsForSavePlot)e.Argument;
            var worker = sender as BackgroundWorker;
            string modeSaveImage = "";
            worker.ReportProgress(0);
            string scriptPython = "";
            if (paramsForSavePlot.RadioButtonChecked == 0)
            {
                modeSaveImage = "PersonalPatients";
                scriptPython = @"CancerVolumePlot.py";

            }
            else if(paramsForSavePlot.RadioButtonChecked == 1)
            {
                modeSaveImage = "Any";
                scriptPython = @"OneCancerVolumePlot.py";
            }
            else if(paramsForSavePlot.RadioButtonChecked == 2)
            {
                modeSaveImage = "PersonalPatients";
                scriptPython = @"TimeTemperaturePlot.py";
            }
            // Create Process start info
            var psi = new ProcessStartInfo();

            // Provide Script
            string pathPythonInterpreter = paramsForSavePlot.PathPythonInterpreter.Trim();
            // checking current path to python interpreter
            if (pathPythonInterpreter == "Please, input your path to python interpreter" || pathPythonInterpreter == "")
            {
                pathPythonInterpreter = @"env\Scripts\python.exe";
            }
            worker.ReportProgress(20);
            psi.FileName = pathPythonInterpreter;
            
            //Provide Arguments
            var numberPatientToSavePlot = paramsForSavePlot.NumberPatientSavePlot.ToString();

            psi.Arguments = $"\"{scriptPython}\" \"{numberPatientToSavePlot}\" \"{type}\" \"{modeSaveImage}\"";

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
            SolidColorBrush brushForUnpressedButton = new SolidColorBrush(Colors.White);
            SavePlots.Foreground = brushForUnpressedButton;
            LabelPatientNumberPlot.Content = "Patient number plot";
        }


        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SavePlots.Content = "Saving...";
        }


        private void ButtonShowTotal_Click(object sender, RoutedEventArgs e)
        {
            string type = @"Volume";
            string pathImg = @"dataTumor\PredictData\Total\" + type + @"\img\All.png";

            textBoxCancerParameters.Text = "";

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
            bmpVolume.CacheOption = BitmapCacheOption.None;
            bmpVolume.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            bmpVolume.CacheOption = BitmapCacheOption.OnLoad;
            bmpVolume.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmpVolume.UriSource = new Uri(pathImg, UriKind.Relative);
            bmpVolume.EndInit();

            Image1.Stretch = Stretch.Fill;
            Image1.Source = bmpVolume;
            Image2.Stretch = Stretch.Fill;
            Image2.Source = null;
        }
    }
}
