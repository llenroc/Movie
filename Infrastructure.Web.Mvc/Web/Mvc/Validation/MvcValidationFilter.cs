using System.Web.Mvc;
using Infrastructure.Dependency;
using Infrastructure.Web.Mvc.Configuration;
using Infrastructure.Web.Mvc.Extensions;

namespace Infrastructure.Web.Mvc.Validation
{
    public class MvcValidationFilter : IActionFilter, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;
        private readonly IMvcConfiguration _configuration;

        public MvcValidationFilter(IIocResolver iocResolver, IMvcConfiguration configuration)
        {
            _iocResolver = iocResolver;
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!_configuration.IsValidationEnabledForControllers)
            {
                return;
            }
            var methodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();

            if (methodInfo == null)
            {
                return;
            }

            using (var validator = _iocResolver.ResolveAsDisposable<MvcActionInvocationValidator>())
            {
                validator.Object.Initialize(filterContext, methodInfo);
                validator.Object.Validate();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
