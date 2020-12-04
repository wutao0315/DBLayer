using DBLayer.Core.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBLayer.Persistence
{
    public class DbContext:IDbContext
    {
        public DbContext(IUnitOfWork uow,
            IDbProvider dbProvider,
            IDbFactory dbFactory,
            IDataSource dataSource,
            IConnectionString connectionString,
            IGenerator generator,
            IPagerGenerator pagerGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            this.Uow = uow;
            this.DbProvider = dbProvider;
            this.ConnectionString = connectionString;
            this.Generator = generator;
            this.PagerGenerator = pagerGenerator;
            this.DbFactory = dbFactory;
            this.DataSource = dataSource;
            this.HttpContextAccesser = httpContextAccessor;
        }
        public IUnitOfWork Uow { get; private set; }
        public IDbProvider DbProvider { get; private set; }

        public IConnectionString ConnectionString { get; private set; }

        public IGenerator Generator { get; private set; }

        public IPagerGenerator PagerGenerator { get; private set; }

        public IDbFactory DbFactory { get; private set; }

        public IDataSource DataSource { get; private set; }

        public IHttpContextAccessor HttpContextAccesser { get; private set; }
    }
}
