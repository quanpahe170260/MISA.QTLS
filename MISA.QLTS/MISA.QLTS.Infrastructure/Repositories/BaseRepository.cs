using Dapper;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.MISAAtribute;
using MISA.QLTS.Infrastructure.DapperContext;
using MISA.QLTS.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.QLTS.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext _context;
        public BaseRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<int> DeleteAsync(Guid entityId)
        {
            var tableName = MISAAttributeHelper<T>.GetTableName();
            var propKey = MISAAttributeHelper<T>.GetPrimaryKey();
            var sql = $"DELETE FROM {tableName} WHERE {propKey} = @id";
            using var connection = _context.CreateConnection();
            var row = await connection.ExecuteAsync(sql, new {Id = entityId});
            return row;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var tableName = MISAAttributeHelper<T>.GetTableName();
            var sql = $"SELECT * FROM {tableName}";
            using var connection = _context.CreateConnection();
            var data = await connection.QueryAsync<T>(sql);
            return data;
        }

        public async Task<T?> GetByIdAsync(Guid entityId)
        {
            var tableName = MISAAttributeHelper<T>.GetTableName();
            var propKey = MISAAttributeHelper<T>.GetPrimaryKey();
            var sql = $"SELECT * FROM {tableName} WHERE {propKey} = @entityId";
            using var connection = _context.CreateConnection();
            var data = await connection.QueryFirstOrDefaultAsync<T>(sql, new { entityId = entityId});
            return data;
        }

        public async Task<int> InsertAsync(T entity)
        {   
            var properties = typeof(T).GetProperties();
            var tableName = MISAAttributeHelper<T>.GetTableName();
            var columnBuilder = new StringBuilder();
            var columnParamBuilder = new StringBuilder();
            var parameters = new DynamicParameters();
            var propKey = MISAAttributeHelper<T>.GetPrimaryKey();
            var uniqueAttr = MISAAttributeHelper<T>.GetUniqueAttr();
            var codeProp = typeof(T).GetProperties()
                .FirstOrDefault(p =>
                {
                    var columnAttr = p.GetCustomAttribute<MISAColumnName>();
                    var columnName = columnAttr != null ? columnAttr.ColumnName : p.Name;
                    return columnName.Equals(uniqueAttr, StringComparison.OrdinalIgnoreCase);
                });

            var codeValue = codeProp?.GetValue(entity)?.ToString();

            if (!string.IsNullOrEmpty(codeValue))
            {
                var isExist = await CheckCodeExist(codeValue, Guid.Empty);
                if (isExist)
                {
                    throw new Exception($"Mã {codeValue} đã tồn tại, vui lòng chọn mã khác.");
                }
            }
            foreach (var property in properties)
            {
                var columnAttr = property.GetCustomAttribute<MISAColumnName>();
                var columnName = columnAttr != null ? columnAttr.ColumnName : property.Name;
                if(columnName.Equals(propKey))
                {
                    property.SetValue(entity, Guid.NewGuid());
                }
                columnBuilder.Append($"{columnName},");
                columnParamBuilder.Append($"@{columnName},");
                parameters.Add($"@{columnName}", property.GetValue(entity));
            }

            var columns = columnBuilder.ToString().TrimEnd(',',' ');
            var columnParams = columnParamBuilder.ToString().TrimEnd(',', ' ');
            var sql = $"INSERT INTO {tableName} ({columns}) VALUE ({columnParams})";
            using var connection = _context.CreateConnection();
            var row = await connection.ExecuteAsync(sql, parameters);
            return row;
        }

        public async Task<int> UpdateAsync(T entity, Guid entityId)
        {
            var tableName = MISAAttributeHelper<T>.GetTableName() ;
            var propKey = MISAAttributeHelper<T>.GetPrimaryKey() ;
            var uniqueAttr = MISAAttributeHelper<T>.GetUniqueAttr();
            var parameters = new DynamicParameters() ;
            var properties = typeof(T).GetProperties();
            var clauseBuilder = new StringBuilder();
            var codeProp = typeof(T).GetProperties()
                .FirstOrDefault(p =>
                {
                    var columnAttr = p.GetCustomAttribute<MISAColumnName>();
                    var columnName = columnAttr != null ? columnAttr.ColumnName : p.Name;
                    return columnName.Equals(uniqueAttr, StringComparison.OrdinalIgnoreCase);
                });

            var codeValue = codeProp?.GetValue(entity)?.ToString();

            if (!string.IsNullOrEmpty(codeValue))
            {
                var isExist = await CheckCodeExist(codeValue, entityId);
                if (isExist)
                {
                    throw new Exception($"Mã {codeValue} đã tồn tại, vui lòng chọn mã khác.");
                }
            }
            foreach (var  prop in properties)
            {
                var ketAttr = prop.GetCustomAttribute<MISAKey>();
                if(ketAttr != null)
                {
                    continue;
                }

                var columnAttr = prop.GetCustomAttribute<MISAColumnName>();
                var columnName = columnAttr != null ? columnAttr.ColumnName : prop.Name;
                clauseBuilder.Append($"{columnName} = @{columnName}, ");
                parameters.Add($"@{columnName}", prop.GetValue(entity));
            }
            var setClause = clauseBuilder.ToString().TrimEnd(',', ' ');
            parameters.Add("entityId", entityId);
            var sql = $"UPDATE {tableName} SET {setClause} WHERE {propKey} = @entityId";
            using var connection = _context.CreateConnection();
            var row = await connection.ExecuteAsync(sql, parameters);
            return row;
        }

        public async Task<string> GenerateCode()
        {   
            var tableName = MISAAttributeHelper<T>.GetTableName();
            var uniqueAttr = MISAAttributeHelper<T>.GetUniqueAttr();
            var sql = $"SELECT {uniqueAttr} FROM {tableName} WHERE {uniqueAttr} REGEXP '^TS[0-9]{{5,}}$'  order BY {uniqueAttr} DESC LIMIT 1";
            using var connection = _context.CreateConnection();
            var data = await connection.QueryFirstOrDefaultAsync<string>(sql);
            if(data == null)
            {
                return "TS00001";
            }
            var code = IncreaseCode(data);
            return code;
        }

        private async Task<bool> CheckCodeExist(string code, Guid entityId)
        {
            var tableName = MISAAttributeHelper<T>.GetTableName();
            var uniqueAttr = MISAAttributeHelper<T>.GetUniqueAttr();
            var propKey = MISAAttributeHelper<T>.GetPrimaryKey();
            var parameters = new DynamicParameters();
            var sql = $"SELECT * FROM {tableName} WHERE {uniqueAttr} = @code AND {propKey} <> @entityId" ;
            parameters.Add("@code", code);
            parameters.Add("@entityId", entityId);
            using var connection = _context.CreateConnection();
            var data = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
            if(data == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Hàm tăng mã code
        /// </summary>
        /// <param name="lastCode">Mã code lớn nhất</param>
        /// <returns>Mã code</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        private string IncreaseCode(string lastCode)
        {
            var match = Regex.Match(lastCode, @"(\d+)$");

            if (!match.Success)
                throw new Exception("Code format invalid");

            string numberPart = match.Value;              
            int number = int.Parse(numberPart);     
            number++;                         

            string newNumberPart = number.ToString(new string('0', numberPart.Length));

            return lastCode.Substring(0, lastCode.Length - numberPart.Length) + newNumberPart;
        }

        public async Task<int> DeleteMultipleAsync(List<Guid> entitiesId)
        {
            var tableName = MISAAttributeHelper<T>.GetTableName();
            var propKey = MISAAttributeHelper<T>.GetPrimaryKey();
            var lsIdBuilder = new StringBuilder();
            foreach(var entity in entitiesId)
            {
                lsIdBuilder.Append($"'{entity}', ");
            }
            var lsId = lsIdBuilder.ToString().TrimEnd(',',' ');
            var sql = $"DELETE FROM {tableName} WHERE {propKey} IN ({lsId})";
            var parameter = new DynamicParameters();
            using var connection = _context.CreateConnection();
            var row = await connection.ExecuteAsync(sql);
            return row;
        }
    }
}
