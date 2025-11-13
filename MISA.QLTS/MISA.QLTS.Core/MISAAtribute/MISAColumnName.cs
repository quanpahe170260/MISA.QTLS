using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.MISAAtribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAColumnName : Attribute
    {
        public string columnName { get; set; }
        public MISAColumnName(string columnName)
        {
            this.columnName = columnName;
        }
    }
}
