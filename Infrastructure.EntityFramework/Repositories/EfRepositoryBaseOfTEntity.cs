using System.Data.Entity;
using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Repositories;

namespace Infrastructure.EntityFramework.Repositories
{
    public class EfRepositoryBase<TDbContext, TEntity> : EfRepositoryBase<TDbContext, TEntity, int>, IRepository<TEntity>where TEntity : class, IEntity<int> where TDbContext : DbContext
    {
        public EfRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}