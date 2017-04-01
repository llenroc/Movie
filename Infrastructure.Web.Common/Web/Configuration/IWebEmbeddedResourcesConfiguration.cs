using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Web.Configuration
{
    public interface IWebEmbeddedResourcesConfiguration
    {
        /// <summary>
        /// List of file extensions (without dot) to ignore for embedded resources.
        /// Default extensions: cshtml, config.
        /// </summary>
        HashSet<string> IgnoredFileExtensions { get; }
    }
}
