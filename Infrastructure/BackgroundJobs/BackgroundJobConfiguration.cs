using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Configuration.Startup;

namespace Infrastructure.BackgroundJobs
{
    internal class BackgroundJobConfiguration : IBackgroundJobConfiguration
    {
        public bool IsJobExecutionEnabled { get; set; }

        public IStartupConfiguration Configuration { get; private set; }

        public BackgroundJobConfiguration(IStartupConfiguration configuration)
        {
            Configuration = configuration;

            IsJobExecutionEnabled = true;
        }
    }
}
