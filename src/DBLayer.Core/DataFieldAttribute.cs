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

public enum KeyType
{
    /// <summary>
    /// MANUAL identity generator
    /// </summary>
    MANUAL,
    /// <summary>
    /// 数据库 identity generator
    /// </summary>
    SEQ
}
/// <summary>
/// 自定义特性：映射数据库字段名
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field)]
public class DataFieldAttribute : Attribute
{
    //字段名
    private KeyType _keyType = KeyType.SEQ;

    /// <summary>
    /// 字段
    /// </summary>
    public string FieldName { get; set; } = default!;

    /// <summary>
    /// 是否是主键
    /// </summary>
    public bool IsKey { get; set; }

    /// <summary>
    /// 是不是自动生成
    /// </summary>
    public bool IsAuto { get; set; }

    /// <summary>
    /// 键的类型
    /// </summary>
    public KeyType KeyType
    {
        get { return _keyType; }
        set { _keyType = value; }
    }

    /// <summary>
    /// 默认值
    /// </summary>
    public object DefaultValue { get; set; } = default!;
    /// <summary>
    /// 映射数据库字段名
    /// </summary>
    public DataFieldAttribute() { }
    /// <summary>
    /// 映射数据库字段名
    /// </summary>
    /// <param name="fieldName">数据库字段名</param>
    public DataFieldAttribute(string fieldName)
    {
        this.FieldName = fieldName;
    }

}

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
public class DataTableAttribute : Attribute
{
    // Methods
    public DataTableAttribute(string tableName)
    {
        this.TableName = tableName;
    }

    // Properties
    public string TableName { get; set; }
    /// <summary>
    /// 唯一标识序列名称
    /// </summary>
    public string SequenceName { get; set; } = default!;
}
