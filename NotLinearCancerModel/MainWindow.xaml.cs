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
            string pathImg1 = @"..\..\..\dataTumor\PredictData\PersonalPatients\Volume\img\" + patientNumber.ToString() + @"Volume.png";
            //string pathImg2 = @"dataTumor\PredictData\PersonalPatients\Diameter\img\" + patientNumber.ToString() + "Diameter.png";
            string pathImg2 = @"..\..\..\dataTumor\PredictData\PersonalPatients\Volume\timeValue\img\" + patientNumber.ToString() + @"Volume.png";
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
        }


        private void SavePlots_Click(object sender, RoutedEventArgs e)
        {
            // Create Process start info
            var psi = new ProcessStartInfo();
            string pathPython = @"..\..\..\env\Scripts\python.exe";
            //string pathPython = TextBoxPythonInterpreter.Text;
            //if()
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
