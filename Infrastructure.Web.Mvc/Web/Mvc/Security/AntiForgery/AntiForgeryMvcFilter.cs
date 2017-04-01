using System.Net;
using System.Reflection;
using System.Web.Mvc;
using Infrastructure.Dependency;
using Infrastructure.Web.Models;
using Infrastructure.Web.Mvc.Configuration;
using Infrastructure.Web.Mvc.Controllers.Results;
using Infrastructure.Web.Mvc.Extensions;
using Infrastructure.Web.Mvc.Helpers;
using Infrastructure.Web.Security.AntiForgery;
using Castle.Core.Logging;

namespace Infrastructure.Web.Mvc.Security.AntiForgery
{
    public class AntiForgeryMvcFilter : IAuthorizationFilter, ITransientDependency
    {
        public ILogger Logger { get; set; }

        private readonly IInfrastructureAntiForgeryManager _AntiForgeryManager;
        private readonly IMvcConfiguration _mvcConfiguration;
        private readonly IAntiForgeryWebConfiguration _antiForgeryWebConfiguration;

        public AntiForgeryMvcFilter(IInfrastructureAntiForgeryManager AntiForgeryManager, IMvcConfiguration mvcConfiguration,IAntiForgeryWebConfiguration antiForgeryWebConfiguration)
        {
            _AntiForgeryManager = AntiForgeryManager;
            _mvcConfiguration = mvcConfiguration;
            _antiForgeryWebConfiguration = antiForgeryWebConfiguration;
            Logger = NullLogger.Instance;
        }

        public void OnAuthorization(AuthorizationContext context)
        {
            var methodInfo = context.ActionDescriptor.GetMethodInfoOrNull();

            if (methodInfo == null)
            {
                return;
            }
            var httpVerb = HttpVerbHelper.Create(context.HttpContext.Request.HttpMethod);

            if (!_AntiForgeryManager.ShouldValidate(_antiForgeryWebConfiguration, methodInfo, httpVerb, _mvcConfiguration.IsAutomaticAntiForgeryValidationEnabled))
            {
                return;
            }

            if (!_AntiForgeryManager.IsValid(context.HttpContext))
            {
                CreateErrorResponse(context, methodInfo, "Empty or invalid anti forgery header token.");
            }
        }

        private void CreateErrorResponse(AuthorizationContext context,MethodInfo methodInfo,string message)
        {
            Logger.Warn(message);
            Logger.Warn("Requested URI: " + context.HttpContext.Request.Url);

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.HttpContext.Response.StatusDescription = message;

            var isJsonResult = MethodInfoHelper.IsJsonResult(methodInfo);

            if (isJsonResult)
            {
                context.Result = CreateUnAuthorizedJsonResult(message);
            }
            else
            {
                context.Result = CreateUnAuthorizedNonJsonResult(context, message);
            }

            if (isJsonResult || context.HttpContext.Request.IsAjaxRequest())
            {
                context.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }
        }

        protected virtual InfrastructureJsonResult CreateUnAuthorizedJsonResult(string message)
        {
            return new InfrastructureJsonResult(new AjaxResponse(new ErrorInfo(message), true))
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected virtual HttpStatusCodeResult CreateUnAuthorizedNonJsonResult(AuthorizationContext filterContext, string message)
        {
            return new HttpStatusCodeResult(filterContext.HttpContext.Response.StatusCode, message);
        }
    }
}
