using DBLayer.Core.Interface;
using System.Data;
using System.Data.Common;

namespace DBLayer.Persistence.Data;

/// <summary>
/// 数据库链接工厂类，负责生成和销毁数据库链接
/// </summary>
public class DbFactory : IDbFactory
{
    private readonly IConnectionString _connectionString;
    private AsyncLocal<DbConnection> _asyncConn = new ();
    private AsyncLocal<DbTransaction> _asyncTrans = new ();

    public DbFactory(IDbProvider dbProvider, IConnectionString connectionString)
    {
        DbProvider = dbProvider;
        _connectionString = connectionString;
    }
    /// <summary>
    /// 长连接
    /// </summary>
    public DbConnection? LongDbConnection
    {
        private set
        {
            _asyncConn.Value = value;
        }
        get
        {
            return _asyncConn.Value;
        }
    }

    /// <summary>
    /// 长连接的事物
    /// </summary>
    public DbTransaction? LongDbTransaction
    {
        private set
        {
            _asyncTrans.Value = value;
        }
        get
        {
            return _asyncTrans.Value;
        }
    }
    /// <summary>
    /// 数据库提供者
    /// </summary>
    public IDbProvider DbProvider { private set; get; }
    /// <summary>
    /// 数据库提供工厂
    /// </summary>
    public DbProviderFactory DbProviderFactory
    {
        get
        {
            var dbProviderFactory = DbProvider.GetDbProviderFactory();
            return dbProviderFactory;
        }
    }

    /// <summary>
    /// 短链接
    /// </summary>
    public DbConnection ShortDbConnection
    {
        get
        {
            var dbConnection = DbProvider.GetDbProviderFactory().CreateConnection();
            dbConnection.ConnectionString = _connectionString.ConnectionValue;
            dbConnection.Open();
            return dbConnection;
        }
    }

    /// <summary>
    /// 开启事务
    /// </summary>
    /// <returns></returns>
    public void BeginTransaction()
    {
        if (LongDbConnection == null)
        {
            LongDbConnection = DbProvider.GetDbProviderFactory().CreateConnection();
            LongDbConnection.ConnectionString = _connectionString.ConnectionValue;
            LongDbConnection.Open();
            LongDbTransaction = LongDbConnection.BeginTransaction();
        }
    }

    public void Dispose()
    {
        LongDbTransaction?.Dispose();
        if (LongDbConnection?.State != ConnectionState.Closed)
        {
            LongDbConnection?.Close();
        }
        LongDbConnection?.Dispose();
        LongDbTransaction = null;
        LongDbConnection = null;
    }
}
