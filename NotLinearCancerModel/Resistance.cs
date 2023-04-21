using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class Resistance
    {
        /// <summary>
        /// Sources function
        /// </summary>
        private float _module;

        public Resistance(float module)
        {
            _module = module;
        }

        public float get(float x, float y, float z, float t)
        {
            float result = _module;

            return result;
        }
    }
}
