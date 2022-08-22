using DBLayer;
using DBLayer.Interface;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DBLayer.Persistence.PagerGenerator;

public class OraclePagerGenerator : IPagerGenerator
{
    /// <summary>
    /// oracle数据库参数 属性名称
    /// OracleType
    /// </summary>
    public string CursorName { get; set; }
    /// <summary>
    /// oracle数据库参数 属性值
    /// T(System.Data.OracleClient.OracleType, System.Data.OracleClient).Cursor
    /// </summary>
    public object CursorValue { get; set; }

    /// <summary>
    /// 查询所有数据-不包含字段*
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataSource"></param>
    /// <param name="paramerList"></param>
    /// <param name="top"></param>
    /// <returns></returns>
    public StringBuilder GetSelectCmdText<T>(IDataSource dataSource, ref List<DbParameter> paramerList, StringBuilder whereStr, StringBuilder orderStr, int? top = null, params string[] inclusionList)
    {
        var cmdText = new StringBuilder();


        var entityType = typeof(T);
        var tableName = entityType.GetDataTableName();
        tableName = string.Format(dataSource.DbFactory.DbProvider.FieldFormat, tableName);
        var fields = dataSource.CreateAllEntityDicSql<T>(inclusionList);

        if (top != null && top.Value > 0)
        {
            var topparameter = dataSource.DbFactory.DbProvider.ParameterPrefix + "topParameter";
            paramerList.Add(dataSource.CreateParameter(topparameter, top.Value));
            cmdText.AppendFormat("select * from (select r.*,rownum rowsn from(SELECT {1} FROM {0} {2} {3})r) where rowsn<={4} ", tableName, fields, whereStr, orderStr, topparameter);
        }
        else
        {
            cmdText.AppendFormat("SELECT {1} FROM {0} {2} {3} ", tableName, fields, whereStr, orderStr);
        }

        return cmdText;
    }
    /// <summary>
    /// 查询所有数据-包含字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataSource"></param>
    /// <param name="paramerList"></param>
    /// <param name="top"></param>
    /// <param name="inclusionList"></param>
    /// <returns></returns>
    public StringBuilder GetSelectDictionaryCmdText<T>(IDataSource dataSource, ref List<DbParameter> paramerList, StringBuilder whereStr, StringBuilder orderStr, int? top = null, params string[] inclusionList)
    {
        var cmdText = new StringBuilder();
        var entityType = typeof(T);

        var tableName = entityType.GetDataTableName();
        tableName = string.Format(dataSource.DbFactory.DbProvider.FieldFormat, tableName);

        var fields = dataSource.CreateAllEntityDicSql<T>(inclusionList);

        if (top != null && top.Value > 0)
        {
            var topparameter = dataSource.DbFactory.DbProvider.ParameterPrefix + "topParameter";
            paramerList.Add(dataSource.CreateParameter(topparameter, top.Value));
            cmdText.AppendFormat("SELECT * FROM (select r.*,rownum rowsn from(SELECT {1} FROM {0} {2} {3})r) where rowsn<={4} ", tableName, fields, whereStr, orderStr, topparameter);
        }
        else
        {
            cmdText.AppendFormat("SELECT {1} FROM {0} {2} {3} ", tableName, fields, whereStr, orderStr);
        }


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
        var entityType = typeof(T);
        var (dataTable, _) = entityType.GetDataTableAttribute();

        sbField.Append(fieldName.ToUpper());
        sbField.Append(',');
        sbValue.AppendFormat("{0}.NEXTVAL", dataTable.SequenceName);
        sbValue.Append(',');
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

        var entityType = typeof(T);
        var (dataTable, _) = entityType.GetDataTableAttribute();

        cmdText.AppendLine("BEGIN ");
        cmdText.AppendFormat("{0}; ", insertCmd);
        cmdText.AppendFormat("SELECT {0}.CURRVAL INTO :current_id FROM DUAL; ", dataTable.SequenceName);
        cmdText.Append("END;");

        paramerList.Add(dataSource.CreateOutPutParameter(":current_id", DbType.Int64));

        var paras = paramerList.ToArray();

        dataSource.ExecuteNonQuery(cmdText.ToString(), connection, CommandType.Text, paras);

        object newID = paras[paras.Length - 1].Value;
        return newID;
    }

    /// <summary>
    /// 执行Insert
    /// </summary>
    /// <param name="dataSource">数据源</param>
    /// <param name="insertCmd">执行命令</param>
    /// <param name="paramerList">参数列表</param>
    /// <param name="connection">会话</param>
    /// <returns>Id</returns>
    public async Task<object> InsertExecutorAsync<T>(IDataSource dataSource, StringBuilder insertCmd, List<DbParameter> paramerList, DbConnection connection)
    {
        var cmdText = new StringBuilder();

        var entityType = typeof(T);
        var (dataTable, _) = entityType.GetDataTableAttribute();

        cmdText.AppendLine("BEGIN ");
        cmdText.AppendFormat("{0}; \t\n", insertCmd);
        cmdText.AppendFormat("OPEN :p_cursor_1 FOR SELECT {0}.CURRVAL  INTO :current_id FROM DUAL; \t\n", dataTable.SequenceName);
        cmdText.Append("END;");

        paramerList.Add(dataSource.CreateOutPutParameter("p_cursor_1", CursorName, CursorValue));
        paramerList.Add(dataSource.CreateOutPutParameter(":current_id"));

        var paras = paramerList.ToArray();

        var retval = await dataSource.ExecuteNonQueryAsync(cmdText.ToString(), connection, CommandType.Text, paras);

        object newID = null;
        if (retval > 0)
        {
            newID = paras[^1].Value;
        }
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
        result.AppendFormat("{0} IN (SELECT TRIM(regexp_substr(str,'[^,]+',1,LEVEL)) strRows FROM (SELECT {1} AS str FROM DUAL ) t CONNECT BY INSTR(str,',',1,LEVEL-1)>0)", leftString, rightString);
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
        result.AppendFormat("{0} NOT IN (SELECT TRIM(regexp_substr(str,'[^,]+',1,LEVEL)) strRows FROM (SELECT {1} AS str FROM DUAL) t CONNECT BY INSTR(str,',',1,LEVEL-1)>0)", leftString, rightString);
        return result;
    }
    /// <summary>
    /// 生成分页sql
    /// </summary>
    /// <param name="unionText"></param>
    /// <param name="tableName"></param>
    /// <param name="fldName"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="filter"></param>
    /// <param name="group"></param>
    /// <param name="sort"></param>
    /// <param name="parameter"></param>
    /// <param name="paramers"></param>
    /// <returns></returns>
    public (StringBuilder, DbParameter[]) GetPageCmdText(IDataSource dataSource, string unionText, string tableName, string fldName, ref int? pageIndex, ref int? pageSize, string filter, string group, string sort, params DbParameter[] paramers)
    {
        var cmdText = new StringBuilder();
        var strFilter = "";
        var strGroup = "";
        var strSort = "";
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
        if (!string.IsNullOrEmpty(sort))
        {
            strGroup = " ORDER BY " + sort + " ";
        }
        cmdText.Append("BEGIN ");
        cmdText.AppendLine(unionText);
        cmdText.AppendFormat("OPEN :p_cursor_1 FOR SELECT * FROM (SELECT * FROM (SELECT A.*,ROWNUM RN FROM (SELECT {1} FROM {0} {2} {3} {4}) A) WHERE ROWNUM <= {5}strEndRow) WHERE RN> = {5}strStartRow; ",
            tableName, fldName, strFilter, strGroup, strSort, dataSource.DbFactory.DbProvider.ParameterPrefix);

        cmdText.AppendLine(unionText);
        if (string.IsNullOrEmpty(strGroup))
        {
            cmdText.AppendFormat("OPEN :p_cursor_2 FOR SELECT COUNT(0) AS TotalRecords FROM {0}{1}; \t\n", tableName, strFilter);
        }
        else
        {
            cmdText.AppendFormat("OPEN :p_cursor_2 FOR SELECT COUNT(0) AS TotalRecords FROM (SELECT COUNT(0) AS C0 FROM {0} {1} {2}) T1;\t\n", tableName, strFilter, strGroup);
        }
        cmdText.Append("END;");
        var paras = new List<DbParameter>
            {
                dataSource.CreateParameter(dataSource.DbFactory.DbProvider.ParameterPrefix + "strStartRow", strStartRow),
                dataSource.CreateParameter(dataSource.DbFactory.DbProvider.ParameterPrefix + "strEndRow", strEndRow),

                dataSource.CreateOutPutParameter("p_cursor_1", CursorName, CursorValue),
                dataSource.CreateOutPutParameter("p_cursor_2", CursorName, CursorValue)
            };

        if (paramers?.Count() > 0)
        {
            paras.AddRange(paramers);
        }


        return (cmdText, paras.ToArray());
    }


}
