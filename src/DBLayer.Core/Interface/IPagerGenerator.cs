using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Core.Interface
{
    public interface IPagerGenerator
    {
        /// <summary>
        /// 查询所有数据-不包含字段*
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="paramerList"></param>
        /// <param name="whereStr"></param>
        /// <param name="orderStr"></param>
        /// <param name="top"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        StringBuilder GetSelectCmdText<T>(IDataSource dataSource, ref List<DbParameter> paramerList, StringBuilder whereStr, StringBuilder orderStr, int? top = null, params string[] inclusionList);
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
        StringBuilder GetSelectDictionaryCmdText<T>(IDataSource dataSource, ref List<DbParameter> paramerList, StringBuilder whereStr, StringBuilder orderStr , int? top = null, params string[] inclusionList);
         /// <summary>
        /// 在insert中id自动编号的处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="sbField"></param>
        /// <param name="sbValue"></param>
        void ProcessInsertId<T>(string fieldName, ref StringBuilder sbField, ref StringBuilder sbValue);
        /// <summary>
        /// 执行Insert
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="insertCmd">执行命令</param>
        /// <param name="paramerList">参数列表</param>
        /// <param name="trans">会话</param>
        /// <returns>Id</returns>
        object InsertExecutor<T>(IDataSource dataSource, StringBuilder insertCmd, List<DbParameter> paramerList, DbConnection connection);
        /// <summary>
        /// 执行Insert
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="insertCmd">执行命令</param>
        /// <param name="paramerList">参数列表</param>
        /// <param name="trans">会话</param>
        /// <returns>Id</returns>
        Task<object> InsertExecutorAsync<T>(IDataSource dataSource, StringBuilder insertCmd, List<DbParameter> paramerList, DbConnection connection);
        /// <summary>
        /// 查询条件 InFunc
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        StringBuilder GetInFunc(Func<StringBuilder> left, Func<StringBuilder> right);
        /// <summary>
        /// 查询条件 NotInFunc
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        StringBuilder GetNotInFunc(Func<StringBuilder> left, Func<StringBuilder> right);

        /// <summary>
        /// 生成分页sql
        /// </summary>
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
        /// <returns></returns>
        (StringBuilder,DbParameter[]) GetPageCmdText(IDataSource dataSource, string unionText, string tableName, string fldName, ref int? pageIndex, ref int? pageSize, string filter, string group, string sort, params DbParameter[] paramers);
    }
}
