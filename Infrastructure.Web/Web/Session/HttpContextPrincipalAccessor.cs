using System.Security.Claims;
using System.Web;
using Infrastructure.Runtime.Session;

namespace Infrastructure.Web.Session
{

    public class HttpContextPrincipalAccessor : DefaultPrincipalAccessor
    {
        public override ClaimsPrincipal Principal => HttpContext.Current?.User as ClaimsPrincipal ?? base.Principal;
    }
}
