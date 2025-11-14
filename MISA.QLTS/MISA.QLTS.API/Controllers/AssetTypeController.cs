using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Services;

namespace MISA.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypeController : ApiBaseController<AssetType>
    {
        public AssetTypeController(IBaseService<AssetType> baseService) : base(baseService)
        {
        }
    }
}
