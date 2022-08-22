using DBLayer.Interface;
using System.Data.Common;

namespace DBLayer.Persistence.Data;

/// <summary>
/// Commmon DbProvider
/// </summary>
public class DbProvider : IDbProvider
{
    /// <summary>
    /// ProviderName Type
    /// </summary>
    public string ProviderName { get; }
    /// <summary>
    /// Parameter prefix use in store procedure.
    /// </summary>
    /// <example> @ for Sql Server.</example>
    public string ParameterPrefix { get; }
    /// <summary>
    /// SelectKey
    /// </summary>
    public string SelectKey { get; }
    /// <summary>
    /// field format {0}
    /// </summary>
    public string FieldFormat { get; } = "{0}";

    public DbProvider(string providerName, string parameterPrefix, string selectKey, string fieldFormat)
    {
        ProviderName = providerName;
        ParameterPrefix = parameterPrefix;
        SelectKey = selectKey;
        FieldFormat = fieldFormat;
    }
    #region 接口

    public DbProviderFactory GetDbProviderFactory()
    {
        var factoryType = Type.GetType(this.ProviderName);
        var dbProviderFactory = factoryType.GetField("Instance").GetValue(null) as DbProviderFactory;
        return dbProviderFactory;
    }
    #endregion
}
