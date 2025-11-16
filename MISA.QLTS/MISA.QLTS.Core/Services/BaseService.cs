using MISA.QLTS.Core.Exceptions;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;
using MISA.QLTS.Core.MISAAtribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {   
        private readonly IBaseRepository<T> _baseRepository;
        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<int> DeleteAsync(Guid entityId)
        {
            return await _baseRepository.DeleteAsync(entityId);
        }

        public async Task<int> DeleteMultipleAsync(List<Guid> entitiesId)
        {
            return await _baseRepository.DeleteMultipleAsync(entitiesId);
        }

        public async Task<string> GenerateCode()
        {
            return await _baseRepository.GenerateCode();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _baseRepository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(Guid entityId)
        {
            return await _baseRepository.GetByIdAsync(entityId);
        }

        public async Task<int> InsertAsync(T entity)
        {
            await ValidateData(entity);
            return await _baseRepository.InsertAsync(entity);
        }

        public async Task<int> UpdateAsync(T entity, Guid entityId)
        {   
            var entityExist = await _baseRepository.GetByIdAsync(entityId);
            if (entityExist == null) throw new NotFoundException($"{typeof(T).Name} not found");
            await ValidateData(entity);
            return await _baseRepository.UpdateAsync(entity, entityId);
        }

        /// <summary>
        /// Hàm validate dữ liệu
        /// </summary>
        /// <param name="entity">entity validate dữ liệu</param>
        /// <returns></returns>
        /// CreatedBy: QuanPA - 14/11/2025
        public async Task ValidateData(T entity)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                var requiredAttr = property.GetCustomAttributes(typeof(MISARequire), true);
                if(requiredAttr.Length > 0)
                {
                    if(value == null || value is string str && string.IsNullOrEmpty(str))
                    {
                        throw new ValidateException($"{property.Name} không được để trống");
                    }
                }

                var minMaxAttr = property.GetCustomAttribute<MISAMinMax>();
                if (minMaxAttr != null && value != null)
                {
                    double numericValue = 0;

                    if (value is int i) numericValue = i;
                    else if (value is decimal d) numericValue = (double)d;
                    else if (value is double db) numericValue = db;
                    else
                        throw new ValidateException($"{property.Name} không phải là số");

                    if (numericValue < minMaxAttr.Min || numericValue > minMaxAttr.Max)
                    {
                        throw new ValidateException($"{property.Name} phải nằm trong khoảng {minMaxAttr.Min} đến {minMaxAttr.Max}");
                    }
                }
            }
        }
    }
}
