using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Core.Interface
{
    public interface IDataSource
    {
        #region 连接命令
        public IDbFactory DbFactory { get; }

        public IPagerGenerator PagerGenerator { get; }
        #endregion

        #region 数据执行方法
        /// <summary>
        /// 创建带参数的只读器
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="trans">事务</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>reader</returns>
        DbDataReader CreateDataReader(string cmdText, DbConnection connection, CommandType commandType = CommandType.Text, params DbParameter[] paramers);


        /// <summary>
        /// 创建带参数的只读器
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="tran">事务</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>reader</returns>
        Task<DbDataReader> CreateDataReaderAsync(string cmdText, DbConnection connection, CommandType commandType = CommandType.Text, params DbParameter[] paramers);

        /// <summary>
        /// 执行一个带参数的SQL/存储过程有事务的
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="commmandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>影响行数</returns>
        int ExecuteNonQuery(string cmdText, DbConnection connection, CommandType commandType = CommandType.Text, params DbParameter[] paramers);

        /// <summary>
        /// 执行一个带参数的SQL/存储过程有事务的
        /// </summary>
        /// 
        /// <param name="cmdText">命令</param>
        /// <param name="commmandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>影响行数</returns>
        Task<int> ExecuteNonQueryAsync(string cmdText, DbConnection connection, CommandType commandType = CommandType.Text, params DbParameter[] paramers);

        /// <summary>
        /// 执行一个带参数的SQL/存储过程返回首行首列
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="trans">事务</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>值</returns>
        object ExecuteScalar(string cmdText, DbConnection connection, CommandType commandType = CommandType.Text, params DbParameter[] paramers);

        /// <summary>
        /// 执行一个带参数的SQL/存储过程返回首行首列
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>值</returns>
        Task<object> ExecuteScalarAsync(string cmdText, DbConnection connection, CommandType commandType = CommandType.Text, params DbParameter[] paramers);

        /// <summary>
        /// 执行一个语句返回数据集
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>数据集</returns>
        DataSet CreateDataSet(string cmdText, DbConnection connection, CommandType commandType = CommandType.Text, params DbParameter[] paramers);

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
        /// <summary>
        /// 替换参数
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        string ReplaceParameter(string cmdText);
        #endregion

        #endregion

    }
}
