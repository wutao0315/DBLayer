using DBLayer.Core.Interface;
using DBLayer.Core.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;


namespace DBLayer.Persistence.Data
{
    public class DataSource: IDataSource
    {
        private static readonly Func<Action<LogLevel, string, Exception>> Logger = () => LogManager.CreateLogger(typeof(DataSource));
        private static readonly Func<Action<LogLevel, string, Exception>> LoggerTrans = () => LogManager.CreateLogger(typeof(UnitOfWork));

        public DataSource(IDbFactory dbFactory, 
            IPagerGenerator pagerGenerator)
        {
            DbFactory = dbFactory;
            PagerGenerator = pagerGenerator;
        }

        #region 连接命令

        public IDbFactory DbFactory { private set; get; }

        public IPagerGenerator PagerGenerator { private set; get; }

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="connection">链接</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>command</returns>
        internal DbCommand CreateCommand(string cmdText, DbConnection connection, CommandType commandType=CommandType.Text, params DbParameter[] paramers)
        {
            Logger.LogSQL(cmdText, paramers);

            var dbCmd = connection.CreateCommand();

            if (DbFactory.LongDbConnection == connection) 
            {
                dbCmd.Transaction = DbFactory.LongDbTransaction;
                LoggerTrans.LogSQL(cmdText, paramers);
            }
            dbCmd.CommandText = cmdText;
            dbCmd.CommandType = commandType;
            
            if (paramers != null){
                foreach (var item in paramers)
                {
                    dbCmd.Parameters.Add(item);
                }
            }

            return dbCmd;
        }
        #endregion

        #region 数据执行方法

        /// <summary>
        /// 创建带参数的只读器
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="connection">链接</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>reader</returns>
        public virtual DbDataReader CreateDataReader(string cmdText, DbConnection connection, CommandType commandType= CommandType.Text, params DbParameter[] paramers)
        {
            var reader = CreateCommand(cmdText, connection, commandType, paramers).ExecuteReader();

            return reader;
        }


        /// <summary>
        /// 创建带参数的只读器
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="connection">链接</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>reader</returns>
        public virtual async Task<DbDataReader> CreateDataReaderAsync(string cmdText, DbConnection connection, CommandType commandType=CommandType.Text, params DbParameter[] paramers)
        {
            var reader = await CreateCommand( cmdText, connection, commandType, paramers).ExecuteReaderAsync();

            return reader;
        }



        /// <summary>
        /// 执行一个带参数的SQL/存储过程有事务的
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="connection">链接</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery(string cmdText, DbConnection connection, CommandType commandType=CommandType.Text, params DbParameter[] paramers)
        {
            var dbCmd = CreateCommand(cmdText, connection, commandType, paramers);
            var retval = dbCmd.ExecuteNonQuery();
            return retval;
        }

        /// <summary>
        /// 执行一个带参数的SQL/存储过程有事务的
        /// </summary>
        /// 
        /// <param name="cmdText">命令</param>
        /// <param name="connection">链接</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> ExecuteNonQueryAsync( string cmdText, DbConnection connection, CommandType commandType= CommandType.Text, params DbParameter[] paramers)
        {
            var dbCmd = CreateCommand(cmdText, connection, commandType, paramers);
            var retval = await dbCmd.ExecuteNonQueryAsync();
            return retval;
        }

        /// <summary>
        /// 执行一个带参数的SQL/存储过程返回首行首列
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="connection">链接</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>值</returns>
        public virtual object ExecuteScalar( string cmdText, DbConnection connection, CommandType commandType=CommandType.Text, params DbParameter[] paramers)
        {
            var dbCmd = CreateCommand(cmdText, connection, commandType, paramers);
            object retval = dbCmd.ExecuteScalar();
            return retval;
        }

        /// <summary>
        /// 执行一个带参数的SQL/存储过程返回首行首列
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="connection">链接</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>值</returns>
        public virtual async Task<object> ExecuteScalarAsync( string cmdText, DbConnection connection, CommandType commandType=CommandType.Text, params DbParameter[] paramers)
        {
            var dbCmd = CreateCommand(cmdText, connection, commandType, paramers);
            object retval = await dbCmd.ExecuteScalarAsync();
            return retval;
        }

        /// <summary>
        /// 执行一个语句返回数据集
        /// </summary>
        /// <param name="cmdText">命令</param>
        /// <param name="connection">链接</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="paramers">参数数组</param>
        /// <returns>数据集</returns>
        public virtual DataSet CreateDataSet( string cmdText, DbConnection connection, CommandType commandType=CommandType.Text, params DbParameter[] paramers)
        {
            var dataSet = new DataSet();
            var dbCmd = CreateCommand(cmdText, connection, commandType, paramers);
            var dbDA = DbFactory.DbProviderFactory.CreateDataAdapter();
            dbDA.SelectCommand = dbCmd;
            dbDA.Fill(dataSet);
            return dataSet;
        }

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
        public virtual DbParameter CreateParameter(string pName, object pValue, DbType? pType=null, int? pSize=null)
        {
            pName = ReplaceParameter(pName);
            
            var paramer = DbFactory.DbProviderFactory.CreateParameter();
            paramer.ParameterName = pName;
            paramer.Value = pValue;
            if (pType != null)
            {
                paramer.DbType = pType.Value;
            }
            if (pSize != null)
            {
                paramer.Size = pSize.Value;
            }

            return paramer;
        }

        #region output
        /// <summary>
        /// 创建一个带有返回值的存储过程参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="sourceType">映射属性值</param>
        /// <returns></returns>
        public virtual DbParameter CreateOutPutParameter(string pName,string propertyName,object propertyValue)
        {
            pName = ReplaceParameter(pName);
            var paramer = DbFactory.DbProviderFactory.CreateParameter();
            paramer.ParameterName = pName;
            paramer.Direction = ParameterDirection.Output;

            paramer.SetValueByPropertyName(propertyName, propertyValue);

            return paramer;
        }

        /// <summary>
        /// 创建一个带有返回值的存储过程参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="pType">参数类型</param>
        /// <param name="pSize">长度</param>
        /// <returns></returns>
        public virtual DbParameter CreateOutPutParameter(string pName, DbType? pType=null, int? pSize=null)
        {
            pName = ReplaceParameter(pName);

            var paramer = DbFactory.DbProviderFactory.CreateParameter();
            paramer.ParameterName = pName;
            if (pType == null)
            {
                paramer.DbType = pType.Value;
            }
            if (pSize != null)
            {
                paramer.Size = pSize.Value;
            }
            paramer.Direction = ParameterDirection.Output;
           
            return paramer;
        }

        /// <summary>
        /// 创建一个带有返回值的存储过程参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="pValue">参数值</param>
        /// <param name="pType">参数类型</param>
        /// <param name="pSize">长度</param>
        /// <returns></returns>
        public virtual DbParameter CreateOutPutParameter(string pName, object pValue, DbType? pType=null, int? pSize=null)
        {
            pName = ReplaceParameter(pName);
            var paramer = DbFactory.DbProviderFactory.CreateParameter();
            paramer.ParameterName = pName;
            if (pType==null)
            {
                paramer.DbType = pType.Value;
            }
            if (pSize!=null)
            {
                paramer.Size = pSize.Value;
            }
            paramer.Direction = ParameterDirection.Output;
            paramer.Value = pValue;

            return paramer;
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
            pName = ReplaceParameter(pName);
            var paramer = DbFactory.DbProviderFactory.CreateParameter();
            paramer.ParameterName = pName;
            if (pType == null)
            {
                paramer.DbType = pType.Value;
            }
            if (pSize != null)
            {
                paramer.Size = pSize.Value;
            }
            paramer.Direction = ParameterDirection.ReturnValue;

            return paramer;
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
            pName=ReplaceParameter(pName);
            var paramer = DbFactory.DbProviderFactory.CreateParameter();
            paramer.ParameterName = pName;
            if (pType == null)
            {
                paramer.DbType = pType.Value;
            }
            if (pSize != null)
            {
                paramer.Size = pSize.Value;
            }
            paramer.Direction = ParameterDirection.ReturnValue;
            paramer.Value = pValue;

            return paramer;
        }
        #endregion

        #endregion
        #region 过滤sql 支持 #参数
        public string ReplaceParameter(string cmdText)
        {
            cmdText = cmdText?.Replace("@", DbFactory.DbProvider.ParameterPrefix);
            return cmdText;
        }
        #endregion
    }

   
}
