﻿using System.Data.Entity;
using Infrastructure.MultiTenancy;

namespace Infrastructure.EntityFramework
{
    public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : DbContext
    {
        public TDbContext DbContext { get; }

        public SimpleDbContextProvider(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TDbContext GetDbContext()
        {
            return DbContext;
        }

        public TDbContext GetDbContext(MultiTenancySides? multiTenancySide)
        {
            return DbContext;
        }
    }
}
