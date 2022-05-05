using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class Q
    {
        /// <summary>
        /// Sources function
        /// </summary>
        private float module;

        public Q(float module)
        {
            this.module = module;
        }

        public float get(float x, float y, float z, float t)
        {
            if (x == 10 && y == 15 && z == 10)
            {
                return (float)Math.Log(t) + 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
