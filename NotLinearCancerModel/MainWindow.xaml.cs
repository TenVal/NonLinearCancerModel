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
using System.Windows.Shapes;
using System.Windows.Controls;

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
            string pathToDataTumor = @"dataTumor";
            string pathToAssets = @"Assets";
            string pathToActionDataFile = @"ActionDataFile.py";
            string pathToCancerVolumePlot = @"CancerVolumePlot.py";
            string pathToOneCancerVolumePlot = @"OneCancerVolumePlot.py";
            string pathToCancerVolumeDiffPlot = @"CancerVolumeDiffPlot.py";
            string pathToTimeTemperaturePlot = @"TimeTemperaturePlot.py";
            
            if (Directory.Exists(pathToEnv) == false)
            {
                // This path is a directory
                ActionDataFile.copyDir(pathFrom + pathToEnv, pathToEnv);
            }
            if (Directory.Exists(pathToDataTumor) == false)
            {
                // This path is a directory
                ActionDataFile.copyDir(pathFrom + pathToDataTumor, pathToDataTumor);
            }
            if (Directory.Exists(pathToDataTumor) == false)
            {
                // This path is a directory
                ActionDataFile.copyDir(pathFrom + pathToAssets, pathToAssets);
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
            // This path is a file
            File.Delete(pathToCancerVolumeDiffPlot);
            File.Copy(pathFrom + pathToCancerVolumeDiffPlot, pathToCancerVolumeDiffPlot);
            // This path is a file
            File.Delete(pathToCancerVolumeDiffPlot);
            File.Copy(pathFrom + pathToTimeTemperaturePlot, pathToTimeTemperaturePlot);
        }


        private int outputImage(System.Windows.Controls.Image Image, string path)
        {
            Image.Source = null;
            Image.InvalidateMeasure();
            Image.InvalidateArrange();

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.None;
            bmp.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmp.UriSource = new Uri(path, UriKind.Relative);
            bmp.EndInit();

            Image.Stretch = Stretch.Fill;
            Image.Source = bmp;
            return 0;
        }


        private int outputImages(string pathImgVolume, string pathImgTimeVolume)
        {
            int res1 = outputImage(Image1, pathImgVolume);
            int res2 = outputImage(Image2, pathImgTimeVolume);
            return res1 + res2;
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


        private string[] getPathsImages(int number, string typeMode, string type="Volume")
        {
            string[] pathsToImages = new string[3];
            pathsToImages[0] = String.Format(@"dataTumor\PredictData\{0}\{1}\img\{2}{3}.png", typeMode, this.type, number, type);
            pathsToImages[1] = String.Format(@"dataTumor\PredictData\{0}\{1}\timeValue\img\{2}{3}.png", typeMode, this.type, number, type);
            pathsToImages[2] = String.Format(@"dataTumor\PredictData\{0}\{1}\txt\params\{2}Params.txt", typeMode, this.type, number);
            return pathsToImages;
        }


        private string getTextParams(int number, string typeMode, string type = "Params")
        {
            string textLabelParams = String.Format("Cancer\tParameters {0}:\n", number);
            if (typeMode == "Any")
            {
                textLabelParams = String.Format("{0}Cancer\tParameters {1}:\n", typeMode, number);
            }
            return textLabelParams;
        }


        private Dictionary<string, string> getInfOutputImages(int number, string typeMode, string type = "Volume")
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string[] pathsToImages = new string[3];
            pathsToImages = getPathsImages(number, typeMode, type: type);

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
            if (RadioButtonLinearModel.IsChecked == true)
            {
                paths = getInfOutputImages(_numberPatientOutputPlotFindMin, typeMode: "PersonalPatients", type: "VolumeLin");
                outputImage(Image2, paths["pathImgTimeVolume"]); 
                paths["pathParameters"] = String.Format(@"dataTumor\PredictData\{0}\{1}\txt\params\{2}ParamsLinear.txt", "PersonalPatients", this.type, _numberPatientOutputPlotFindMin);
            }
            else
            {
                // Output images (plots)
                outputImages(paths["pathImgVolume"], paths["pathImgTimeVolume"]);
            }
            // Output Parameters
            Dictionary<string, float> cancerParameters = ActionDataFile.getParametersFromFile(paths["pathParameters"]);
            outputParameters(cancerParameters, paths["textLabelParams"]);
        }


        private void ButtonGoBackImg_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, float> cancerParameters;
            Dictionary<string, string> paths;

            if (RadioButtonFindMin.IsChecked == true || RadioButtonTemperatureFunction.IsChecked == true || RadioButtonLinearModel.IsChecked == true)
            {
                _numberPatientOutputPlotFindMin--;
                if (_numberPatientOutputPlotFindMin < 1)
                {
                    _numberPatientOutputPlotFindMin = 10;
                }
                paths = getInfOutputImages(_numberPatientOutputPlotFindMin, typeMode: "PersonalPatients");
            }
            else
            {
                numberPatientOutputPlotOne--;
                if (numberPatientOutputPlotOne < 0)
                {
                    numberPatientOutputPlotOne = 10;
                }
                paths = getInfOutputImages(numberPatientOutputPlotOne, typeMode: "Any");

            }
            if (RadioButtonTemperatureFunction.IsChecked == true)
            {
                string typeTemperature = "Temperature";
                paths["pathImgVolume"] = String.Format(@"dataTumor\PredictData\{0}\{1}\timeValue\img\{2}{3}.png", "PersonalPatients", type, _numberPatientOutputPlotFindMin, typeTemperature);
            }
            if (RadioButtonLinearModel.IsChecked == true)
            {
                paths = getInfOutputImages(_numberPatientOutputPlotFindMin, typeMode: "PersonalPatients", type: "VolumeLin");
                outputImage(Image2, paths["pathImgTimeVolume"]);
                paths["pathParameters"] = String.Format(@"dataTumor\PredictData\{0}\{1}\txt\params\{2}ParamsLinear.txt", "PersonalPatients", this.type, _numberPatientOutputPlotFindMin);
            }
            else
            {
                // Output images (plots)
                outputImages(paths["pathImgVolume"], paths["pathImgTimeVolume"]);
            }
            // Output Parameters
            cancerParameters = ActionDataFile.getParametersFromFile(paths["pathParameters"]);
            outputParameters(cancerParameters, paths["textLabelParams"]);
        }


        private void ButtonGoNextImg_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, float> cancerParameters;
            Dictionary<string, string> paths;

            if (RadioButtonFindMin.IsChecked == true || RadioButtonTemperatureFunction.IsChecked == true || RadioButtonLinearModel.IsChecked == true)
            {
                _numberPatientOutputPlotFindMin++;
                if (_numberPatientOutputPlotFindMin > 10)
                {
                    _numberPatientOutputPlotFindMin = 1;
                }
                paths = getInfOutputImages(_numberPatientOutputPlotFindMin, typeMode: "PersonalPatients");
            }
            else
            {
                numberPatientOutputPlotOne++;
                if (numberPatientOutputPlotOne > 10)
                {
                    numberPatientOutputPlotOne = 1;
                }
                paths = getInfOutputImages(numberPatientOutputPlotOne, typeMode: "Any");
            }
            if (RadioButtonTemperatureFunction.IsChecked == true)
            {
                string typeTemperature = "Temperature";
                paths["pathImgVolume"] = String.Format(@"dataTumor\PredictData\{0}\{1}\timeValue\img\{2}{3}.png", "PersonalPatients", type, _numberPatientOutputPlotFindMin, typeTemperature);
            }
            if(RadioButtonLinearModel.IsChecked == true)
            {
                paths = getInfOutputImages(_numberPatientOutputPlotFindMin, typeMode: "PersonalPatients", type: "VolumeLin");
                Debug.WriteLine(paths["pathImgTimeVolume"]);
                outputImage(Image2, paths["pathImgTimeVolume"]); 
                paths["pathParameters"] = String.Format(@"dataTumor\PredictData\{0}\{1}\txt\params\{2}ParamsLinear.txt", "PersonalPatients", this.type, _numberPatientOutputPlotFindMin);
            }
            else
            {
                // Output images (plots)
                outputImages(paths["pathImgVolume"], paths["pathImgTimeVolume"]);
            }
            // Output Parameters
            cancerParameters = ActionDataFile.getParametersFromFile(paths["pathParameters"]);
            outputParameters(cancerParameters, paths["textLabelParams"]);
        }


        private void changeVisibleOfElements(Visibility visibilityBrain, Visibility visibilityNonBrain)
        {
            Image1.Visibility = visibilityNonBrain;

            ImageBrainLinear1.Visibility = visibilityBrain;
            ImageBrainNonLinear1.Visibility = visibilityBrain;
            ImageBrainLinear2.Visibility = visibilityBrain;
            ImageBrainNonLinear2.Visibility = visibilityBrain;

            SliderTime1.Visibility = visibilityBrain;
            LabelSliderTime1.Visibility = visibilityBrain;
            LabelTimeEnd1.Visibility = visibilityBrain;
            LabelTimeStart1.Visibility = visibilityBrain;

            SliderTime2.Visibility = visibilityBrain;
            LabelSliderTime2.Visibility = visibilityBrain;
            LabelTimeEnd2.Visibility = visibilityBrain;
            LabelTimeStart2.Visibility = visibilityBrain;

            CircleBrainLinear1.Visibility = visibilityBrain;
            CircleBrainNonLinear1.Visibility = visibilityBrain;
            CircleBrainLinear2.Visibility = visibilityBrain;
            CircleBrainNonLinear2.Visibility = visibilityBrain;
        }


        private void RadioButtonFindMin_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Patient Number Plot";
            changeVisibleOfElements(Visibility.Collapsed, Visibility.Visible);

        }


        private void RadioButtonTemperatureFunction_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Input number patient to save plots\nOutput it";
            changeVisibleOfElements(Visibility.Collapsed, Visibility.Visible);
        }


        private void RadioButtonWithoutFindMin_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Input number patient to save plots\nOutput it";
            changeVisibleOfElements(Visibility.Collapsed, Visibility.Visible);
        }


        private void RadioButtonLinearModel_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Input number patient to save plots\nOutput it";

            outputImage(ImageBrainNonLinear1, @"Assets\brain.jpg");
            outputImage(ImageBrainLinear1, @"Assets\brain.jpg");
            outputImage(ImageBrainNonLinear2, @"Assets\brain.jpg");
            outputImage(ImageBrainLinear2, @"Assets\brain.jpg");

            changeVisibleOfElements(Visibility.Visible, Visibility.Collapsed);
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
            else if(RadioButtonLinearModel.IsChecked == true)
            {
                radioButtonChecked = 3;
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
            else if(paramsForSavePlot.RadioButtonChecked == 3)
            {
                modeSaveImage = "PersonalPatients";
                scriptPython = @"CancerVolumeDiffPlot.py";
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

            outputImage(Image1, pathImg);

            Image2.Source = null;
            Image2.InvalidateMeasure();
            Image2.InvalidateArrange();
            Image2.InvalidateVisual();
            Image2.UpdateLayout();
            
            Image2.Stretch = Stretch.Fill;
            Image2.Source = null;
        }


        private float[] getRadiusValuesSlider()
        {
            float[] borders = new float[2];
            return borders;
        }

        private float[] getBorderValuesSlider()
        {
            float[] borders = new float[2];
            return borders;
        }


        private void changeEllipse(Ellipse ellipse, StackPanel stackPanel, float radius)
        {
            ellipse.Width = radius * 2;
            ellipse.Height = radius * 2;
            int marginTop = (int)(stackPanel.Height / 2 - radius);
            int marginLeft = (int)(stackPanel.Width / 2 - radius);
            ellipse.Margin = new Thickness(marginLeft, marginTop, 0, 0);
        }


        private void SliderTime1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            changeEllipse(CircleBrainLinear1, StackPanelBrainLin1, (float)e.NewValue);
            changeEllipse(CircleBrainNonLinear1, StackPanelBrainNonLin1, (float)e.NewValue);
            LabelSliderTime1.Content = $"Time {(int)e.NewValue}";
        }


        private void SliderTime2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            changeEllipse(CircleBrainLinear2, StackPanelBrainLin2, (float)e.NewValue);
            changeEllipse(CircleBrainNonLinear2, StackPanelBrainNonLin2, (float)e.NewValue);
            LabelSliderTime2.Content = $"Time {(int)e.NewValue}";
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int startValueSlider = 20;
            changeEllipse(CircleBrainLinear1, StackPanelBrainLin1, startValueSlider);
            changeEllipse(CircleBrainNonLinear1, StackPanelBrainNonLin1, startValueSlider);
            changeEllipse(CircleBrainLinear2, StackPanelBrainLin1, startValueSlider);
            changeEllipse(CircleBrainNonLinear2, StackPanelBrainNonLin1, startValueSlider);
            SliderTime1.Value = startValueSlider;
            SliderTime2.Value = startValueSlider;
            LabelSliderTime1.Content = $"Time {SliderTime1.Value}";
            LabelSliderTime2.Content = $"Time {SliderTime2.Value}";

            changeVisibleOfElements(Visibility.Collapsed, Visibility.Visible);
        }
    }
}
