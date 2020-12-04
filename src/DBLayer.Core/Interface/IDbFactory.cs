using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DBLayer.Core.Interface
{
    public interface IDbFactory : IDisposable
    {
        /// <summary>
        /// 长链接
        /// </summary>
        DbConnection LongDbConnection { get; }

        /// <summary>
        /// 长链接的事物
        /// </summary>
        DbTransaction LongDbTransaction { get; }

        /// <summary>
        /// 短链接
        /// </summary>
        DbConnection ShortDbConnection { get; }
        /// <summary>
        /// 数据库提供工厂
        /// </summary>
        DbProviderFactory DbProviderFactory { get; }
        /// <summary>
        /// 数据库提供者
        /// </summary>
        IDbProvider DbProvider { get; }

        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();
    }
}
