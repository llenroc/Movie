using System;

namespace Infrastructure.Web.Security.AntiForgery
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class ValidateAntiForgeryTokenAttribute : Attribute
    {

    }
}
