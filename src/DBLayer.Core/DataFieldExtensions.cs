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
namespace DBLayer.Core;

public static class DataFieldExtensions
{
    internal static bool IsExcluded(this IEnumerable<string> inclusionList, string propertyName)
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
    internal static (DataFieldAttribute?, string) GetDataFieldAttribute(this PropertyInfo prop)
    {
        var fieldName = prop.Name;
        var result = prop.GetCustomAttribute<DataFieldAttribute>(true);
        if (result != null)
        {
            fieldName = result.FieldName;
        }
        return (result, fieldName);

    }
    internal static string GetFieldName(this PropertyInfo member)
    {
        var fieldName = member.Name;

        var df = member.GetCustomAttribute<DataFieldAttribute>(true);
        if (df != null)
        {
            fieldName = df.FieldName;
        }
        return fieldName;
    }
    internal static string GetFieldName(this MemberInfo member)
    {
        var fieldName = member.Name;

        var df = member.GetCustomAttribute<DataFieldAttribute>(true);
        if (df != null)
        {
            fieldName = df.FieldName;
        }
        return fieldName;
    }
    /// <summary>
    ///  获取DataFieldAttribute特性
    /// </summary>
    /// <param name="propType">Class类</param>
    /// <returns>DataFieldAttribute</returns>
    internal static string GetDataTableName(this Type propType)
    {
        var tableName = propType.Name;
        var result = propType.GetCustomAttribute<DataTableAttribute>(true);
        if (result != null)
        {
            tableName = result.TableName;
        }
        return tableName;
    }
    /// <summary>
    ///  获取DataFieldAttribute特性
    /// </summary>
    /// <param name="entityType">Class类</param>
    /// <returns>DataFieldAttribute</returns>
    internal static (DataTableAttribute?, string) GetDataTableAttribute(this Type propType)
    {
        var tableName = propType.Name;
        var result = propType.GetCustomAttribute<DataTableAttribute>(true);
        if (result != null)
        {
            tableName = result.TableName;
        }
        return (result, tableName);
    }
}
