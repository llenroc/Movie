using Castle.Facilities.Logging;
using Infrastructure.Castle.Logging.Log4Net;
using Infrastructure.Configuration;
using Infrastructure.Extensions;
using Infrastructure.Localization;
using Infrastructure.Logging;
using Infrastructure.Timing;
using Infrastructure.Web;
using System;
using System.Globalization;
using System.Web;

namespace Application.WebSite
{
    public class MvcApplication : WebApplication<WebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            bootstrapper.IocManager.IocContainer.
                AddFacility<LoggingFacility>(f => f.UseInfrastructureLog4Net().WithConfig("log4net.config"));
            base.Application_Start(sender, e);
        }

        protected override void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            base.Application_AuthenticateRequest(sender, e);
            //SetTentantId();
        }

        protected override void Session_Start(object sender, EventArgs e)
        {
            //RestoreUserLanguage();
            base.Session_Start(sender, e);
        }


        protected override void Session_End(object sender, EventArgs e)
        {
            base.Session_Start(sender, e);
        }

        private void RestoreUserLanguage()
        {
            var settingManager = bootstrapper.IocManager.Resolve<ISettingManager>();
            var defaultLanguage = settingManager.GetSettingValue(LocalizationSettingNames.DefaultLanguage);

            if (defaultLanguage.IsNullOrEmpty())
            {
                return;
            }

            try
            {
                CultureInfo.GetCultureInfo(defaultLanguage);
                Response.Cookies.Add(new HttpCookie("Infrastructure.Localization.CultureName", defaultLanguage) { Expires = Clock.Now.AddYears(2) });
            }
            catch (CultureNotFoundException exception)
            {
                LogHelper.Logger.Warn(exception.Message, exception);
            }
        }
    }
}
