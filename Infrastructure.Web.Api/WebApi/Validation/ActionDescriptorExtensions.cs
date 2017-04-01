using System.Reflection;
using System.Web.Http.Controllers;
using Infrastructure.Extensions;

namespace Infrastructure.WebApi.Validation
{
    public static class ActionDescriptorExtensions
    {
        public static MethodInfo GetMethodInfoOrNull(this HttpActionDescriptor actionDescriptor)
        {
            if (actionDescriptor is ReflectedHttpActionDescriptor)
            {
                return actionDescriptor.As<ReflectedHttpActionDescriptor>().MethodInfo;
            }
            return null;
        }

        public static bool IsDynamicInfrastructureAction(this HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor
                .ControllerDescriptor
                .Properties
                .ContainsKey("__InfrastructureDynamicApiControllerInfo");
        }
    }
}
