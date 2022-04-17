using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class MethodDiffusion
    {
        /// <summary>
        /// Numerical method for calculating diffusion in 3D space
        /// (density in everu poinе)
        /// </summary>
        private D d;
        private C c;
        private Q q;
        private List<float> tValues;
        private List<float> numberPointsVolume;

        public  List<float> TValues 
        {
            get
            {
                return tValues;
            }
        }

        public List<float> NumberPointsVolume
        {
            get
            {
                return numberPointsVolume;
            }
        }

        public MethodDiffusion(D d, C c, Q q)
        {
            this.tValues = new List<float>();
            this.numberPointsVolume = new List<float>();
            this.d = d;
            this.c = c;
            this.q = q;
        }

        public int getValues(float tMax, float h, float K, float length, double[,,] valuesP2)
        {
            this.tValues = new List<float>();
            this.numberPointsVolume = new List<float>();
            int currentPoints = 0;
            int N = (int)(length / h);
            float t = 0;

            double[,,] valuesP1 = new double[N, N, N];

            while (true)
            {
                float[] minTau = { h * h / (2 * d.get(0, 0, 0, length)) + h / this.c.getProjectionX(0, 0, 0),
                                                 h * h / (2 * d.get(0, 0, 0, length)) + h / this.c.getProjectionY(0, 0, 0),
                                                 h * h / (2 * d.get(0, 0, 0, length)) + h / this.c.getProjectionZ(0, 0, 0)};

                if (d.get(0, 0, 0, length) <= 1)
                {
                    minTau[0] = h / this.c.getProjectionX(0, 0, 0);
                    minTau[1] = h / this.c.getProjectionY(0, 0, 0);
                    minTau[2] = h / this.c.getProjectionZ(0, 0, 0);
                }
                float tau = K * minTau.Min();
                for (int i = 0; i < (N - 1); i++)
                {
                    for (int j = 0; j < (N - 1); j++)
                    {
                        for (int k = 0; k < (N - 1); k++)
                        {
                            double M1 = 0;
                            double M2 = 0;
                            double M3 = 0;
                            double M4 = 0;
                            double M5 = 0;
                            double M6 = 0;
                            
                            if (this.c.getProjectionX(i * h, j * h, k * h) > 0)
                            {
                                M1 = valuesP1[i, j, k] * this.c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                                if (i - 1 < 0)
                                    M2 = valuesP1[valuesP1.GetLength(0) - 1, j, k] * this.c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                                else
                                    M2 = valuesP1[i - 1, j, k] * this.c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                            }
                            else if (this.c.getProjectionX(i * h, j * h, k * h) < 0)
                            {
                                M1 = valuesP1[i + 1, j, k] * this.c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                                M2 = valuesP1[i, j, k] * this.c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                            }

                            if (this.c.getProjectionY(i * h, j * h, k * h) > 0)
                            {
                                M3 = valuesP1[i, j, k] * this.c.getProjectionY(i * h, j * h, k * h) * h * h * tau;
                                if (j - 1 < 0)
                                    M4 = valuesP1[i, valuesP1.GetLength(1) - 1, k] * this.c.getProjectionY(i * h, j * h, k * h) * h * h * tau;
                                else
                                    M4 = valuesP1[i, j - 1, k] * this.c.getProjectionY(i * h, j * h, k * h) * h * h * tau;
                            }
                            else if (this.c.getProjectionY(i * h, j * h, k * h) < 0)
                            {
                                M3 = valuesP1[i, j + 1, k] * this.c.getProjectionY(i * h, j * h, k * h) * h * tau;
                                M4 = valuesP1[i, j, k] * this.c.getProjectionY(i * h, j * h, k * h) * h * tau;
                            }

                            if (this.c.getProjectionZ(i * h, j * h, k * h) > 0)
                            {
                                M5 = valuesP1[i, j, k] * this.c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                                if (k - 1 < 0)
                                    M6 = valuesP1[i, j, valuesP1.GetLength(2) - 1] * this.c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                                else
                                    M6 = valuesP1[i, j, k - 1] * this.c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                            }
                            else if (this.c.getProjectionZ(i * h, j * h, k * h) < 0)
                            {
                                M5 = valuesP1[i, j, k + 1] * this.c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                                M6 = valuesP1[i, j, k] * this.c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                            }

                            // operands of finite difference scheme
                            double S1 = valuesP1[i, j, k];
                            double S2 = 1 / (h * h * h) * (M1 - M2 + M3 - M4 + M5 - M6);
                            double valuesP1S3 = 0;
                            double valuesP1S4 = 0;
                            double valuesP1S5 = 0;

                            if (i - 1 < 0)
                                valuesP1S3 = valuesP1[valuesP1.GetLength(0) - 1, j, k];
                            else
                                valuesP1S3 = valuesP1[i - 1, j, k];
                            if (j - 1 < 0)
                                valuesP1S4 = valuesP1[i, valuesP1.GetLength(1) - 1, k];
                            else
                                valuesP1S4 = valuesP1[i, j - 1, k];
                            if (k - 1 < 0)
                                valuesP1S5 = valuesP1[i, j, valuesP1.GetLength(2) - 1];
                            else
                                valuesP1S5 = valuesP1[i, j, k - 1];
                            double S3 = (tau / (h * h)) * (d.get(i * h, j * h, k * h, length) * (valuesP1[i + 1, j, k] - valuesP1[i, j, k]) -
                                                    d.get(i * h, j * h, k * h, length) * (valuesP1[i, j, k] - valuesP1S3));
                            double S4 = (tau / (h * h)) * (d.get(i * h, j * h, k * h, length) * (valuesP1[i, j + 1, k] - valuesP1[i, j, k]) -
                                                    d.get(i * h, j * h, k * h, length) * (valuesP1[i, j, k] - valuesP1S4));
                            double S5 = (tau / (h * h)) * (K * (valuesP1[i, j, k + 1] - valuesP1[i, j, k]) - K * (valuesP1[i, j, k] - valuesP1S5));
                            // sources
                            double S6 = this.q.get(i, j, k, t) * tau;
                            // Implementation of a finite difference scheme
                            valuesP2[i, j, k] = S1 - S2 + S3 + S4 + S5 + S6;
                            // System.Diagnostics.Debug.WriteLine(valuesP2[i, j, k]);
                            // Counting the number of volume points in each time layer
                            if (valuesP2[i, j, k] > 0)
                            {
                                currentPoints++;
                            }
                                
                        }
                    }
                }
                // border conditions
                for (int j = 0; j < N; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        valuesP2[0, j, k] = 0;
                        valuesP2[N - 1, j, k] = 0;
                    }  
                }

                for (int i = 0; i < N; i++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        valuesP2[i, 0, k] = 0;
                        valuesP2[i, N - 1, k] = 0;
                    }
                }

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        valuesP2[i, j, 0] = 0;
                        valuesP2[i, j, N - 1] = 0;
                    }
                }

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        for (int k = 0; k < N; k++)
                        {
                            valuesP1[i, j, k] = valuesP2[i, j, k];
                        }
                    }
                }
                t += tau;
                numberPointsVolume.Add(currentPoints);
                tValues.Add(t);
                if (t > tMax)
                    break;
            }
            return 0;
        }
    }
}
