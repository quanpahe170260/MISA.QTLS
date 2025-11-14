using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {   
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách tất cả các bản ghi</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Lấy bản ghi theo id
        /// </summary>
        /// <param name="entityId">Id của bản ghi</param>
        /// <returns>Bản ghi theo id</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        Task<T?> GetByIdAsync(Guid entityId);
        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <param name="entity">Dữ liệu bản ghi thêm</param>
        /// <returns>Số bản ghi được thêm vào database (1 - thành công, 0 - thất bại)</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        Task<int> InsertAsync(T entity);
        /// <summary>
        /// Sửa bản ghi
        /// </summary>
        /// <param name="entity">Dữ liệu bản ghi sửa </param>
        /// <param name="entityId">Id bản ghi cần sửa </param>
        /// <returns>Số bản ghi được sửa trong database ( 1 - thành công, 0 - thất bại)</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        Task<int> UpdateAsync(T entity, Guid entityId);
        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="entityId">Id bản ghi cần xóa</param>
        /// <returns>Số bản ghi bị xóa trong database (1 - thành công, 0 - thất bại)</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        Task<int> DeleteAsync(Guid entityId);
        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="entitiesId">Danh sách Id bản ghi cần xóa</param>
        /// <returns>Số bản ghi bị xóa trong database (>= 1 - thành công, 0 - thất bại)</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        Task<int> DeleteMultipleAsync(List<Guid> entitiesId);

    }
}
