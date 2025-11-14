using MISA.QLTS.Core.MISAAtribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    [MISATableName("asset")]
    public class Asset
    {
        [MISAKey]
        [MISAColumnName("asset_id")]
        public Guid AssetId { get; set; }
        [MISAColumnName("asset_code")]
        [MISAUnique("mã tài sản")]
        [MISARequire("Không được để trống")]
        public string AssetCode { get; set; }
        [MISARequire("Không được để trống")]
        [MISAColumnName("asset_name")]
        public string AssetName { get; set; }
        [MISAColumnName("date_puschase")]
        [MISARequire("Không được để trống")]
        public DateTime DatePurchase { get; set; }
        [MISAColumnName("quantity")]
        [MISARequire("Không được để trống")]
        public decimal Quantity { get; set; }
        [MISAColumnName("original_price")]
        [MISARequire("Không được để trống")]
        public decimal OriginalPrice { get; set; }
        [MISAColumnName("depreciation_value_year")]
        [MISARequire("Không được để trống")]
        public decimal DepreciationValueYear { get; set; }
        [MISAColumnName("asset_type_id")]
        [MISARequire("Không được để trống")]
        public Guid AssetTypeId { get; set; }
        [MISAColumnName("department_id")]
        [MISARequire("Không được để trống")]
        public Guid DepartmentId { get; set; }

    }
}
