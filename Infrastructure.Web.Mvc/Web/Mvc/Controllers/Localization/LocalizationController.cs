using System.Web;
using System.Web.Mvc;
using Infrastructure.Auditing;
using Infrastructure.Configuration;
using Infrastructure.Localization;
using Infrastructure.Runtime.Session;
using Infrastructure.Timing;
using Infrastructure.Web.Configuration;
using Infrastructure.Web.Models;

namespace Infrastructure.Web.Mvc.Controllers.Localization
{
    public class LocalizationController : InfrastructureController
    {
        private readonly IWebLocalizationConfiguration _webLocalizationConfiguration;

        public LocalizationController(IWebLocalizationConfiguration webLocalizationConfiguration)
        {
            _webLocalizationConfiguration = webLocalizationConfiguration;
        }

        [DisableAuditing]
        public virtual ActionResult ChangeCulture(string cultureName, string returnUrl = "")
        {
            if (!GlobalizationHelper.IsValidCultureCode(cultureName))
            {
                throw new InfrastructureException("Unknown language: " + cultureName + ". It must be a valid culture!");
            }

            Response.Cookies.Add(
                new HttpCookie(_webLocalizationConfiguration.CookieName, cultureName)
                {
                    Expires = Clock.Now.AddYears(2)
                }
            );

            if (InfrastructureSession.UserId.HasValue)
            {
                SettingManager.ChangeSettingForUser(
                    InfrastructureSession.ToUserIdentifier(),
                    LocalizationSettingNames.DefaultLanguage,
                    cultureName
                );
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new AjaxResponse(), JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}
