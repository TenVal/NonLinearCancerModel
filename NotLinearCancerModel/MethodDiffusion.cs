﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private D _d;
        private C _c;
        private Q _q;
        private Resistance _alpha;
        private List<float> _tValues;
        private List<float> _numberPointsVolume;
        public float tMax = 0;

        public  List<float> TValues 
        {
            get
            {
                return _tValues;
            }
        }

        public List<float> NumberPointsVolume
        {
            get
            {
                return _numberPointsVolume;
            }
        }

        public MVVM.View.HomeView HomeView
        {
            get => default;
            set
            {
            }
        }

        public MVVM.View.CalculateOneView CalculateOneView
        {
            get => default;
            set
            {
            }
        }

        public MethodDiffusion(D d, C c, Q q, Resistance alpha)
        {
            this._tValues = new List<float>();
            this._numberPointsVolume = new List<float>();
            this._d = d;
            this._c = c;
            this._q = q;
            this._alpha = alpha;
        }

        public int getValues(float tMax, float tStep, float h, float K, float length, double[,,] valuesP2)
        {
            this._tValues = new List<float>();
            this._numberPointsVolume = new List<float>();
            int currentPoints = 0;
            int N = (int)(length / h);
            float t = 0;

            double[,,] valuesP1 = new double[N, N, N];
            float tCheck = 0;

            currentPoints = 0;
            float[] minTau = {  (h * h / (2 * _d.get(0, 0, 0, length)) + h / Math.Abs(this._c.getProjectionX(0, 0, 0))),
                                (h * h / (2 * _d.get(0, 0, 0, length)) + h / Math.Abs(this._c.getProjectionY(0, 0, 0))),
                                (h * h / (2 * _d.get(0, 0, 0, length)) + h / Math.Abs(this._c.getProjectionZ(0, 0, 0)))};
            Debug.WriteLine(this._d.get(0, 0, 0, length));
            Debug.WriteLine(h);

            tCheck = K * minTau.Min();
            if (tStep >= tCheck)
            {
                Debug.WriteLine("ERROR!!!!!!!!!");
                throw new Exception("Шаг по времени не дает устойчивую схему!");
            }
            Debug.WriteLine($"tStep = {tStep}\ttCheck = {tCheck}");

            while (true)
            {
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
                            
                            if (this._c.getProjectionX(i * h, j * h, k * h) > 0)
                            {
                                M1 = valuesP1[i, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tStep;
                                if (i - 1 < 0)
                                    M2 = valuesP1[valuesP1.GetLength(0) - 1, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tStep;
                                else
                                    M2 = valuesP1[i - 1, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tStep;
                            }
                            else if (this._c.getProjectionX(i * h, j * h, k * h) < 0)
                            {
                                M1 = valuesP1[i + 1, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tStep;
                                M2 = valuesP1[i, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tStep;
                            }

                            if (this._c.getProjectionY(i * h, j * h, k * h) > 0)
                            {
                                M3 = valuesP1[i, j, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * h * tStep;
                                if (j - 1 < 0)
                                    M4 = valuesP1[i, valuesP1.GetLength(1) - 1, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * h * tStep;
                                else
                                    M4 = valuesP1[i, j - 1, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * h * tStep;
                            }
                            else if (this._c.getProjectionY(i * h, j * h, k * h) < 0)
                            {
                                M3 = valuesP1[i, j + 1, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * tStep;
                                M4 = valuesP1[i, j, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * tStep;
                            }

                            if (this._c.getProjectionZ(i * h, j * h, k * h) > 0)
                            {
                                M5 = valuesP1[i, j, k] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tStep;
                                if (k - 1 < 0)
                                    M6 = valuesP1[i, j, valuesP1.GetLength(2) - 1] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tStep;
                                else
                                    M6 = valuesP1[i, j, k - 1] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tStep;
                            }
                            else if (this._c.getProjectionZ(i * h, j * h, k * h) < 0)
                            {
                                M5 = valuesP1[i, j, k + 1] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tStep;
                                M6 = valuesP1[i, j, k] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tStep;
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
                            double S3 = (tStep / (h * h)) * (_d.get(i * h, j * h, k * h, length) * (valuesP1[i + 1, j, k] - valuesP1[i, j, k]) -
                                                    _d.get(i * h, j * h, k * h, length) * (valuesP1[i, j, k] - valuesP1S3));
                            double S4 = (tStep / (h * h)) * (_d.get(i * h, j * h, k * h, length) * (valuesP1[i, j + 1, k] - valuesP1[i, j, k]) -
                                                    _d.get(i * h, j * h, k * h, length) * (valuesP1[i, j, k] - valuesP1S4));
                            double S5 = (tStep / (h * h)) * (K * (valuesP1[i, j, k + 1] - valuesP1[i, j, k]) - K * (valuesP1[i, j, k] - valuesP1S5));
                            // sources
                            double S6 = (this._q.get(i, j, k, t) - this._alpha.get(i, j, k, t) * valuesP1[i, j, k]) * tStep;
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
                t += tStep;
                Debug.WriteLine($"t = {t}\ttau = {tStep}");
                _numberPointsVolume.Add(currentPoints);
                _tValues.Add(t);
                if (t > (tMax + tStep))
                    break;
            }
            this.tMax = t - tStep;
            return 0;
        }


        public int getValues2(float tMax, float tStep, float h, float K, float length, double[,,] valuesP2)
        {
            this._tValues = new List<float>();
            this._numberPointsVolume = new List<float>();
            int currentPoints = 0;
            int N = (int)(length / h);
            float t = 0;

            double[,,] valuesP1 = new double[N, N, N];
            float tau = 0;

            while (true)
            {
                currentPoints = 0;
                float[] minTau = {  (h * h / (2 * _d.get(0, 0, 0, length)) + h / Math.Abs(this._c.getProjectionX(0, 0, 0))),
                                    (h * h / (2 * _d.get(0, 0, 0, length)) + h / Math.Abs(this._c.getProjectionY(0, 0, 0))),
                                    (h * h / (2 * _d.get(0, 0, 0, length)) + h / Math.Abs(this._c.getProjectionZ(0, 0, 0)))};



                /*if (_d.get(0, 0, 0, length) <= 1)
                {
                    minTau[0] = h / this._c.getProjectionX(0, 0, 0);
                    minTau[1] = h / this._c.getProjectionY(0, 0, 0);
                    minTau[2] = h / this._c.getProjectionZ(0, 0, 0);
                }*/
                tau = K * minTau.Min();
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

                            if (this._c.getProjectionX(i * h, j * h, k * h) > 0)
                            {
                                M1 = valuesP1[i, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                                if (i - 1 < 0)
                                    M2 = valuesP1[valuesP1.GetLength(0) - 1, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                                else
                                    M2 = valuesP1[i - 1, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                            }
                            else if (this._c.getProjectionX(i * h, j * h, k * h) < 0)
                            {
                                M1 = valuesP1[i + 1, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                                M2 = valuesP1[i, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                            }

                            if (this._c.getProjectionY(i * h, j * h, k * h) > 0)
                            {
                                M3 = valuesP1[i, j, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * h * tau;
                                if (j - 1 < 0)
                                    M4 = valuesP1[i, valuesP1.GetLength(1) - 1, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * h * tau;
                                else
                                    M4 = valuesP1[i, j - 1, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * h * tau;
                            }
                            else if (this._c.getProjectionY(i * h, j * h, k * h) < 0)
                            {
                                M3 = valuesP1[i, j + 1, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * tau;
                                M4 = valuesP1[i, j, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * tau;
                            }

                            if (this._c.getProjectionZ(i * h, j * h, k * h) > 0)
                            {
                                M5 = valuesP1[i, j, k] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                                if (k - 1 < 0)
                                    M6 = valuesP1[i, j, valuesP1.GetLength(2) - 1] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                                else
                                    M6 = valuesP1[i, j, k - 1] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                            }
                            else if (this._c.getProjectionZ(i * h, j * h, k * h) < 0)
                            {
                                M5 = valuesP1[i, j, k + 1] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                                M6 = valuesP1[i, j, k] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
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
                            double S3 = (tau / (h * h)) * (_d.get(i * h, j * h, k * h, length) * (valuesP1[i + 1, j, k] - valuesP1[i, j, k]) -
                                                    _d.get(i * h, j * h, k * h, length) * (valuesP1[i, j, k] - valuesP1S3));
                            double S4 = (tau / (h * h)) * (_d.get(i * h, j * h, k * h, length) * (valuesP1[i, j + 1, k] - valuesP1[i, j, k]) -
                                                    _d.get(i * h, j * h, k * h, length) * (valuesP1[i, j, k] - valuesP1S4));
                            double S5 = (tau / (h * h)) * (K * (valuesP1[i, j, k + 1] - valuesP1[i, j, k]) - K * (valuesP1[i, j, k] - valuesP1S5));
                            // sources
                            double S6 = (this._q.get(i, j, k, t) - this._alpha.get(i, j, k, t) * valuesP1[i, j, k]) * tau;
                            // Implementation of a finite difference scheme             
                            valuesP2[i, j, k] = S1 - S2 + S3 + S4 + S5 + S6;
                            //Debug.WriteLine($"D: {_d.get(i * h, j * h, k * h, length)}");
                            //Debug.WriteLine($"C: {_c.getModule(i * h, j * h, k * h)}");
                            //Debug.WriteLine($"alpha: {_alpha}");
                            // System.Diagnostics.Debug.WriteLine(valuesP2[i, j, k]);
                            // Counting the number of volume points in each time layer
                            if (valuesP2[i, j, k] != 0)
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
                _numberPointsVolume.Add(currentPoints);
                _tValues.Add(t);
                if (t > (tMax + tau))
                    break;
            }
            this.tMax = t - tau;

            Debug.WriteLine($"{tau}");
            return 0;
        }


        public int getValues3(float tMax, float tStep, float h, float K, float length, int numberPatient, double[,,] valuesP2)
        {
            this._tValues = new List<float>();
            this._numberPointsVolume = new List<float>();
            int currentPoints = 0;
            int N = (int)(length / h);
            float t = 0;

            double[,,] valuesP1 = new double[N, N, N];
            float tau = 0;

            while (true)
            {
                currentPoints = 0;
                float[] minTau = {  (h * h / (2 * _d.get(0, 0, 0, length)) + h / Math.Abs(this._c.getProjectionX(0, 0, 0))),
                                    (h * h / (2 * _d.get(0, 0, 0, length)) + h / Math.Abs(this._c.getProjectionY(0, 0, 0))),
                                    (h * h / (2 * _d.get(0, 0, 0, length)) + h / Math.Abs(this._c.getProjectionZ(0, 0, 0)))};


                tau = K * minTau.Min();
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

                            if (this._c.getProjectionX(i * h, j * h, k * h) > 0)
                            {
                                M1 = valuesP1[i, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                                if (i - 1 < 0)
                                    M2 = valuesP1[valuesP1.GetLength(0) - 1, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                                else
                                    M2 = valuesP1[i - 1, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                            }
                            else if (this._c.getProjectionX(i * h, j * h, k * h) < 0)
                            {
                                M1 = valuesP1[i + 1, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                                M2 = valuesP1[i, j, k] * this._c.getProjectionX(i * h, j * h, k * h) * h * h * tau;
                            }

                            if (this._c.getProjectionY(i * h, j * h, k * h) > 0)
                            {
                                M3 = valuesP1[i, j, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * h * tau;
                                if (j - 1 < 0)
                                    M4 = valuesP1[i, valuesP1.GetLength(1) - 1, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * h * tau;
                                else
                                    M4 = valuesP1[i, j - 1, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * h * tau;
                            }
                            else if (this._c.getProjectionY(i * h, j * h, k * h) < 0)
                            {
                                M3 = valuesP1[i, j + 1, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * tau;
                                M4 = valuesP1[i, j, k] * this._c.getProjectionY(i * h, j * h, k * h) * h * tau;
                            }

                            if (this._c.getProjectionZ(i * h, j * h, k * h) > 0)
                            {
                                M5 = valuesP1[i, j, k] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                                if (k - 1 < 0)
                                    M6 = valuesP1[i, j, valuesP1.GetLength(2) - 1] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                                else
                                    M6 = valuesP1[i, j, k - 1] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                            }
                            else if (this._c.getProjectionZ(i * h, j * h, k * h) < 0)
                            {
                                M5 = valuesP1[i, j, k + 1] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
                                M6 = valuesP1[i, j, k] * this._c.getProjectionZ(i * h, j * h, k * h) * h * h * tau;
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
                            double S3 = (tau / (h * h)) * (_d.get(i * h, j * h, k * h, length) * (valuesP1[i + 1, j, k] - valuesP1[i, j, k]) -
                                                    _d.get(i * h, j * h, k * h, length) * (valuesP1[i, j, k] - valuesP1S3));
                            double S4 = (tau / (h * h)) * (_d.get(i * h, j * h, k * h, length) * (valuesP1[i, j + 1, k] - valuesP1[i, j, k]) -
                                                    _d.get(i * h, j * h, k * h, length) * (valuesP1[i, j, k] - valuesP1S4));
                            double S5 = (tau / (h * h)) * (K * (valuesP1[i, j, k + 1] - valuesP1[i, j, k]) - K * (valuesP1[i, j, k] - valuesP1S5));
                            // sources
                            double S6 = (this._q.get(i, j, k, t) - this._alpha.get(i, j, k, t) * valuesP1[i, j, k]) * tau;
                            // Implementation of a finite difference scheme             
                            valuesP2[i, j, k] = S1 - S2 + S3 + S4 + S5 + S6;

                            if (valuesP2[i, j, k] != 0)
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
                _numberPointsVolume.Add(currentPoints);
                _tValues.Add(t);
                string path = @"dataTumor\PredictData\PersonalPatients\Volume\txt\" + numberPatient.ToString() + "Volume.txt";
                ActionDataFile.writeDataToFile(valuesP2, path);
                if (t > (tMax + tau))
                    break;
            }
            this.tMax = t - tau;

            Debug.WriteLine($"{tau}");
            return 0;
        }


    }
}
