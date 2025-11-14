using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.DTOs.Response
{
    public class AssetResponseDTO
    {
        /// <summary>
        /// ID tài sản
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>
        public string AssetCode { get; set; }

        /// <summary>
        /// Tên tài sản 
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// Số lượng (số nguyên dương)
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Ngày mua tài sản
        /// </summary>
        public DateTime DatePurchase { get; set; }

        /// <summary>
        /// Nguyên giá (VNĐ)
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Giá trị hao mòn năm (tự động tính)
        /// </summary>
        public decimal DepreciationValueYear { get; set; }
        /// <summary>
        /// Id bộ phận sử dụng
        /// </summary>
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// Tên bộ phận
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Id loại tài sản
        /// </summary>
        public Guid AssetTypeId { get; set; }
        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string AssetTypeName { get; set; }
    }
}
