using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class C
    {
        private float module;
        private float angleXY;
        private float angleZ;

        public C(float module, float angleXY, float angleZ)
        {
            this.module = module;
            this.angleXY = angleXY;
            this.angleZ = angleZ;
        }

        public float getModule(float x, float y, float z)
        {
            if (x > 0 && y > 0)
                return this.module;
            else
                return this.module;
        }

        public float getProjectionX(float x, float y, float z)
        {
            return (float)(this.getModule(x, y, z) * Math.Cos(this.angleXY));
        }

        public float getProjectionY(float x, float y, float z)
        {
            return (float)(this.getModule(x, y, z) * Math.Sin(this.angleXY));
        }

        public float getProjectionZ(float x, float y, float z)
        {
            return (float)(this.getModule(x, y, z) * Math.Sin(this.angleZ));
        }
    }
}
