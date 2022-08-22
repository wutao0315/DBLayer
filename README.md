


#### DBLayer is a orm db access project.

* it's light weight easy to use.
* automatic generate distribute ID
* pager code easy to use
* support sqlserver、oracle、mysql
* generate draft sql for debug and coding

### install

nuget:Install-Package DBLayer -Version

nuget:Install-Package DBLayer.Persistence -Version

nuget:Install-Package DBLayer.Extensions -Version

## ioc code
```C#
collection.AddDBLayer(new DBLayerOptions
{
ConnectionString = new ConnectionString
(
    properties : new NameValueCollection
    {
        { "userid","sa"},
        { "password","***"},
        { "passwordKey",""},
        { "database","***"},
        { "datasource","127.0.0.1"}
    },
    connectionToken : "Password=${password};Persist Security Info=True;User ID=${userid};Initial Catalog=${database};Data Source=${datasource};pooling=true;min pool size=5;max pool size=10"
),
DbProvider = new DbProvider
{
    ProviderName = "System.Data.SqlClient.SqlClientFactory, System.Data.SqlClient",
    //ProviderName = "MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data",
    //ProviderName = "Oracle.ManagedDataAccess.Client.OracleClientFactory, Oracle.ManagedDataAccess.Core",
    //ParameterPrefix = "@",
    //ParameterPrefix = ":",
    ParameterPrefix = "@",
    SelectKey = "SELECT @@IDENTITY;"
},
Generator = new GUIDGenerator(),
PageGenerator = new SqlServerPagerGenerator()
});
```
## service code
```C#
//add a log data to db
var id = TheService.InsertEntity<SysLog, long>(
        () => new SysLog()
        {
            LogId = -1,
            LogContentJson = "test",
            LogCreater = "test",
            LogCreateTime = DateTime.Now,
            LogType = "1"
        });

//paged search engine

/// <summary>
/// paged search
/// </summary>
/// <param name="condition">the search condition</param>
/// <returns></returns>
public IEnumerable<SysUser> Seach(SysUserCondition.Search condition)
{
    var page = new Pager<SysUserCondition.Search>()
    {
        Condition = condition,
        Table = "sys_user",
        Order = string.Empty,
        Field = "*",
        WhereAction = (Where, Paramters) =>
        {
            if (!string.IsNullOrEmpty(condition.UserName))
            {
                Where.Append("AND user_name LIKE @user_name ");
                Paramters.Add(base.CreateParameter("@user_name", string.Concat("%", condition.UserName, "%")));
            }
            if (!string.IsNullOrEmpty(condition.UserEmail))
            {
                Where.Append("AND user_email LIKE @user_email ");
                Paramters.Add(base.CreateParameter("@user_email", string.Concat("%", condition.UserEmail, "%")));
            }
            if (!string.IsNullOrEmpty(condition.UserMobile))
            {
                Where.Append("AND user_mobile LIKE @user_mobile ");
                Paramters.Add(base.CreateParameter("@user_mobile", string.Concat("%", condition.UserMobile, "%")));
            }
        }
    };

    var result = base.GetResultByPager<SysUser, SysUserCondition.Search>(page);
    return result;
}
```

## plan 
* simplize all code, remove no using code

## cake cmd
```
Set-ExecutionPolicy Unrestricted
./build.ps1
```
