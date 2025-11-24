using MISA.QLTS.Core.MISAAtribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    [MISATableName("asset_type")]
    public class AssetType
    {
        [MISAColumnName("asset_type_id")]
        [MISAKey]
        public Guid AssetTypeId { get; set; }
        [MISAColumnName("asset_type_code")]
        [MISAUnique("asset_type_code")]
        [MISARequire("Không được để trống")]
        public string AssetTypeCode { get; set; }
        [MISAColumnName("asset_type_name")]
        [MISARequire("Không được để trống")]
        public string AssetTypeName { get; set; }
        [MISAColumnName("year_of_use")]
        [MISARequire("Không được để trống")]
        public decimal YearOfUse { get; set; }
        [MISAColumnName("ware_rate")]
        [MISARequire("Không được để trống")]
        public decimal WearRate { get; set; }
        [MISAColumnName("asset_type_abbreviated")]
        public string AssetTypeAbbreviated { get; set; }
    }
}
