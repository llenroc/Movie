using Infrastructure.Dependency;
using Infrastructure.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityFramework.UnitOfWork
{
    public interface IEfTransactionStrategy
    {
        void InitOptions(UnitOfWorkOptions options);

        void Commit();

        DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver)
             where TDbContext : DbContext;

        void Dispose(IIocResolver iocResolver);
    }
}
