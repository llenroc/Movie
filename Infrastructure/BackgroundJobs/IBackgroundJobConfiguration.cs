using Infrastructure.Configuration.Startup;

namespace Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Used to configure background job system.
    /// </summary>
    public interface IBackgroundJobConfiguration
    {
        /// <summary>
        /// Used to enable/disable background job execution.
        /// </summary>
        bool IsJobExecutionEnabled { get; set; }

        /// <summary>
        /// Gets the  configuration object.
        /// </summary>
        IStartupConfiguration Configuration { get; }
    }
}
