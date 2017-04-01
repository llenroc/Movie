using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Infrastructure.Authorization;
using Infrastructure.Dependency;
using Infrastructure.Event.Bus;
using Infrastructure.Event.Bus.Exceptions;
using Infrastructure.Localization;
using Infrastructure.Logging;
using Infrastructure.Web;
using Infrastructure.Web.Models;
using Infrastructure.WebApi.Configuration;
using Infrastructure.WebApi.Validation;

namespace Infrastructure.WebApi.Authorization
{
    public class InfrastructureApiAuthorizeFilter : IAuthorizationFilter, ITransientDependency
    {
        public bool AllowMultiple => false;

        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IInfrastructureWebApiConfiguration _configuration;
        private readonly ILocalizationManager _localizationManager;
        private readonly IEventBus _eventBus;

        public InfrastructureApiAuthorizeFilter(
            IAuthorizationHelper authorizationHelper,
            IInfrastructureWebApiConfiguration configuration,
            ILocalizationManager localizationManager,
            IEventBus eventBus)
        {
            _authorizationHelper = authorizationHelper;
            _configuration = configuration;
            _localizationManager = localizationManager;
            _eventBus = eventBus;
        }

        public virtual async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return await continuation();
            }
            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();

            if (methodInfo == null)
            {
                return await continuation();
            }

            if (actionContext.ActionDescriptor.IsDynamicInfrastructureAction())
            {
                return await continuation();
            }

            try
            {
                await _authorizationHelper.AuthorizeAsync(methodInfo);
                return await continuation();
            }
            catch (AuthorizationException ex)
            {
                LogHelper.Logger.Warn(ex.ToString(), ex);
                _eventBus.Trigger(this, new HandledExceptionData(ex));
                return CreateUnAuthorizedResponse(actionContext);
            }
        }

        protected virtual HttpResponseMessage CreateUnAuthorizedResponse(HttpActionContext actionContext)
        {
            HttpStatusCode statusCode;
            ErrorInfo error;

            if (actionContext.RequestContext.Principal?.Identity?.IsAuthenticated ?? false)
            {
                statusCode = HttpStatusCode.Forbidden;
                error = new ErrorInfo(
                    _localizationManager.GetString(WebConsts.LocalizaionSourceName, "DefaultError403"),
                    _localizationManager.GetString(WebConsts.LocalizaionSourceName, "DefaultErrorDetail403")
                );
            }
            else
            {
                statusCode = HttpStatusCode.Unauthorized;
                error = new ErrorInfo(
                    _localizationManager.GetString(WebConsts.LocalizaionSourceName, "DefaultError401"),
                    _localizationManager.GetString(WebConsts.LocalizaionSourceName, "DefaultErrorDetail401")
                );
            }

            var response = new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent<AjaxResponse>(
                    new AjaxResponse(error, true),
                    _configuration.HttpConfiguration.Formatters.JsonFormatter
                )
            };
            return response;
        }
    }
}
