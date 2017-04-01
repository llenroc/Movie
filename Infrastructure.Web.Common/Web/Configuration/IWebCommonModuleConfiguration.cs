using Infrastructure.Web.Security.AntiForgery;
using Infrastructure.Web.Api.ProxyScripting.Configuration;
using Infrastructure.Web.Web.MultiTenancy;

namespace Infrastructure.Web.Configuration
{
    /// <summary>
    /// Used to configure  Web Common module.
    /// </summary>
    public interface IWebCommonModuleConfiguration
    {
        /// <summary>
        /// If this is set to true, all exception and details are sent directly to clients on an error.
        /// Default: false ( hides exception details from clients except special exceptions.)
        /// </summary>
        bool SendAllExceptionsToClients { get; set; }

        /// <summary>
        /// Used to configure Api proxy scripting.
        /// </summary>
        IApiProxyScriptingConfiguration ApiProxyScripting { get; }

        /// <summary>
        /// Used to configure Anti Forgery security settings.
        /// </summary>
        IAntiForgeryConfiguration AntiForgery { get; }

        IWebEmbeddedResourcesConfiguration EmbeddedResources { get; }

        IWebMultiTenancyConfiguration MultiTenancy { get; }
    }
}
