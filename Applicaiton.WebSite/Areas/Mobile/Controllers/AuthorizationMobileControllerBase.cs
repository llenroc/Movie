using Application.WebSite.Areas.Mobile.Filters;
using Infrastructure.Web.Mvc.Authorization;

namespace Application.WebSite.Areas.Mobile.Controllers
{
    [MvcAuthorize]
    [PageTitleFilter]
    public abstract class AuthorizationMobileControllerBase : MobileControllerBase
    {
    }
}