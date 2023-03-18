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

namespace NotLinearCancerModel.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для TemperatureFunctionView.xaml
    /// </summary>
    public partial class TemperatureFunctionView : UserControl
    {
        public TemperatureFunctionView()
        {
            InitializeComponent();
        }

        private void unpressedButton()
        {
            TemperatureFunction.Content = "Calculate";
            TemperatureFunction.IsEnabled = true;
            SolidColorBrush brushForUnpressedButton = new SolidColorBrush(Colors.White);
            TemperatureFunction.Foreground = brushForUnpressedButton;
            PercentProgressBarCalculate.Visibility = Visibility.Collapsed;
            ProgressBarCalculate.Visibility = Visibility.Collapsed;
            ProgressBarCalculate.Value = 0;
        }

        private void TemperatureFunction_Click(object sender, RoutedEventArgs e)
        {
            PercentProgressBarCalculate.Visibility = Visibility.Visible;
            ProgressBarCalculate.Visibility = Visibility.Visible;
            TemperatureFunction.Content = "Calculate...";
            SolidColorBrush brushForPressedButton = new SolidColorBrush(Colors.Black);
            TemperatureFunction.Foreground = brushForPressedButton;
            TemperatureFunction.IsEnabled = false;

            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += workerTemperature_RunWorkerComplited;
            worker.WorkerReportsProgress = true;
            worker.DoWork += workerTemperature_Calculate;
            worker.ProgressChanged += workerTemperature_ProgressChanged;
            worker.RunWorkerAsync();

        }


        private void workerTemperature_RunWorkerComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done Temperature Calculate!");
            TemperatureFunction.Content = "Temperature Function";
            TemperatureFunction.IsEnabled = true;
            SolidColorBrush brushForUnpressedButton = new SolidColorBrush(Colors.White);
            TemperatureFunction.Foreground = brushForUnpressedButton;
            PercentProgressBarCalculate.Visibility = Visibility.Collapsed;
            ProgressBarCalculate.Visibility = Visibility.Collapsed;
            ProgressBarCalculate.Value = 0;
        }

        private void workerTemperature_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarCalculate.Value = e.ProgressPercentage;
        }

        private float calculateRadiusValue(float volume)
        {
            float rValue = 0;
            try
            {
                rValue = (float)Math.Sqrt(volume / Math.PI);
            }
            return rValue;
        }

        private void workerTemperature_Calculate(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            worker.ReportProgress(0, String.Format("Processing Iteration 1."));

            int numberPatients = 10;
            float valueOfDivisionProgressBar = 100 / numberPatients;

            for (int i = 0; i < numberPatients; i++)
            {
                string pathWriteData = @"dataTumor\PredictData\PersonalPatients\Volume\timeValue\txt\";
                float[][] Values = ActionDataFile.getDynamicDataFromFile("Volume", i + 1, pathWriteData);
                int lenthValues = Values[0].Length;
                for (int k = 0; k < lenthValues; k++)
                {
                    Values[1][k] = calculateRadiusValue(Values[1][k]);
                    Debug.WriteLine(String.Format("{1}\t{0}", Values[1][k], Values[0][k]));
                }
                ActionDataFile.writeTimeValueToFile(type: "Temperature", number: i, tValues: Values[0], cancerValues: Values[1], pathToSave: pathWriteData);

                worker.ReportProgress((i + 1) * (int)valueOfDivisionProgressBar, String.Format("Processing Iteration {0}", i + 1));
            }

            worker.ReportProgress(100, "Done Calculate min!");
        }
    }
}
