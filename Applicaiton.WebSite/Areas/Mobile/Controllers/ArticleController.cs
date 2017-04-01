using Application.Features;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Features;
using System.Web.Mvc;

namespace Application.WebSite.Areas.Mobile.Controllers
{
    public class ArticleController : AuthorizationMobileControllerBase
    {
        [RequiresFeature(AppFeatures.ArticleFeature)]
        public ActionResult Index()
        {
            return View();
        }

        [RequiresFeature(AppFeatures.ArticleFeature)]
        public ActionResult Content(IdInput input)
        {
            return View(input);
        }
    }
}