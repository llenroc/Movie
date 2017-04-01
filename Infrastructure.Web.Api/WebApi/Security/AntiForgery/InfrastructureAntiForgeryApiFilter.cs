using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Infrastructure.Dependency;
using Infrastructure.Web.Security.AntiForgery;
using Infrastructure.WebApi.Configuration;
using Infrastructure.WebApi.Controllers.Dynamic.Selectors;
using Infrastructure.WebApi.Validation;
using Castle.Core.Logging;

namespace Infrastructure.WebApi.Security.AntiForgery
{
    public class InfrastructureAntiForgeryApiFilter : IAuthorizationFilter, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public bool AllowMultiple => false;

        private readonly IInfrastructureAntiForgeryManager _InfrastructureAntiForgeryManager;
        private readonly IInfrastructureWebApiConfiguration _webApiConfiguration;
        private readonly IAntiForgeryWebConfiguration _antiForgeryWebConfiguration;

        public InfrastructureAntiForgeryApiFilter(
            IInfrastructureAntiForgeryManager InfrastructureAntiForgeryManager,
            IInfrastructureWebApiConfiguration webApiConfiguration,
            IAntiForgeryWebConfiguration antiForgeryWebConfiguration)
        {
            _InfrastructureAntiForgeryManager = InfrastructureAntiForgeryManager;
            _webApiConfiguration = webApiConfiguration;
            _antiForgeryWebConfiguration = antiForgeryWebConfiguration;
            Logger = NullLogger.Instance;
        }

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();

            if (methodInfo == null)
            {
                return await continuation();
            }

            if (!_InfrastructureAntiForgeryManager.ShouldValidate(_antiForgeryWebConfiguration, methodInfo, actionContext.Request.Method.ToHttpVerb(), _webApiConfiguration.IsAutomaticAntiForgeryValidationEnabled))
            {
                return await continuation();
            }

            if (!_InfrastructureAntiForgeryManager.IsValid(actionContext.Request.Headers))
            {
                return CreateErrorResponse(actionContext, "Empty or invalid anti forgery header token.");
            }
            return await continuation();
        }

        protected virtual HttpResponseMessage CreateErrorResponse(HttpActionContext actionContext, string reason)
        {
            Logger.Warn(reason);
            Logger.Warn("Requested URI: " + actionContext.Request.RequestUri);
            return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = reason };
        }
    }
}
