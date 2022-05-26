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
using System.Globalization;

namespace NotLinearCancerModel
{
    class Q
    {
        /// <summary>
        /// Sources function
        /// </summary>
        private float _module;

        public Q(float module)
        {
            _module = module;
        }

        public float get(float x, float y, float z, float t)
        {
            float result;
            if (x == 10 && y == 15 && z == 10)
            {
                float a = (float) 10e-1;
                // result = 1 / (a * t);
                result = (float)(Math.Log(x) + 1 / (x));
                // result = (float)(-Math.Exp(-t) + 1);
                //result = (float)(Math.Log(t + 1));
                //result = (float)(1 / t);
            }
            else
            {
                result = 0;
            }
            return result;
        }
    }
}
