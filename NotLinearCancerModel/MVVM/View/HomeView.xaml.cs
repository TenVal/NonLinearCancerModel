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

namespace NotLinearCancerModel.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            /*// get data from files with polynomial regression
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

                    for (int n = 0; n < listAllValuesP.Count ; n++)
                    {
                        for (int o = 0; o < listAllValuesP[n].GetLength(0); o++)
                        {
                            for (int p = 0; p < listAllValuesP[n].GetLength(1); p++)
                            {
                                for (int l = 0; l < listAllValuesP[n].GetLength(2); l++)
                                {

                                    Debug.WriteLine(listAllValuesP[n][o, p, l]);
                                }
                            }
                        }
                    }
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
            
            MessageBox.Show("Success calculate");*/
            // get data from files with polynomial regression
            DataCancer modelData = new DataCancer();

            // get data from forms of interface
            /*float length = float.Parse(TextBoxLength.Text, CultureInfo.InvariantCulture);
            float RightX = length;
            float h = float.Parse(TextBoxH.Text, CultureInfo.InvariantCulture);
            float d = float.Parse(TextBoxD.Text, CultureInfo.InvariantCulture);
            float k = float.Parse(TextBoxK.Text, CultureInfo.InvariantCulture);
            float accuracy = float.Parse(TextBoxAccuracy.Text, CultureInfo.InvariantCulture);
            float stepAccuracy = float.Parse(TextBoxStepAccuracy.Text, CultureInfo.InvariantCulture);
            float speed = float.Parse(TextBoxSpeed.Text, CultureInfo.InvariantCulture);
            float angleXY = float.Parse(TextBoxAngleXY.Text, CultureInfo.InvariantCulture);
            float angleZ = float.Parse(TextBoxAngleZ.Text, CultureInfo.InvariantCulture);*/
            float length = float.Parse(TextBoxLength.Text);
            float RightX = length;
            float h = float.Parse(TextBoxH.Text);
            float d = float.Parse(TextBoxD.Text);
            float k = float.Parse(TextBoxK.Text);
            float accuracy = float.Parse(TextBoxAccuracy.Text);
            float stepAccuracy = float.Parse(TextBoxStepAccuracy.Text);
            float speed = float.Parse(TextBoxSpeed.Text);
            float angleXY = float.Parse(TextBoxAngleXY.Text);
            float angleZ = float.Parse(TextBoxAngleZ.Text);

            C c = new C(speed, angleXY, angleZ);
            Q q = new Q(0);
            MethodDiffusion diffusion;

            //Storage to keep data about matched value
            Dictionary<string, List<float>> cancerValuesParameters = new Dictionary<string, List<float>>()
            {
                {"Speed" , new List<float>()},
                {"Difference" , new List<float>()}
            };
            List<double[,,]> listAllValuesP = new List<double[,,]>();

            // Counting for everyone patient and find required by min difference between predict data and actual data
            for (int i = 0; i < 10; i++)
            {
                do
                {
                    cancerValuesParameters["Speed"].Add(speed);
                    D dF = new D(speed, d);
                    diffusion = new MethodDiffusion(dF, c, q);
                    float tMax = modelData.Patients[i]["Diameter"][0][modelData.Patients[i]["Diameter"][0].Count - 1];

                    int N = (int)(length / h);
                    double[,,] valuesP = new double[N, N, N];
                    diffusion.getValues(tMax, h, k, length, valuesP);
                    /*var valuesP = diffusion.getValues(tMax, h, k, length);
                    Array.Copy(diffusion.getValues(tMax, h, k, length), valuesP, valuesP.GetLength(0) + valuesP.GetLength(1) + valuesP.GetLength(2));*/
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
                double[,,] requiredValuesP = listAllValuesP[indexMinDifference];

                // write every data about modeling to files
                ActionDataFile.writeDataToFile("Volume", i, requiredValuesP);
                // Write time-value data to file
                ActionDataFile.writeTimeValueToFile("Volume", i, tValues, numberPointsVolume);
                // write params of modeling to file
                float[] paramsForCancer = { speed, d, k };
                ActionDataFile.writeParametersToFile(type: "Volume", number: i, cancerParameters: paramsForCancer);
            }
            /*for (int n = 0; n < listAllValuesP.Count; n++)
            {
                for (int o = 0; o < listAllValuesP[n].GetLength(0); o++)
                {
                    for (int p = 0; p < listAllValuesP[n].GetLength(1); p++)
                    {
                        for (int l = 0; l < listAllValuesP[n].GetLength(2); l++)
                        {
                            Debug.WriteLine(listAllValuesP[n][o, p, l]);
                        }
                    }
                }
            }*/
            MessageBox.Show("success");
        }
    }
}
