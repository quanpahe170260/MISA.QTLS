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
        [HttpGet("/paging")]
        public async Task<IActionResult> GetAllAssetPaging([FromQuery] string? search, [FromQuery] string? departmentID, [FromQuery] string? assetTypeID, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(ApiResponse<PagedResult<AssetResponseDTO>>.Ok(await _assetService.GetPagedAsync(page, pageSize, departmentID, assetTypeID, search)));
        }
    }
}
