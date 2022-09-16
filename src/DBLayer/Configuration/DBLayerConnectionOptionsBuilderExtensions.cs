using DBLayer.DataProvider.Access;
using DBLayer.DataProvider.ClickHouse;
using DBLayer.DataProvider.DB2;
using DBLayer.DataProvider.Oracle;
using DBLayer.DataProvider.PostgreSQL;
using DBLayer.DataProvider.SapHana;
using DBLayer.DataProvider.SqlServer;

namespace DBLayer.Configuration;

/// <summary>
/// Set of provider-specific extensions for <see cref="DBLayerConnectionOptionsBuilder"/>.
/// </summary>
public static class DBLayerConnectionOptionsBuilderExtensions
{
	#region UseSqlServer
	/// <summary>
	/// Configure connection to use SQL Server default provider, dialect and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SQL Server connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// Default provider configured using <see cref="SqlServerTools.Provider"/> option and set to <see cref="SqlServerProvider.SystemDataSqlClient"/> by default.
	/// </para>
	/// <para>
	/// SQL Server dialect will be choosen automatically:
	/// <list type="bullet">
	/// <item>if <see cref="SqlServerTools.AutoDetectProvider"/> (default: <c>true</c>) enabled, Linq To DB will query server for version</item>
	/// <item>otherwise <see cref="SqlServerVersion.v2008"/> will be used as default dialect.</item>
	/// </list>
	/// </para>
	/// For more fine-grained configuration see <see cref="UseSqlServer(DBLayerConnectionOptionsBuilder, string, SqlServerProvider, SqlServerVersion)"/> overload.
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UseSqlServer(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.SqlServer, connectionString);
	}

	/// <summary>
	/// Configure connection to use specific SQL Server provider, dialect and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SQL Server connection string.</param>
	/// <param name="provider">SQL Server provider to use.</param>
	/// <param name="dialect">SQL Server dialect support level.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseSqlServer(this DBLayerConnectionOptionsBuilder builder, string connectionString, SqlServerProvider provider, SqlServerVersion dialect)
	{
		return builder.UseConnectionString(SqlServerTools.GetDataProvider(dialect, provider), connectionString);
	}
	#endregion

	#region UseOracle
	/// <summary>
	/// Configure connection to use Oracle default provider, dialect and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">Oracle connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// By default Linq To DB tries to load managed version of Oracle provider.
	/// </para>
	/// <para>
	/// Oracle dialect will be choosen automatically:
	/// <list type="bullet">
	/// <item>if <see cref="OracleTools.AutoDetectProvider"/> (default: <c>true</c>) enabled, Linq To DB will query server for version</item>
	/// <item>otherwise <see cref="OracleTools.DefaultVersion"/> (default: <see cref="OracleVersion.v12"/>) will be used as default dialect.</item>
	/// </list>
	/// </para>
	/// For more fine-grained configuration see <see cref="UseOracle(DBLayerConnectionOptionsBuilder, string, OracleVersion, OracleProvider)"/> overload.
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UseOracle(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.Oracle, connectionString);
	}

	/// <summary>
	/// Configure connection to use specific Oracle provider, dialect and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">Oracle connection string.</param>
	/// <param name="dialect">Oracle dialect support level.</param>
	/// <param name="provider">ADO.NET provider to use.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseOracle(this DBLayerConnectionOptionsBuilder builder, string connectionString, OracleVersion dialect, OracleProvider provider)
	{
		return builder.UseConnectionString(
			OracleTools.GetDataProvider(dialect, provider),
			connectionString);
	}
	#endregion

	#region UsePostgreSQL
	/// <summary>
	/// Configure connection to use PostgreSQL Npgsql provider, default dialect and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">PostgreSQL connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// PostgreSQL dialect will be choosen automatically:
	/// <list type="bullet">
	/// <item>if <see cref="PostgreSQLTools.AutoDetectProvider"/> (default: <c>true</c>) enabled, Linq To DB will query server for version</item>
	/// <item>otherwise <see cref="PostgreSQLVersion.v92"/> will be used as default dialect.</item>
	/// </list>
	/// </para>
	/// For more fine-grained configuration see <see cref="UsePostgreSQL(DBLayerConnectionOptionsBuilder, string, PostgreSQLVersion)"/> overload.
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UsePostgreSQL(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.PostgreSQL, connectionString);
	}

	/// <summary>
	/// Configure connection to use PostgreSQL Npgsql provider, specific dialect and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">PostgreSQL connection string.</param>
	/// <param name="dialect">POstgreSQL dialect support level.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UsePostgreSQL(this DBLayerConnectionOptionsBuilder builder, string connectionString, PostgreSQLVersion dialect)
	{
		return builder.UseConnectionString(PostgreSQLTools.GetDataProvider(dialect), connectionString);
	}
	#endregion

	#region UseMySql
	/// <summary>
	/// Configure connection to use MySql default provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">MySql connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// Default provider will be choosen by probing current folder for provider assembly and if it is not found, default to <c>MySql.Data</c> provider.
	/// </para>
	/// For more fine-grained configuration see <see cref="UseMySqlData(DBLayerConnectionOptionsBuilder, string)"/> and <see cref="UseMySqlConnector(DBLayerConnectionOptionsBuilder, string)"/> methods.
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UseMySql(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.MySql, connectionString);
	}

	/// <summary>
	/// Configure connection to use <c>MySql.Data</c> MySql provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">MySql connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseMySqlData(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.MySqlOfficial, connectionString);
	}

	/// <summary>
	/// Configure connection to use <c>MySqlConnector</c> MySql provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">MySql connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseMySqlConnector(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.MySqlConnector, connectionString);
	}
	#endregion

	#region UseSQLite
	/// <summary>
	/// Configure connection to use SQLite default provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SQLite connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// Default provider will be choosen by probing current folder for provider assembly and if it is not found, default to <c>System.Data.Sqlite</c> provider.
	/// </para>
	/// For more fine-grained configuration see <see cref="UseSQLiteOfficial(DBLayerConnectionOptionsBuilder, string)"/> and <see cref="UseSQLiteMicrosoft(DBLayerConnectionOptionsBuilder, string)"/> methods.
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UseSQLite(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.SQLite, connectionString);
	}

	/// <summary>
	/// Configure connection to use <c>System.Data.Sqlite</c> SQLite provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SQLite connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseSQLiteOfficial(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.SQLiteClassic, connectionString);
	}

	/// <summary>
	/// Configure connection to use <c>Microsoft.Data.Sqlite</c> SQLite provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SQLite connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseSQLiteMicrosoft(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.SQLiteMS, connectionString);
	}
	#endregion

	#region UseAccess
	/// <summary>
	/// Configure connection to use Access default provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">Access connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// Default provider determined by inspecting connection string for OleDb or ODBC-specific markers and otherwise defaults to OleDb provider.
	/// </para>
	/// For more fine-grained configuration see <see cref="UseAccessOleDb(DBLayerConnectionOptionsBuilder, string)"/> and <see cref="UseAccessODBC(DBLayerConnectionOptionsBuilder, string)"/> methods.
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UseAccess(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.Access, connectionString);
	}

	/// <summary>
	/// Configure connection to use Access OleDb provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">Access connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseAccessOleDb(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(AccessTools.GetDataProvider(null), connectionString);
	}

	/// <summary>
	/// Configure connection to use Access ODBC provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">Access connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseAccessODBC(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.AccessOdbc, connectionString);
	}
	#endregion

	#region UseDB2
	/// <summary>
	/// Configure connection to use DB2 default provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">DB2 connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// DB2 provider will be choosen automatically:
	/// <list type="bullet">
	/// <item>if <see cref="DB2Tools.AutoDetectProvider"/> (default: <c>true</c>) enabled, Linq To DB will query server for version</item>
	/// <item>otherwise <c>DB2 LUW</c> provider will be choosen.</item>
	/// </list>
	/// </para>
	/// For more fine-grained configuration see <see cref="UseDB2(DBLayerConnectionOptionsBuilder, string, DB2Version)"/> overload.
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UseDB2(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.DB2, connectionString);
	}

	/// <summary>
	/// Configure connection to use specific DB2 provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">DB2 connection string.</param>
	/// <param name="version">DB2 server version.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseDB2(this DBLayerConnectionOptionsBuilder builder, string connectionString, DB2Version version)
	{
		return builder.UseConnectionString(DB2Tools.GetDataProvider(version), connectionString);
	}
	#endregion

	#region UseFirebird
	/// <summary>
	/// Configure connection to use Firebird provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">Firebird connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseFirebird(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.Firebird, connectionString);
	}
	#endregion

	#region UseInformix
	/// <summary>
	/// Configure connection to use Informix default provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">Informix connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// Default provider will be choosen by probing current folder for provider assembly and if it is not found, default to <c>IBM.Data.DB2</c> provider.
	/// This is not applicable to .NET Core applications as they always use <c>IBM.Data.DB2</c> provider.
	/// </para>
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UseInformix(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.Informix, connectionString);
	}

	#endregion

	#region UseSapHana
	/// <summary>
	/// Configure connection to use SAP HANA default provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SAP HANA connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// Default provider will be <c>Sap.Data.Hana</c> native provider for .NET Framework and .NET Core applications and ODBC provider for .NET STANDARD builds.
	/// </para>
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UseSapHana(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.SapHana, connectionString);
	}

	/// <summary>
	/// Configure connection to use native SAP HANA provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SAP HANA connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseSapHanaNative(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(
			SapHanaTools.GetDataProvider(ProviderName.SapHanaNative),
			connectionString);
	}

	/// <summary>
	/// Configure connection to use SAP HANA ODBC provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SAP HANA connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseSapHanaODBC(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(
			SapHanaTools.GetDataProvider(ProviderName.SapHanaOdbc),
			connectionString);
	}
	#endregion

	#region UseSqlCe
	/// <summary>
	/// Configure connection to use SQL CE provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SQL CE connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseSqlCe(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.SqlCe, connectionString);
	}
	#endregion

	#region UseAse
	/// <summary>
	/// Configure connection to use SAP/Sybase ASE default provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="connectionString">SAP/Sybase ASE connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	/// <remarks>
	/// <para>
	/// Provider selection available only for .NET Framework applications.
	/// </para>
	/// <para>
	/// Default provider will be choosen by probing current folder for provider assembly and if it is not found, default to official <c>Sybase.AdoNet45.AseClient</c> provider.
	/// </para>
	/// </remarks>
	public static DBLayerConnectionOptionsBuilder UseAse(this DBLayerConnectionOptionsBuilder builder, string connectionString)
	{
		return builder.UseConnectionString(ProviderName.Sybase, connectionString);
	}

	#endregion

	#region UseClickHouse
	/// <summary>
	/// Configure connection to use UseClickHouse provider and connection string.
	/// </summary>
	/// <param name="builder">Instance of <see cref="DBLayerConnectionOptionsBuilder"/>.</param>
	/// <param name="provider">ClickHouse provider.</param>
	/// <param name="connectionString">ClickHouse connection string.</param>
	/// <returns>The builder instance so calls can be chained.</returns>
	public static DBLayerConnectionOptionsBuilder UseClickHouse(this DBLayerConnectionOptionsBuilder builder, ClickHouseProvider provider, string connectionString)
	{
		return builder.UseConnectionString(ClickHouseTools.GetDataProvider(provider), connectionString);
	}
	#endregion
}
