using System;
using System.Reflection;
using System.Web.Mvc;
using Infrastructure.Configuration.Startup;
using Infrastructure.Modules;
using Infrastructure.Web.Mvc.Auditing;
using Infrastructure.Web.Mvc.Authorization;
using Infrastructure.Web.Mvc.Configuration;
using Infrastructure.Web.Mvc.Controllers;
using Infrastructure.Web.Mvc.ModelBinding.Binders;
using Infrastructure.Web.Mvc.Security.AntiForgery;
using Infrastructure.Web.Mvc.UnitOfWork;
using Infrastructure.Web.Mvc.Validation;
using Infrastructure.Web.Security.AntiForgery;

namespace Infrastructure.Web.Mvc
{
    /// <summary>
    /// This module is used to build ASP.NET MVC web sites using .
    /// </summary>
    [DependsOn(typeof(WebModule))]
    public class WebMvcModule : InfrastructureModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ControllerConventionalRegistrar());

            IocManager.Register<IMvcConfiguration, MvcConfiguration>();

            Configuration.ReplaceService<IInfrastructureAntiForgeryManager, MvcAntiForgeryManager>();
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(IocManager));
        }

        /// <inheritdoc/>
        public override void PostInitialize()
        {
            GlobalFilters.Filters.Add(IocManager.Resolve<MvcAuthorizeFilter>());
            GlobalFilters.Filters.Add(IocManager.Resolve<AntiForgeryMvcFilter>());
            GlobalFilters.Filters.Add(IocManager.Resolve<MvcAuditFilter>());
            GlobalFilters.Filters.Add(IocManager.Resolve<MvcValidationFilter>());
            GlobalFilters.Filters.Add(IocManager.Resolve<MvcUnitOfWorkFilter>());

            var MvcDateTimeBinder = new MvcDateTimeBinder();
            ModelBinders.Binders.Add(typeof(DateTime), MvcDateTimeBinder);
            ModelBinders.Binders.Add(typeof(DateTime?), MvcDateTimeBinder);
        }
    }
}
