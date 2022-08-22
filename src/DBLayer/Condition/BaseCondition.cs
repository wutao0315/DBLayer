namespace DBLayer.Condition;

public class BaseCondition
{
    /// <summary>
    /// 当前页
    /// </summary>
    public int? PageIndex { get; set; }

    /// <summary>
    /// 页面大小
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// 页面提交的请求数
    /// </summary>
    public int? PageDraw { get; set; }
}
