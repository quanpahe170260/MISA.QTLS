using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.DTOs.Response;
using MISA.QLTS.Core.Interfaces.Services;

namespace MISA.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController<T> : ControllerBase where T : class
    {
        private readonly IBaseService<T> _baseService;
        public ApiBaseController(IBaseService<T> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(ApiResponse<IEnumerable<T>>.Ok(await _baseService.GetAllAsync()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            return Ok(ApiResponse<T?>.Ok(await _baseService.GetByIdAsync(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]T entity)
        {
            return Ok(ApiResponse<int>.Ok(await _baseService.InsertAsync(entity)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody]T entity, [FromQuery]Guid entityId)
        {
            return Ok(ApiResponse<int>.Ok(await _baseService.UpdateAsync(entity, entityId)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery]Guid entityId)
        {
            return Ok(ApiResponse<int>.Ok(await _baseService.DeleteAsync(entityId)));
        }
    }
}
