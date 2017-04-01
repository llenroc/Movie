using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Infrastructure.Dependency;
using Infrastructure.Domain.Entities;
using Infrastructure.Event.Bus;
using Infrastructure.Event.Bus.Exceptions;
using Infrastructure.Extensions;
using Infrastructure.Logging;
using Infrastructure.Runtime.Session;
using Infrastructure.Web.Models;
using Infrastructure.WebApi.Configuration;
using Infrastructure.WebApi.Controllers;
using Castle.Core.Logging;

namespace Infrastructure.WebApi.ExceptionHandling
{
    /// <summary>
    /// Used to handle exceptions on web api controllers.
    /// </summary>
    public class InfrastructureApiExceptionFilterAttribute : ExceptionFilterAttribute, ITransientDependency
    {
        /// <summary>
        /// Reference to the <see cref="ILogger"/>.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Reference to the <see cref="IEventBus"/>.
        /// </summary>
        public IEventBus EventBus { get; set; }

        public IInfrastructureSession InfrastructureSession { get; set; }

        private readonly IInfrastructureWebApiConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfrastructureApiExceptionFilterAttribute"/> class.
        /// </summary>
        public InfrastructureApiExceptionFilterAttribute(IInfrastructureWebApiConfiguration configuration)
        {
            _configuration = configuration;
            Logger = NullLogger.Instance;
            EventBus = NullEventBus.Instance;
            InfrastructureSession = NullInfrastructureSession.Instance;
        }

        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="context">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            var wrapResultAttribute = HttpActionDescriptorHelper
                .GetWrapResultAttributeOrNull(context.ActionContext.ActionDescriptor) ??
                _configuration.DefaultWrapResultAttribute;

            if (wrapResultAttribute.LogError)
            {
                LogHelper.LogException(Logger, context.Exception);
            }

            if (!wrapResultAttribute.WrapOnError)
            {
                return;
            }

            if (IsIgnoredUrl(context.Request.RequestUri))
            {
                return;
            }

            context.Response = context.Request.CreateResponse(
                GetStatusCode(context),
                new AjaxResponse(
                    SingletonDependency<ErrorInfoBuilder>.Instance.BuildForException(context.Exception),
                    context.Exception is Infrastructure.Authorization.AuthorizationException)
            );

            EventBus.Trigger(this, new HandledExceptionData(context.Exception));
        }

        private HttpStatusCode GetStatusCode(HttpActionExecutedContext context)
        {
            if (context.Exception is Infrastructure.Authorization.AuthorizationException)
            {
                return InfrastructureSession.UserId.HasValue
                    ? HttpStatusCode.Forbidden
                    : HttpStatusCode.Unauthorized;
            }

            if (context.Exception is EntityNotFoundException)
            {
                return HttpStatusCode.NotFound;
            }

            return HttpStatusCode.InternalServerError;
        }

        private bool IsIgnoredUrl(Uri uri)
        {
            if (uri == null || uri.AbsolutePath.IsNullOrEmpty())
            {
                return false;
            }
            return _configuration.ResultWrappingIgnoreUrls.Any(url => uri.AbsolutePath.StartsWith(url));
        }
    }
}
