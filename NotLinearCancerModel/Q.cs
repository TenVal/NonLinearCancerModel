using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class Q
    {
        private float module;

        public Q(float module)
        {
            this.module = module;
        }

        public float get(float x, float y, float z, float t)
        {
            if (x == 10 && y == 15 && z == 10)
            {
                return 10 * t;
            }
            else
            {
                return 0;
            }
        }
    }
}
