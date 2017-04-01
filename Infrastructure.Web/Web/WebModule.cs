using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Infrastructure.Auditing;
using Infrastructure.Modules;
using Infrastructure.Runtime.Session;
using Infrastructure.Web.Session;
using Infrastructure.Configuration.Startup;
using Infrastructure.Web.Configuration;
using Infrastructure.Web.Security.AntiForgery;
using Infrastructure.Collections.Extensions;
using Infrastructure.Dependency;
using Infrastructure.Web.MultiTenancy;

namespace Infrastructure.Web
{
    /// <summary>
    /// This module is used to use  in ASP.NET web applications.
    /// </summary>
    [DependsOn(typeof(WebCommonModule))]
    public class WebModule : InfrastructureModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.Register<IAntiForgeryWebConfiguration, AntiForgeryWebConfiguration>();
            IocManager.Register<IWebLocalizationConfiguration, WebLocalizationConfiguration>();
            IocManager.Register<IWebModuleConfiguration, WebModuleConfiguration>();

            Configuration.ReplaceService<IPrincipalAccessor, HttpContextPrincipalAccessor>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IClientInfoProvider, WebAuditInfoProvider>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IAuditInfoProvider, DefaultAuditInfoProvider>(DependencyLifeStyle.Transient);

            Configuration.MultiTenancy.Resolvers.Add<DomainTenantResolveContributer>();
            Configuration.MultiTenancy.Resolvers.Add<HttpHeaderTenantResolveContributer>();
            Configuration.MultiTenancy.Resolvers.Add<HttpCookieTenantResolveContributer>();

            AddIgnoredTypes();
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        private void AddIgnoredTypes()
        {
            var ignoredTypes = new[]
            {
                typeof(HttpPostedFileBase),
                typeof(IEnumerable<HttpPostedFileBase>),
                typeof(HttpPostedFileWrapper),
                typeof(IEnumerable<HttpPostedFileWrapper>)
            };

            foreach (var ignoredType in ignoredTypes)
            {
                Configuration.Auditing.IgnoredTypes.AddIfNotContains(ignoredType);
                Configuration.Validation.IgnoredTypes.AddIfNotContains(ignoredType);
            }
        }
    }
}
