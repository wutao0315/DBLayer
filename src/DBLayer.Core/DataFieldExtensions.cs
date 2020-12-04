using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
/*------------------------------------------------------------------------------
* 单元名称：
* 单元描述：
* 创建人：
* 创建日期：
* 修改日志
* 修改人   修改日期    修改内容
* 
* ----------------------------------------------------------------------------*/
namespace DBLayer.Core
{
    public static class DataFieldExtensions
    {
        public static bool IsExcluded(this IEnumerable<string> inclusionList, string propertyName)
        {
            //var isSuccess = inclusionList?.Contains(propertyName, StringComparer.OrdinalIgnoreCase)??false;
            //return !isSuccess;

            var pn = inclusionList?.FirstOrDefault(w => w.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
            var isSuccess = string.IsNullOrWhiteSpace(pn);
            return isSuccess;
        }

        /// <summary>
        ///  获取DataFieldAttribute特性
        /// </summary>
        /// <param name="entityType">Class类</param>
        /// <returns>DataFieldAttribute</returns>
        public static DataFieldAttribute GetDataFieldAttribute(this PropertyInfo prop, out string fieldName)
        {
            fieldName = prop.Name;
            var result = prop.GetCustomAttribute<DataFieldAttribute>(true);
            if (result != null) 
            {
                fieldName = result.FieldName;
            }
            return result;
            
        }
       

        /// <summary>
        ///  获取DataFieldAttribute特性
        /// </summary>
        /// <param name="entityType">Class类</param>
        /// <returns>DataFieldAttribute</returns>
        public static DataTableAttribute GetDataTableAttribute(this Type propType, out string tableName)
        {
            tableName = propType.Name;
            var result = propType.GetCustomAttribute<DataTableAttribute>(true);
            if (result != null)
            {
                tableName = result.TableName;
            }
            return result;
        }
    }
}
