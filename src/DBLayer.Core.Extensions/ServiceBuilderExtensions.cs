using DBLayer.Core.Interface;
using DBLayer.Persistence;
using DBLayer.Persistence.Data;
using DBLayer.Persistence.Generator;
using DBLayer.Persistence.PagerGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace DBLayer.Core.Extensions;

public class DBLayerOption
{
    public DbProviderOption DbProvider { get; set; }
    public ConnectionStringOption ConnectionString { get; set; }
    public SnowflakeOption Snowflake { get; set; }
}
public class DbProviderOption
{
    public string ProviderName { get; set; }
    public string ParameterPrefix { get; set; }
    public string SelectKey { get; set; }
    public string FieldFormat { get; set; }
}
public class ConnectionStringOption
{
    public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    public string ConnectionToken { get; set; }
}

public class SnowflakeOption
{
    public int WorkerId { get; set; }
    public int DataCenterId { get; set; }
    public DateTime StartDt { get; set; }
}

public static class ServiceBuilderExtensions
{
    public static void AddDBLayer(this IServiceCollection services, Func<IServiceProvider, IDbProvider> map)
    {
        services.AddSingleton((serviceProvider) => map(serviceProvider));

        services.AddSingleton<IConnectionString>((serviceProvider) =>
        {
            var option = serviceProvider.GetService<IOptions<DBLayerOption>>().Value;

            var provider = new ConnectionString(
                properties: option.ConnectionString.Properties,
                connectionToken: option.ConnectionString.ConnectionToken
            );
            return provider;
        });

        services.AddScoped<IDbFactory, DbFactory>();
        services.AddScoped<IDataSource, DataSource>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbContext, DbContext>();


    }
    public static void AddDBLayer(this IServiceCollection services)
    {
        services.AddDBLayer((serviceProvider) =>
        {
            var option = serviceProvider.GetService<IOptions<DBLayerOption>>().Value;

            var provider = new DbProvider(option.DbProvider.ProviderName,
                option.DbProvider.ParameterPrefix,
                option.DbProvider.SelectKey,
                option.DbProvider.FieldFormat);

            return provider;
        });
    }

    public static void AddRepositoyProxy(this IServiceCollection services, params string[] prefix)
    {
        services.TryAddSingleton<IRepositoyProxyBuilder, RepositoryProxyBuilder>();

        var assemblys = GetPlatform(prefix);

        var types = new List<Type>();
        foreach (var assembly in assemblys)
        {
            types.AddRange(assembly.GetExportedTypes());
        }

        var repositoryGenericType = typeof(IRepository<,>);
        var gncTypes = repositoryGenericType.GetGenericTypeDefinition();

        var repositoryTypes = types.Where(it => it.IsInterface && it.GetInterfaces().Any(x => gncTypes == (x.IsGenericType ? x.GetGenericTypeDefinition() : x))).ToList();

        foreach (var type in repositoryTypes)
        {
            services.AddRepositoryService(type, ServiceLifetime.Scoped);
        }
    }
    private static List<Assembly> GetPlatform(params string[] prefix)
    {
        var platform = Environment.OSVersion.Platform.ToString();
        var runtimeAssemblyNames = DependencyContext.Default.GetRuntimeAssemblyNames(platform);
        var assemblyNames = new List<AssemblyName>();
        foreach (var item in prefix)
        {
            assemblyNames.AddRange(runtimeAssemblyNames.Where(w => w.FullName.StartsWith(item)));
        }

        var assemblys = (from name in assemblyNames
                         select Assembly.Load(name)).ToList();
        return assemblys;
    }
    private static IServiceCollection AddRepositoryService(this IServiceCollection services,
        Type serviceType,
       ServiceLifetime lifetime)
    {
        if (!serviceType.IsInterface) throw new ArgumentException(nameof(serviceType));
        var serviceDescriptor = new ServiceDescriptor(serviceType, Factory, lifetime);
        services.Add(serviceDescriptor);

        return services;


        object Factory(IServiceProvider provider)
        {
            var repositoyProxyBuilder = provider.GetService<IRepositoyProxyBuilder>();
            var dbContext = provider.GetService<IDbContext>();
            var proxy = repositoyProxyBuilder.Build(serviceType, dbContext);
            return proxy;
        };
    }
    public static void AddSnowflake(this IServiceCollection services)
    {
        services.AddSingleton((serviceProvider) =>
        {
            var option = serviceProvider.GetService<IOptions<DBLayerOption>>().Value;
            var provider = new SnowflakeGenerator(
                datacenterId: option?.Snowflake?.DataCenterId ?? 1,
                workerId: option?.Snowflake?.WorkerId ?? 1,
                startDt: option?.Snowflake?.StartDt ?? new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            );
            return provider;
        });

        services.AddSingleton<IGenerator>((serviceProvider) =>
        {
            var generator = serviceProvider.GetService<SnowflakeGenerator>();
            return generator;
        });

        services.AddSingleton<IDeGenerator>((serviceProvider) =>
        {
            var generator = serviceProvider.GetService<SnowflakeGenerator>();
            return generator;
        });
    }
    public static void AddGUID(this IServiceCollection services)
    {
        services.AddSingleton<IGenerator>((serviceProvider) =>
        {
            var provider = new GUIDGenerator();
            return provider;
        });
    }

    public static void AddDUID(this IServiceCollection services)
    {
        services.AddSingleton<IGenerator>((serviceProvider) =>
        {
            var provider = new DUIDGenerator();
            return provider;
        });
    }

    public static void AddMySqlPager(this IServiceCollection services)
    {
        services.AddSingleton<IPagerGenerator>(new MySqlPagerGenerator());
    }

    public static void AddOraclePager(this IServiceCollection services)
    {
        services.AddSingleton<IPagerGenerator>(new OraclePagerGenerator());
    }

    public static void AddSqlServerPager(this IServiceCollection services)
    {
        services.AddSingleton<IPagerGenerator>(new SqlServerPagerGenerator());
    }

    public static void AddDBLayerDefault(this IServiceCollection services, params string[] prefix)
    {
        services.AddSnowflake();
        services.AddSqlServerPager();
        services.AddDBLayer();
        services.AddRepositoyProxy(prefix);
    }

    public static void AddDBLayerDefault(this IServiceCollection services, Func<IServiceProvider, IDbProvider> map, params string[] prefix)
    {
        services.AddSnowflake();
        services.AddSqlServerPager();
        services.AddDBLayer(map);
        services.AddRepositoyProxy(prefix);
    }
}
