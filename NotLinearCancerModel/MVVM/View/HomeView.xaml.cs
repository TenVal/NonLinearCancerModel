﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        public struct ParametersCancer
        {
            public float length;
            public float RightX;
            public float h;
            public float d;
            public float k;
            public float accuracy;
            public float stepAccuracy;
            public float speed;
            public float angleXY;
            public float angleZ;

            public ParametersCancer(
                float length,
                float RightX,
                float h,
                float d,
                float k,
                float accuracy,
                float stepAccuracy,
                float speed,
                float angleXY,
                float angleZ)
            {
                this.length = length;
                this.RightX = RightX;
                this.h = h;
                this.d = d;
                this.k = k;
                this.accuracy = accuracy;
                this.stepAccuracy = stepAccuracy;
                this.speed = speed;
                this.angleXY = angleXY;
                this.angleZ = angleZ;
            }
        }


        public HomeView()
        {
            InitializeComponent();
        }

        private void CalculateMin_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += workerMin_RunWorkerComplited;
            worker.WorkerReportsProgress = true;
            worker.DoWork += workerMin_Calculate;
            worker.ProgressChanged += workerMin_ProgressChanged;

            ParametersCancer paramsCancer = new ParametersCancer(
                                    float.Parse(TextBoxLength.Text),
                                    float.Parse(TextBoxLength.Text),
                                    float.Parse(TextBoxH.Text),
                                    float.Parse(TextBoxD.Text),
                                    float.Parse(TextBoxK.Text),
                                    float.Parse(TextBoxAccuracy.Text),
                                    float.Parse(TextBoxStepAccuracy.Text),
                                    float.Parse(TextBoxSpeed.Text),
                                    float.Parse(TextBoxAngleXY.Text),
                                        float.Parse(TextBoxAngleZ.Text));
            worker.RunWorkerAsync(paramsCancer);
        }

        private void workerMin_RunWorkerComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done Calculate min!");
            ProgressBarCalculate.Value = 0;
        }

        private void workerMin_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarCalculate.Value = e.ProgressPercentage;
        }

        private void workerMin_Calculate(object sender, DoWorkEventArgs e)
        {
            ParametersCancer paramsCancer = (ParametersCancer)e.Argument;
            var worker = sender as BackgroundWorker;
            worker.ReportProgress(0, String.Format("Processing Iteration 1."));
            DataCancer modelData = new DataCancer();

            int numberPatients = 10;
            float valueOfDivision = 100 / numberPatients;

            float length = paramsCancer.length;
            float RightX = paramsCancer.RightX;
            float h = paramsCancer.h;
            float d = paramsCancer.d;
            float k = paramsCancer.k;
            float accuracy = paramsCancer.accuracy;
            float stepAccuracy = paramsCancer.stepAccuracy;
            float speed = paramsCancer.speed;
            float angleXY = paramsCancer.angleXY;
            float angleZ = paramsCancer.angleZ;
            int N = (int)(length / h);

            float tMax;

            C c = new C(speed, angleXY, angleZ);
            Q q = new Q(0);
            MethodDiffusion diffusion;

            float speedForFindMin = speed;
            // Counting for everyone patient and find required by min difference between predict data and actual data
            for (int i = 0; i < numberPatients; i++)
            {

                //Storage to keep data about matched value
                Dictionary<string, List<float>> cancerValuesParameters = new Dictionary<string, List<float>>()
                {
                    {"Length" , new List<float>() },
                    {"H" , new List<float>() },
                    {"D" , new List<float>() },
                    {"K" , new List<float>() },
                    {"Accuracy" , new List<float>() },
                    {"StepAccuracy" , new List<float>() },
                    {"Speed" , new List<float>()},
                    {"AngleXY" , new List<float>() },
                    {"AngleZ" , new List<float>() },
                    {"Difference" , new List<float>()},
                };
                List<double[,,]> listAllValuesP = new List<double[,,]>();
                List<float[]> listAllValuesT = new List<float[]>();
                List<float[]> listAllValuesNumberPointsVolume = new List<float[]>();
                do
                {
                    // add to list every parameter
                    cancerValuesParameters["Length"].Add(length);
                    cancerValuesParameters["H"].Add(h);
                    cancerValuesParameters["D"].Add(d);
                    cancerValuesParameters["K"].Add(k);
                    cancerValuesParameters["Accuracy"].Add(accuracy);
                    cancerValuesParameters["StepAccuracy"].Add(stepAccuracy);
                    cancerValuesParameters["Speed"].Add(speedForFindMin);
                    cancerValuesParameters["AngleXY"].Add(angleXY);
                    cancerValuesParameters["AngleZ"].Add(angleZ);

                    D dF = new D(speedForFindMin, d);
                    diffusion = new MethodDiffusion(dF, c, q);

                    //float tStart = modelData.Patients[i]["Diameter"][0][0];
                    float tEnd = modelData.Patients[i]["Diameter"][0][modelData.Patients[i]["Diameter"][0].Count - 1];
                    //tMax = tEnd - tStart;
                    tMax = tEnd;
                    tMax /= 30;

                    double[,,] valuesP = new double[N, N, N];
                    diffusion.getValues(tMax, h, k, length, valuesP);

                    // Data for time-volume plot
                    float[] numberPointsVolume = new float[diffusion.NumberPointsVolume.Count];
                    Array.Copy(diffusion.NumberPointsVolume.ToArray(), numberPointsVolume, numberPointsVolume.Length);
                    float[] tValues = new float[diffusion.TValues.Count];
                    Array.Copy(diffusion.TValues.ToArray(), tValues, tValues.Length);
                    /*var valuesP = diffusion.getValues(tMax, h, k, length);
                    Array.Copy(diffusion.getValues(tMax, h, k, length), valuesP, valuesP.GetLength(0) + valuesP.GetLength(1) + valuesP.GetLength(2));*/
                    listAllValuesP.Add(valuesP);
                    listAllValuesT.Add(tValues);
                    listAllValuesNumberPointsVolume.Add(numberPointsVolume);

                    // find difference between modelData and diffusionModelData and add to list
                    cancerValuesParameters["Difference"].Add(Math.Abs(
                        diffusion.NumberPointsVolume[diffusion.NumberPointsVolume.Count - 1] -
                        modelData.Patients[i]["Volume"][1][modelData.Patients[i]["Volume"][1].Count - 1]));

                    speedForFindMin += stepAccuracy;

                }
                while (speedForFindMin <= accuracy);

                speedForFindMin = speed;
                // Find required speed and others parameters
                float minDifference = cancerValuesParameters["Difference"].Min();
                int indexMinDifference = cancerValuesParameters["Difference"].IndexOf(cancerValuesParameters["Difference"].Min());
                float requiredSpeed = cancerValuesParameters["Speed"][indexMinDifference];
                Debug.WriteLine("indexMinDifference\t" + indexMinDifference.ToString());
                Debug.WriteLine("listAllValuesP\t" + listAllValuesP.Count.ToString());
                Debug.WriteLine("listAllValuesT\t" + listAllValuesT.Count.ToString());
                Debug.WriteLine("listAllValuesNumberPointsVolume\t" + listAllValuesNumberPointsVolume.Count.ToString());
                Debug.WriteLine("cancerValuesParameters(Speed)\t" + cancerValuesParameters["Speed"].Count.ToString());
                double[,,] requiredValuesP = listAllValuesP[indexMinDifference];
                float[] requiredTValue = listAllValuesT[indexMinDifference];
                float[] requiredNumberPointsVolume = listAllValuesNumberPointsVolume[indexMinDifference];

                // prepare data about parameters of cancer for writing into files
                Dictionary<string, float> requiredCancerValuesParameters = new Dictionary<string, float>()
                {
                    {"Length" ,  cancerValuesParameters["Length"][indexMinDifference] },
                    {"H" , cancerValuesParameters["H"][indexMinDifference] },
                    {"D" , cancerValuesParameters["D"][indexMinDifference] },
                    {"K" , cancerValuesParameters["K"][indexMinDifference] },
                    {"Accuracy" , cancerValuesParameters["Accuracy"][indexMinDifference] },
                    {"StepAccuracy" , cancerValuesParameters["StepAccuracy"][indexMinDifference] },
                    {"Speed" , cancerValuesParameters["Speed"][indexMinDifference]},
                    {"AngleXY" , cancerValuesParameters["AngleXY"][indexMinDifference] },
                    {"AngleZ" , cancerValuesParameters["AngleZ"][indexMinDifference] },
                    {"TMax" , tMax },
                    {"Difference" , cancerValuesParameters["Difference"][indexMinDifference]},
                };


                // write every data about modeling to files
                ActionDataFile.writeDataToFile("Volume", i, requiredValuesP);
                // Write time-value data to file
                ActionDataFile.writeTimeValueToFile("Volume", i, requiredTValue, requiredNumberPointsVolume);
                // write params of modeling to file
                float[] paramsForCancer = { requiredSpeed, d, k };
                ActionDataFile.writeParametersToFile(type: "Volume", number: i, cancerParameters: requiredCancerValuesParameters);

                worker.ReportProgress((i + 2) * (int)valueOfDivision, String.Format("Processing Iteration {0}", i + 2));
            }
            
            worker.ReportProgress(100, "Done Calculate min!");
        }
    }
}
