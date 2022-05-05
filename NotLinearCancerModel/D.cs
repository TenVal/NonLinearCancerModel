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
        private float d;
        private float c;

        public D(float d, float c)
        {
            this.d = d;
            this.c = c;
        }

        public float get(float x, float y, float z, float lenthX = 250, float lenthY = 250, float lenthZ = 250)
        {
            return this.d;
        }
    }
}
