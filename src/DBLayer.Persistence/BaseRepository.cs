using DBLayer;
using DBLayer.Condition;
using DBLayer.Interface;
using DBLayer.Logging;
using DBLayer.Utilities;
using DBLayer.Persistence.Linq;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

/*------------------------------------------------------------------------------
 * 单元名称：
 * 单元描述： 
 * 创 建 人：wutao
 * 创建日期：2011-11-19
 * 修改日志
 * 修 改 人   修改日期    修改内容
 * 
 * ----------------------------------------------------------------------------*/
namespace DBLayer.Persistence;

public abstract class BaseRepository : IRepository
{
    private static readonly Func<Action<LogLevel, string, Exception>> Logger = () => LogManager.CreateLogger(typeof(BaseRepository));
    public IUnitOfWork Uow { get; }

    internal readonly IDataSource _dataSource;
    private readonly ClaimsPrincipal? _user;

    public BaseRepository(IDbContext dbContext)
    {
        Uow = dbContext.Uow;
        _dataSource = dbContext.DataSource;
        _generator = dbContext.Generator;
        _pagerGenerator = dbContext.PagerGenerator;
        _user = dbContext.User;
    }
    public IDataSource DataSource => _dataSource;
    public IQueryable<T> Queryable<T>()
    {
        return new SqlQueryable<T>(this);
    }
    #region public method
    protected readonly IGenerator _generator;
    protected readonly IPagerGenerator _pagerGenerator;
    #region GetEntity

    /// <summary>
    /// 获取 T 单个实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="order"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public T GetEntity<T>(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
         {
             var orderStr = _dataSource.Order(order);
             var result = _pagerGenerator.GetSelectCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, 1, inclusionList);
             return result.ToString();
         });

        T entity = GetEntity<T>(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);
        return entity;
    }

    /// <summary>
    /// 获取 T 单个实体
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    public T GetEntity<T>(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList)
        where T : new()
    {
        T entity = GetEntity<T>(cmdText, CommandType.Text, paramers, inclusionList);

        return entity;
    }
    /// <summary>
    /// 获得单个对象
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="cmdText">sql</param>
    /// <param name="obj">参数</param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public T GetEntity<T>(string cmdText, object obj, params string[] inclusionList)
        where T : new()
    {
        var parameter = _dataSource.ToDbParameters(obj);

        T entity = GetEntity<T>(cmdText, CommandType.Text, parameter, inclusionList);
        return entity;
    }
    /// <summary>
    /// 获取 T 单个实体
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    public T GetEntity<T>(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList)
        where T : new()
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        T entity = default!;
        using var reader = _dataSource.CreateDataReader(cmdText, conn, commandType, paramers);
        if (reader.Read())
        {
            entity = reader.ReadObject<T>(inclusionList);
        }

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }

        return entity;
    }

    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="order"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> where,
        Expression<Func<OrderExpression<T>, object>>? order = null,
        params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var result = _pagerGenerator.GetSelectCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, 1, inclusionList);
            return result.ToString();
        });

        var entity = await GetEntityAsync<T>(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);
        return entity;
    }

    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="cmdText">sql</param>
    /// <param name="obj">参数</param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public async Task<T> GetEntityAsync<T>(string cmdText, object obj, params string[] inclusionList)
        where T : new()
    {
        var parameter = _dataSource.ToDbParameters(obj);

        var entity = await GetEntityAsync<T>(cmdText, CommandType.Text, parameter, inclusionList);
        return entity;
    }

    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    public async Task<T> GetEntityAsync<T>(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList)
        where T : new()
    {
        var entity = await GetEntityAsync<T>(cmdText, CommandType.Text, paramers, inclusionList);

        return entity;
    }

    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    public async Task<T> GetEntityAsync<T>(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList)
        where T : new()
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;


        T entity = default!;
        using var reader = await _dataSource.CreateDataReaderAsync(cmdText, conn, commandType, paramers);
        if (reader.Read())
        {
            entity = reader.ReadObject<T>(inclusionList);
        }

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        return entity;
    }

    /// <summary>
    /// 获取 查询 数量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public int GetEntityCount<T>(Expression<Func<T, bool>> where)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var text = GetCountCmdText<T>(whereStr);
            return text.ToString();
        });

        var result = this.ExecuteScalar<int>(cmdText.ToString(), CommandType.Text, paramerArray);
        return result;
    }

    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <returns></returns>
    public async Task<int> GetEntityCountAsync<T>(Expression<Func<T, bool>> where)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var text = GetCountCmdText<T>(whereStr);
            return text.ToString();
        });
        var result = await this.ExecuteScalarAsync<int>(cmdText.ToString(), CommandType.Text, paramerArray);
        return result;
    }
    #endregion

    #region GetEntityDic
    /// <summary>
    /// 获得单个实体字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public IDictionary<string, object> GetEntityDic<T>(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var text = _pagerGenerator.GetSelectDictionaryCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, 1, inclusionList);
            return text.ToString();
        });

        var result = GetEntityDic(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);
        return result;
    }


    /// <summary>
    /// 获得字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public IDictionary<string, object> GetEntityDic(string cmdText, DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var returnDictionary = GetEntityDic(cmdText, CommandType.Text, parameters, inclusionList);

        return returnDictionary;
    }

    /// <summary>
    /// 获得字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数</param>
    /// <param name="inclusionList">事物</param>
    /// <returns></returns>
    public IDictionary<string, object> GetEntityDic(string cmdText, object obj, params string[] inclusionList)
    {
        var parameter = _dataSource.ToDbParameters(obj);

        var returnDictionary = GetEntityDic(cmdText, CommandType.Text, parameter, inclusionList);
        return returnDictionary;
    }

    /// <summary>
    /// 获得字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public IDictionary<string, object> GetEntityDic(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? parameters = null, params string[] inclusionList)
    {

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        IDictionary<string, object> entity = new Dictionary<string, object>();
        using var reader = _dataSource.CreateDataReader(cmdText, conn, commandType, parameters);
        if (reader.Read())
        {
            entity = reader.ReadSelf(inclusionList);
        }

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }

        return entity;
    }


    /// <summary>
    /// 获得单个实体字典异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<IDictionary<string, object>> GetEntityDicAsync<T>(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var text = _pagerGenerator.GetSelectDictionaryCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, 1, inclusionList);
            return text.ToString();
        });

        var result = await GetEntityDicAsync(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);
        return result;
    }
    /// <summary>
    /// 获得字典异步
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public async Task<IDictionary<string, object>> GetEntityDicAsync(string cmdText, DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var returnDictionary = await GetEntityDicAsync(cmdText, CommandType.Text, parameters, inclusionList);

        return returnDictionary;
    }
    /// <summary>
    /// 获得字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数</param>
    /// <param name="inclusionList">事物</param>
    /// <returns></returns>
    public async Task<IDictionary<string, object>> GetEntityDicAsync(string cmdText, object obj, params string[] inclusionList)
    {
        var parameter = _dataSource.ToDbParameters(obj);

        var returnDictionary = await GetEntityDicAsync(cmdText, CommandType.Text, parameter, inclusionList);
        return returnDictionary;
    }
    /// <summary>
    /// 获得字典异步
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public async Task<IDictionary<string, object>> GetEntityDicAsync(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        IDictionary<string, object> entity = new Dictionary<string, object>();
        using var reader = await _dataSource.CreateDataReaderAsync(cmdText, conn, commandType, parameters);
        if (reader.Read())
        {
            entity = reader.ReadSelf(inclusionList);
        }

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        return entity;
    }

    #endregion

    #region GetEntityDicList

    /// <summary>
    /// 获得实体字典列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, object>> GetEntityDicList<T>(Expression<Func<T, bool>>? where = null,
        Expression<Func<OrderExpression<T>, object>>? order = null,
        int? top = null,
        params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var text = _pagerGenerator.GetSelectDictionaryCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, top, inclusionList);
            return text.ToString();
        });

        var result = GetEntityDicList(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);
        return result;
    }


    /// <summary>
    /// 获得字典列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, object>> GetEntityDicList(string cmdText,

        DbParameter[]? parameters = null,
        params string[] inclusionList)
    {
        var result = GetEntityDicList(cmdText, CommandType.Text, parameters, inclusionList);

        return result;
    }

    /// <summary>
    /// 获得字典列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, object>> GetEntityDicList(string cmdText, object? obj = null, params string[] inclusionList)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var entityDicList = GetEntityDicList(cmdText, CommandType.Text, parameters, inclusionList);

        return entityDicList;
    }
    /// <summary>
    /// 获得字典列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="parameters">参数数组</param>
    /// <exception cref="">这里是 Exception</exception>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, object>> GetEntityDicList(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = _dataSource.CreateDataReader(cmdText, conn, commandType, parameters);
        var result = reader.ReadList(r => r.ReadSelf(inclusionList));

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }

        return result;
    }


    /// <summary>
    /// 获得实体字典列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync<T>(Expression<Func<T, bool>>? where = null,
        Expression<Func<OrderExpression<T>, object>>? order = null,
        int? top = null,
        params string[] inclusionList)
        where T : new()
    {

        var paramerList = new List<DbParameter>();
        var whereStr = _dataSource.Where(where, ref paramerList);
        var orderStr = _dataSource.Order(order);

        var cmdText = _pagerGenerator.GetSelectDictionaryCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, top, inclusionList);

        var result = await GetEntityDicListAsync(cmdText.ToString(), CommandType.Text, paramerList.ToArray(), inclusionList);
        return result;
    }

    /// <summary>
    /// 获得字典列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync(string cmdText,

        DbParameter[]? parameters = null,
        params string[] inclusionList)
    {
        var result = await GetEntityDicListAsync(cmdText, CommandType.Text, parameters, inclusionList);

        return result;
    }
    /// <summary>
    /// 获得字典列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync(string cmdText,
        object obj,

        params string[] inclusionList)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var entityDicList = await GetEntityDicListAsync(cmdText, CommandType.Text, parameters, inclusionList);

        return entityDicList;
    }
    /// <summary>
    /// 获得字典列表 异步
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="parameters">参数数组</param>
    /// <exception cref="">这里是 Exception</exception>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync(string cmdText,

        CommandType commandType = CommandType.Text,
        DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = await _dataSource.CreateDataReaderAsync(cmdText, conn, commandType, parameters);
        var result = reader.ReadList(r => r.ReadSelf(inclusionList));

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        return result;
    }
    #endregion

    #region GetEntityList
    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="top"></param>
    /// <returns></returns>
    public IEnumerable<T> GetEntityList<T>(Expression<Func<T, bool>>? where = null,
        Expression<Func<OrderExpression<T>, object>>? order = null,

        int? top = null,
        params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var text = _pagerGenerator.GetSelectDictionaryCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, top, inclusionList);
            return text.ToString();
        });

        var entityList = GetEntityList<T>(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);

        return entityList;
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="exeEntityType">按属性/特性映射</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>实体集合</returns>
    public IEnumerable<T> GetEntityList<T>(string cmdText,

        DbParameter[]? paramers = null, params string[] inclusionList)
        where T : new()
    {
        var entityList = GetEntityList<T>(cmdText, CommandType.Text, paramers, inclusionList);

        return entityList;
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="exeEntityType">按属性/特性映射</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>实体集合</returns>
    public IEnumerable<T> GetEntityList<T>(string cmdText, object obj, params string[] inclusionList)
        where T : new()
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var entityList = GetEntityList<T>(cmdText, CommandType.Text, parameters, inclusionList);

        return entityList;
    }

    /// <summary>
    /// 获取 T 实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体集合</returns>
    public IEnumerable<T> GetEntityList<T>(string cmdText,

        CommandType commandType = CommandType.Text,
        DbParameter[]? parameters = null, params string[] inclusionList) where T : new()
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = _dataSource.CreateDataReader(cmdText, conn, commandType, parameters);
        var result = reader.ReadList(r => r.ReadObject<T>(inclusionList));

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }

        return result;
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="top"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetEntityListAsync<T>(Expression<Func<T, bool>>? where = null,
        Expression<Func<OrderExpression<T>, object>>? order = null,

        int? top = null,
        params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var text = _pagerGenerator.GetSelectCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, top, inclusionList);
            return text.ToString();
        });

        var result = await GetEntityListAsync<T>(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);

        return result;
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="exeEntityType">按属性/特性映射</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>实体集合</returns>
    public async Task<IEnumerable<T>> GetEntityListAsync<T>(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList)
        where T : new()
    {
        var result = await GetEntityListAsync<T>(cmdText, CommandType.Text, paramers, inclusionList);

        return result;
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="exeEntityType">按属性/特性映射</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>实体集合</returns>
    public async Task<IEnumerable<T>> GetEntityListAsync<T>(string cmdText, object obj, params string[] inclusionList)
        where T : new()
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var result = await GetEntityListAsync<T>(cmdText, CommandType.Text, parameters, inclusionList);

        return result;
    }

    /// <summary>
    /// 获取 T 实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体集合</returns>
    public async Task<IEnumerable<T>> GetEntityListAsync<T>(string cmdText,

        CommandType commandType = CommandType.Text,
        DbParameter[]? parameters = null, params string[] inclusionList)
        where T : new()
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = await _dataSource.CreateDataReaderAsync(cmdText, conn, commandType, parameters);
        var result = reader.ReadList(r => r.ReadObject<T>(inclusionList));

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        return result;
    }
    #endregion

    #region GetScalar
    /// <summary>
    /// 返回首行首列
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>返回首行首列</returns>
    public T GetSingle<T>(object obj)
    {
        T single;
        try
        {
            single = (T)obj.ChangeType(typeof(T));
        }
        catch (Exception ex)
        {
            Logger().LogError(ex, ex.GetDetailMessage());
            throw;
        }

        return single;
    }
    #endregion

    #region GetList

    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    public IEnumerable<T> GetList<T>(string cmdText, params DbParameter[] paramers)
    {
        return GetList<T>(cmdText, CommandType.Text, paramers);
    }

    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>数据集合</returns>
    public IEnumerable<T> GetList<T>(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        return GetList<T>(cmdText, CommandType.Text, parameters);
    }

    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    public IEnumerable<T> GetList<T>(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[]? parameters)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = _dataSource.CreateDataReader(cmdText, conn, commandType, parameters);
        var result = reader.ReadList(r =>
        {
            var data = r[0];
            if (data is not DBNull)
            {
                var entity = GetSingle<T>(data);
                return entity;
            }
            return default;
        });

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }

        return result!;
    }
    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns>数据集合</returns>
    public async Task<IEnumerable<T>> GetListAsync<T>(string cmdText, params DbParameter[] parameters)
    {
        var result = await GetListAsync<T>(cmdText, CommandType.Text, parameters);
        return result;
    }
    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>数据集合</returns>
    public async Task<IEnumerable<T>> GetListAsync<T>(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var result = await GetListAsync<T>(cmdText, CommandType.Text, parameters);
        return result;
    }
    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="parameters">参数数组</param>
    /// <returns>数据集合</returns>
    public async Task<IEnumerable<T>> GetListAsync<T>(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[]? parameters)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = await _dataSource.CreateDataReaderAsync(cmdText, conn, commandType, parameters);
        var result = reader.ReadList(r =>
        {
            var data = r[0];
            if (data is not DBNull)
            {
                var entity = GetSingle<T>(data);
                return entity;
            }
            return default;
        });

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        return result!;
    }
    #endregion

    #region GetEntityDicStr
    /// <summary>
    /// 获得单个实体字符串字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public IDictionary<string, string> GetEntityDicStr<T>(Expression<Func<T, bool>> where,
        Expression<Func<OrderExpression<T>, object>> order,

        params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var text = _pagerGenerator.GetSelectDictionaryCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, 1, inclusionList);
            return text.ToString();
        });

        var result = GetEntityDicStr(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);
        return result;
    }

    /// <summary>
    /// 获得字符串字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public IDictionary<string, string> GetEntityDicStr(string cmdText,

        DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var returnDictionary = GetEntityDicStr(cmdText, CommandType.Text, parameters, inclusionList);

        return returnDictionary;
    }
    /// <summary>
    /// 获得字符串字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public IDictionary<string, string> GetEntityDicStr(string cmdText, object obj, params string[] inclusionList)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var returnDictionary = GetEntityDicStr(cmdText, CommandType.Text, parameters, inclusionList);

        return returnDictionary;
    }

    /// <summary>
    /// 获得字符串字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public IDictionary<string, string> GetEntityDicStr(string cmdText,

        CommandType commandType = CommandType.Text,
        DbParameter[]? parameters = null,
        params string[] inclusionList)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        IDictionary<string, string> result = new Dictionary<string, string>();
        using var reader = _dataSource.CreateDataReader(cmdText, conn, commandType, parameters);
        if (reader.Read())
        {
            result = reader.ReadSelfString(inclusionList);
        }

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }


    /// <summary>
    /// 获得单个实体字符串字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<IDictionary<string, string>> GetEntityDicStrAsync<T>(Expression<Func<T, bool>> where,
        Expression<Func<OrderExpression<T>, object>> order,

        params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var text = _pagerGenerator.GetSelectDictionaryCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, 1, inclusionList);
            return text.ToString();
        });

        var result = await GetEntityDicStrAsync(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);
        return result;
    }


    /// <summary>
    /// 获得字符串字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public async Task<IDictionary<string, string>> GetEntityDicStrAsync(string cmdText, DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var result = await GetEntityDicStrAsync(cmdText, CommandType.Text, parameters, inclusionList);

        return result;
    }
    /// <summary>
    /// 获得字符串字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public async Task<IDictionary<string, string>> GetEntityDicStrAsync(string cmdText, object obj, params string[] inclusionList)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var result = await GetEntityDicStrAsync(cmdText, CommandType.Text, parameters, inclusionList);

        return result;
    }
    /// <summary>
    /// 获得字符串字典
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public async Task<IDictionary<string, string>> GetEntityDicStrAsync(string cmdText,

        CommandType commandType = CommandType.Text,
        DbParameter[]? parameters = null, params string[] inclusionList)
    {

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        IDictionary<string, string> result = new Dictionary<string, string>();
        using var reader = await _dataSource.CreateDataReaderAsync(cmdText, conn, commandType, parameters);
        if (reader.Read())
        {
            result = reader.ReadSelfString(inclusionList);
        }

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }
        return result;
    }

    #endregion

    #region GetEntityDicStrList
    /// <summary>
    /// 获得实体字典字符串列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, string>> GetEntityDicStrList<T>(Expression<Func<T, bool>>? where = null,
        Expression<Func<OrderExpression<T>, object>>? order = null,

        int? top = null,
        params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var text = _pagerGenerator.GetSelectDictionaryCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, top, inclusionList);
            return text.ToString();
        });

        var result = GetEntityDicStrList(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);
        return result;
    }

    /// <summary>
    /// 获得典字符串列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, string>> GetEntityDicStrList(string cmdText, DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var entityDicList = GetEntityDicStrList(cmdText, CommandType.Text, parameters, inclusionList);

        return entityDicList;
    }
    /// <summary>
    /// 获得典字符串列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, string>> GetEntityDicStrList(string cmdText, object obj, params string[] inclusionList)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var entityDicList = GetEntityDicStrList(cmdText, CommandType.Text, parameters, inclusionList);

        return entityDicList;
    }

    /// <summary>
    /// 获得典字符串列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="parameters">参数数组</param>
    /// <exception cref="">这里是 Exception</exception>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, string>> GetEntityDicStrList(string cmdText,

        CommandType commandType = CommandType.Text,
        DbParameter[]? parameters = null,
        params string[] inclusionList)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = _dataSource.CreateDataReader(cmdText, conn, commandType, parameters);
        var result = reader.ReadList(r => r.ReadSelfString(inclusionList));

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }

    /// <summary>
    /// 获得实体字典字符串列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync<T>(Expression<Func<T, bool>>? where = null,
        Expression<Func<OrderExpression<T>, object>>? order = null,

        int? top = null,
        params string[] inclusionList)
        where T : new()
    {
        var (cmdText, paramerArray) = GetEntityText(where, (whereStr, paramerList) =>
        {
            var orderStr = _dataSource.Order(order);
            var text = _pagerGenerator.GetSelectDictionaryCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, top, inclusionList);
            return text.ToString();
        });

        var result = await GetEntityDicStrListAsync(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);
        return result;
    }

    /// <summary>
    /// 获得典字符串列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync(string cmdText, DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var result = await GetEntityDicStrListAsync(cmdText, CommandType.Text, parameters, inclusionList);

        return result;
    }

    /// <summary>
    /// 获得典字符串列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="parameters">参数数组</param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync(string cmdText, object obj, params string[] inclusionList)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var result = await GetEntityDicStrListAsync(cmdText, CommandType.Text, parameters, inclusionList);

        return result;
    }
    /// <summary>
    /// 获得典字符串列表
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="parameters">参数数组</param>
    /// <exception cref="">这里是 Exception</exception>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync(string cmdText,

        CommandType commandType = CommandType.Text,
        DbParameter[]? parameters = null, params string[] inclusionList)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = await _dataSource.CreateDataReaderAsync(cmdText, conn, commandType, parameters);
        var result = reader.ReadList(r => r.ReadSelfString(inclusionList));

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }
        return result;

    }
    #endregion
    #region Get
    internal (string, DbParameter[]) GetEntityText<T>(Expression<Func<T, bool>>? where, Func<StringBuilder, List<DbParameter>, string> map)
        where T : new()
    {
        var paramerList = new List<DbParameter>();

        var whereStr = _dataSource.Where(where, ref paramerList);
        //添加默认查询条件
        if (typeof(T).HasImplementedRawGeneric(typeof(VirtulDelEntity<>)))
        {
            AppendDeleteSql(whereStr, paramerList);
        }

        var cmdText = map(whereStr, paramerList);
        return (cmdText, paramerList.ToArray());
    }
    #endregion

    #region InsertEntity
    /// <summary>
    /// 通过实体类型 T 向表中增加一条记录
    /// </summary>
    /// <typeparam name="T">实体泛型(类)</typeparam>
    /// <param name="entity">实体对象</param>
    /// <param name="inclusionList"></param>
    /// <returns>所新增记录的 ID,如果返回 -1 则表示增加失败</returns>
    public R InsertEntity<T, R>(T entity, params string[] inclusionList)
        where T : new()
    {
        var cmdText = new StringBuilder();
        object? newID = null;
        var paramerList = CreateInsertSql<T, R>(ref cmdText, entity, ref newID, inclusionList);
        R result = default!;
        var para = paramerList.ToArray();

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        if (newID == null)
        {
            newID = _pagerGenerator.InsertExecutor<T>(_dataSource, cmdText, paramerList, conn);
            result = GetSingle<R>(newID);
        }
        else
        {
            var retval = _dataSource.ExecuteNonQuery(cmdText.ToString(), conn, CommandType.Text, para);

            if (retval > 0)
            {
                result = GetSingle<R>(newID);
            }
        }

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }
    /// <summary>
    /// 通过实体类型 T 向表中增加一条记录
    /// </summary>
    /// <typeparam name="T">实体泛型(类)</typeparam>
    /// <param name="entity">实体对象</param>
    /// <param name="inclusionList"></param>
    /// <returns>所新增记录的 ID,如果返回 -1 则表示增加失败</returns>
    public async Task<R> InsertEntityAsync<T, R>(T entity, params string[] inclusionList)
        where T : new()
    {
        var cmdText = new StringBuilder();
        object? newID = null;

        var paramerList = CreateInsertSql<T, R>(ref cmdText, entity, ref newID, inclusionList);

        R result = default!;

        var para = paramerList.ToArray();

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        if (newID == null)
        {
            newID = await _pagerGenerator.InsertExecutorAsync<T>(_dataSource, cmdText, paramerList, conn);
            result = GetSingle<R>(newID);
        }
        else
        {
            var retval = await _dataSource.ExecuteNonQueryAsync(cmdText.ToString(), conn, CommandType.Text, para);

            if (retval > 0)
            {
                result = GetSingle<R>(newID);
            }
        }

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }
        return result;
    }
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    private object InsertEntity<T>(Expression<Func<T>> expression)
        where T : new()
    {
        var retval = -1;
        var (newID, cmdText, paramerList) = GetInsertText(expression);

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        if (newID == null)
        {
            newID = _pagerGenerator.InsertExecutor<T>(_dataSource, cmdText, paramerList, conn);
        }
        else
        {
            retval = _dataSource.ExecuteNonQuery(cmdText.ToString(), conn, CommandType.Text, paramerList.ToArray());
        }

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }

        if (retval <= 0)
        {
            return default!;
        }
        return newID;
    }
    private (object, StringBuilder, List<DbParameter>) GetInsertText<T>(Expression<Func<T>> expression) where T : new()
    {
        object? newID = null;
        var paramerList = new List<DbParameter>();
        var userText = GetUserText();
        var (sbField, sbValue) = _dataSource.Insert(expression, ref newID, ref paramerList, _generator, userText);

        //添加默认更新字段
        if (typeof(T).HasImplementedRawGeneric(typeof(BaseEntity<>)) && sbField.Length > 0)
        {
            var username = GetText();
            var fieldStrLower = sbField.ToString();

            var idKey = $"{string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "id")}";
            if (!fieldStrLower.Contains(idKey, StringComparison.OrdinalIgnoreCase))
            {
                var idName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}id_0_dl_0";
                sbField.Append($",{idKey}");
                sbValue.Append($",{idName}");
                paramerList.Add(_dataSource.CreateParameter(idName, _generator.Generate()));
            }

            var createDtKey = $"{string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "create_dt")}";
            if (!fieldStrLower.Contains(createDtKey, StringComparison.OrdinalIgnoreCase))
            {
                var createDtName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}create_dt_0_dl_0";
                sbField.Append($",{createDtKey}");
                sbValue.Append($",{createDtName}");
                paramerList.Add(_dataSource.CreateParameter(createDtName, DateTime.Now));
            }
            var createKey = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "creater");
            if (!fieldStrLower.Contains(createKey, StringComparison.OrdinalIgnoreCase))
            {
                var createrName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}creater_0_dl_0";
                sbField.Append($",{createKey}");
                sbValue.Append($",{createrName}");
                paramerList.Add(_dataSource.CreateParameter(createrName, username));
            }
            var updateDtKey = $"{string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "update_dt")}";
            if (!fieldStrLower.Contains(updateDtKey, StringComparison.OrdinalIgnoreCase))
            {
                var updateDtName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}update_dt_0_dl_0";
                sbField.Append($",{updateDtKey}");
                sbValue.Append($",{updateDtName}");
                paramerList.Add(_dataSource.CreateParameter(updateDtName, DateTime.Now));
            }
            var updaterKey = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "updater");
            if (!fieldStrLower.Contains(updaterKey, StringComparison.OrdinalIgnoreCase))
            {
                var updaterName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}updater_0_dl_0";
                sbField.Append($",{updaterKey}");
                sbValue.Append($",{updaterName}");
                paramerList.Add(_dataSource.CreateParameter(updaterName, username));
            }
        }
        var allInsert = CreateInsertAllSql<T>();
        var cmdText = new StringBuilder();

        cmdText.AppendFormat("{0}({1})VALUES({2})", allInsert, sbField, sbValue);

        return (newID, cmdText, paramerList)!;
    }
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public R InsertEntity<T, R>(Expression<Func<T>> expression)
        where T : new()
    {
        var newID = this.InsertEntity(expression);
        var result = GetSingle<R>(newID);

        return result;
    }
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    private async Task<object> InsertEntityAsync<T>(Expression<Func<T>> expression)
        where T : new()
    {
        var retval = -1;
        var (newID, cmdText, paramerList) = GetInsertText(expression);

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        if (newID == null)
        {
            newID = await _pagerGenerator.InsertExecutorAsync<T>(_dataSource, cmdText, paramerList, conn);
        }
        else
        {
            retval = await _dataSource.ExecuteNonQueryAsync(cmdText.ToString(), conn, CommandType.Text, paramerList.ToArray());
        }

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        if (retval <= 0)
        {
            return default!;
        }
        return newID;
    }
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<R> InsertEntityAsync<T, R>(Expression<Func<T>> expression)
        where T : new()
    {
        var newID = await this.InsertEntityAsync(expression);

        var result = GetSingle<R>(newID);

        return result;
    }
    #endregion

    #region UpdateEntity

    /// <summary>
    /// 更新操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public int UpdateEntity<T>(Expression<Func<T>> expression, Expression<Func<T, bool>> where)
        where T : new()
    {
        var (cmdText, paramerList) = getUpdateText(expression, where);
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        int result = _dataSource.ExecuteNonQuery(cmdText, conn, CommandType.Text, paramerList);
        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }

    private void AppendDeleteSql(StringBuilder whereStr, List<DbParameter> paramerList)
    {
        var delName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}is_delete_0_dl_0";
        var delFileld = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "is_delete");
        if (whereStr.Length > 0)
        {
            var whereStrUpper = whereStr.ToString().ToLower();

            if (!whereStrUpper.Contains(delFileld))
            {
                if (whereStrUpper.StartsWith("(") && whereStrUpper.EndsWith(")"))
                {
                    whereStr.Insert(0, "(");
                    whereStr.Append(")");
                }

                whereStr.Append($" AND {delFileld} = {delName}");
                paramerList.Add(_dataSource.CreateParameter(delName, false));
            }
        }
        else
        {
            whereStr.Append($" WHERE {delFileld} = {delName} ");
            paramerList.Add(_dataSource.CreateParameter(delName, false));
        }
    }

    private (string, DbParameter[]) getUpdateText<T>(Expression<Func<T>> expression, Expression<Func<T, bool>> where) where T : new()
    {
        var paramerList = new List<DbParameter>();

        var whereStr = _dataSource.Where(where, ref paramerList);
        //添加默认查询条件
        if (typeof(T).HasImplementedRawGeneric(typeof(VirtulDelEntity<>)))
        {
            AppendDeleteSql(whereStr, paramerList);
        }

        var updateStr = _dataSource.Update(expression, ref paramerList);
        //添加默认更新字段
        if (typeof(T).HasImplementedRawGeneric(typeof(BaseEntity<>)) && updateStr.Length > 0)
        {
            var updateStrUpper = updateStr.ToString();

            var updateDt = $"{string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "update_dt")}";
            if (!updateStrUpper.Contains(updateDt, StringComparison.OrdinalIgnoreCase))
            {
                var parameterName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}update_dt_0_dl_0";
                updateStr.Append($",{updateDt} = {parameterName}");
                paramerList.Add(_dataSource.CreateParameter(parameterName, DateTime.Now));
            }
            var updater = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "updater");
            if (!updateStrUpper.Contains(updater, StringComparison.OrdinalIgnoreCase))
            {
                var username = GetText();
                var parameterName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}updater_0_dl_0";
                updateStr.Append($",{updater} = {parameterName}");
                paramerList.Add(_dataSource.CreateParameter(parameterName, username));
            }
        }
        var allUpdate = this.CreateUpdateAllSql<T>();
        var cmdText = string.Format("{0}{1}{2}", allUpdate, updateStr, whereStr);
        return (cmdText, paramerList.ToArray());
    }

    /// <summary>
    /// 修改 T 实体
    /// </summary>
    /// <typeparam name="T">实体泛型</typeparam>
    /// <param name="entity">实体对象</param>
    /// <returns>影响数据条数</returns>
    public int UpdateEntity<T>(T entity, params string[] inclusionList)
    {
        var (paramerList, cmdText) = CreateUpdateSql(entity, inclusionList);
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        var result = _dataSource.ExecuteNonQuery(cmdText, conn, CommandType.Text, paramerList.ToArray());

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }

    /// <summary>
    /// 更新操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public async Task<int> UpdateEntityAsync<T>(Expression<Func<T>> expression,
        Expression<Func<T, bool>> where)
        where T : new()
    {
        var (cmdText, paramerList) = getUpdateText(expression, where);
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        var result = await _dataSource.ExecuteNonQueryAsync(cmdText, conn, CommandType.Text, paramerList);
        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }
        return result;
    }

    /// <summary>
    /// 修改 T 实体
    /// </summary>
    /// <typeparam name="T">实体泛型</typeparam>
    /// <param name="entity">实体对象</param>
    /// <returns>影响数据条数</returns>
    public async Task<int> UpdateEntityAsync<T>(T entity, params string[] inclusionList)
    {
        var (paramerList, cmdText) = CreateUpdateSql(entity, inclusionList);
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        var result = await _dataSource.ExecuteNonQueryAsync(cmdText, conn, CommandType.Text, paramerList.ToArray());
        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }
        return result;
    }


    #endregion

    #region DeleteEntity
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="isLogic"></param>
    /// <returns></returns>
    public int DeleteEntity<T>(Expression<Func<T, bool>> where, bool isLogic = true)
        where T : new()
    {
        var (cmdText, paramerList) = GetDeleteText(where, isLogic);

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        var result = _dataSource.ExecuteNonQuery(cmdText, conn, CommandType.Text, paramerList);

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }

    private (string, DbParameter[]) GetDeleteText<T>(Expression<Func<T, bool>> where, bool isLogic) where T : new()
    {
        var paramerList = new List<DbParameter>();
        var whereStr = _dataSource.Where(where, ref paramerList);

        string allDelete;
        if (typeof(T).HasImplementedRawGeneric(typeof(VirtulDelEntity<>)) && isLogic)
        {
            allDelete = this.CreateUpdateAllSql<T>();

            var username = GetText();
            var deleteKey = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "is_delete");
            var deleteName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}is_delete_0_dl_0";
            paramerList.Add(_dataSource.CreateParameter(deleteName, true));
            var updateDtKey = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "update_dt");
            var updateDtName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}update_dt_0_dl_0";
            paramerList.Add(_dataSource.CreateParameter(updateDtName, DateTime.Now));
            var updaterKey = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, "updater");
            var updaterName = $"{_dataSource.DbFactory.DbProvider.ParameterPrefix}updater_0_dl_0";
            paramerList.Add(_dataSource.CreateParameter(updaterName, username));
            allDelete += $"{deleteKey} = {deleteName},{updateDtKey} = {updateDtName},{updaterKey} = {updaterName}";

        }
        else
        {
            allDelete = this.CreateDeleteAllSql<T>();
        }
        var cmdText = string.Format("{0}{1}", allDelete, whereStr);
        return (cmdText, paramerList.ToArray());
    }

    /// <summary>
    /// 删除数据异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="isLogic"></param>
    /// <returns></returns>
    public async Task<int> DeleteEntityAsync<T>(Expression<Func<T, bool>> where, bool isLogic = true) where T : new()
    {
        var (cmdText, paramerList) = GetDeleteText(where, isLogic);

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        var result = await _dataSource.ExecuteNonQueryAsync(cmdText, conn, CommandType.Text, paramerList.ToArray());

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        return result;
    }
    #endregion

    #region Other

    #region ExecuteNonQuery
    /// <summary>
    /// 执行 SQL 语句
    /// </summary>
    /// <param name="cmdText">>SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>SQL 语句所影响的行数</returns>
    public int ExecuteNonQuery(string cmdText, params DbParameter[] paramers)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        var returnValue = _dataSource.ExecuteNonQuery(cmdText, conn, CommandType.Text, paramers);

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return returnValue;
    }

    /// <summary>
    /// 执行 SQL 语句
    /// </summary>
    /// <param name="cmdText">>SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>SQL 语句所影响的行数</returns>
    public int ExecuteNonQuery(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        var returnValue = _dataSource.ExecuteNonQuery(cmdText, conn, CommandType.Text, parameters);

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return returnValue;
    }


    /// <summary>
    /// 执行 SQL 语句
    /// </summary>
    /// <param name="cmdText">>SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>SQL 语句所影响的行数</returns>
    public async Task<int> ExecuteNonQueryAsync(string cmdText, params DbParameter[] paramers)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        var returnValue = await _dataSource.ExecuteNonQueryAsync(cmdText, conn, CommandType.Text, paramers);

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        return returnValue;
    }

    /// <summary>
    /// 执行 SQL 语句
    /// </summary>
    /// <param name="cmdText">>SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>SQL 语句所影响的行数</returns>
    public async Task<int> ExecuteNonQueryAsync(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;
        var returnValue = await _dataSource.ExecuteNonQueryAsync(cmdText, conn, CommandType.Text, parameters);

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        return returnValue;
    }


    #endregion

    #region ExecuteScalar
    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cmdText"></param>
    /// <param name="paramers"></param>
    /// <returns></returns>
    public T ExecuteScalar<T>(string cmdText, params DbParameter[] paramers)
    {
        var result = this.ExecuteScalar<T>(cmdText, CommandType.Text, paramers);
        return result;
    }
    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cmdText"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public T ExecuteScalar<T>(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var result = this.ExecuteScalar<T>(cmdText, parameters);
        return result;
    }

    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cmdText"></param>
    /// <param name="commandType"></param>
    /// <param name="paramers"></param>
    /// <returns></returns>
    public T ExecuteScalar<T>(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers)
    {
        var obj = ExecuteScalar(cmdText, commandType, paramers);

        var result = GetSingle<T>(obj);
        return result;
    }
    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>返回首行首列</returns>
    public object ExecuteScalar(string cmdText, params DbParameter[] paramers)
    {
        var returnObj = ExecuteScalar(cmdText, CommandType.Text, paramers);

        return returnObj;
    }
    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>返回首行首列</returns>
    public object ExecuteScalar(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var returnObj = ExecuteScalar(cmdText, CommandType.Text, parameters);

        return returnObj;
    }

    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cmdText"></param>
    /// <param name="commandType"></param>
    /// <param name="paramers"></param>
    /// <returns></returns>
    public object ExecuteScalar(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        var result = _dataSource.ExecuteScalar(cmdText, conn, commandType, paramers);

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }


    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>返回首行首列</returns>
    public async Task<object> ExecuteScalarAsync(string cmdText, params DbParameter[] paramers)
    {
        var result = await ExecuteScalarAsync(cmdText, CommandType.Text, paramers);
        return result;
    }
    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>返回首行首列</returns>
    public async Task<object> ExecuteScalarAsync(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        var result = await _dataSource.ExecuteScalarAsync(cmdText, conn, commandType, paramers);

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }
        return result;
    }
    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>返回首行首列</returns>
    public async Task<object> ExecuteScalarAsync(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var result = await ExecuteScalarAsync(cmdText, CommandType.Text, parameters);
        return result;
    }
    /// <summary>
    /// 执行数据库操作，返回首行首列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cmdText"></param>
    /// <param name="commandType"></param>
    /// <param name="paramers"></param>
    /// <returns></returns>
    public async Task<T> ExecuteScalarAsync<T>(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers)
    {
        var obj = await ExecuteScalarAsync(cmdText, commandType, paramers);
        var result = GetSingle<T>(obj);
        return result;
    }
    #endregion

    #region IsExists
    /// <summary>
    /// 判断记录是否存在
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
    public bool IsExists(string cmdText, params DbParameter[] paramers)
    {
        var returnValue = IsExists(cmdText, CommandType.Text, paramers);

        return returnValue;
    }
    /// <summary>
    /// 判断记录是否存在
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
    public bool IsExists(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var returnValue = IsExists(cmdText, CommandType.Text, parameters);

        return returnValue;
    }

    /// <summary>
    /// 判断记录是否存在
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commondType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
    public bool IsExists(string cmdText, CommandType commondType = CommandType.Text, params DbParameter[] paramers)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        var result = true;

        var obj = _dataSource.ExecuteScalar(cmdText, conn, commondType, paramers);

        if (Equals(obj, null) || Equals(obj, DBNull.Value))
        {
            result = false;
        }

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }

        return result;
    }

    /// <summary>
    /// 判断记录是否存在
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
    public async Task<bool> IsExistsAsync(string cmdText, params DbParameter[] paramers)
    {
        var returnValue = await IsExistsAsync(cmdText, CommandType.Text, paramers);

        return returnValue;
    }

    /// <summary>
    /// 判断记录是否存在
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
    public async Task<bool> IsExistsAsync(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var returnValue = await IsExistsAsync(cmdText, CommandType.Text, parameters);

        return returnValue;
    }

    /// <summary>
    /// 判断记录是否存在
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commondType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
    public async Task<bool> IsExistsAsync(string cmdText, CommandType commondType = CommandType.Text, params DbParameter[] paramers)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        var result = true;

        var obj = await _dataSource.ExecuteScalarAsync(cmdText, conn, commondType, paramers);

        if (Equals(obj, null) || Equals(obj, DBNull.Value))
        {
            result = false;
        }
        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }

        return result;
    }
    #endregion

    #region GetDataSet
    /// <summary>
    /// 返回数据集
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集</returns>
    public DataSet GetDataSet(string cmdText, params DbParameter[] paramers)
    {
        var returnDataSet = this.GetDataSet(cmdText, CommandType.Text, paramers);

        return returnDataSet;
    }
    /// <summary>
    /// 返回数据集
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集</returns>
    public DataSet GetDataSet(string cmdText, object obj)
    {
        var parameters = _dataSource.ToDbParameters(obj);
        var returnDataSet = this.GetDataSet(cmdText, CommandType.Text, parameters);

        return returnDataSet;
    }

    /// <summary>
    /// 返回数据集
    /// </summary>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集</returns>
    public DataSet GetDataSet(string cmdText, CommandType commondType = CommandType.Text, params DbParameter[] paramers)
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        var returnDataSet = _dataSource.CreateDataSet(cmdText, conn, commondType, paramers);

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return returnDataSet;
    }

    #endregion

    #endregion

    #endregion

    /// <summary>
    /// 生成id
    /// </summary>
    /// <returns></returns>
    public R NewId<R>()
    {
        var data = _generator.Generate();
        var result = GetSingle<R>(data);
        return result;
    }

    #region 分页
    /// <summary>
    /// 生成分页sql
    /// </summary>
    /// <param name="unionText"></param>
    /// <param name="tableName"></param>
    /// <param name="fldName"></param>
    /// <param name="PageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="filter"></param>
    /// <param name="group"></param>
    /// <param name="sort"></param>
    /// <param name="parameter"></param>
    /// <param name="paramers"></param>
    /// <returns></returns>
    private (string, DbParameter[]) GetPageCmdText<T>(string unionText,
        string tableName,
        string fldName,
        ref int? pageIndex,
        ref int? pageSize,
        string filter,
        string group,
        string sort,
        DbParameter[] paramers,
        params string[] inclusionList)
    {
        if (string.IsNullOrWhiteSpace(fldName) || fldName.Trim().Equals("*"))
        {
            var field = _dataSource.CreateAllEntityDicSql<T>(inclusionList);
            fldName = field.ToString();
        }

        var (result, pageParameters) = GetPageCmdText(unionText, tableName, fldName, ref pageIndex, ref pageSize, filter, group, sort, paramers);
        return (result, pageParameters);
    }
    /// <summary>
    /// 生成分页sql
    /// </summary>
    /// <param name="unionText"></param>
    /// <param name="tableName"></param>
    /// <param name="fldName"></param>
    /// <param name="PageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="filter"></param>
    /// <param name="group"></param>
    /// <param name="sort"></param>
    /// <param name="parameter"></param>
    /// <param name="paramers"></param>
    /// <returns></returns>
    private (string, DbParameter[]) GetPageCmdText(string unionText,
        string tableName,
        string fldName,
        ref int? pageIndex,
        ref int? pageSize,
        string filter,
        string group,
        string sort,
        DbParameter[] paramers)
    {
        if (string.IsNullOrWhiteSpace(fldName))
        {
            fldName = "*";
        }
        var (cmdText, pageParameters) = _pagerGenerator.GetPageCmdText(_dataSource, unionText, tableName, fldName, ref pageIndex, ref pageSize, filter, group, sort, paramers);
        var result = _dataSource.ReplaceParameter(cmdText.ToString());
        return (result, pageParameters);
    }
    #region pager
    private DbDataReader GetResultByPager<T, T1>(Pager<T1> page, DbConnection conn, params string[] inclusionList)
        where T : new()
        where T1 : BasePageCondition, new()
    {
        page.Execute();

        var pageIndex = page.Condition.PageIndex;
        var pageSize = page.Condition.PageSize;

        var (cmdText, parameter) = GetPageCmdText<T>(page.UnionText,
            page.Table,
            page.Field,
            ref pageIndex,
            ref pageSize,
            page.Where.ToString(),
            page.Group,
            page.Order,
            page.Parameters.ToArray(),
            inclusionList);

        page.Condition.PageIndex = pageIndex;
        page.Condition.PageSize = pageSize;

        var reader = _dataSource.CreateDataReader(cmdText, conn, CommandType.Text, parameter);
        return reader;
    }
    private async Task<DbDataReader> GetResultByPagerAsync<T, T1>(Pager<T1> page, DbConnection conn, params string[] inclusionList)
        where T : new()
        where T1 : BasePageCondition, new()
    {
        page.Execute();
        var pageIndex = page.Condition.PageIndex;
        var pageSize = page.Condition.PageSize;

        var (cmdText, parameter) = GetPageCmdText<T>(page.UnionText,
            page.Table,
            page.Field,
            ref pageIndex,
            ref pageSize,
            page.Where.ToString(),
            page.Group,
            page.Order,
            page.Parameters.ToArray(),
            inclusionList);

        page.Condition.PageIndex = pageIndex;
        page.Condition.PageSize = pageSize;

        var reader = await _dataSource.CreateDataReaderAsync(cmdText, conn, CommandType.Text, parameter);
        return reader;
    }
    private DbDataReader GetResultByPager<T1>(Pager<T1> page, DbConnection conn)
        where T1 : BasePageCondition, new()
    {
        page.Execute();

        var pageIndex = page.Condition.PageIndex;
        var pageSize = page.Condition.PageSize;

        var (cmdText, parameter) = GetPageCmdText(page.UnionText,
            page.Table,
            page.Field,
            ref pageIndex,
            ref pageSize,
            page.Where.ToString(),
            page.Group,
            page.Order,
            page.Parameters.ToArray());

        page.Condition.PageIndex = pageIndex;
        page.Condition.PageSize = pageSize;

        var reader = _dataSource.CreateDataReader(cmdText, conn, CommandType.Text, parameter);
        return reader;
    }
    private async Task<DbDataReader> GetResultByPagerAsync<T1>(Pager<T1> page, DbConnection conn)
        where T1 : BasePageCondition, new()
    {
        page.Execute();
        var pageIndex = page.Condition.PageIndex;
        var pageSize = page.Condition.PageSize;

        var (cmdText, parameter) = GetPageCmdText(page.UnionText,
            page.Table,
            page.Field,
            ref pageIndex,
            ref pageSize,
            page.Where.ToString(),
            page.Group,
            page.Order,
            page.Parameters.ToArray());

        page.Condition.PageIndex = pageIndex;
        page.Condition.PageSize = pageSize;

        var reader = await _dataSource.CreateDataReaderAsync(cmdText, conn, CommandType.Text, parameter);
        return reader;
    }
    #region pager result list


    /// <summary>
    /// 返回分页实体列表集合
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T> GetResultByPager<T, T1>(Pager<T1> page, params string[] inclusionList)
        where T : new()
        where T1 : BasePageCondition, new()
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = GetResultByPager<T, T1>(page, conn, inclusionList);
        var totalCount = 0;
        var result = reader.ReadList(r => r.ReadObject<T>(inclusionList), ref totalCount);
        page.SetTotalCount(totalCount);


        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }

    /// <summary>
    /// 返回分页实体列表集合异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetResultByPageAsync<T, T1>(Pager<T1> page, params string[] inclusionList)
        where T : new()
        where T1 : BasePageCondition, new()
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = await GetResultByPagerAsync<T, T1>(page, conn, inclusionList);
        var totalCount = 0;
        var result = reader.ReadList(r => r.ReadObject<T>(inclusionList), ref totalCount);
        page.SetTotalCount(totalCount);

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }
        return result;
    }

    /// <summary>
    /// 返回分页实体字典列表集合
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="page"></param>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, object>> GetResultByPagerDic<T1>(Pager<T1> page)
        where T1 : BasePageCondition, new()
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = GetResultByPager(page, conn);
        var totalCount = 0;
        var result = reader.ReadList(r => r.ReadSelf(), ref totalCount);
        page.SetTotalCount(totalCount);

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }

    /// <summary>
    /// 返回分页实体字典列表集合异步
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, object>>> GetResultByPagerDicAsync<T1>(Pager<T1> page)
        where T1 : BasePageCondition, new()
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = await GetResultByPagerAsync(page, conn);
        var totalCount = 0;
        var result = reader.ReadList(r => r.ReadSelf(), ref totalCount);
        page.SetTotalCount(totalCount);

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }
        return result;
    }


    /// <summary>
    /// 返回分页实体字典列表集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <param name="page"></param>
    /// <returns></returns>
    public IEnumerable<IDictionary<string, object>> GetResultByPagerDic<T, T1>(Pager<T1> page, params string[] inclusionList)
        where T : new()
        where T1 : BasePageCondition, new()
    {
        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = GetResultByPager<T, T1>(page, conn, inclusionList);
        var totalCount = 0;
        var result = reader.ReadList(r => r.ReadSelf(inclusionList), ref totalCount);
        page.SetTotalCount(totalCount);

        if (Uow.ActiveNumber == 0)
        {
            conn.Close();
            conn.Dispose();
        }
        return result;
    }

    /// <summary>
    /// 返回分页实体字典列表集合异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, object>>> GetResultByPagerDicAsync<T, T1>(Pager<T1> page, params string[] inclusionList)
        where T : new()
        where T1 : BasePageCondition, new()
    {

        var conn = Uow.ActiveNumber == 0 ? _dataSource.DbFactory.ShortDbConnection : _dataSource.DbFactory.LongDbConnection;

        using var reader = await GetResultByPagerAsync<T, T1>(page, conn, inclusionList);
        var totalCount = 0;
        var result = reader.ReadList(r => r.ReadSelf(inclusionList), ref totalCount);
        page.SetTotalCount(totalCount);

        if (Uow.ActiveNumber == 0)
        {
            await conn.CloseAsync();
            await conn.DisposeAsync();
        }
        return result;
    }

    /// <summary>
    /// 返回分页数据集
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="page"></param>
    /// <returns></returns>
    public DataSet GetResultByPagerDs<T1>(Pager<T1> page)
        where T1 : BasePageCondition, new()
    {
        page.Execute();
        var pageIndex = page.Condition.PageIndex;
        var pageSize = page.Condition.PageSize;

        var (cmdText, parameter) = GetPageCmdText(page.UnionText,
            page.Table,
            page.Field,
            ref pageIndex,
            ref pageSize,
            page.Where.ToString(),
            page.Group,
            page.Order,
            page.Parameters.ToArray());

        page.Condition.PageIndex = pageIndex;
        page.Condition.PageSize = pageSize;

        var result = this.GetDataSet(cmdText, parameter);

        if (int.TryParse(result.Tables[1].Rows[0]["TotalRecords"].ToString(), out int totalCount))
        {
            page.SetTotalCount(totalCount);
        }
        return result;
    }
    #endregion
    #endregion
    #endregion

    #region 创建参数方法

    /// <summary>
    /// 创建参数
    /// </summary>
    /// <param name="pName">参数名</param>
    /// <param name="pValue">参数值</param>
    /// <param name="pType">参数类型</param>
    /// <param name="pSize">长度</param>
    /// <returns>参数</returns>
    public virtual DbParameter CreateParameter(string pName, object pValue, DbType? pType = null, int? pSize = null)
    {
        var result = _dataSource.CreateParameter(pName, pValue, pType, pSize);
        return result;
    }

    #region output
    /// <summary>
    /// 创建一个带有返回值的存储过程参数
    /// </summary>
    /// <param name="pName">参数名称</param>
    /// <param name="propertyName">属性名</param>
    /// <param name="sourceType">映射属性值</param>
    /// <returns></returns>
    public virtual DbParameter CreateOutPutParameter(string pName, string propertyName, object propertyValue)
    {
        var result = _dataSource.CreateOutPutParameter(pName, propertyName, propertyValue);
        return result;
    }

    /// <summary>
    /// 创建一个带有返回值的存储过程参数
    /// </summary>
    /// <param name="pName">参数名称</param>
    /// <param name="pType">参数类型</param>
    /// <param name="pSize">长度</param>
    /// <returns></returns>
    public virtual DbParameter CreateOutPutParameter(string pName, DbType? pType = null, int? pSize = null)
    {
        var result = _dataSource.CreateOutPutParameter(pName, pType, pSize);
        return result;
    }

    /// <summary>
    /// 创建一个带有返回值的存储过程参数
    /// </summary>
    /// <param name="pName">参数名称</param>
    /// <param name="pValue">参数值</param>
    /// <param name="pType">参数类型</param>
    /// <param name="pSize">长度</param>
    /// <returns></returns>
    public virtual DbParameter CreateOutPutParameter(string pName, object pValue, DbType? pType = null, int? pSize = null)
    {
        var result = _dataSource.CreateOutPutParameter(pName, pType, pSize);
        return result;
    }
    #endregion

    #region 返回参数类型
    /// <summary>
    /// 创建一个带有返回值的存储过程参数
    /// </summary>
    /// <param name="pName">参数名称</param>
    /// <param name="pType">参数类型</param>
    /// <param name="pSize">长度</param>
    /// <returns></returns>
    public virtual DbParameter CreateReturnValueParameter(string pName, DbType? pType = null, int? pSize = null)
    {
        var result = _dataSource.CreateReturnValueParameter(pName, pType, pSize);
        return result;
    }

    /// <summary>
    /// 创建一个带有返回值的存储过程参数
    /// </summary>
    /// <param name="pName">参数名称</param>
    /// <param name="pValue">参数值</param>
    /// <param name="pType">参数类型</param>
    /// <param name="pSize">长度</param>
    /// <returns></returns>
    public virtual DbParameter CreateReturnValueParameter(string pName, object pValue, DbType? pType = null, int? pSize = null)
    {
        var result = _dataSource.CreateReturnValueParameter(pName, pValue, pType, pSize);
        return result;
    }
    #endregion

    #endregion

    #region 生成SQL
    /// <summary>
    /// 获取用户申明
    /// </summary>
    /// <returns></returns>
    public ClaimsPrincipal? GetUser()
    {
        return _user;
    }
    /// <summary>
    /// 获取权限集合
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetPermissions()
    {
        var permission = _user?.Claims?.FirstOrDefault(w => w.Type.Equals("Permission", StringComparison.OrdinalIgnoreCase));
        if (permission == null)
        {
            return new List<string>();
        }

        var permissionValue = permission.Value ?? "";
        var permissions = permissionValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

        return permissions;
    }
    /// <summary>
    /// 获取用户名信息
    /// </summary>
    /// <returns></returns>
    public string GetUserName()
    {
        var username = _user?.Claims?.FirstOrDefault(w => w.Type.Equals("UserName", StringComparison.OrdinalIgnoreCase))?.Value ?? "anonymous";
        return username;
    }
    /// <summary>
    /// 获取名称昵称信息
    /// </summary>
    /// <returns></returns>
    public string GetText()
    {
        var text = _user?.Claims?.FirstOrDefault(w => w.Type.Equals("Text", StringComparison.OrdinalIgnoreCase))?.Value ?? "匿名";
        return text;
    }
    /// <summary>
    /// 用户名和昵称组合
    /// </summary>
    /// <returns></returns>
    public string GetUserText(string split = "/")
    {
        var username = _user?.Claims?.FirstOrDefault(w => w.Type.Equals("UserName", StringComparison.OrdinalIgnoreCase))?.Value ?? "anonymous";
        var nickname = _user?.Claims?.FirstOrDefault(w => w.Type.Equals("NickName", StringComparison.OrdinalIgnoreCase))?.Value ?? "匿名";
        var realname = _user?.Claims?.FirstOrDefault(w => w.Type.Equals("RealName", StringComparison.OrdinalIgnoreCase))?.Value ?? "匿名";
        var text = GetText();
        return $"{username}{split}{nickname}{split}{realname}";
    }
    /// <summary>
    /// yonh外键获取
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <returns></returns>
    public R GetUserId<R>()
    {
        try
        {
            var userId = _user?.Claims.FirstOrDefault(w => w.Type.Equals("UserId", StringComparison.OrdinalIgnoreCase))?.Value ?? "";
            R result = (R)userId.ChangeType(typeof(R));
            return result;
        }
        catch { }
        return default;
    }
    /// <summary>
    /// 查询所有数据-不包含字段*
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="paramerList"></param>
    /// <param name="top"></param>
    /// <returns></returns>
    public StringBuilder GetCountCmdText<T>(StringBuilder whereStr)
    {
        var cmdText = new StringBuilder();

        var entityType = typeof(T);
        var tableName = entityType.GetDataTableName();
        tableName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, tableName);

        cmdText.AppendFormat("SELECT COUNT(0) FROM {0} {1}", tableName, whereStr);

        return cmdText;
    }
    /// <summary>
    /// 生成添加SQL语句
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="cmdText">执行SQL</param>
    /// <param name="data">实体对象</param>
    /// <param name="inclusionList">不包括属性名</param>
    /// <returns>返回[T-SQL:Insert]</returns>
    private List<DbParameter> CreateInsertSql<T, R>(ref StringBuilder cmdText, T entity, ref object? newID, params string[] inclusionList)
    {
        if (entity is BaseEntity<R> data)
        {
            var username = GetText();
            if (string.IsNullOrWhiteSpace(data.Updater))
            {
                data.Updater = username;
            }
            if (string.IsNullOrWhiteSpace(data.Creater))
            {
                data.Creater = username;
            }
            if (data.UpdateDt == DateTime.MinValue)
            {
                data.UpdateDt = DateTime.Now;
            }
            if (data.CreateDt == DateTime.MinValue)
            {
                data.CreateDt = DateTime.Now;
            }
        }

        var paramerList = new List<DbParameter>();
        var entityType = typeof(T);
        //var propertyInfos = entityType.GetProperties();
        var propertyInfos = entityType.GetCachedProperties();//.GetProperties();
        var sqlFields = new StringBuilder();
        var sqlValues = new StringBuilder();

        var tableName = entityType.GetDataTableName();
        tableName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, tableName);
        cmdText = new StringBuilder("");

        foreach (var property in propertyInfos)
        {
            //不可读
            if (!property.Key.CanRead || !property.Key.CanWrite || (inclusionList?.Count() > 0 && inclusionList.IsExcluded(property.Key.Name)))
            {
                continue;
            }

            object? oval = null;
            object? genOval = null;

            var propVal = property.Value.Getter(entity);//.GetValue(entity, null);

            var (da, fieldName) = property.Key.GetDataFieldAttribute();
            if (da != null)
            {
                if (da.IsKey && da.IsAuto && da.KeyType == KeyType.SEQ)
                {
                    _pagerGenerator.ProcessInsertId<T>(fieldName, ref sqlFields, ref sqlValues);
                    continue;
                }
                else if (da.IsKey && da.IsAuto && da.KeyType != KeyType.SEQ)
                {
                    genOval = propVal;

                    if (genOval != null && !string.IsNullOrEmpty(genOval.ToString()))
                    {
                        try
                        {
                            var currentLongId = -1L;
                            var currentGuidId = Guid.Empty;
                            if (long.TryParse(genOval.ToString(), out currentLongId))
                            {
                                if (currentLongId <= 0)
                                {
                                    genOval = _generator.Generate();
                                }
                            }
                            else if (Guid.TryParse(genOval.ToString(), out currentGuidId))
                            {
                                if (currentGuidId == Guid.Empty)
                                {
                                    genOval = _generator.Generate();
                                }
                            }
                        }
                        catch
                        {
                            genOval = _generator.Generate();
                        }
                    }
                    else
                    {
                        genOval = _generator.Generate();
                    }

                    newID = genOval;
                }
            }

            oval = genOval ?? propVal;

            if (oval == null && da.DefaultValue != null)
            {
                oval = da.DefaultValue;
            }

            if (oval != null)
            {

                paramerList.Add(_dataSource.CreateParameter(_dataSource.DbFactory.DbProvider.ParameterPrefix + fieldName, oval));
                sqlFields.AppendFormat(_dataSource.DbFactory.DbProvider.FieldFormat, fieldName);
                sqlFields.Append(',');
                sqlValues.Append(_dataSource.DbFactory.DbProvider.ParameterPrefix);
                sqlValues.Append(fieldName);
                sqlValues.Append(',');
            }
        }

        if (sqlFields.Length > 0)
        {
            sqlFields.Length -= 1;
        }

        if (sqlValues.Length > 0)
        {
            sqlValues.Length -= 1;
        }

        //dataTable.SequenceName

        cmdText.AppendFormat("INSERT INTO {0}({1}) VALUES({2})", tableName, sqlFields, sqlValues);

        return paramerList;
    }
    /// <summary>
    /// 生成Insert SQL语句
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private string CreateInsertAllSql<T>()
    {
        var entityType = typeof(T);

        string tableName = entityType.GetDataTableName();
        tableName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, tableName);

        var cmdText = $"INSERT INTO {tableName} ";

        return cmdText;
    }


    /// <summary>
    /// 生成修改SQL语句
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity">实体对象</param>
    /// <param name="inclusionList">不包括属性名</param>
    /// <returns>返回[T-SQL:UPDATE]</returns>
    private (List<DbParameter>, string) CreateUpdateSql<T>(T entity, params string[] inclusionList)
    {
        var isBaseEntity = typeof(T).HasImplementedRawGeneric(typeof(BaseEntity<>));
        if (isBaseEntity)
        {
            var username = GetText();
            entity.SetValueByPropertyName(nameof(BaseEntity<long>.Updater), username);
            entity.SetValueByPropertyName(nameof(BaseEntity<long>.UpdateDt), DateTime.Now);
        }

        var paramerList = new List<DbParameter>();
        var entityType = typeof(T);
        //var propertyInfos = entityType.GetProperties();
        var propertyInfos = entityType.GetCachedProperties();
        var sqlFields = new StringBuilder();
        var sqlValues = new StringBuilder();
        string tableName = entityType.GetDataTableName();
        tableName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, tableName);

        foreach (var property in propertyInfos)
        {
            //不可读
            if (!property.Key.CanRead
                || !property.Key.CanWrite
                || (inclusionList?.Count() > 0 && inclusionList.IsExcluded(property.Key.Name)))
            {
                continue;
            }

            var (datafieldAttribute, fieldName) = property.Key.GetDataFieldAttribute();

            object oval;
            if (datafieldAttribute?.IsKey == true)
            {
                sqlValues.AppendFormat(_dataSource.DbFactory.DbProvider.FieldFormat, fieldName);
                sqlValues.Append("=");
                sqlValues.Append(_dataSource.DbFactory.DbProvider.ParameterPrefix);
                sqlValues.Append(fieldName);
                sqlValues.Append(',');
                //oval = property.GetValue(entity, null);
                oval = property.Value.Getter(entity);//.GetValue(entity, null);
                oval ??= DBNull.Value;
                paramerList.Add(_dataSource.CreateParameter(_dataSource.DbFactory.DbProvider.ParameterPrefix + fieldName, oval));
                continue;
            }

            if (isBaseEntity
                && (fieldName.Equals(nameof(BaseEntity<long>.Creater), StringComparison.OrdinalIgnoreCase)
                    || fieldName.Equals(nameof(BaseEntity<long>.CreateDt), StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            oval = property.Value.Getter(entity);//.GetValue(entity, null);

            if (oval == null && datafieldAttribute.DefaultValue != null)
            {
                oval = datafieldAttribute.DefaultValue;
            }

            if (oval != null)
            {
                paramerList.Add(_dataSource.CreateParameter(_dataSource.DbFactory.DbProvider.ParameterPrefix + fieldName, oval));
                sqlFields.AppendFormat(_dataSource.DbFactory.DbProvider.FieldFormat, fieldName);
                sqlFields.Append("=");
                sqlFields.Append(_dataSource.DbFactory.DbProvider.ParameterPrefix);
                sqlFields.Append(fieldName);
                sqlFields.Append(',');
            }
        }
        if (sqlFields.Length > 0)
        {
            sqlFields.Length -= 1;
        }

        if (sqlValues.Length > 0)
        {
            sqlValues.Length -= 1;
        }

        var cmdText = $"UPDATE {tableName} SET {sqlFields} WHERE {sqlValues} ";

        return (paramerList, cmdText);
    }
    /// <summary>
    /// 生成修改SQL语句
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private string CreateUpdateAllSql<T>()
    {
        var entityType = typeof(T);

        string tableName = entityType.GetDataTableName();
        tableName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, tableName);

        var cmdText = $"UPDATE {tableName} SET ";

        return cmdText;
    }



    /// <summary>
    /// 删除所有该表的数据的sql
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private string CreateDeleteAllSql<T>()
    {
        var entityType = typeof(T);
        string tableName = entityType.GetDataTableName();
        tableName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, tableName);

        var cmdText = $"DELETE FROM {tableName} ";

        return cmdText;
    }

    #endregion

    /// <summary>
    /// 获取查询需要的字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inclusionList"></param>
    /// <param name="prex"></param>
    /// <returns></returns>
    public string GetSelectFields<T>(string[]? inclusionList = null, string prex = "")
    {
        var result = _dataSource.CreateAllEntityDicSql<T>(inclusionList, prex);
        return result.ToString();
    }
}
