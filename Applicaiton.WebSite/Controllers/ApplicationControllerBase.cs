using Application.Files;
using Application.WebSite.MultiTenancy;
using Infrastructure.Configuration;
using Infrastructure.Extensions;
using Infrastructure.IdentityFramework;
using Infrastructure.Localization;
using Infrastructure.Logging;
using Infrastructure.Timing;
using Infrastructure.UI;
using Infrastructure.Web.Models;
using Infrastructure.Web.Mvc.Controllers;
using Microsoft.AspNet.Identity;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Application.WebSite.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class ApplicationControllerBase : InfrastructureController
    {
        public FileManager FileManager {get; set; }
        public TenantIdAccessor TenantIdAccessor { get; set; }
        public IAppFolders AppFolders { get; set; }

        protected ApplicationControllerBase()
        {
            LocalizationSourceName = ApplicationConsts.LocalizationSourceName;
        }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            base.OnAuthentication(filterContext);
        }

        /// <summary>
        /// This method tries to set current tenant id if current user has not login.
        /// Thus, we can get IAbpSession.TenantId later.
        /// </summary>
        protected void SetTentantId()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                return;
            }
            TenantIdAccessor.SetCurrentTenantId();
        }

        protected void RestoreUserLanguage()
        {
            var defaultLanguage = SettingManager.GetSettingValue(LocalizationSettingNames.DefaultLanguage);

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

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Json(new ErrorInfo("no file"));
            }

            if(file.ContentLength> 10485760)
            {
                throw new UserFriendlyException(L("YourFileExceed10485760Byte"));
            }
            var fileName = Path.GetFileName(file.FileName);
            var relativePath = AppFolders.UploadFolder + fileName;
            var filePath = Request.MapPath(relativePath);

            try
            {
                file.SaveAs(filePath);
                InfrastructureFileInfo fileInfo = new InfrastructureFileInfo() { Name = fileName, Path = relativePath };
                fileInfo = FileManager.Create(fileInfo);
                return Json(fileInfo);
            }
            catch (Exception exception)
            {
                return Json(new ErrorInfo(exception.Message));
            }
        }
    }
}