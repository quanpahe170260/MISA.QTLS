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
            foreach(var property in properties)
            {
                var propKey = MISAAttributeHelper<T>.GetPrimaryKey();
                if(propKey != null)
                {
                    property.SetValue(entity, Guid.NewGuid());
                }

                var columnAttr = property.GetCustomAttribute<MISAColumnName>();
                var columnName = columnAttr != null ? columnAttr.ColumnName : property.Name;
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
            var parameters = new DynamicParameters() ;
            var properties = typeof(T).GetProperties();
            var clauseBuilder = new StringBuilder();
            foreach(var  prop in properties)
            {
                var ignoreAttr = prop.GetCustomAttribute<MISAUnique>();
                var ketAttr = prop.GetCustomAttribute<MISAKey>();
                if(ignoreAttr != null)
                {
                    continue;
                }
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
    }
}
