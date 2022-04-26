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
    /// Логика взаимодействия для CalculateOneView.xaml
    /// </summary>
    public partial class CalculateOneView : UserControl
    {
        public CalculateOneView()
        {
            InitializeComponent();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            float length = float.Parse(TextBoxLength.Text);
            float RightX = length;
            float h = float.Parse(TextBoxH.Text);
            float d = float.Parse(TextBoxD.Text);
            float k = float.Parse(TextBoxK.Text);
            float speed = float.Parse(TextBoxSpeed.Text);
            float angleXY = float.Parse(TextBoxAngleXY.Text);
            float angleZ = float.Parse(TextBoxAngleZ.Text);
            float tMax = float.Parse(TextBoxTMax.Text);
            string path = @"..\..\..\dataTumor\PredictData\Any\";

            C c = new C(speed, angleXY, angleZ);
            Q q = new Q(0);
            D dF = new D(speed, d);

            MethodDiffusion diffusion = new MethodDiffusion(dF, c, q);

            for (int i = 0; i < 10; i++)
            {
                int N = (int)(length / h);
                double[,,] valuesP = new double[N, N, N];
                diffusion.getValues(tMax, h, k, length, valuesP);

                float[] numberPointsVolume = new float[diffusion.NumberPointsVolume.Count];
                Array.Copy(diffusion.NumberPointsVolume.ToArray(), numberPointsVolume, numberPointsVolume.Length);
                float[] tValues = new float[diffusion.TValues.Count];
                Array.Copy(diffusion.TValues.ToArray(), tValues, tValues.Length);

                // write every data about modeling to files
                ActionDataFile.writeDataToFile("Volume", i, valuesP, path);
                // Write time-value data to file
                ActionDataFile.writeTimeValueToFile("Volume", i, tValues, numberPointsVolume, path);
                // write params of modeling to file
                float[] paramsForCancer = { speed, d, k };
                ActionDataFile.writeParametersToFile(type: "Volume", path, number: i, cancerParameters: paramsForCancer);
            }

            MessageBox.Show("UUUUUUURRRRAAAAAAA");
        }
    }
}
