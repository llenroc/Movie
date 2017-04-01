using Infrastructure.Auditing;
using System.Web.Mvc;

namespace Application.WebSite.Controllers
{
    public class ErrorController : ApplicationControllerBase
    {
        [DisableAuditing]
        public ActionResult E403()
        {
            return View();
        }

        [DisableAuditing]
        public ActionResult E404()
        {
            return View();
        }

        [DisableAuditing]
        public ActionResult E500()
        {
            return View();
        }

        [DisableAuditing]
        public ActionResult WebSiteOff()
        {
            return View();
        }
    }
}