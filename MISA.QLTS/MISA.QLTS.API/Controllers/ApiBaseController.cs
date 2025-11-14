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
        /// <summary>
        /// API lấy tất cả danh sách bản ghi
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(ApiResponse<IEnumerable<T>>.Ok(await _baseService.GetAllAsync()));
        }
        /// <summary>
        /// API lấy bản ghi theo Id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>Bản ghi được lấy theo ID</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            return Ok(ApiResponse<T?>.Ok(await _baseService.GetByIdAsync(id)));
        }
        /// <summary>
        /// API Thêm bản ghi
        /// </summary>
        /// <param name="entity">Bản ghi cần thêm</param>
        /// <returns>Số bản ghi được thêm vào database</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]T entity)
        {
            return Ok(ApiResponse<int>.Ok(await _baseService.InsertAsync(entity)));
        }
        /// <summary>
        /// API sửa bản ghi
        /// </summary>
        /// <param name="entity">dữ liệu bản ghi sửa</param>
        /// <param name="entityId">Id của bản ghi cần sửa</param>
        /// <returns>Số bản ghi được sửa</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody]T entity, [FromQuery]Guid entityId)
        {
            return Ok(ApiResponse<int>.Ok(await _baseService.UpdateAsync(entity, entityId)));
        }
        /// <summary>
        /// API xóa bản ghi
        /// </summary>
        /// <param name="entityId">Id của bản ghi cần xóa</param>
        /// <returns>Số bản ghi bị xóa</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery]Guid entityId)
        {
            return Ok(ApiResponse<int>.Ok(await _baseService.DeleteAsync(entityId)));
        }
        /// <summary>
        /// API xóa nhiều bản ghi
        /// </summary>
        /// <param name="lsId">Danh sách Id của bàn ghi cần xóa</param>
        /// <returns>Số bản ghi bị xóa</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        [HttpDelete]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<Guid> lsId)
        {
            return Ok(ApiResponse<int>.Ok(await _baseService.DeleteMultipleAsync(lsId)));
        }
    }
}
