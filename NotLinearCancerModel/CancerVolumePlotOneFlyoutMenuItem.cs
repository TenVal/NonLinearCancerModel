using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    public class CancerVolumePlotOneFlyoutMenuItem
    {
        public CancerVolumePlotOneFlyoutMenuItem()
        {
            TargetType = typeof(CancerVolumePlotOneFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}