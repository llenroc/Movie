using Infrastructure.Configuration.Startup;

namespace Infrastructure.AutoMapper
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Abp.AutoMapper module.
    /// </summary>
    public static class AutoMapperConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Abp.AutoMapper module.
        /// </summary>
        public static IAutoMapperConfiguration AutoMapper(this IModuleConfigurations configurations)
        {
            return configurations.Configuration.Get<IAutoMapperConfiguration>();
        }
    }
}
