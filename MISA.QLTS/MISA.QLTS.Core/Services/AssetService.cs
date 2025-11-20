using MISA.QLTS.Core.DTOs.Response;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Exceptions;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Services
{
    public class AssetService : BaseService<Asset>, IAssetService
    {   
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetTypeRepository _assetTypeRepository;
        public AssetService(IAssetRepository assetRepository, IAssetTypeRepository assetTypeRepository) : base(assetRepository)
        {
            _assetRepository = assetRepository;
            _assetTypeRepository = assetTypeRepository;
        }

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
        public async Task<PagedResult<AssetResponseDTO>> GetPagedAsync(int page, int pageSize, string? departmentID, string assetTypeID, string? search)
        {
            return await _assetRepository.GetPagedAsync(page, pageSize, departmentID, assetTypeID, search);
        }

        /// <summary>
        /// Override hàm InsertAsync để thêm validate business logic
        /// </summary>
        public async Task<int> InsertAsync(Asset asset)
        {
            await ValidateData(asset);
            if (asset.DepreciationValueYear > asset.OriginalPrice)
            {
                throw new ValidateException("Hao mòn năm phải nhỏ hơn hoặc bằng nguyên giá");
            }
            return await _assetRepository.InsertAsync(asset);
        }
    }
}
