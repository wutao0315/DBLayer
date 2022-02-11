using DBLayer.Core.Condition;
using System.Data.Common;
using System.Text;

namespace DBLayer.Core;

public class BasePager<T> where T : BaseCondition, new()
{
    public BasePager()
    {
        Where = new StringBuilder(" 1=1 ");
        Condition = new T();
        UnionText = string.Empty;
        Table = string.Empty;
        Field = "*";
        Group = string.Empty;
        Order = string.Empty;
        Parameters = new List<DbParameter>();
    }

    public StringBuilder Where { get; set; }
    public T Condition { get; set; }
    public string UnionText { get; set; }
    public string Table { get; set; }
    public string Field { get; set; }
    public string Group { get; set; }
    public string Order { get; set; }
    public Action<StringBuilder, IList<DbParameter>> WhereAction { get; set; }
    public IList<DbParameter> Parameters { get; set; }

    public void Execute()
    {
        WhereAction?.Invoke(Where, Parameters);
    }
}
public class Pager<T> : BasePager<T> where T : BasePageCondition, new()
{
    public void SetTotalCount(int totalCount)
    {
        Condition.TotalCount = totalCount;
        Condition.PageCount = (int)Math.Ceiling(Condition.TotalCount / (double)Condition.PageSize);
    }
}
