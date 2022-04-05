﻿using System;
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

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

using System.Diagnostics;

using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.ILMath;
using static ILNumerics.Globals;
//Tools -> Options -> ILNumerics -> Licenses
//Инструменты -> Параметры -> ILNumerics -> Лицензии

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
            D dF;
            Q q = new Q(0);

            MethodDiffusion diffusion;
            float tMax;
            float[,,] valuesP;

            //Storage to keep data about matched value
            Dictionary<string, List<float>> cancerValuesParameters = new Dictionary<string, List<float>>()
            {
                {"Speed" , new List<float>()},
                {"Difference" , new List<float>()}
            };
            List<float[,,]> listAllValuesP = new List<float[,,]>();

            for (int i = 0; i < 10; i++)
            {
                do
                {
                    cancerValuesParameters["Speed"].Add(speed);
                    dF = new D(speed, d);
                    diffusion = new MethodDiffusion(dF, c, q);
                    tMax = modelData.Patients[i]["Diameter"][0][modelData.Patients[i]["Diameter"][0].Count - 1];

                    valuesP = diffusion.getValues(tMax, h, k, length);
                    listAllValuesP.Add(valuesP);

                    // find difference between modelData and diffusionModelData
                    cancerValuesParameters["Difference"].Add(Math.Abs(
                        diffusion.NumberPointsVolume[diffusion.NumberPointsVolume.Count - 1] - 
                        modelData.Patients[i]["Volume"][1][modelData.Patients[i]["Volume"][1].Count - 1]));
                    speed += stepAccuracy;
                }
                while (speed <= accuracy);

                float minDifference = cancerValuesParameters["Difference"].Min();
                int indexMinDifference = cancerValuesParameters["Difference"].IndexOf(cancerValuesParameters["Difference"].Min());
                float requiredSpeed = cancerValuesParameters["Speed"][indexMinDifference];
                float[,,] requiredValuesP = listAllValuesP[indexMinDifference];

                // write every data about modeling to files
                ActionDataFile.writeDataToFile("Volume", i, requiredValuesP);
                // write params of modeling to file
                float[] paramsForCancer = { speed, d, k };
                ActionDataFile.writeParametersToFile(type:"Volume", number:i, cancerParameters:paramsForCancer);
            }
            MessageBox.Show("success");
        }

        private void ShowPlots_Click(object sender, RoutedEventArgs e)
        {
            /*// Create engine
            var engine = Python.CreateEngine();
            var searchPath = engine.GetSearchPaths();
            searchPath.Add(@"D:\VolSU\НИР\ScienceArticle\NotLinearCancerModel\.venv\Lib");
            searchPath.Add(@"D:\VolSU\НИР\ScienceArticle\NotLinearCancerModel\.venv\Lib\site-packages");
           
            engine.SetSearchPaths(searchPath);

            // Provide scripts and arguments
            var nameScriptPlotVolume = @"CancerVolumePlot.py";
            var source = engine.CreateScriptSourceFromFile(nameScriptPlotVolume);

            var argv = new List<string>();
            argv.Add("");

            engine.GetSysModule().SetVariable("argv", argv);

            // Output redirect
            var eIO = engine.Runtime.IO;

            var errors = new MemoryStream();
            eIO.SetErrorOutput(errors, Encoding.Default);

            var results = new MemoryStream();
            eIO.SetOutput(results, Encoding.Default);

            // Execute script
            var scope = engine.CreateScope();
            source.Execute(scope);
            
            // Display output
            string str(byte[] x) => Encoding.Default.GetString(x);
            Debug.WriteLine("ERRORS");
            Debug.WriteLine(str(errors.ToArray()));
            Debug.WriteLine("RESULTS");
            Debug.WriteLine(str(results.ToArray()));*/

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

            using(var process = Process.Start(psi))
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

        private void WFHost1_Loaded(object sender, RoutedEventArgs e)
        {

            /*var panel = new ILNumerics.Drawing.Panel();
            WFHost1.Child = panel;

            panel.Scene.Add(new PlotCube(twoDMode: false));*/

        }
    }
}
