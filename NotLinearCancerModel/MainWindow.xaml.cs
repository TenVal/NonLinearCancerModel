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
            DataCancer modelData = new DataCancer();
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
            D dF;
            Q q = new Q(0);

            MethodDiffusion diffusion;
            float tMax;
            float[,,] valuesP;

            List<float> difference = new List<float>();

            for (int i = 0; i < 11; i++)
            {
                do
                {
                    dF = new D(speed, d);
                    diffusion = new MethodDiffusion(dF, c, q);
                    tMax = modelData.Patients[i]["Diameter"][0][modelData.Patients[i]["Diameter"][0].Count - 1];
                    valuesP = diffusion.getValues(tMax, h, k, length);

                    difference.Add(diffusion.NumberPointsVolume[diffusion.NumberPointsVolume.Count - 1] - 
                        modelData.Patients[i]["Volume"][1][modelData.Patients[i]["Volume"][1].Count - 1]);
                    speed += stepAccuracy;
                }
                while (speed <= accuracy);

                float minValue = difference.Min();
            }
        }
    }
}
