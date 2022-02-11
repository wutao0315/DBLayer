using System.Data.Common;

namespace DBLayer.Core.Interface;

public interface IDbProvider
{
    /// <summary>
    /// Provider Type Name
    /// </summary>
    string ProviderName { get; }
    /// <summary>
    /// Parameter prefix use in store procedure.
    /// </summary>
    /// <example> @ for Sql Server.</example>
    string ParameterPrefix { get; }
    /// <summary>
    /// SelectKey
    /// </summary>
    string SelectKey { get; }
    /// <summary>
    /// field format {0}
    /// </summary>
    string FieldFormat { get; }
    /// <summary>
    /// Get DbProvider Factory
    /// </summary>
    /// <returns></returns>
    DbProviderFactory GetDbProviderFactory();
}
