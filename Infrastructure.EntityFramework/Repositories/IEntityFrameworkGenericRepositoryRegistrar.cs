using System;
using Infrastructure.Dependency;

namespace Infrastructure.EntityFramework.Repositories
{
    public interface IEntityFrameworkGenericRepositoryRegistrar
    {
        void RegisterForDbContext(Type dbContextType, IIocManager iocManager);
    }
}