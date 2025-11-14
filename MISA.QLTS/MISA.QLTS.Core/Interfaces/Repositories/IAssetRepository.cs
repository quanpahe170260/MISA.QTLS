using MISA.QLTS.Core.DTOs.Response;
using MISA.QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Repositories
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {   
        /// <summary>
        /// Hàm lấy danh sách tài sản theo tìm kiếm và phân trang
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="departmentID">ID bộ phận cần lọc</param>
        /// <param name="assetTypeID">ID loại tài sản cần lọc</param>
        /// <param name="search">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách tài sản theo tìm kiếm, phân trang</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        Task<PagedResult<AssetResponseDTO>> GetPagedAsync(int page, int pageSize, string? departmentID, string assetTypeID, string? search);
    }
}
