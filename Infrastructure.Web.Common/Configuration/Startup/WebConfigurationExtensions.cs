using Infrastructure.Web.Configuration;

namespace Infrastructure.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure  Web module.
    /// </summary>
    public static class WebConfigurationExtensions
    {
        /// <summary>
        /// Used to configure  Web Common module.
        /// </summary>
        public static IWebCommonModuleConfiguration WebCommon(this IModuleConfigurations configurations)
        {
            return configurations.Configuration.Get<IWebCommonModuleConfiguration>();
        }
    }
}
