using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.MISAAtribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAMinMax : Attribute
    {
        public double Min { get; }
        public double Max { get; }

        public MISAMinMax(double min, double max)
        {
            Min = min;
            Max = max;
        }
    }
}
