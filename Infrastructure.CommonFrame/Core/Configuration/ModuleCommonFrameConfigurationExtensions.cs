using Infrastructure.Configuration.Startup;

namespace Infrastructure.CommonFrame.Configuration
{
    /// <summary>
    /// Extension methods for module zero configurations.
    /// </summary>
    public static class ModuleCommonFrameConfigurationExtensions
    {
        /// <summary>
        /// Used to configure module zero.
        /// </summary>
        /// <param name="moduleConfigurations"></param>
        /// <returns></returns>
        public static ICommonFrameConfig CommonFrame(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.Configuration.Get<ICommonFrameConfig>();
        }
    }
}