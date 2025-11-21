using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.DTOs.Response
{
    public class AssetTotalResponse
    {
        public decimal TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDepreciation { get; set; }
        public decimal TotalRemain { get; set; }
    }
}
