using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.MISAAtribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAUnique : Attribute
    {
        public string UniqueName { get; set; }
        public MISAUnique(string uniqueName)
        {
            UniqueName = uniqueName;
        }
    }
}
