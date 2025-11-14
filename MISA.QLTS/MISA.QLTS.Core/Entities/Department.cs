using MISA.QLTS.Core.MISAAtribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    [MISATableName("department")]
    public class Department
    {
        [MISAKey]
        [MISAColumnName("deparment_id")]
        public Guid DepartmentId { get; set; }
        [MISAColumnName("department_code")]
        [MISAUnique("department_code")]
        public string DepartmentCode { get; set; }
        [MISAColumnName("department_name")]
        public string DepartmentName { get; set; }
        [MISAColumnName("department_abbreviated")]
        public string DepartmentAbbreviated { get; set; }
    }
}
