using DBLayer.Core;
using DBLayer.Core.Condition;
using DBLayer.Core.Interface;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace DBLayer.Persistence;

public abstract class BaseRepository<T, R> : BaseRepository, IRepository<T, R>
    where T : new()
{

    public BaseRepository(IDbContext dbContext)
        : base(dbContext) { }

    #region public method

    #region GetEntity

    /// <summary>
    /// 获取 T 单个实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public T GetEntity(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList)
    {
        var entity = base.GetEntity<T>(where, order, inclusionList);
        return entity;
    }
    /// <summary>
    /// 获取 T 单个实体
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    public T GetEntity(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList)
    {
        T entity = GetEntity<T>(cmdText, paramers, inclusionList);

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
    public T GetEntity(string cmdText, object obj, params string[] inclusionList)
    {
        T entity = GetEntity<T>(cmdText, obj, inclusionList);
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
    public T GetEntity(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList)
    {
        T entity = GetEntity<T>(cmdText, commandType, paramers, inclusionList);
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
    public async Task<T> GetEntityAsync(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList)
    {
        var entity = await GetEntityAsync<T>(where, order, inclusionList);
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
    public async Task<T> GetEntityAsync(string cmdText, object obj, params string[] inclusionList)
    {
        var entity = await GetEntityAsync<T>(cmdText, obj, inclusionList);
        return entity;
    }

    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    public async Task<T> GetEntityAsync(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList)
    {
        var entity = await GetEntityAsync<T>(cmdText, paramers, inclusionList);

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
    public async Task<T> GetEntityAsync(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList)
    {
        var entity = await GetEntityAsync<T>(cmdText, commandType, paramers, inclusionList);

        return entity;
    }
    /// <summary>
    /// 获取 查询 数量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <returns></returns>
    public int GetEntityCount(Expression<Func<T, bool>> where)
    {
        var paramerList = new List<DbParameter>();
        var whereStr = _dataSource.Where<T>(where, ref paramerList);

        var cmdText = GetCountCmdText<T>(whereStr);

        var result = this.ExecuteScalar<int>(cmdText.ToString(), CommandType.Text, paramerList.ToArray());
        return result;
    }

    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <returns></returns>
    public async Task<int> GetEntityCountAsync(Expression<Func<T, bool>> where)
    {
        var paramerList = new List<DbParameter>();
        var whereStr = _dataSource.Where<T>(where, ref paramerList);

        var cmdText = GetCountCmdText<T>(whereStr);

        var result = await this.ExecuteScalarAsync<int>(cmdText.ToString(), CommandType.Text, paramerList.ToArray());
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
    public IDictionary<string, object> GetEntityDic(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList)
    {
        var result = GetEntityDic<T>(where, order, inclusionList);
        return result;
    }

    /// <summary>
    /// 获得单个实体字典异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<IDictionary<string, object>> GetEntityDicAsync(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList)
    {
        var result = await GetEntityDicAsync<T>(where, order, inclusionList);
        return result;
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
    public IEnumerable<IDictionary<string, object>> GetEntityDicList(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList)
    {
        var result = base.GetEntityDicList<T>(where, order, top, inclusionList);
        return result;
    }

    /// <summary>
    /// 获得实体字典列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList)
    {
        var result = await base.GetEntityDicListAsync<T>(where, order, top, inclusionList);
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
    public IEnumerable<T> GetEntityList(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList)
    {
        var result = base.GetEntityList<T>(where, order, top, inclusionList);
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
    public IEnumerable<T> GetEntityList(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList)
    {
        var entityList = GetEntityList<T>(cmdText, paramers, inclusionList);

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
    public IEnumerable<T> GetEntityList(string cmdText, object obj, params string[] inclusionList)
    {
        var entityList = GetEntityList<T>(cmdText, obj, inclusionList);

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
    public IEnumerable<T> GetEntityList(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList)
    {
        var entityList = GetEntityList<T>(cmdText, commandType, paramers, inclusionList);

        return entityList;
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="top"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetEntityListAsync(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList)
    {
        var entityList = await GetEntityListAsync<T>(where, order, top, inclusionList);

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
    public async Task<IEnumerable<T>> GetEntityListAsync(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList)
    {
        var result = await GetEntityListAsync<T>(cmdText, paramers, inclusionList);

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
    public async Task<IEnumerable<T>> GetEntityListAsync(string cmdText, object obj, params string[] inclusionList)
    {
        var result = await GetEntityListAsync<T>(cmdText, obj, inclusionList);

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
    public async Task<IEnumerable<T>> GetEntityListAsync(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList)
    {
        var result = await GetEntityListAsync<T>(cmdText, commandType, paramers, inclusionList);

        return result;
    }
    #endregion

    /// <summary>
    /// 生成id
    /// </summary>
    /// <returns></returns>
    public R NewId()
    {
        var result = base.NewId<R>();
        return result;
    }

    /// <summary>
    /// 获取用户id
    /// </summary>
    /// <returns></returns>
    public R GetUserId()
    {
        var result = base.GetUserId<R>();
        return result;
    }

    #region GetList
    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    public IEnumerable<T> GetList(string cmdText, params DbParameter[] paramers)
    {
        var result = GetList<T>(cmdText, paramers);
        return result;
    }

    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    public IEnumerable<T> GetList(string cmdText, object obj)
    {
        var result = GetList<T>(cmdText, obj);
        return result;
    }

    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    public IEnumerable<T> GetList(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers)
    {
        var result = GetList<T>(cmdText, commandType, paramers);
        return result;
    }

    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    public async Task<IEnumerable<T>> GetListAsync(string cmdText, params DbParameter[] paramers)
    {
        var result = await GetListAsync<T>(cmdText, paramers);
        return result;
    }
    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>数据集合</returns>
    public async Task<IEnumerable<T>> GetListAsync(string cmdText, object obj)
    {
        var result = await GetListAsync<T>(cmdText, obj);
        return result;
    }
    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    public async Task<IEnumerable<T>> GetListAsync(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers)
    {
        var result = await GetListAsync(cmdText, commandType, paramers);
        return result;
    }
    #endregion

    #region GetEntityDicStr
    /// <summary>
    /// 获得单个实体字符串字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public IDictionary<string, string> GetEntityDicStr(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>> order, params string[] inclusionList)
    {
        var result = GetEntityDicStr<T>(where, order, inclusionList);
        return result;
    }

    /// <summary>
    /// 获得单个实体字符串字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<IDictionary<string, string>> GetEntityDicStrAsync(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>> order, params string[] inclusionList)
    {
        var result = await GetEntityDicStrAsync<T>(where, order, inclusionList);
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
    public IEnumerable<IDictionary<string, string>> GetEntityDicStrList(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList)
    {

        var result = GetEntityDicStrList<T>(where, order, top, inclusionList);
        return result;
    }
    /// <summary>
    /// 获得实体字典字符串列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList)
    {

        var result = await GetEntityDicStrListAsync<T>(where, order, top, inclusionList);
        return result;
    }
    #endregion

    #region InsertEntity
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public R InsertEntity(Expression<Func<T>> expression)
    {
        var result = InsertEntity<T, R>(expression);
        return result;
    }
    /// <summary>
    /// 通过实体类型 T 向表中增加一条记录
    /// </summary>
    /// <typeparam name="T">实体泛型(类)</typeparam>
    /// <param name="entity">实体对象</param>
    /// <param name="inclusionList"></param>
    /// <returns>所新增记录的 ID,如果返回 -1 则表示增加失败</returns>
    public R InsertEntity(T entity, params string[] inclusionList)
    {
        var result = InsertEntity<T, R>(entity, inclusionList);
        return result;
    }
    /// <summary>
    /// 通过实体类型 T 向表中增加一条记录
    /// </summary>
    /// <typeparam name="T">实体泛型(类)</typeparam>
    /// <param name="entity">实体对象</param>
    /// <param name="inclusionList"></param>
    /// <returns>所新增记录的 ID,如果返回 -1 则表示增加失败</returns>
    public async Task<R> InsertEntityAsync(T entity, params string[] inclusionList)
    {
        var result = await InsertEntityAsync<T, R>(entity, inclusionList);
        return result;
    }
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<R> InsertEntityAsync(Expression<Func<T>> expression)

    {
        var result = await InsertEntityAsync<T, R>(expression);
        return result;
    }
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public R InsertEntity<TR>(Expression<Func<TR>> expression)
        where TR : new()
    {
        var result = InsertEntity<TR, R>(expression);
        return result;
    }
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<R> InsertEntityAsync<TR>(Expression<Func<TR>> expression)
        where TR : new()
    {
        var result = await InsertEntityAsync<TR, R>(expression);
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
    public int UpdateEntity(Expression<Func<T>> expression, Expression<Func<T, bool>> where)
    {
        var result = UpdateEntity<T>(expression, where);
        return result;
    }

    /// <summary>
    /// 修改 T 实体
    /// </summary>
    /// <typeparam name="T">实体泛型</typeparam>
    /// <param name="entity">实体对象</param>
    /// <returns>影响数据条数</returns>
    public int UpdateEntity(T entity, params string[] inclusionList)
    {
        var result = UpdateEntity<T>(entity, inclusionList);
        return result;
    }

    /// <summary>
    /// 更新操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public async Task<int> UpdateEntityAsync(Expression<Func<T>> expression, Expression<Func<T, bool>> where)
    {
        var result = await UpdateEntityAsync<T>(expression, where);
        return result;
    }

    /// <summary>
    /// 修改 T 实体
    /// </summary>
    /// <typeparam name="T">实体泛型</typeparam>
    /// <param name="entity">实体对象</param>
    /// <returns>影响数据条数</returns>
    public async Task<int> UpdateEntityAsync(T entity, params string[] inclusionList)
    {
        var result = await UpdateEntityAsync<T>(entity, inclusionList);
        return result;
    }


    #endregion

    #region DeleteEntity
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="logic"></param>
    /// <returns></returns>
    public int DeleteEntity(Expression<Func<T, bool>> where, bool logic = true)
    {
        var result = DeleteEntity<T>(where, logic);
        return result;
    }

    /// <summary>
    /// 删除数据异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="where"></param>
    /// <param name="logic"></param>
    /// <returns></returns>
    public async Task<int> DeleteEntityAsync(Expression<Func<T, bool>> where, bool logic = true)
    {
        var result = await DeleteEntityAsync<T>(where, true);
        return result;
    }
    #endregion

    #endregion

    #region pager
    /// <summary>
    /// 返回分页实体列表集合
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T> GetResultByPager<T1>(Pager<T1> page, params string[] inclusionList)
        where T1 : BasePageCondition, new()
    {
        var result = GetResultByPager<T, T1>(page, inclusionList);
        return result;
    }

    /// <summary>
    /// 返回分页实体列表集合异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetResultByPageAsync<T1>(Pager<T1> page, params string[] inclusionList)
        where T1 : BasePageCondition, new()
    {
        var result = await GetResultByPageAsync<T, T1>(page, inclusionList);
        return result;
    }
    #endregion

}
