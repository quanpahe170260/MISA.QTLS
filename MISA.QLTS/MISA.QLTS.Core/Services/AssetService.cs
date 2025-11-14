using MISA.QLTS.Core.DTOs.Response;
using MISA.QLTS.Core.Entities;
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
        public AssetService(IAssetRepository assetRepository) : base(assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<PagedResult<AssetResponseDTO>> GetPagedAsync(int page, int pageSize, string? departmentID, string assetTypeID, string? search)
        {
            return await _assetRepository.GetPagedAsync(page, pageSize, departmentID, assetTypeID, search);
        }
    }
}
