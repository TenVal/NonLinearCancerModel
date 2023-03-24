using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class MethodDiffEquation
    {
        public List<List<float>> RK2D(float t, float x0, float y0, float right, float h, SystemOfEquation Eq)
        {
            List<List<float>> values = new List<List<float>>();
            List<float> xV = new List<float>();
            xV.Add(x0);
            List<float> yV = new List<float>();
            yV.Add(y0);
            List<float> tV = new List<float>();
            tV.Add(t);

            float x = x0;
            float y = y0;

            float a1;
            float b1;
            float a2;
            float b2;
            float a3;
            float b3;
            float a4;
            float b4;
            while (t <= right)
            {
                try
                {
                    a1 = h * Eq.dx(t, x, y);
                    b1 = h * Eq.dy(t, x, y);
                    a2 = h * Eq.dx(t + h / 2, x + a1 / 2, y + b1 / 2);
                    b2 = h * Eq.dy(t + h / 2, x + a1 / 2, y + b1 / 2);
                    a3 = h * Eq.dx(t + h / 2, x + a2 / 2, y + b2 / 2);
                    b3 = h * Eq.dy(t + h / 2, x + a2 / 2, y + b2 / 2);
                    a4 = h * Eq.dx(t + h, x + a3, y + b3);
                    b4 = h * Eq.dy(t + h, x + a3, y + b3);
                    x += (a1 + 2 * a2 + 2 * a3 + a4) / 6;
                    y += (b1 + 2 * b2 + 2 * b3 + b4) / 6;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"{e}");
                }
                t += h;
                xV.Add(x);
                yV.Add(y);
                tV.Add(t);
            }

            values.Add(tV);
            values.Add(xV);
            values.Add(yV);
            return values;
        }


        public List<List<float>> RK3D(float t, float x0, float y0, float z0, float right, float h, SystemOfEquation Eq)
        {
            List<List<float>> values = new List<List<float>>();
            List<float> tV = new List<float>();
            tV.Add(t);
            List<float> xV = new List<float>();
            xV.Add(x0);
            List<float> yV = new List<float>();
            yV.Add(y0);
            List<float> zV = new List<float>();
            zV.Add(z0);


            float x = x0;
            float y = y0;
            float z = z0;

            float a1;
            float b1;
            float c1;
            float a2;
            float b2;
            float c2;
            float a3;
            float b3;
            float c3;
            float a4;
            float b4;
            float c4;
            while (t <= right)
            {
                try
                {
                    a1 = h * Eq.dx(t, x, y, z);
                    b1 = h * Eq.dy(t, x, y, z);
                    c1 = h * Eq.dz(t, x, y, z);
                    a2 = h * Eq.dx(t + h / 2, x + a1 / 2, y + b1 / 2, z + c1 / 2);
                    b2 = h * Eq.dy(t + h / 2, x + a1 / 2, y + b1 / 2, z + c1 / 2);
                    c2 = h * Eq.dz(t + h / 2, x + a1 / 2, y + b1 / 2, z + c1 / 2);
                    a3 = h * Eq.dx(t + h / 2, x + a2 / 2, y + b2 / 2, z + c2 / 2);
                    b3 = h * Eq.dy(t + h / 2, x + a2 / 2, y + b2 / 2, z + c2 / 2);
                    c3 = h * Eq.dz(t + h / 2, x + a2 / 2, y + b2 / 2, z + c2 / 2);
                    a4 = h * Eq.dx(t + h, x + a3, y + b3, z + c3);
                    b4 = h * Eq.dy(t + h, x + a3, y + b3, z + c3);
                    c4 = h * Eq.dy(t + h, x + a3, y + b3, z + c3);
                    x += (a1 + 2 * a2 + 2 * a3 + a4) / 6;
                    y += (b1 + 2 * b2 + 2 * b3 + b4) / 6;
                    z += (c1 + 2 * c2 + 2 * c3 + c4) / 6;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"{e}");
                }
                t += h;
                xV.Add(x);
                yV.Add(y);
                zV.Add(y);
                tV.Add(t);
            }

            values.Add(tV);
            values.Add(xV);
            values.Add(yV);
            values.Add(zV);
            return values;
        }
    }
}
