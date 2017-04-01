using Infrastructure.WebApi.Configuration;

namespace Infrastructure.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Infrastructure.Web.Api module.
    /// </summary>
    public static class WebApiConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Infrastructure.Web.Api module.
        /// </summary>
        public static IInfrastructureWebApiConfiguration WebApi(this IModuleConfigurations configurations)
        {
            return configurations.Configuration.Get<IInfrastructureWebApiConfiguration>();
        }
    }
}
