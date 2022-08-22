namespace DBLayer;

public abstract class BaseEntity<R>
{
    /// <summary>
    /// 主键
    /// </summary>
    [DataField("id", IsAuto = true, IsKey = true, KeyType = KeyType.MANUAL)]
    public virtual R Id { get; set; } = default!;
    /// <summary>
    /// 创建用户
    /// </summary>
    [DataField("creater")]
    public string Creater { get; set; } = string.Empty;
    /// <summary>
    /// 创建时间
    /// </summary>
    [DataField("create_dt")]
    public DateTime CreateDt { get; set; } = DateTime.Now;
    /// <summary>
    /// 更新用户
    /// </summary>
    [DataField("updater")]
    public string Updater { get; set; } = string.Empty;
    /// <summary>
    /// 更新时间 
    /// </summary>
    [DataField("update_dt")]
    public DateTime UpdateDt { get; set; } = DateTime.Now;
}
public abstract class VirtulDelEntity<R> : BaseEntity<R>
{
    /// <summary>
    /// 是否删除
    /// </summary>
    [DataField("is_delete")]
    public bool IsDelete { get; set; }
}
