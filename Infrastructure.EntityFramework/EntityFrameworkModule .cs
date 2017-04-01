using System.Reflection;
using Infrastructure.Collections.Extensions;
using Infrastructure.Configuration.Startup;
using Infrastructure.Dependency;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Modules;
using Infrastructure.Reflection;
using Castle.MicroKernel.Registration;
using System;

namespace Infrastructure.EntityFramework
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in EntityFramework.
    /// </summary>
    [DependsOn(typeof(KernelModule))]
    public class EntityFrameworkModule : InfrastructureModule
    {
        private readonly ITypeFinder _typeFinder;

        public EntityFrameworkModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        public override void PreInitialize()
        {
            Configuration.ReplaceService<IUnitOfWorkFilterExecuter>(() =>
            {
                IocManager.IocContainer.Register(Component.For<IUnitOfWorkFilterExecuter, IEfUnitOfWorkFilterExecuter>().ImplementedBy<EfDynamicFiltersUnitOfWorkFilterExecuter>().LifestyleTransient());
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(Component.For(typeof(IDbContextProvider<>)).ImplementedBy(typeof(UnitOfWorkDbContextProvider<>)).LifestyleTransient());
            RegisterGenericRepositoriesAndMatchDbContexes();
        }

        private void RegisterGenericRepositoriesAndMatchDbContexes()
        {
            var dbContextTypes =_typeFinder.Find(type =>
            type.IsPublic &&!type.IsAbstract &&type.IsClass && typeof(InfrastructureDbContext).IsAssignableFrom(type));

            if (dbContextTypes.IsNullOrEmpty())
            {
                Logger.Warn("No class found derived from InfrastructureDbContext.");
                return;
            }

            using (var repositoryRegistrar = IocManager.ResolveAsDisposable<IEntityFrameworkGenericRepositoryRegistrar>())
            {
                foreach (var dbContextType in dbContextTypes)
                {
                    Logger.Debug("Registering DbContext: " + dbContextType.AssemblyQualifiedName);
                    repositoryRegistrar.Object.RegisterForDbContext(dbContextType, IocManager);
                }
            }

            using (var dbContextMatcher = IocManager.ResolveAsDisposable<IDbContextTypeMatcher>())
            {
                dbContextMatcher.Object.Populate(dbContextTypes);
            }
        }
    }
}
