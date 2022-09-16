using System.Collections.Generic;
using System.Linq;

namespace DBLayer.Configuration;

/// <summary>
/// Provides explicitly-defined <see cref="IDBLayerSettings"/> implementation.
/// </summary>
public class DBLayerSettings : IDBLayerSettings
{
    private readonly IConnectionStringSettings _connectionStringSettings;

    public DBLayerSettings(
        string connectionName,
        string providerName,
        string connectionString)
    {
        _connectionStringSettings = new ConnectionStringSettings(connectionName, connectionString, providerName);
    }

    public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();
    public string DefaultConfiguration => _connectionStringSettings.Name;
    public string DefaultDataProvider => ProviderName.SqlServer;

    public IEnumerable<IConnectionStringSettings> ConnectionStrings
    {
        get
        {
            yield return _connectionStringSettings;
        }
    }
}

