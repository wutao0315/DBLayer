using DBLayer.Core.Condition;
using DBLayer.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DBLayer.Core.Interface
{
    public interface IRepository
    {
        Func<Action<LogLevel, string, Exception>> Logger 
        {
            get => ()=>LogManager.CreateLogger(typeof(IRepository));
        }
        IUnitOfWork Uow { get; }
        IQueryable<T> Queryable<T>();

        #region insert
        /// <summary>
        /// 通过实体类型 T 向表中增加一条记录
        /// </summary>
        /// <typeparam name="T">实体泛型(类)</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="inclusionList"></param>
        /// <returns>所新增记录的 ID,如果返回 -1 则表示增加失败</returns>
        R InsertEntity<T, R>(T entity, params string[] inclusionList)
            where T : new();
        /// <summary>
        /// 通过实体类型 T 向表中增加一条记录
        /// </summary>
        /// <typeparam name="T">实体泛型(类)</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="inclusionList"></param>
        /// <returns>所新增记录的 ID,如果返回 -1 则表示增加失败</returns>
        Task<R> InsertEntityAsync<T, R>(T entity, params string[] inclusionList) 
            where T : new();
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        R InsertEntity<T, R>(Expression<Func<T>> expression)
            where T : new();
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<R> InsertEntityAsync<T, R>(Expression<Func<T>> expression)
            where T : new();
        #endregion
        #region delete
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="isLogic"></param>
        /// <returns></returns>
        int DeleteEntity<T>(Expression<Func<T, bool>> where, bool isLogic = true) 
            where T : new();

        /// <summary>
        /// 删除数据异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="isLogic"></param>
        /// <returns></returns>
        Task<int> DeleteEntityAsync<T>(Expression<Func<T, bool>> where, bool isLogic = true) 
            where T : new();

        #endregion
        #region update
        /// <summary>
        /// update entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        int UpdateEntity<T>(Expression<Func<T>> expression, Expression<Func<T, bool>> where) 
            where T : new();

        /// <summary>
        /// update entity async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<int> UpdateEntityAsync<T>(Expression<Func<T>> expression, Expression<Func<T, bool>> where) 
            where T : new();

        #endregion
        #region select
        #region GetEntity

        /// <summary>
        /// 获取 T 单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        T GetEntity<T>(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>> order = null, params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取 T 单个实体
        /// </summary>
        /// <typeparam name="T">实体(泛型)类</typeparam>
        /// <param name="paramers">参数数组</param>
        /// <returns>T 实体</returns>
        T GetEntity<T>(string cmdText, DbParameter[] paramers = null, params string[] inclusionList)
            where T : new();
        /// <summary>
        /// 获得单个对象
        /// </summary>
        /// <typeparam name="T">实体(泛型)类</typeparam>
        /// <param name="cmdText">sql</param>
        /// <param name="obj">参数</param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        T GetEntity<T>(string cmdText, object obj, params string[] inclusionList)
            where T : new();
        /// <summary>
        /// 获取 T 单个实体
        /// </summary>
        /// <typeparam name="T">实体(泛型)类</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>T 实体</returns>
        T GetEntity<T>(string cmdText, CommandType commandType = CommandType.Text, DbParameter[] paramers = null, params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取 T 单个实体异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> where,
            Expression<Func<OrderExpression<T>, object>> order = null,
            params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取 T 单个实体异步
        /// </summary>
        /// <typeparam name="T">实体(泛型)类</typeparam>
        /// <param name="cmdText">sql</param>
        /// <param name="obj">参数</param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        Task<T> GetEntityAsync<T>(string cmdText, object obj, params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取 T 单个实体异步
        /// </summary>
        /// <typeparam name="T">实体(泛型)类</typeparam>
        /// <param name="paramers">参数数组</param>
        /// <returns>T 实体</returns>
        Task<T> GetEntityAsync<T>(string cmdText, DbParameter[] paramers = null, params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取 T 单个实体异步
        /// </summary>
        /// <typeparam name="T">实体(泛型)类</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>T 实体</returns>
        Task<T> GetEntityAsync<T>(string cmdText, CommandType commandType = CommandType.Text, DbParameter[] paramers = null, params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取 查询 数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        int GetEntityCount<T>(Expression<Func<T, bool>> where)
            where T : new();

        /// <summary>
        /// 获取 T 单个实体异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<int> GetEntityCountAsync<T>(Expression<Func<T, bool>> where)
            where T : new();
        #endregion

        #region GetEntityDic
        /// <summary>
        /// 获得单个实体字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        IDictionary<string, object> GetEntityDic<T>(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>> order = null, params string[] inclusionList)
            where T : new();


        /// <summary>
        /// 获得字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        IDictionary<string, object> GetEntityDic(string cmdText, DbParameter[] parameters = null, params string[] inclusionList );

        /// <summary>
        /// 获得字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="obj">参数</param>
        /// <param name="inclusionList">事物</param>
        /// <returns></returns>
        IDictionary<string, object> GetEntityDic(string cmdText, object obj, params string[] inclusionList);

        /// <summary>
        /// 获得字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        IDictionary<string, object> GetEntityDic(string cmdText, CommandType commandType = CommandType.Text, DbParameter[] parameters = null, params string[] inclusionList);


        /// <summary>
        /// 获得单个实体字典异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<IDictionary<string, object>> GetEntityDicAsync<T>(Expression<Func<T, bool>> where, Expression<Func<OrderExpression<T>, object>> order = null, params string[] inclusionList)
            where T : new();
        /// <summary>
        /// 获得字典异步
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<IDictionary<string, object>> GetEntityDicAsync(string cmdText, DbParameter[] parameters = null, params string[] inclusionList);
        /// <summary>
        /// 获得字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="obj">参数</param>
        /// <param name="inclusionList">事物</param>
        /// <returns></returns>
        Task<IDictionary<string, object>> GetEntityDicAsync(string cmdText, object obj, params string[] inclusionList);
        /// <summary>
        /// 获得字典异步
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<IDictionary<string, object>> GetEntityDicAsync(string cmdText, CommandType commandType = CommandType.Text, DbParameter[] parameters = null, params string[] inclusionList);
        #endregion

        #region GetEntityDicList

        /// <summary>
        /// 获得实体字典列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        IEnumerable<IDictionary<string, object>> GetEntityDicList<T>(Expression<Func<T, bool>> where = null,
            Expression<Func<OrderExpression<T>, object>> order = null,
            int? top = null,
            params string[] inclusionList)
            where T : new();


        /// <summary>
        /// 获得字典列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        IEnumerable<IDictionary<string, object>> GetEntityDicList(string cmdText,
            DbParameter[] parameters = null, params string[] inclusionList);

        /// <summary>
        /// 获得字典列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        IEnumerable<IDictionary<string, object>> GetEntityDicList(string cmdText, object obj = null, params string[] inclusionList);

        /// <summary>
        /// 获得字典列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="parameters">参数数组</param>
        /// <exception cref="">这里是 Exception</exception>
        /// <returns></returns>
        IEnumerable<IDictionary<string, object>> GetEntityDicList(string cmdText, CommandType commandType = CommandType.Text, DbParameter[] parameters = null, params string[] inclusionList);


        /// <summary>
        /// 获得实体字典列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync<T>(Expression<Func<T, bool>> where = null,
            Expression<Func<OrderExpression<T>, object>> order = null,

            int? top = null,
            params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获得字典列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync(string cmdText,

            DbParameter[] parameters = null, params string[] inclusionList);
        /// <summary>
        /// 获得字典列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync(string cmdText,
            object obj,

            params string[] inclusionList);
        /// <summary>
        /// 获得字典列表 异步
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="parameters">参数数组</param>
        /// <exception cref="">这里是 Exception</exception>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, object>>> GetEntityDicListAsync(string cmdText,

            CommandType commandType = CommandType.Text,
            DbParameter[] parameters = null, params string[] inclusionList);
        #endregion

        #region GetEntityList
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        IEnumerable<T> GetEntityList<T>(Expression<Func<T, bool>> where = null,
            Expression<Func<OrderExpression<T>, object>> order = null,

            int? top = null,
            params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T">实体(泛型)</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="exeEntityType">按属性/特性映射</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>实体集合</returns>
        IEnumerable<T> GetEntityList<T>(string cmdText,

            DbParameter[] paramers = null, params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T">实体(泛型)</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="exeEntityType">按属性/特性映射</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>实体集合</returns>
        IEnumerable<T> GetEntityList<T>(string cmdText, object obj, params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取 T 实体集合
        /// </summary>
        /// <typeparam name="T">实体(泛型)</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>T 实体集合</returns>
        IEnumerable<T> GetEntityList<T>(string cmdText,

            CommandType commandType = CommandType.Text,
            DbParameter[] parameters = null, params string[] inclusionList) where T : new();

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetEntityListAsync<T>(Expression<Func<T, bool>> where = null,
            Expression<Func<OrderExpression<T>, object>> order = null,

            int? top = null,
            params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T">实体(泛型)</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="exeEntityType">按属性/特性映射</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>实体集合</returns>
        Task<IEnumerable<T>> GetEntityListAsync<T>(string cmdText, DbParameter[] paramers = null, params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T">实体(泛型)</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="exeEntityType">按属性/特性映射</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>实体集合</returns>
        Task<IEnumerable<T>> GetEntityListAsync<T>(string cmdText, object obj, params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获取 T 实体集合
        /// </summary>
        /// <typeparam name="T">实体(泛型)</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>T 实体集合</returns>
        Task<IEnumerable<T>> GetEntityListAsync<T>(string cmdText,

            CommandType commandType = CommandType.Text,
            DbParameter[] parameters = null, params string[] inclusionList)
            where T : new();
        #endregion

        #region GetList

        /// <summary>
        /// 获取 T 类型的数据集
        /// </summary>
        /// <typeparam name="T">数据类型（泛型）</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>数据集合</returns>
        IEnumerable<T> GetList<T>(string cmdText, params DbParameter[] paramers);

        /// <summary>
        /// 获取 T 类型的数据集
        /// </summary>
        /// <typeparam name="T">数据类型（泛型）</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="obj">参数数组</param>
        /// <returns>数据集合</returns>
        IEnumerable<T> GetList<T>(string cmdText, object obj);

        /// <summary>
        /// 获取 T 类型的数据集
        /// </summary>
        /// <typeparam name="T">数据类型（泛型）</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>数据集合</returns>
        IEnumerable<T> GetList<T>(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] parameters);
        /// <summary>
        /// 获取 T 类型的数据集
        /// </summary>
        /// <typeparam name="T">数据类型（泛型）</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据集合</returns>
        Task<IEnumerable<T>> GetListAsync<T>(string cmdText, params DbParameter[] parameters);
        /// <summary>
        /// 获取 T 类型的数据集
        /// </summary>
        /// <typeparam name="T">数据类型（泛型）</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="obj">参数数组</param>
        /// <returns>数据集合</returns>
        Task<IEnumerable<T>> GetListAsync<T>(string cmdText, object obj);
        /// <summary>
        /// 获取 T 类型的数据集
        /// </summary>
        /// <typeparam name="T">数据类型（泛型）</typeparam>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据集合</returns>
        Task<IEnumerable<T>> GetListAsync<T>(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] parameters);
        #endregion

        #region GetEntityDicStr
        /// <summary>
        /// 获得单个实体字符串字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        IDictionary<string, string> GetEntityDicStr<T>(Expression<Func<T, bool>> where,
            Expression<Func<OrderExpression<T>, object>> order,

            params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获得字符串字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        IDictionary<string, string> GetEntityDicStr(string cmdText,

            DbParameter[] parameters = null, params string[] inclusionList);
        /// <summary>
        /// 获得字符串字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        IDictionary<string, string> GetEntityDicStr(string cmdText, object obj, params string[] inclusionList);

        /// <summary>
        /// 获得字符串字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        IDictionary<string, string> GetEntityDicStr(string cmdText,

            CommandType commandType = CommandType.Text,
            DbParameter[] parameters = null, params string[] inclusionList);


        /// <summary>
        /// 获得单个实体字符串字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<IDictionary<string, string>> GetEntityDicStrAsync<T>(Expression<Func<T, bool>> where,
            Expression<Func<OrderExpression<T>, object>> order,

            params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获得字符串字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<IDictionary<string, string>> GetEntityDicStrAsync(string cmdText, DbParameter[] parameters = null, params string[] inclusionList);
        /// <summary>
        /// 获得字符串字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<IDictionary<string, string>> GetEntityDicStrAsync(string cmdText, object obj, params string[] inclusionList);
        /// <summary>
        /// 获得字符串字典
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<IDictionary<string, string>> GetEntityDicStrAsync(string cmdText,

            CommandType commandType = CommandType.Text,
            DbParameter[] parameters = null,
            params string[] inclusionList);

        #endregion

        #region GetEntityDicStrList
        /// <summary>
        /// 获得实体字典字符串列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        IEnumerable<IDictionary<string, string>> GetEntityDicStrList<T>(Expression<Func<T, bool>> where = null,
            Expression<Func<OrderExpression<T>, object>> order = null,

            int? top = null,
            params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获得典字符串列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        IEnumerable<IDictionary<string, string>> GetEntityDicStrList(string cmdText, DbParameter[] parameters = null, params string[] inclusionList);
        /// <summary>
        /// 获得典字符串列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        IEnumerable<IDictionary<string, string>> GetEntityDicStrList(string cmdText, object obj, params string[] inclusionList);

        /// <summary>
        /// 获得典字符串列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="parameters">参数数组</param>
        /// <exception cref="">这里是 Exception</exception>
        /// <returns></returns>
        IEnumerable<IDictionary<string, string>> GetEntityDicStrList(string cmdText,
            CommandType commandType = CommandType.Text,
            DbParameter[] parameters = null, params string[] inclusionList);

        /// <summary>
        /// 获得实体字典字符串列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync<T>(Expression<Func<T, bool>> where = null,
            Expression<Func<OrderExpression<T>, object>> order = null,

            int? top = null,
            params string[] inclusionList)
            where T : new();

        /// <summary>
        /// 获得典字符串列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync(string cmdText, DbParameter[] parameters = null, params string[] inclusionList);

        /// <summary>
        /// 获得典字符串列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync(string cmdText, object obj, params string[] inclusionList);
        /// <summary>
        /// 获得典字符串列表
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commandType">cmdText 执行类型</param>
        /// <param name="parameters">参数数组</param>
        /// <exception cref="">这里是 Exception</exception>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, string>>> GetEntityDicStrListAsync(string cmdText,

            CommandType commandType = CommandType.Text,
            DbParameter[] parameters = null, params string[] inclusionList);
        #endregion
        #endregion

        #region Other
        /// <summary>
        /// 获取新id
        /// </summary>
        /// <returns></returns>
        R NewId<R>();
        /// <summary>
        /// 获取用户申明
        /// </summary>
        /// <returns></returns>
        ClaimsPrincipal GetUser();
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <returns></returns>
        string GetUserName();
        /// <summary>
        /// 名称昵称
        /// </summary>
        /// <returns></returns>
        string GetText();
        /// <summary>
        /// 用户名和昵称组合
        /// </summary>
        /// <returns></returns>
        string GetUserText(string split = "/");
        /// <summary>
        /// 获取权限集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetPermissions();
        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        R GetUserId<R>();
        /// <summary>
        /// 获取查询需要的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inclusionList"></param>
        /// <param name="prex"></param>
        /// <returns></returns>
        string GetSelectFields<T>(string[] inclusionList = null, string prex="");
        #region ExecuteNonQuery
        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">>SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>SQL 语句所影响的行数</returns>
        int ExecuteNonQuery(string cmdText, params DbParameter[] paramers);

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">>SQL 语句</param>
        /// <param name="obj">参数数组</param>
        /// <returns>SQL 语句所影响的行数</returns>
        int ExecuteNonQuery(string cmdText, object obj);


        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">>SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>SQL 语句所影响的行数</returns>
        Task<int> ExecuteNonQueryAsync(string cmdText, params DbParameter[] paramers);

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="cmdText">>SQL 语句</param>
        /// <param name="obj">参数数组</param>
        /// <returns>SQL 语句所影响的行数</returns>
        Task<int> ExecuteNonQueryAsync(string cmdText, object obj);


        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText"></param>
        /// <param name="paramers"></param>
        /// <returns></returns>
        T ExecuteScalar<T>(string cmdText, params DbParameter[] paramers);
        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        T ExecuteScalar<T>(string cmdText, object obj);

        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText"></param>
        /// <param name="commandType"></param>
        /// <param name="paramers"></param>
        /// <returns></returns>
        T ExecuteScalar<T>(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers);
        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>返回首行首列</returns>
        object ExecuteScalar(string cmdText, params DbParameter[] paramers);
        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="obj">参数数组</param>
        /// <returns>返回首行首列</returns>
        object ExecuteScalar(string cmdText, object obj);

        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText"></param>
        /// <param name="commandType"></param>
        /// <param name="paramers"></param>
        /// <returns></returns>
        object ExecuteScalar(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers);


        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>返回首行首列</returns>
        Task<object> ExecuteScalarAsync(string cmdText, params DbParameter[] paramers);
        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>返回首行首列</returns>
        Task<object> ExecuteScalarAsync(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers);
        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="obj">参数数组</param>
        /// <returns>返回首行首列</returns>
        Task<object> ExecuteScalarAsync(string cmdText, object obj);
        /// <summary>
        /// 执行数据库操作，返回首行首列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText"></param>
        /// <param name="commandType"></param>
        /// <param name="paramers"></param>
        /// <returns></returns>
        Task<T> ExecuteScalarAsync<T>(string cmdText, CommandType commandType = CommandType.Text, params DbParameter[] paramers);
        #endregion

        #region IsExists
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
        bool IsExists(string cmdText, params DbParameter[] paramers);
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="obj">参数数组</param>
        /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
        bool IsExists(string cmdText, object obj);

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commondType">cmdText 执行类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
        bool IsExists(string cmdText, CommandType commondType = CommandType.Text, params DbParameter[] paramers);

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
        Task<bool> IsExistsAsync(string cmdText, params DbParameter[] paramers);

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="obj">参数数组</param>
        /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
        Task<bool> IsExistsAsync(string cmdText, object obj);

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="commondType">cmdText 执行类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>记录是否存在,true:表示存在记录,false:表示不存在记录</returns>
        Task<bool> IsExistsAsync(string cmdText, CommandType commondType = CommandType.Text, params DbParameter[] paramers);
        #endregion

        #region GetDataSet
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>数据集</returns>
        DataSet GetDataSet(string cmdText, params DbParameter[] paramers);
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>数据集</returns>
        DataSet GetDataSet(string cmdText, object obj);

        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>数据集</returns>
        DataSet GetDataSet(string cmdText, CommandType commondType = CommandType.Text, params DbParameter[] paramers);

        #endregion

        #endregion

        #region 分页
        /// <summary>
        /// 返回分页实体列表集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetResultByPager<T, T1>(Pager<T1> page, params string[] inclusionList)
            where T : new()
            where T1 : BasePageCondition, new();

        /// <summary>
        /// 返回分页实体列表集合异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetResultByPageAsync<T, T1>(Pager<T1> page, params string[] inclusionList)
            where T : new()
            where T1 : BasePageCondition, new();

        /// <summary>
        /// 返回分页实体字典列表集合
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        IEnumerable<IDictionary<string, object>> GetResultByPagerDic<T1>(Pager<T1> page)
            where T1 : BasePageCondition, new();

        /// <summary>
        /// 返回分页实体字典列表集合异步
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, object>>> GetResultByPagerDicAsync<T1>(Pager<T1> page)
            where T1 : BasePageCondition, new();


        /// <summary>
        /// 返回分页实体字典列表集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        IEnumerable<IDictionary<string, object>> GetResultByPagerDic<T, T1>(Pager<T1> page, params string[] inclusionList)
            where T : new()
            where T1 : BasePageCondition, new();

        /// <summary>
        /// 返回分页实体字典列表集合异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, object>>> GetResultByPagerDicAsync<T, T1>(Pager<T1> page, params string[] inclusionList)
            where T : new()
            where T1 : BasePageCondition, new();

        /// <summary>
        /// 返回分页数据集
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        public DataSet GetResultByPagerDs<T1>(Pager<T1> page)
            where T1 : BasePageCondition, new();
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
        DbParameter CreateParameter(string pName, object pValue, DbType? pType = null, int? pSize = null);

        #region output
        /// <summary>
        /// 创建一个带有返回值的存储过程参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="sourceType">映射属性值</param>
        /// <returns></returns>
        DbParameter CreateOutPutParameter(string pName, string propertyName, object propertyValue);

        /// <summary>
        /// 创建一个带有返回值的存储过程参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="pType">参数类型</param>
        /// <param name="pSize">长度</param>
        /// <returns></returns>
        DbParameter CreateOutPutParameter(string pName, DbType? pType = null, int? pSize = null);

        /// <summary>
        /// 创建一个带有返回值的存储过程参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="pValue">参数值</param>
        /// <param name="pType">参数类型</param>
        /// <param name="pSize">长度</param>
        /// <returns></returns>
        DbParameter CreateOutPutParameter(string pName, object pValue, DbType? pType = null, int? pSize = null);
        #endregion

        #region 返回参数类型
        /// <summary>
        /// 创建一个带有返回值的存储过程参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="pType">参数类型</param>
        /// <param name="pSize">长度</param>
        /// <returns></returns>
        DbParameter CreateReturnValueParameter(string pName, DbType? pType = null, int? pSize = null);

        /// <summary>
        /// 创建一个带有返回值的存储过程参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="pValue">参数值</param>
        /// <param name="pType">参数类型</param>
        /// <param name="pSize">长度</param>
        /// <returns></returns>
        DbParameter CreateReturnValueParameter(string pName, object pValue, DbType? pType = null, int? pSize = null);
        #endregion

        #endregion
    }
}
