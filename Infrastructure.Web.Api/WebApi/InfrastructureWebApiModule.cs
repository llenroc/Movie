using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Infrastructure.Logging;
using Infrastructure.Modules;
using Infrastructure.Web;
using Infrastructure.WebApi.Configuration;
using Infrastructure.WebApi.Controllers;
using Infrastructure.WebApi.Controllers.Dynamic;
using Infrastructure.WebApi.Controllers.Dynamic.Formatters;
using Infrastructure.WebApi.Controllers.Dynamic.Selectors;
using Infrastructure.WebApi.Runtime.Caching;
using Castle.MicroKernel.Registration;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Infrastructure.Configuration.Startup;
using Infrastructure.Json;
using Infrastructure.WebApi.Auditing;
using Infrastructure.WebApi.Authorization;
using Infrastructure.WebApi.Controllers.ApiExplorer;
using Infrastructure.WebApi.Controllers.Dynamic.Binders;
using Infrastructure.WebApi.Controllers.Dynamic.Builders;
using Infrastructure.WebApi.ExceptionHandling;
using Infrastructure.WebApi.Security.AntiForgery;
using Infrastructure.WebApi.Uow;
using Infrastructure.WebApi.Validation;

namespace Infrastructure.WebApi
{
    /// <summary>
    /// This module provides Infrastructure features for ASP.NET Web API.
    /// </summary>
    [DependsOn(typeof(WebModule))]
    public class InfrastructureWebApiModule : InfrastructureModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ApiControllerConventionalRegistrar());

            IocManager.Register<IDynamicApiControllerBuilder, DynamicApiControllerBuilder>();
            IocManager.Register<IInfrastructureWebApiConfiguration, InfrastructureWebApiConfiguration>();

            Configuration.Settings.Providers.Add<ClearCacheSettingProvider>();

            Configuration.Modules.WebApi().ResultWrappingIgnoreUrls.Add("/swagger");
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            var httpConfiguration = IocManager.Resolve<IInfrastructureWebApiConfiguration>().HttpConfiguration;

            InitializeAspNetServices(httpConfiguration);
            InitializeFilters(httpConfiguration);
            InitializeFormatters(httpConfiguration);
            InitializeRoutes(httpConfiguration);
            InitializeModelBinders(httpConfiguration);

            foreach (var controllerInfo in IocManager.Resolve<DynamicApiControllerManager>().GetAll())
            {
                IocManager.IocContainer.Register(
                    Component.For(controllerInfo.InterceptorType).LifestyleTransient(),
                    Component.For(controllerInfo.ApiControllerType)
                        .Proxy.AdditionalInterfaces(controllerInfo.ServiceInterfaceType)
                        .Interceptors(controllerInfo.InterceptorType)
                        .LifestyleTransient()
                    );
                LogHelper.Logger.DebugFormat("Dynamic web api controller is created for type '{0}' with service name '{1}'.", controllerInfo.ServiceInterfaceType.FullName, controllerInfo.ServiceName);
            }
            Configuration.Modules.WebApi().HttpConfiguration.EnsureInitialized();
        }

        private void InitializeAspNetServices(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Services.Replace(typeof(IHttpControllerSelector), new InfrastructureHttpControllerSelector(httpConfiguration, IocManager.Resolve<DynamicApiControllerManager>()));
            httpConfiguration.Services.Replace(typeof(IHttpActionSelector), new InfrastructureApiControllerActionSelector(IocManager.Resolve<IInfrastructureWebApiConfiguration>()));
            httpConfiguration.Services.Replace(typeof(IHttpControllerActivator), new InfrastructureApiControllerActivator(IocManager));
            httpConfiguration.Services.Replace(typeof(IApiExplorer), IocManager.Resolve<InfrastructureApiExplorer>());
        }

        private void InitializeFilters(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Filters.Add(IocManager.Resolve<InfrastructureApiAuthorizeFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<InfrastructureAntiForgeryApiFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<InfrastructureApiAuditFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<InfrastructureApiValidationFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<InfrastructureApiUowFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<InfrastructureApiExceptionFilterAttribute>());

            httpConfiguration.MessageHandlers.Add(IocManager.Resolve<ResultWrapperHandler>());
        }

        private static void InitializeFormatters(HttpConfiguration httpConfiguration)
        {
            //Remove formatters except JsonFormatter.
            foreach (var currentFormatter in httpConfiguration.Formatters.ToList())
            {
                if (!(currentFormatter is JsonMediaTypeFormatter ||
                    currentFormatter is JQueryMvcFormUrlEncodedFormatter))
                {
                    httpConfiguration.Formatters.Remove(currentFormatter);
                }
            }
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.Converters.Insert(0, new DateTimeConverter());
            httpConfiguration.Formatters.Add(new PlainTextFormatter());
        }

        private static void InitializeRoutes(HttpConfiguration httpConfiguration)
        {
            //Dynamic Web APIs

            httpConfiguration.Routes.MapHttpRoute(
                name: "InfrastructureDynamicWebApi",
                routeTemplate: "api/services/{*serviceNameWithAction}"
                );

            //Other routes

            httpConfiguration.Routes.MapHttpRoute(
                name: "InfrastructureCacheController_Clear",
                routeTemplate: "api/InfrastructureCache/Clear",
                defaults: new { controller = "InfrastructureCache", action = "Clear" }
                );

            httpConfiguration.Routes.MapHttpRoute(
                name: "InfrastructureCacheController_ClearAll",
                routeTemplate: "api/InfrastructureCache/ClearAll",
                defaults: new { controller = "InfrastructureCache", action = "ClearAll" }
                );
        }

        private static void InitializeModelBinders(HttpConfiguration httpConfiguration)
        {
            var InfrastructureApiDateTimeBinder = new InfrastructureApiDateTimeBinder();
            httpConfiguration.BindParameter(typeof(DateTime), InfrastructureApiDateTimeBinder);
            httpConfiguration.BindParameter(typeof(DateTime?), InfrastructureApiDateTimeBinder);
        }
    }
}
