using System.Web;
using System.Web.Mvc;
using Infrastructure.Dependency;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Web.Mvc.Configuration;
using Infrastructure.Web.Mvc.Extensions;

namespace Infrastructure.Web.Mvc.UnitOfWork
{
    public class MvcUnitOfWorkFilter : IActionFilter, ITransientDependency
    {
        public const string UowHttpContextKey = "__UnitOfWork";

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMvcConfiguration _configuration;

        public MvcUnitOfWorkFilter(IUnitOfWorkManager unitOfWorkManager,IMvcConfiguration configuration)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            var methodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();

            if (methodInfo == null)
            {
                return;
            }

            var unitOfWorkAttr =UnitOfWorkAttribute.GetUnitOfWorkAttributeOrNull(methodInfo) ??_configuration.DefaultUnitOfWorkAttribute;

            if (unitOfWorkAttr.IsDisabled)
            {
                return;
            }
            SetCurrentUow(
                filterContext.HttpContext,
                _unitOfWorkManager.Begin(unitOfWorkAttr.CreateOptions())
            );
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            var unitOfWork = GetCurrentUow(filterContext.HttpContext);

            if (unitOfWork == null)
            {
                return;
            }

            try
            {
                if (filterContext.Exception == null)
                {
                    unitOfWork.Complete();
                }
            }
            finally
            {
                unitOfWork.Dispose();
                SetCurrentUow(filterContext.HttpContext, null);
            }
        }

        private static IUnitOfWorkCompleteHandle GetCurrentUow(HttpContextBase httpContext)
        {
            return httpContext.Items[UowHttpContextKey] as IUnitOfWorkCompleteHandle;
        }

        private static void SetCurrentUow(HttpContextBase httpContext, IUnitOfWorkCompleteHandle unitOfWork)
        {
            httpContext.Items[UowHttpContextKey] = unitOfWork;
        }
    }
}
