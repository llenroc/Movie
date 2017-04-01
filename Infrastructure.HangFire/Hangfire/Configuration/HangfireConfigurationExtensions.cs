using Infrastructure.BackgroundJobs;
using Infrastructure.Configuration.Startup;
using System;

namespace Infrastructure.Hangfire.Configuration
{
    public static class HangfireConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Hangfire module.
        /// </summary>
        public static IHangfireConfiguration Hangfire(this IModuleConfigurations configurations)
        {
            return configurations.Configuration.Get<IHangfireConfiguration>();
        }

        /// <summary>
        /// Configures to use Hangfire for background job management.
        /// </summary>
        public static void UseHangfire(this IBackgroundJobConfiguration backgroundJobConfiguration, Action<IHangfireConfiguration> configureAction)
        {
            backgroundJobConfiguration.Configuration.ReplaceService<IBackgroundJobManager, HangfireBackgroundJobManager>();
            configureAction(backgroundJobConfiguration.Configuration.Modules.Hangfire());
        }
    }
}
