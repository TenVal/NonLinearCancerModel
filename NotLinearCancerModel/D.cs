using System;
using System.Collections.Generic;
using System.Linq
    
    
    
    ;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class D
    {
        /// <summary>
        /// Diffusion function
        /// </summary>
        private float _d;
        private float _c;

        public D(float d, float c)
        {
            this._d = d;
            this._c = c;
        }

        public float get(float x, float y, float z, float lengthX = 250, float lengthY = 250, float lengthZ = 250)
        {
            /*
            float length = lengthX;
            float resultD = this.d;
            
            if (x > length / 2 && y > length / 2 && z > length / 2)
            {
                return resultD * 2;
            }
            else if (x > length / 2 && y > length / 2 && z < length / 2)
            {
                return resultD * 2;
            }
            else if (x > length / 2 && y < length / 2 && z > length / 2)
            {
                return resultD;
            }
            else if (x > length / 2 && y < length / 2 && z < length / 2)
            {
                return resultD / 2;
            }
            else if (x < length / 2 && y > length / 2 && z > length / 2)
            {
                return resultD;
            }
            else if (x < length / 2 && y > length / 2 && z < length / 2)
            {
                return resultD * 2;
            }
            else if (x < length / 2 && y < length / 2 && z > length / 2)
            {
                return resultD;
            }
            else if (x < length / 2 && y < length / 2 && z < length / 2)
            {
                return resultD;
            }
            else
            {
                return resultD;
            }
            */
            return this._d;
        }
    }
}
