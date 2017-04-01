using Infrastructure.Web.Security.AntiForgery;
using Infrastructure.Web.Api.ProxyScripting.Configuration;
using Infrastructure.Web.Web.MultiTenancy;

namespace Infrastructure.Web.Configuration
{
    internal class WebCommonModuleConfiguration : IWebCommonModuleConfiguration
    {
        public bool SendAllExceptionsToClients { get; set; }

        public IApiProxyScriptingConfiguration ApiProxyScripting { get; }

        public IAntiForgeryConfiguration AntiForgery { get; }

        public IWebEmbeddedResourcesConfiguration EmbeddedResources { get; }

        public IWebMultiTenancyConfiguration MultiTenancy { get; }

        public WebCommonModuleConfiguration(IApiProxyScriptingConfiguration apiProxyScripting, 
            IAntiForgeryConfiguration antiForgery,
            IWebEmbeddedResourcesConfiguration embeddedResources,
            IWebMultiTenancyConfiguration multiTenancy)
        {
            ApiProxyScripting = apiProxyScripting;
            AntiForgery = antiForgery;
            EmbeddedResources = embeddedResources;
            MultiTenancy = multiTenancy;
        }
    }
}
