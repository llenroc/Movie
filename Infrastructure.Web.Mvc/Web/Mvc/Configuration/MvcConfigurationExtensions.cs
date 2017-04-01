using Infrastructure.Configuration.Startup;

namespace Infrastructure.Web.Mvc.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure .Web.Api module.
    /// </summary>
    public static class MvcConfigurationExtensions
    {
        /// <summary>
        /// Used to configure .Web.Api module.
        /// </summary>
        public static IMvcConfiguration Mvc(this IModuleConfigurations configurations)
        {
            return configurations.Configuration.Get<IMvcConfiguration>();
        }
    }
}
