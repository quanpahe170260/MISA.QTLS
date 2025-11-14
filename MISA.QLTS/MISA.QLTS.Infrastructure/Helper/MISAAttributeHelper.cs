using MISA.QLTS.Core.MISAAtribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Infrastructure.Helper
{
    public class MISAAttributeHelper<T> where T : class
    {
        private static readonly string _tableName = InitTableName();
        private static readonly string _primaryKey = InitPrimaryKey();
        /// <summary>
        /// Hàm lấy tên bảng database từ attribute của entity
        /// </summary>
        /// <returns>Tên bảng database</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        private static string InitTableName()
        {
            var tableName = typeof(T).Name;
            var attrTableName = typeof(T).GetCustomAttribute<MISATableName>();
            if (attrTableName != null)
            {
                tableName = attrTableName.TableName;
            }
            return tableName;
        }
        /// <summary>
        /// Hàm lấy khóa chính của bảng từ attribute của cột trong bảng
        /// </summary>
        /// <returns>Tên cột chứa khóa chính</returns>
        /// CreatedBy: QuanPA - 14/11/2025
        private static string InitPrimaryKey()
        {
            var type = typeof(T);
            var propKey = type.GetProperties().FirstOrDefault(prop => prop.GetCustomAttribute<MISAKey>() != null);
            if (propKey == null)
            {
                throw new InvalidOperationException(
                    $"Entity {type.Name} không có thuộc tính nào được đánh dấu bằng [MISAKey].");
            }
            var columnAttr = propKey.GetCustomAttribute<MISAColumnName>();
            var columnName = columnAttr?.ColumnName ?? propKey.Name;
            return columnName;
        }

        public static string GetTableName() => _tableName;
        public static string GetPrimaryKey() => _primaryKey;

    }
}
