using Infrastructure.Reflection;
using System.Reflection;

namespace Infrastructure.Web.Security.AntiForgery
{
    public static class AntiForgeryManagerWebExtensions
    {
        public static bool ShouldValidate(this IInfrastructureAntiForgeryManager manager, IAntiForgeryWebConfiguration antiForgeryWebConfiguration,MethodInfo methodInfo,HttpVerb httpVerb, bool defaultValue)
        {
            if (!antiForgeryWebConfiguration.IsEnabled)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(ValidateAntiForgeryTokenAttribute), true))
            {
                return true;
            }

            if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableAntiForgeryTokenValidationAttribute>(methodInfo) != null)
            {
                return false;
            }

            if (antiForgeryWebConfiguration.IgnoredHttpVerbs.Contains(httpVerb))
            {
                return false;
            }

            if (methodInfo.DeclaringType?.IsDefined(typeof(ValidateAntiForgeryTokenAttribute), true) ?? false)
            {
                return true;
            }

            return defaultValue;
        }
    }
}
