using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBLayer.Core.Interface
{
    public interface IDbContext
    {
        IUnitOfWork Uow { get; }
        IDbProvider DbProvider { get; }
        IDbFactory DbFactory { get; }
        IDataSource DataSource { get; }
        IConnectionString ConnectionString { get; }
        IGenerator Generator { get; }
        IPagerGenerator PagerGenerator { get; }
        IHttpContextAccessor HttpContextAccesser { get; }
    }
}
