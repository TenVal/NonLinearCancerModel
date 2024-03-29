﻿using System;
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
using System.Drawing;
using Color = System.Windows.Media.Color;

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


        private float scaleX;
        private float scaleY;
        private List<List<float>> linData;
        private List<List<float>> nonLinData;
        

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
            if (Directory.Exists(pathToAssets) == false)
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
            File.Delete(pathToTimeTemperaturePlot);
            File.Copy(pathFrom + pathToTimeTemperaturePlot, pathToTimeTemperaturePlot);

            this.scaleX = (float)(StackPanelBrainNonLin1.Width / 14);
            this.scaleY = (float)(StackPanelBrainNonLin1.Height / 17);
            this.numberPatientOutputPlotOne = 1;
            this._numberPatientOutputPlotFindMin = 1;
            try
            {
                changeDataPatientSlider();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error!\n{ex}");
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int startValueSlider = 10;
            //changeEllipse(CircleBrainLinear1, StackPanelBrainLin1, startValueSlider);
            //changeEllipse(CircleBrainNonLinear1, StackPanelBrainNonLin1, startValueSlider);
            //changeEllipse(CircleBrainLinear2, StackPanelBrainLin1, startValueSlider);
            //changeEllipse(CircleBrainNonLinear2, StackPanelBrainNonLin1, startValueSlider);
            string path = @"Assets\brain.jpg";
            Bitmap brainBmp = new Bitmap(path, true);
            
            CircleBrainNonLinear1.Plot.Style(dataBackgroundImage: brainBmp, figBg: Color.FromRgb(222, 27, 27));
            CircleBrainNonLinear1.Plot.XLabel("cm");
            CircleBrainNonLinear1.Plot.YLabel("cm");
            CircleBrainNonLinear1.Plot.Title($"{_numberPatientOutputPlotFindMin} Patient");
            CircleBrainNonLinear1.Plot.SetAxisLimits(xMin: -7, xMax: 7, yMin: -8.5, yMax: 8.5);

            CircleBrainLinear1.Plot.Style(dataBackgroundImage: brainBmp);
            CircleBrainLinear1.Plot.XLabel("cm");
            CircleBrainLinear1.Plot.YLabel("cm");
            CircleBrainLinear1.Plot.Title($"{_numberPatientOutputPlotFindMin} Patient");
            CircleBrainLinear1.Plot.SetAxisLimits(xMin: -7, xMax: 7, yMin: -8.5, yMax: 8.5);

            CircleBrainNonLinear2.Plot.Style(dataBackgroundImage: brainBmp);
            CircleBrainNonLinear2.Plot.XLabel("cm");
            CircleBrainNonLinear2.Plot.YLabel("cm");
            CircleBrainNonLinear2.Plot.Title($"{_numberPatientOutputPlotFindMin} Patient");
            CircleBrainNonLinear2.Plot.SetAxisLimits(xMin: -7, xMax: 7, yMin: -8.5, yMax: 8.5);

            CircleBrainLinear2.Plot.Style(dataBackgroundImage: brainBmp);
            CircleBrainLinear2.Plot.XLabel("cm");
            CircleBrainLinear2.Plot.YLabel("cm");
            CircleBrainLinear2.Plot.Title($"{_numberPatientOutputPlotFindMin} Patient");
            CircleBrainLinear2.Plot.SetAxisLimits(xMin: -7, xMax: 7, yMin: -8.5, yMax: 8.5);

            CircleBrainLinear1.Refresh();
            CircleBrainNonLinear1.Refresh();

            CircleBrainLinear2.Refresh();
            CircleBrainNonLinear2.Refresh();

            SliderTime1.Value = startValueSlider;
            SliderTime2.Value = startValueSlider;
            LabelSliderTime1.Content = $"Time {SliderTime1.Value}";
            LabelSliderTime2.Content = $"Time {SliderTime2.Value}";

            changeVisibleOfElements(Visibility.Collapsed, Visibility.Visible);
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
                changeDataPatientSlider();
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
                changeDataPatientSlider();
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
                outputImage(Image2, paths["pathImgTimeVolume"]); 
                paths["pathParameters"] = String.Format(@"dataTumor\PredictData\{0}\{1}\txt\params\{2}ParamsLinear.txt", "PersonalPatients", this.type, _numberPatientOutputPlotFindMin);
                changeDataPatientSlider();
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

            /*ImageBrainLinear1.Visibility = visibilityBrain;
            ImageBrainNonLinear1.Visibility = visibilityBrain;
            ImageBrainLinear2.Visibility = visibilityBrain;
            ImageBrainNonLinear2.Visibility = visibilityBrain;*/

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

            LabelBrainImageLinear.Visibility = visibilityBrain;
            LabelBrainImageNonLinear.Visibility = visibilityBrain;

            LabelBrainY.Visibility = visibilityBrain;
            TextBoxBrainY.Visibility = visibilityBrain;
            LabelBrainX.Visibility = visibilityBrain;
            TextBoxBrainX.Visibility = visibilityBrain;
        }


        private void RadioButtonFindMin_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Patient Number Plot";
            changeVisibleOfElements(Visibility.Collapsed, Visibility.Visible);

        }


        private void RadioButtonTemperatureFunction_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Patient Number Plot";
            changeVisibleOfElements(Visibility.Collapsed, Visibility.Visible);
        }


        private void RadioButtonWithoutFindMin_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Input number patient to save plots\nOutput it";
            changeVisibleOfElements(Visibility.Collapsed, Visibility.Visible);
        }


        private void RadioButtonLinearModel_Checked(object sender, RoutedEventArgs e)
        {
            LabelPatientNumberPlot.Content = "Patient Number Plot";
            /*outputImage(ImageBrainNonLinear1, @"Assets\brain.jpg");
            outputImage(ImageBrainLinear1, @"Assets\brain.jpg");
            outputImage(ImageBrainNonLinear2, @"Assets\brain.jpg");
            outputImage(ImageBrainLinear2, @"Assets\brain.jpg");*/
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
            Debug.WriteLine("pathPythonInterpreter\t" + pathPythonInterpreter);
            // checking current path to python interpreter
            if (pathPythonInterpreter == "Please, input your path to python interpreter" || pathPythonInterpreter == "")
            {
                pathPythonInterpreter = @"env\Scripts\python.exe";
            }
            Debug.WriteLine("pathPythonInterpreter\t" + pathPythonInterpreter);

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
            // Display output
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


        

        private List<List<float>> getPolygonShapeOfCancerSlider()
        {
            List<List<float>> xy = new List<List<float>>();
            string pathToFile = @"dataTumor\PredictData\PersonalPatients\Volume\txt\" + _numberPatientOutputPlotFindMin.ToString() + "Volume.txt";

            List<List<float>> xyz = ActionDataFile.getDataXYZFromFile(pathToFile);

            for (int i = 0; i < xyz[0].Count; i++)
            {
                for(int j = 0; j < xyz[1].Count; j++)
                {
                    
                }
            }
            return xy;

        }



        private float getValueForCircleSlider(List<List<float>> data, float valueSlider)
        {
            float radius = 0;
            float time = 0;
            float delta = valueSlider;
            foreach (var val in data[0])
            {
                if (Math.Abs(valueSlider - val) < delta)
                {
                    delta = Math.Abs(valueSlider - val);
                    time = val;
                }
            }
            int index = data[0].IndexOf(time);
            try
            {
                radius = (float)(Math.Cbrt(data[1][index]) / Math.PI);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error!\n{ex}");
            }
            
            return radius;
        }


        private float getValueNonLinearSlider()
        {
            float result = 0;

            return result;
        }


        private float[] getBorderValuesSlider(List<List<float>> Data)
        {
            float[] borders = new float[2];
            borders[0] = Data[0][0];
            borders[1] = Data[0][Data[0].Count-1];
            return borders;
        }


        private void changeEllipse(Ellipse ellipse, StackPanel stackPanel, float radius)
        {
            float brainX = 0;
            float brainY = 0;
            int marginTop = 0;
            int marginLeft = 0;
            if (float.TryParse(TextBoxBrainX.Text, out brainX))
            {
            }
            else
            {
                MessageBox.Show($"Error!\n{brainX}"); // converted value
            }
            if (float.TryParse(TextBoxBrainY.Text, out brainY))
            {
            }
            else
            {
                MessageBox.Show($"Error!\n{brainY}"); // converted value
            }

            marginTop = (int)(stackPanel.Height / 2 - radius + brainY);
            marginLeft = (int)(stackPanel.Width / 2 - radius + brainX);
            if ((marginTop + 2 * radius) > (StackPanelBrainNonLin1.Width) || (marginLeft + 2 * radius) > (StackPanelBrainNonLin1.Height)
                || brainX < 0 || brainY < 0)
            {
                MessageBox.Show($"Error!\nInput other coordinates x = {brainX}, y = {brainY}");
            }
            else
            {
                ellipse.Width = radius * 2;
                ellipse.Height = radius * 2;
                ellipse.Margin = new Thickness(marginLeft, marginTop, 0, 0);
            }
        }


        private void changeDataPatientSlider()
        {
            string pathLinData = @"dataTumor\PredictData\PersonalPatients\Volume\timeValue\txt\" + _numberPatientOutputPlotFindMin.ToString() + "VolumeLin.txt";
            string pathNonLinData = @"dataTumor\PredictData\PersonalPatients\Volume\timeValue\txt\" + _numberPatientOutputPlotFindMin.ToString() + "Volume.txt";

            this.linData = ActionDataFile.getDataFromFile(pathLinData);
            this.nonLinData = ActionDataFile.getDataFromFile(pathNonLinData);

            float[] borders = getBorderValuesSlider(linData);

            SliderTime1.Minimum = borders[0];
            SliderTime1.Maximum = borders[1];
            LabelTimeStart1.Content = borders[0].ToString();
            LabelTimeEnd1.Content = borders[1].ToString();

            SliderTime2.Minimum = borders[0];
            SliderTime2.Maximum = borders[1];
            LabelTimeStart2.Content = borders[0].ToString();
            LabelTimeEnd2.Content = borders[1].ToString();
        }

        private float[] getXYBrain()
        {
            float[] xy = new float[2];
            float brainX = 0;
            float brainY = 0;
            if (float.TryParse(TextBoxBrainX.Text, out brainX))
            {
            }
            else
            {
                MessageBox.Show($"Error!\n{brainX}"); // converted value
            }
            if (float.TryParse(TextBoxBrainY.Text, out brainY))
            {
            }
            else
            {
                MessageBox.Show($"Error!\n{brainY}"); // converted value
            }
            xy[0] = brainX;
            xy[1] = brainY;
            return xy;
        }


        private void SliderTime1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float[] xyBrain = getXYBrain();
            float radiusLin = getValueForCircleSlider(this.linData, (float)e.NewValue);
            float radiusNonLin = getValueForCircleSlider(this.nonLinData, (float)e.NewValue);
            /*radiusLin = (radiusLin * this.scaleX);
            radiusNonLin = (radiusNonLin * this.scaleX);*/
            CircleBrainLinear1.Plot.Clear();
            CircleBrainNonLinear1.Plot.Clear();

            CircleBrainLinear1.Plot.SetAxisLimits(xMin: -7, xMax: 7, yMin: -8.5, yMax: 8.5);
            CircleBrainNonLinear1.Plot.SetAxisLimits(xMin: -7, xMax: 7, yMin: -8.5, yMax: 8.5);

            CircleBrainLinear1.Plot.AddCircle(
                x: xyBrain[0],
                y: xyBrain[1],
                radius: radiusLin,
                lineWidth: 1);
            CircleBrainNonLinear1.Plot.AddCircle(
                x: xyBrain[0],
                y: xyBrain[1],
                radius: radiusNonLin,
                lineWidth: 1);
            CircleBrainLinear1.Refresh();
            CircleBrainNonLinear1.Refresh();
            //changeEllipse(CircleBrainLinear1, StackPanelBrainLin1, radiusLin);
            //changeEllipse(CircleBrainNonLinear1, StackPanelBrainNonLin1, radiusNonLin);
            LabelSliderTime1.Content = $"Time " + ((int)e.NewValue).ToString();
        }


        private void SliderTime2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float[] xyBrain = getXYBrain();
            float radiusLin = getValueForCircleSlider(this.linData, (float)e.NewValue);
            float radiusNonLin = getValueForCircleSlider(this.nonLinData, (float)e.NewValue);

            CircleBrainLinear2.Plot.Clear();
            CircleBrainNonLinear2.Plot.Clear();

            CircleBrainLinear2.Plot.SetAxisLimits(xMin: -7, xMax: 7, yMin: -8.5, yMax: 8.5);
            CircleBrainNonLinear2.Plot.SetAxisLimits(xMin: -7, xMax: 7, yMin: -8.5, yMax: 8.5);

            CircleBrainLinear2.Plot.AddCircle(
                x: xyBrain[0],
                y: xyBrain[1],
                radius: radiusLin,
                lineWidth: 1);
            CircleBrainNonLinear2.Plot.AddCircle(
                x: xyBrain[0],
                y: xyBrain[1],
                radius: radiusNonLin,
                lineWidth: 1);
            CircleBrainLinear2.Refresh();
            CircleBrainNonLinear2.Refresh();
            //changeEllipse(CircleBrainLinear2, StackPanelBrainLin2, radiusLin);
            //changeEllipse(CircleBrainNonLinear2, StackPanelBrainNonLin2, radiusNonLin);
            LabelSliderTime2.Content = $"Time " + ((int)e.NewValue).ToString();
        }
    }
}
