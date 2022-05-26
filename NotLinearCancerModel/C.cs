using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class C
    {
        /// <summary>
        /// Cancer rate function (module of speed, projections etc.)
        /// </summary>
        private float _module;
        private float _angleXY;
        private float _angleZ;

        public C(float module, float angleXY, float angleZ)
        {
            _module = module;
            _angleXY = angleXY;
            _angleZ = angleZ;
        }

        public float getModule(float x, float y, float z)
        {
            if (x > 0 && y > 0)
                return this._module;
            else
                return this._module;
        }

        public float getProjectionX(float x, float y, float z)
        {
            return (float)(this.getModule(x, y, z) * Math.Cos(this._angleXY));
        }

        public float getProjectionY(float x, float y, float z)
        {
            return (float)(this.getModule(x, y, z) * Math.Sin(this._angleXY));
        }

        public float getProjectionZ(float x, float y, float z)
        {
            return (float)(this.getModule(x, y, z) * Math.Sin(this._angleZ));
        }
    }
}
