using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class SystemOfEquation
    {
        private float l1;
        private float b;
        private float d;
        private float e;
        private float l3;
        private float u;

        public SystemOfEquation(float l1, float b, float d, float e, float l3, float  u)
        {
            this.l1 = l1;
            this.b = b;
            this.d = d;
            this.e = e;
            this.l3 = l3;
            this.u = u;
        }

        public float dxR(float t, float x, float y, float z = 0)
        {
            float value = 0;
            try
            {
                value =  (float)(-this.l1 * x * Math.Log(4 * Math.PI * (x * x * x) / (3 * y)) / 3);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e}");
            }
            return value;
        }

        public float dyR(float t, float x, float y, float z = 0)
        {
            float value = 0;
            try
            {
                value = (float)(this.b * 4 * Math.PI * (x * x * x) / 3 - this.d * (Math.Pow(4 * Math.PI / 3, 2 / 3)) * (x * x) * y - this.e * z * y);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e}");
            }
            return value;
        }

        public float dx(float t, float x, float y, float z = 0)
        {
            float value = 0;
            value = -this.l3 * z + this.u;
            return value;
        }

        public float dy(float t, float x, float y, float z = 0)
        {
            float value = 0;
            try
            {
                value = (float)(this.b * x - this.d * Math.Pow(x, 2 / 3) * y - this.e * z * y);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e}");
            }
            return value;
        }

        public float dz(float t, float x, float y, float z = 0)
        {
            float value = 0;
            try
            {
                value = (float)(-this.l1 * x * Math.Log(x / y));
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e}");
            }
            return value;
        }
    }
}
