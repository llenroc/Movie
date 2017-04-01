using Castle.MicroKernel.Registration;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Modules;
using Infrastructure.MultiTenancy;
using System.Reflection;

namespace Infrastructure.CommonFrame.EntityFramework
{
    /// <summary>
    /// Entity framework integration module for ASP.NET Boilerplate Zero.
    /// </summary>
    [DependsOn(
        typeof(CommonFrameCoreModule), 
        typeof(Infrastructure.EntityFramework.EntityFrameworkModule)
        )]
    public class CommonFrameEntityFrameworkModule : InfrastructureModule
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService(typeof(IConnectionStringResolver), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IConnectionStringResolver, IDbPerTenantConnectionStringResolver>()
                        .ImplementedBy<DbPerTenantConnectionStringResolver>()
                        .LifestyleTransient()
                    );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
