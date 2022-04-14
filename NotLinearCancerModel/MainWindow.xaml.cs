using System;
using System.Collections.Generic;
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
using System.Diagnostics;

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


        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            // get data from files with polynomial regression
            DataCancer modelData = new DataCancer();
            
            // get data from forms of interface
            float length = float.Parse(TextBoxLength.Text, CultureInfo.InvariantCulture);
            float RightX = length;
            float h = float.Parse(TextBoxH.Text, CultureInfo.InvariantCulture);
            float d = float.Parse(TextBoxD.Text, CultureInfo.InvariantCulture);
            float k = float.Parse(TextBoxK.Text, CultureInfo.InvariantCulture);
            float accuracy = float.Parse(TextBoxAccuracy.Text, CultureInfo.InvariantCulture);
            float stepAccuracy = float.Parse(TextBoxStepAccuracy.Text, CultureInfo.InvariantCulture);
            float speed = float.Parse(TextBoxSpeed.Text, CultureInfo.InvariantCulture);
            float angleXY = float.Parse(TextBoxAngleXY.Text, CultureInfo.InvariantCulture);
            float angleZ = float.Parse(TextBoxAngleZ.Text, CultureInfo.InvariantCulture);

            C c = new C(speed, angleXY, angleZ);       
            Q q = new Q(0);
            MethodDiffusion diffusion;
            
            if (RadioButtonWithoutFindMin.IsChecked == true)
            {
                //Storage to keep data about matched value
                Dictionary<string, List<float>> cancerValuesParameters = new Dictionary<string, List<float>>()
            {
                {"Speed" , new List<float>()},
                {"Difference" , new List<float>()}
            };
                List<float[,,]> listAllValuesP = new List<float[,,]>();

                // Counting for everyone patient and find required by min difference between predict data and actual data
                for (int i = 0; i < 10; i++)
                {
                    do
                    {
                        cancerValuesParameters["Speed"].Add(speed);
                        D dF = new D(speed, d);
                        diffusion = new MethodDiffusion(dF, c, q);
                        float tMax = modelData.Patients[i]["Diameter"][0][modelData.Patients[i]["Diameter"][0].Count - 1];

                        var valuesP = diffusion.getValues(tMax, h, k, length);
                        Array.Copy(diffusion.getValues(tMax, h, k, length), valuesP, valuesP.GetLength(0) + valuesP.GetLength(1) + valuesP.GetLength(2));
                        listAllValuesP.Add(valuesP);

                        // find difference between modelData and diffusionModelData
                        cancerValuesParameters["Difference"].Add(Math.Abs(
                            diffusion.NumberPointsVolume[diffusion.NumberPointsVolume.Count - 1] -
                            modelData.Patients[i]["Volume"][1][modelData.Patients[i]["Volume"][1].Count - 1]));
                        speed += stepAccuracy;

                        valuesP = null;
                        dF = null;
                    }
                    while (speed <= accuracy);

                    // Data for time-volume plot
                    float[] numberPointsVolume = new float[diffusion.NumberPointsVolume.Count];
                    Array.Copy(diffusion.NumberPointsVolume.ToArray(), numberPointsVolume, numberPointsVolume.Length);
                    float[] tValues = new float[diffusion.TValues.Count];
                    Array.Copy(diffusion.TValues.ToArray(), tValues, tValues.Length);

                    // Find required speed
                    float minDifference = cancerValuesParameters["Difference"].Min();
                    int indexMinDifference = cancerValuesParameters["Difference"].IndexOf(cancerValuesParameters["Difference"].Min());
                    float requiredSpeed = cancerValuesParameters["Speed"][indexMinDifference];
                    float[,,] requiredValuesP = listAllValuesP[indexMinDifference];

                    // write every data about modeling to files
                    ActionDataFile.writeDataToFile("Volume", i, requiredValuesP);
                    // Write time-value data to file
                    ActionDataFile.writeTimeValueToFile("Volume", i, tValues, numberPointsVolume);
                    // write params of modeling to file
                    float[] paramsForCancer = { speed, d, k };
                    ActionDataFile.writeParametersToFile(type: "Volume", number: i, cancerParameters: paramsForCancer);
                }
            }
            else
            {
                for (int i = 0; i<10; i++)
                {
                    D dF = new D(speed, d);
                    diffusion = new MethodDiffusion(dF, c, q);
                    float tMax = modelData.Patients[i]["Diameter"][0][modelData.Patients[i]["Diameter"][0].Count - 1];
                    var valuesP = diffusion.getValues(tMax, h, k, length);
                    valuesP = null;
                    dF = null;

                    // Data for time-volume plot
                    float[] numberPointsVolume = new float[diffusion.NumberPointsVolume.Count];
                    Array.Copy(diffusion.NumberPointsVolume.ToArray(), numberPointsVolume, numberPointsVolume.Length);
                    float[] tValues = new float[diffusion.TValues.Count];
                    Array.Copy(diffusion.TValues.ToArray(), tValues, tValues.Length);

                    // write every data about modeling to files
                    ActionDataFile.writeDataToFile("Volume", i, valuesP);
                    // Write time-value data to file
                    ActionDataFile.writeTimeValueToFile("Volume", i, tValues, numberPointsVolume);
                    // write params of modeling to file
                    float[] paramsForCancer = { speed, d, k };
                    ActionDataFile.writeParametersToFile(type: "Volume", number: i, cancerParameters: paramsForCancer);
                }     
            }
            
            MessageBox.Show("Success calculate");
        }


        private void ShowPlots_Click(object sender, RoutedEventArgs e)
        {
            int patientNumber = 1;
            try
            {
                patientNumber = int.Parse(TextBoxPatientNumberPlot.Text, CultureInfo.InvariantCulture);
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Please, input correct data (number patient).");
            }
            string pathImg1 = @"dataTumor/PredictData/PersonalPatients/Volume/img/" + patientNumber.ToString() + "Volume.png";
            string pathImg2 = @"dataTumor/PredictData/PersonalPatients/Diameter/img/" + patientNumber.ToString() + "Diameter.png";
            Uri uri1 = new Uri(pathImg1, UriKind.Relative);
            Uri uri2 = new Uri(pathImg2, UriKind.Relative);
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
            MessageBox.Show(pathImg1);
        }


        private void SavePlots_Click(object sender, RoutedEventArgs e)
        {
            // Create Process start info
            var psi = new ProcessStartInfo();
            string pathPython = @"D:\VolSU\НИР\ScienceArticle\Modeling\venv\Scripts\python.exe";
            psi.FileName = pathPython;

            // Provide Scripts and Arguments
            var scriptPython = @"CancerVolumePlot.py";
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
        }
    }
}
