using System.Reflection;
using Infrastructure.Authorization.Users;
using Infrastructure.Modules;
using Infrastructure.CommonFrame;
using Infrastructure.Configuration.Startup;
using Infrastructure.Owin;

namespace Infrastructure.CommonFrame.Owin
{
    [DependsOn(typeof(CommonFrameCoreModule))]
    public class CommonFrameOwinModule : InfrastructureModule
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService<IUserTokenProviderAccessor, OwinUserTokenProviderAccessor>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
