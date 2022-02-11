using DBLayer.Core.Condition;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace DBLayer.Core.Interface;

public interface IRepository<T, R> : IRepository where T : new()
{
    #region 增
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    R InsertEntity(Expression<Func<T>> expression);
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<R> InsertEntityAsync(Expression<Func<T>> expression);
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    R InsertEntity<TR>(Expression<Func<TR>> expression)
        where TR : new();
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<R> InsertEntityAsync<TR>(Expression<Func<TR>> expression)
        where TR : new();
    /// <summary>
    /// 通过实体类型 T 向表中增加一条记录
    /// </summary>
    /// <typeparam name="T">实体泛型(类)</typeparam>
    /// <param name="entity">实体对象</param>
    /// <param name="inclusionList"></param>
    /// <returns>所新增记录的 ID,如果返回 -1 则表示增加失败</returns>
    Task<R> InsertEntityAsync(T entity, params string[] inclusionList);
    /// <summary>
    /// 通过实体类型 T 向表中增加一条记录
    /// </summary>
    /// <typeparam name="T">实体泛型(类)</typeparam>
    /// <param name="entity">实体对象</param>
    /// <param name="inclusionList"></param>
    /// <returns>所新增记录的 ID,如果返回 -1 则表示增加失败</returns>
    R InsertEntity(T entity, params string[] inclusionList);
    #endregion
    #region 删
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="where"></param>
    /// <param name="logic"></param>
    /// <returns></returns>
    int DeleteEntity(Expression<Func<T, bool>> where, bool logic = true);

    /// <summary>
    /// 删除数据异步
    /// </summary>
    /// <param name="where"></param>
    /// <param name="logic"></param>
    /// <returns></returns>
    Task<int> DeleteEntityAsync(Expression<Func<T, bool>> where, bool logic = true);

    #endregion
    #region 改
    /// <summary>
    /// 更新操作
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    int UpdateEntity(Expression<Func<T>> expression, Expression<Func<T, bool>> where);
    /// <summary>
    /// 修改 T 实体
    /// </summary>
    /// <typeparam name="T">实体泛型</typeparam>
    /// <param name="entity">实体对象</param>
    /// <returns>影响数据条数</returns>
    int UpdateEntity(T entity, params string[] inclusionList);
    /// <summary>
    /// 修改 T 实体
    /// </summary>
    /// <typeparam name="T">实体泛型</typeparam>
    /// <param name="entity">实体对象</param>
    /// <returns>影响数据条数</returns>
    Task<int> UpdateEntityAsync(T entity, params string[] inclusionList);
    /// <summary>
    /// 更新操作
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    Task<int> UpdateEntityAsync(Expression<Func<T>> expression, Expression<Func<T, bool>> where);

    #endregion
    #region 查
    /// <summary>
    /// 获取 T 单个实体
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    T GetEntity(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList);
    /// <summary>
    /// 获取 T 单个实体
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    T GetEntity(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList);
    /// <summary>
    /// 获得单个对象
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="cmdText">sql</param>
    /// <param name="obj">参数</param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    T GetEntity(string cmdText, object obj, params string[] inclusionList);
    /// <summary>
    /// 获取 T 单个实体
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    T GetEntity(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList);
    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <param name="where"></param>
    /// <param name="order"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    Task<T> GetEntityAsync(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList);
    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="cmdText">sql</param>
    /// <param name="obj">参数</param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    Task<T> GetEntityAsync(string cmdText, object obj, params string[] inclusionList);

    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    Task<T> GetEntityAsync(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList);

    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <typeparam name="T">实体(泛型)类</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体</returns>
    Task<T> GetEntityAsync(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList);

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="where"></param>
    /// <param name="top"></param>
    /// <returns></returns>
    IEnumerable<T> GetEntityList(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList);
    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="exeEntityType">按属性/特性映射</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>实体集合</returns>
    IEnumerable<T> GetEntityList(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList);

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="exeEntityType">按属性/特性映射</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>实体集合</returns>
    IEnumerable<T> GetEntityList(string cmdText, object obj, params string[] inclusionList);

    /// <summary>
    /// 获取 T 实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体集合</returns>
    IEnumerable<T> GetEntityList(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList);
    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="where"></param>
    /// <param name="top"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetEntityListAsync(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList);
    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="exeEntityType">按属性/特性映射</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>实体集合</returns>
    Task<IEnumerable<T>> GetEntityListAsync(string cmdText, DbParameter[]? paramers = null, params string[] inclusionList);

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="exeEntityType">按属性/特性映射</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>实体集合</returns>
    Task<IEnumerable<T>> GetEntityListAsync(string cmdText, object obj, params string[] inclusionList);

    /// <summary>
    /// 获取 T 实体集合
    /// </summary>
    /// <typeparam name="T">实体(泛型)</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>T 实体集合</returns>
    Task<IEnumerable<T>> GetEntityListAsync(string cmdText, CommandType commandType = CommandType.Text, DbParameter[]? paramers = null, params string[] inclusionList);

    /// <summary>
    /// 获得单个实体字典
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    IDictionary<string, object> GetEntityDic(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList);

    /// <summary>
    /// 获得单个实体字典异步
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    Task<IDictionary<string, object>> GetEntityDicAsync(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>>? order = null, params string[] inclusionList);
    /// <summary>
    /// 获得实体字典列表
    /// </summary>
    /// <param name="where"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    IEnumerable<IDictionary<string, object>> GetEntityDicList(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList);
    /// <summary>
    /// 获得实体字典列表
    /// </summary>
    /// <param name="where"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList);
    /// <summary>
    /// 获得单个实体字符串字典
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    IDictionary<string, string> GetEntityDicStr(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>> order, params string[] inclusionList);
    /// <summary>
    /// 获得单个实体字符串字典
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    Task<IDictionary<string, string>> GetEntityDicStrAsync(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>> order, params string[] inclusionList);
    /// <summary>
    /// 获得实体字典字符串列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    IEnumerable<IDictionary<string, string>> GetEntityDicStrList(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList);
    /// <summary>
    /// 获得实体字典字符串列表
    /// </summary>
    /// <param name="func"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync(Expression<Func<T, bool>>? where = null, Expression<Func<OrderExpression<T>, object>>? order = null, int? top = null, params string[] inclusionList);

    /// <summary>
    /// 获取 T 单个实体
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    int GetEntityCount(Expression<Func<T, bool>> where);
    /// <summary>
    /// 获取 T 单个实体异步
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    Task<int> GetEntityCountAsync(Expression<Func<T, bool>> where);

    #endregion

    #region GetList
    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    IEnumerable<T> GetList(string cmdText, params DbParameter[] paramers);

    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    IEnumerable<T> GetList(string cmdText, object obj);

    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    IEnumerable<T> GetList(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers);

    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    Task<IEnumerable<T>> GetListAsync(string cmdText, params DbParameter[] paramers);
    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="obj">参数数组</param>
    /// <returns>数据集合</returns>
    Task<IEnumerable<T>> GetListAsync(string cmdText, object obj);
    /// <summary>
    /// 获取 T 类型的数据集
    /// </summary>
    /// <typeparam name="T">数据类型（泛型）</typeparam>
    /// <param name="cmdText">SQL 语句</param>
    /// <param name="commandType">cmdText 执行类型</param>
    /// <param name="paramers">参数数组</param>
    /// <returns>数据集合</returns>
    Task<IEnumerable<T>> GetListAsync(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers);
    #endregion

    #region 分页
    /// <summary>
    /// 返回分页实体列表集合
    /// </summary>
    /// <returns></returns>
    IEnumerable<T> GetResultByPager<T1>(Pager<T1> page, params string[] inclusionList)
        where T1 : BasePageCondition, new();

    /// <summary>
    /// 返回分页实体列表集合异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetResultByPageAsync<T1>(Pager<T1> page, params string[] inclusionList)
        where T1 : BasePageCondition, new();
    #endregion
    /// <summary>
    /// 生成id
    /// </summary>
    /// <returns></returns>
    R NewId();
    /// <summary>
    /// 获取用户id
    /// </summary>
    /// <returns></returns>
    R GetUserId();
}
