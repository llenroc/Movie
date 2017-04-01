using System.Reflection;
using Infrastructure.Hangfire.Configuration;
using Infrastructure.Modules;
using Hangfire;

namespace Infrastructure.Hangfire
{
    [DependsOn(typeof(KernelModule))]
    public class HangfireModule : InfrastructureModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IHangfireConfiguration, HangfireConfiguration>();

            Configuration.Modules
                .Hangfire()
                .GlobalConfiguration
                .UseActivator(new HangfireIocJobActivator(IocManager));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
