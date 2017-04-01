using Infrastructure.Configuration.Startup;
using Infrastructure.Localization.Dictionaries;
using Infrastructure.Localization.Dictionaries.Xml;
using Infrastructure.Modules;
using Infrastructure.Web.Api.ProxyScripting.Configuration;
using Infrastructure.Web.Api.ProxyScripting.Generators.JQuery;
using Infrastructure.Web.Configuration;
using Infrastructure.Web.Security.AntiForgery;
using Infrastructure.Web.Web.MultiTenancy;
using System.Reflection;

namespace Infrastructure.Web
{
    /// <summary>
    /// This module is used to use  in ASP.NET web applications.
    /// </summary>
    [DependsOn(typeof(KernelModule))]
    public class WebCommonModule : InfrastructureModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.Register<IWebMultiTenancyConfiguration, WebMultiTenancyConfiguration>();
            IocManager.Register<IApiProxyScriptingConfiguration, ApiProxyScriptingConfiguration>();
            IocManager.Register<IAntiForgeryConfiguration, AntiForgeryConfiguration>();
            IocManager.Register<IWebEmbeddedResourcesConfiguration, WebEmbeddedResourcesConfiguration>();
            IocManager.Register<IWebCommonModuleConfiguration, WebCommonModuleConfiguration>();

            Configuration.Modules.WebCommon().ApiProxyScripting.Generators[JQueryProxyScriptGenerator.Name] = typeof(JQueryProxyScriptGenerator);

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    WebConsts.LocalizaionSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(Assembly.GetExecutingAssembly(),
                    "Infrastructure.Web.Web.Localization.WebXmlSource")));
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
