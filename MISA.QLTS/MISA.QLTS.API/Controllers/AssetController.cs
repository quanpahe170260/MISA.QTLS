using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.DTOs.Response;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Services;

namespace MISA.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ApiBaseController<Asset>
    {   
        private readonly IAssetService _assetService;
        public AssetController(IAssetService assetService) : base(assetService)
        {
            _assetService = assetService;
        }
        /// <summary>
        /// Hàm lấy API danh sách tài sản có phân trang và search, lọc
        /// </summary>
        /// <param name="search"></param>
        /// <param name="departmentID"></param>
        /// <param name="assetTypeID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// CreatedBy: QuanPA - 14/11/2025
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllAssetPaging([FromQuery] string? search, [FromQuery] string? departmentID, [FromQuery] string? assetTypeID, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(ApiResponse<PagedResult<AssetResponseDTO>>.Ok(await _assetService.GetPagedAsync(page, pageSize, departmentID, assetTypeID, search)));
        }
        /// <summary>
        /// Hàm lấy tổng tài sản
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: QuanPA - 20/11/2025
        [HttpGet("total")]
        public async Task<IActionResult> GetTotalAsset()
        {
            return Ok(ApiResponse<AssetTotalResponse>.Ok(await _assetService.GetTotalAsset()));
        }
    }
}
