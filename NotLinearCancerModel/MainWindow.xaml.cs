using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;

namespace NotLinearCancerModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void ShowPlots_Click(object sender, RoutedEventArgs e)
        {
            int patientNumber = 1;
            string pathImg1;
            string pathImg2;
            string pathParameters;
            string type = "Volume";
            if (RadioButtonFindMin.IsChecked == true)
            {
                try
                {
                    patientNumber = int.Parse(TextBoxPatientNumberPlot.Text, CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show($"Please, input correct data (number patient)!\n{ex}");
                }
                pathImg1 = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\img\" + patientNumber.ToString() + type + @".png";
                pathImg2 = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\timeValue\img\" + patientNumber.ToString() + type + @".png";
                pathParameters = @"..\..\..\dataTumor\PredictData\PersonalPatients\" + type + @"\txt\params\" + patientNumber.ToString() + "Params.txt";
            }
            else
            {
                pathImg1 = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\img\" + patientNumber.ToString() + type + @".png";
                pathImg2 = @"..\..\..\dataTumor\PredictData\Any\" + type + @"\timeValue\img\" + patientNumber.ToString() + type + @".png";
                pathParameters = @"..\dataTumor\PredictData\Any\" + type + @"\txt\params\" + patientNumber.ToString() + "Params.txt";
            }
            string textLabelParams = "Cancer Parameters:\n";
            Dictionary<string, float> cancerParameters = ActionDataFile.getParametersFromFile(type, patientNumber, pathParameters);
            
            foreach (var keyValueCancer in cancerParameters)
            {
                textLabelParams += (keyValueCancer.Key + "\t");
                textLabelParams += (keyValueCancer.Value.ToString() + "\n");
            }
            textBoxCancerParameters.Text = textLabelParams;
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


        private void SavePlots_Click(object sender, RoutedEventArgs e)
        {
            string scriptPython;
            if (RadioButtonFindMin.IsChecked == true)
            {
                scriptPython = @"CancerVolumePlot.py";
            }
            else
            {
                scriptPython = @"OneCancerVolumePlot.py";
            }
            // Create Process start info
            var psi = new ProcessStartInfo();

            string pathPython = TextBoxPythonInterpreter.Text.ToString().Trim();
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
            MessageBox.Show("Success save img!");
        }
    }
}
