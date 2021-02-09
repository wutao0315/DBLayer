using DBLayer.Core;
using DBLayer.Core.Interface;
using DBLayer.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Persistence.PagerGenerator
{
    public class SqlServerPagerGenerator : IPagerGenerator
    {
        /// <summary>
        ///  查询所有数据-不包含字段*
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="paramerList"></param>
        /// <param name="whereStr"></param>
        /// <param name="orderStr"></param>
        /// <param name="top"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        public StringBuilder GetSelectCmdText<T>(IDataSource dataSource, ref List<DbParameter> paramerList, StringBuilder whereStr, StringBuilder orderStr, int? top = null, params string[] inclusionList)
        {
            var cmdText = new StringBuilder();
            var topStr = new StringBuilder();
            if (top != null && top.Value > 0)
            {
                var topparameter = dataSource.DbFactory.DbProvider.ParameterPrefix + "topParameter";
                topStr.AppendFormat("TOP({0})", topparameter);
                paramerList.Add(dataSource.CreateParameter(topparameter, top.Value));
            }

            var entityType = typeof(T);
            var tableName = entityType.GetDataTableName();
            tableName = string.Format(dataSource.DbFactory.DbProvider.FieldFormat, tableName);
            var fields = dataSource.CreateAllEntityDicSql<T>(inclusionList);

            cmdText.AppendFormat("SELECT {2} {1} FROM {0} {3} {4} ", tableName, fields, topStr, whereStr, orderStr);

            return cmdText;
        }
        /// <summary>
        /// 查询所有数据-包含字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="paramerList"></param>
        /// <param name="whereStr"></param>
        /// <param name="orderStr"></param>
        /// <param name="top"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        public StringBuilder GetSelectDictionaryCmdText<T>(IDataSource dataSource, ref List<DbParameter> paramerList, StringBuilder whereStr, StringBuilder orderStr, int? top = null, params string[] inclusionList)
        {
            var cmdText = new StringBuilder();
            var topStr = new StringBuilder();
            if (top != null && top.Value > 0)
            {
                var topparameter = dataSource.DbFactory.DbProvider.ParameterPrefix + "topParameter";
                topStr.AppendFormat("TOP({0})", topparameter);
                paramerList.Add(dataSource.CreateParameter(topparameter, top.Value));
            }

            var entityType = typeof(T);
            var tableName = entityType.GetDataTableName();
            tableName = string.Format(dataSource.DbFactory.DbProvider.FieldFormat, tableName);

            var fields = dataSource.CreateAllEntityDicSql<T>(inclusionList);

            cmdText.AppendFormat("SELECT {2} {1} FROM {0} {3} {4} ", tableName, fields, topStr, whereStr, orderStr);

            return cmdText;
        }
        /// <summary>
        /// 在insert中id自动编号的处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="sbField"></param>
        /// <param name="sbValue"></param>
        public void ProcessInsertId<T>(string fieldName, ref StringBuilder sbField, ref StringBuilder sbValue)
        {
        }
        /// <summary>
        /// 执行Insert
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="insertCmd">执行命令</param>
        /// <param name="paramerList">参数列表</param>
        /// <param name="connection">会话</param>
        /// <returns>Id</returns>
        public object InsertExecutor<T>(IDataSource dataSource, StringBuilder insertCmd, List<DbParameter> paramerList, DbConnection connection)
        {

            var cmdText = new StringBuilder();
            cmdText.AppendFormat("{0};{1}", insertCmd, dataSource.DbFactory.DbProvider.SelectKey);

            var newID = dataSource.ExecuteScalar( cmdText.ToString(), connection, CommandType.Text, paramerList.ToArray());

            return newID;
        }
        /// <summary>
        /// 执行Insert
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="insertCmd">执行命令</param>
        /// <param name="paramerList">参数列表</param>
        /// <param name="connection">链接</param>
        /// <returns>Id</returns>
        public async Task<object> InsertExecutorAsync<T>(IDataSource dataSource, StringBuilder insertCmd, List<DbParameter> paramerList, DbConnection connection)
        {

            var cmdText = new StringBuilder();
            cmdText.AppendFormat("{0};{1}", insertCmd, dataSource.DbFactory.DbProvider.SelectKey);

            var newID = await dataSource.ExecuteScalarAsync(cmdText.ToString(), connection, CommandType.Text, paramerList.ToArray());

            return newID;
        }

        /// <summary>
        /// 查询条件 InFunc
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public StringBuilder GetInFunc(Func<StringBuilder> left, Func<StringBuilder> right)
        {
            var leftString = left();
            var rightString = right();
            var result = new StringBuilder();
            result.AppendFormat("{0} IN (SELECT [value] FROM f_SPLIT({1},default))", leftString, rightString);
            return result;

        }
        /// <summary>
        /// 查询条件 NotInFunc
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public StringBuilder GetNotInFunc(Func<StringBuilder> left, Func<StringBuilder> right)
        {
            var leftString = left();
            var rightString = right();
            var result = new StringBuilder();
            result.AppendFormat("{0} NOT IN (SELECT [value] FROM f_SPLIT({1},default))", leftString, rightString);
            return result;
        }
        /// <summary>
        /// 生成分页sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="unionText"></param>
        /// <param name="tableName"></param>
        /// <param name="fldName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <param name="group"></param>
        /// <param name="sort"></param>
        /// <param name="paramers"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        public (StringBuilder, DbParameter[]) GetPageCmdText(IDataSource dataSource, string unionText, string tableName, string fldName, ref int? pageIndex, ref int? pageSize, string filter, string group, string sort,params DbParameter[] paramers)
        {
            var cmdText = new StringBuilder();
            var strFilter = "";
            var strGroup = "";
            pageIndex ??= 1;
            pageSize ??= 20;

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 20;
            }

            int strStartRow = ((pageIndex - 1) * pageSize + 1).Value;
            int strEndRow = (pageIndex * pageSize).Value;

            if (!string.IsNullOrEmpty(filter))
            {
                strFilter = " WHERE " + filter + " ";
            }

            if (!string.IsNullOrEmpty(group))
            {
                strGroup = " GROUP BY " + group + " ";
            }

            string strSort;
            if (!string.IsNullOrEmpty(sort))
            {
                strSort = " ORDER BY " + sort;
            }
            else
            {
                strSort = " ORDER BY (SELECT 0)";
            }

            cmdText.AppendLine(unionText);

            cmdText.AppendFormat("SELECT * FROM ( SELECT ROW_NUMBER() OVER({0}) AS ROWNUM, {1} FROM {2} {3} {4}) AS QUERY_T1 WHERE ROWNUM >= {5}strStartRow AND ROWNUM <= {5}strEndRow ORDER BY ROWNUM;",
                 strSort, fldName, tableName, strFilter, strGroup, dataSource.DbFactory.DbProvider.ParameterPrefix);

            cmdText.AppendLine(unionText);
            if (string.IsNullOrEmpty(strGroup))
            {
                cmdText.AppendFormat("SELECT COUNT(0) AS TotalRecords FROM {0}{1}", tableName, strFilter);
            }
            else
            {
                cmdText.AppendFormat("SELECT COUNT(0) AS TotalRecords FROM (SELECT COUNT(0) AS C0 FROM {0} {1} {2}) T1", tableName, strFilter, strGroup);
            }

            var paras = new List<DbParameter>
            {
                dataSource.CreateParameter(dataSource.DbFactory.DbProvider.ParameterPrefix + "Pagesize", pageSize),
                dataSource.CreateParameter(dataSource.DbFactory.DbProvider.ParameterPrefix + "strStartRow", strStartRow),
                dataSource.CreateParameter(dataSource.DbFactory.DbProvider.ParameterPrefix + "strEndRow", strEndRow)
            };

            if (paramers?.Count() > 0)
            {
                paras.AddRange(paramers);
            }

            return (cmdText, paras.ToArray());
        }
    }
}
