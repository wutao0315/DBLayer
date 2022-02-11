using DBLayer.Core.Interface;
using DBLayer.Core.Logging;

namespace DBLayer.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private static readonly Func<Action<LogLevel, string, Exception>> Logger = () => LogManager.CreateLogger(typeof(UnitOfWork));
    private readonly IDbFactory _dbFactory;
    private AsyncLocal<int> _activeNumber = new AsyncLocal<int>();
    public UnitOfWork(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }
    /// <summary>
    /// 回调事件
    /// </summary>
    private readonly IList<Action> callBackActions = new List<Action>();

    /// <summary>
    /// 工作单元引用次数，当次数为0时提交，主要为了防止事物嵌套
    /// </summary>
    public int ActiveNumber
    {
        set
        {
            _activeNumber.Value = value;
        }
        get
        {
            return _activeNumber.Value;
        }
    }
    /// <summary>
    /// 是否开启工作单元
    /// </summary>
    private bool EnableUnitOfWork => _dbFactory != null;

    public void BeginTransaction()
    {
        if (this.ActiveNumber == 0 && EnableUnitOfWork)
        {
            _dbFactory.BeginTransaction();

            Logger().LogDebug("开启事务");
        }
        this.ActiveNumber++;
    }

    public void Commit()
    {
        this.ActiveNumber--;
        if (this.ActiveNumber == 0)
        {
            if (EnableUnitOfWork && _dbFactory.LongDbConnection != null)
            {
                var isCommitSuccess = false;
                try
                {
                    _dbFactory.LongDbTransaction.Commit();
                    isCommitSuccess = true;
                }
                catch (Exception e)
                {
                    _dbFactory.LongDbTransaction.Rollback();
                    isCommitSuccess = false;
                    Logger().LogError(e, e.ToString());
                }
                finally
                {
                    if (isCommitSuccess && this.callBackActions != null)
                    {
                        foreach (Action callBackAction in this.callBackActions)
                        {
                            callBackAction();
                        }
                    }
                    this.Dispose();
                }
            }

            Logger().LogDebug("提交事务");
        }

    }

    public void Dispose()
    {
        this.callBackActions.Clear();

        if (EnableUnitOfWork) _dbFactory.Dispose();
    }

    public void RegisterCallBack(Action action)
    {
        callBackActions.Add(action);
    }

    public void Rollback()
    {
        if (this.ActiveNumber > 0 && _dbFactory.LongDbTransaction != null && EnableUnitOfWork)
        {
            try
            {
                _dbFactory.LongDbTransaction.Rollback();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        this.Dispose();
        this.ActiveNumber--;
        Logger().LogDebug("回滚事务");
    }
}
