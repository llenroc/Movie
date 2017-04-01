using Application.Configuration.Host;
using Application.WebSite.Areas.Mobile.Models.Layout;
using System.Web.Mvc;
using Application.MovieCategorys;
using System.Linq;

namespace Application.WebSite.Areas.Mobile.Controllers
{
    public class LayoutController : AnonymousMobileControllerBase
    {
        public MovieCategoryAppService movieCategoryAppService { get; set; }
        public IHostSettingsAppService HostSettingsAppService { get; set; }

        [ChildActionOnly]
        public PartialViewResult Header(string PageTitle = null)
        {
            var headerModel = new HeaderViewModel
            {
                CurrentLoginInformations =SessionAppService.GetCurrentLoginInformations(),
                AppSettings= HostSettingsAppService.GetAppSettings()
            };
            ViewBag.PageTitle = PageTitle;
            return PartialView(headerModel);
        }

        [ChildActionOnly]
        public PartialViewResult Nav()
        {
            var navModel = new NavViewModel
            {
                MovieCategorys = movieCategoryAppService.GetAll().Items.ToList()
            };
            return PartialView(navModel);
        }

        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            return PartialView();
        }
    }
}