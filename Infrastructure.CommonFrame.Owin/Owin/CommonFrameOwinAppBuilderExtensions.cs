using Infrastructure.Authorization.Users;
using Infrastructure.Dependency;
using Infrastructure.Extensions;
using Microsoft.Owin.Security.DataProtection;
using Owin;

namespace Infrastructure.Owin
{
    public static class CommonFrameOwinAppBuilderExtensions
    {
        public static void RegisterDataProtectionProvider(this IAppBuilder app)
        {
            if (!IocManager.Instance.IsRegistered<IUserTokenProviderAccessor>())
            {
                throw new InfrastructureException("IUserTokenProviderAccessor is not registered!");
            }
            var providerAccessor = IocManager.Instance.Resolve<IUserTokenProviderAccessor>();

            if (!(providerAccessor is OwinUserTokenProviderAccessor))
            {
                throw new InfrastructureException($"IUserTokenProviderAccessor should be instance of {nameof(OwinUserTokenProviderAccessor)}!");
            }
            providerAccessor.As<OwinUserTokenProviderAccessor>().DataProtectionProvider = app.GetDataProtectionProvider();
        }
    }
}
