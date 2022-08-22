using System.Security.Claims;

namespace DBLayer.Interface;

public interface IDbContext
{
    IUnitOfWork Uow { get; }
    IDbProvider DbProvider { get; }
    IDbFactory DbFactory { get; }
    IDataSource DataSource { get; }
    IConnectionString ConnectionString { get; }
    IGenerator Generator { get; }
    IPagerGenerator PagerGenerator { get; }

    ClaimsPrincipal? User { get; }
}
