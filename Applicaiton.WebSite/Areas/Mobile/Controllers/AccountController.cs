using Application.Authorization;
using Application.Authorization.Roles;
using Application.Authorization.Users;
using Application.Configuration;
using Application.MultiTenancy;
using Application.Notifications;
using Application.Security;
using Application.Web;
using Application.WebSite.Authorization;
using Application.WebSite.Controllers;
using Application.WebSite.Controllers.Results;
using Application.WebSite.Models.Account;
using Application.WebSite.MultiTenancy;
using Infrastructure.Authorization.Users;
using Infrastructure.AutoMapper;
using Infrastructure.Configuration.Startup;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Extensions;
using Infrastructure.Notifications;
using Infrastructure.Runtime.Caching;
using Infrastructure.Runtime.Security;
using Infrastructure.UI;
using Infrastructure.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.WebSite.Areas.Mobile.Controllers
{
    public class AccountController : AccountControllerBase
    {
        public AccountController(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            LogInManager logInManager,
            ApplicationSignInManager signInManager,
            IUserEmailer userEmailer,
            IUnitOfWorkManager unitOfWorkManager,
            ITenancyNameFinder tenancyNameFinder,
            IMultiTenancyConfig multiTenancyConfig,
            IAppNotifier appNotifier,
            IWebUrlService webUrlService,
            INotificationSubscriptionManager notificationSubscriptionManager,
            ICacheManager cacheManager
            ) : base(
                tenantManager,
                userManager,
                roleManager,
                logInManager,
                signInManager,
                userEmailer,
                unitOfWorkManager,
                tenancyNameFinder,
                multiTenancyConfig,
                appNotifier,
                webUrlService,
                notificationSubscriptionManager,
                cacheManager)
        {
        }

        public ActionResult Login(string userNameOrEmailAddress = "", string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home");
            }

            return View(
                new LoginFormViewModel
                {
                    TenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull(),
                    IsSelfRegistrationEnabled = IsSelfRegistrationEnabled(),
                    ReturnUrl = returnUrl,
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                    UserNameOrEmailAddress = userNameOrEmailAddress
                });
        }

        [UnitOfWork]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            var loginResult = await GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password, loginModel.TenancyName);
            var tenantId = loginResult.Tenant == null ? (int?)null : loginResult.Tenant.Id;

            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                if (loginResult.User.ShouldChangePasswordOnNextLogin)
                {
                    loginResult.User.SetNewPasswordResetCode();

                    return Json(new AjaxResponse
                    {
                        TargetUrl = Url.Action(
                            "ResetPassword",
                            new ResetPasswordViewModel
                            {
                                TenantId = SimpleStringCipher.Instance.Encrypt(tenantId == null ? null : tenantId.ToString()),
                                UserId = SimpleStringCipher.Instance.Encrypt(loginResult.User.Id.ToString()),
                                ResetCode = loginResult.User.PasswordResetCode
                            })
                    });
                }
            }
            await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home");
            }

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }
            return Json(new AjaxResponse { TargetUrl = returnUrl });
        }

        private async Task<LoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case LoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private Exception CreateExceptionForFailedLoginAttempt(LoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case LoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case LoginResultType.InvalidUserNameOrEmailAddress:
                case LoginResultType.InvalidPassword:
                    return new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                case LoginResultType.InvalidTenancyName:
                    return new UserFriendlyException(L("LoginFailed"), L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                case LoginResultType.TenantIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("TenantIsNotActive", tenancyName));
                case LoginResultType.UserIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                case LoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException(L("LoginFailed"), "Your email address is not confirmed. You can not login"); //TODO: localize message
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException(L("LoginFailed"));
            }
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            var registerViewModel = new RegisterViewModel
            {
                TenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull(),
            };
            return RegisterView(registerViewModel);
        }

        private ActionResult RegisterView(RegisterViewModel model)
        {
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            return View("Register", model);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                CheckModelState();

                //Get tenancy name and tenant
                if (!_multiTenancyConfig.IsEnabled)
                {
                    model.TenancyName = Tenant.DefaultTenantName;
                }
                else if (model.TenancyName.IsNullOrEmpty())
                {
                    throw new UserFriendlyException(L("TenantNameCanNotBeEmpty"));
                }
                var tenant = await GetActiveTenantAsync(model.TenancyName);

                //Create user
                var user = new User
                {
                    TenantId = tenant.Id,
                    Name = model.Name,
                    Surname = model.Surname,
                    NickName=model.NickName,
                    EmailAddress = model.EmailAddress,
                    IsActive = true
                };

                //Get external login info if possible
                ExternalLoginInfo externalLoginInfo = null;

                if (model.IsExternalLogin)
                {
                    externalLoginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

                    if (externalLoginInfo == null)
                    {
                        throw new ApplicationException(L("CanNotExternalLogin"));
                    }
                    user.Avatar=externalLoginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == CommonClaimType.Avatar).Value;
                    user.Logins = new List<UserLogin>
                    {
                        new UserLogin
                        {
                            TenantId = tenant.Id,
                            LoginProvider = externalLoginInfo.Login.LoginProvider,
                            ProviderKey = externalLoginInfo.Login.ProviderKey
                        }
                    };

                    if (externalLoginInfo.Login.LoginProvider == "Weixin")
                    {
                        user.Source = UserSource.WeixinExternalLogin;
                    }
                    else
                    {
                        user.Source = UserSource.OtherExternalLogin;
                    }

                    if (model.UserName.IsNullOrEmpty())
                    {
                        model.UserName = externalLoginInfo.ExternalIdentity.Claims.Where(c=>c.Type==ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                    }
                    model.Password = Application.Authorization.Users.User.DefaultPassword;
                }
                else
                {
                    //Username and Password are required if not external login
                    if (model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                    {
                        throw new UserFriendlyException(L("FormIsNotValidMessage"));
                    }
                }
                user.UserName = Application.Authorization.Users.User.PreProcessUserName(model.UserName);
                user.Password = new PasswordHasher().HashPassword(model.Password);

                //Switch to the tenant
                _unitOfWorkManager.Current.EnableFilter(DataFilters.MayHaveTenant); //TODO: Needed?
                _unitOfWorkManager.Current.SetTenantId(tenant.Id);

                //Add default roles
                user.Roles = new List<UserRole>();

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    user.Roles.Add(new UserRole { RoleId = defaultRole.Id });
                }

                //Save user
                CheckErrors(await _userManager.CreateAsync(user));
                await _unitOfWorkManager.Current.SaveChangesAsync();

                //Notifications
                await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
                await _appNotifier.WelcomeToTheApplicationAsync(user);
                await _appNotifier.NewUserRegisteredAsync(user);

                //Directly login if possible
                if (user.IsActive)
                {
                    LoginResult<Tenant, User> loginResult;

                    if (externalLoginInfo != null)
                    {
                        loginResult = await _logInManager.LoginAsync(externalLoginInfo.Login, tenant.TenancyName);
                    }
                    else
                    {
                        loginResult = await GetLoginResultAsync(user.UserName, model.Password, tenant.TenancyName);
                    }

                    if (loginResult.Result == LoginResultType.Success)
                    {
                        await SignInAsync(loginResult.User, loginResult.Identity);
                        return Redirect(Url.Action("Index", "Home"));
                    }
                    Logger.Warn("New registered user could not be login. This should not be normally. login result: " + loginResult.Result);
                }

                //If can not login, show a register result page
                return View("RegisterResult", new RegisterResultViewModel
                {
                    TenancyName = tenant.TenancyName,
                    NameAndSurname = user.Name + " " + user.Surname,
                    UserName = user.UserName,
                    EmailAddress = user.EmailAddress,
                    IsActive = user.IsActive
                });
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
                ViewBag.ErrorMessage = ex.Message;

                return View("Register", model);
            }
        }

        public ActionResult ForgotPassword()
        {
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            ViewBag.TenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull();
            return View();
        }

        [HttpPost]
        public virtual async Task<JsonResult> SendPasswordResetLink(SendPasswordResetLinkViewModel model)
        {
            UnitOfWorkManager.Current.SetTenantId(await GetTenantIdOrDefault(model.TenancyName));
            var user = await GetUserByChecking(model.EmailAddress);

            user.SetNewPasswordResetCode();
            await _userEmailer.SendPasswordResetLinkAsync(user);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Json(new AjaxResponse());
        }

        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var tenantId = model.TenantId.IsNullOrEmpty() ? (int?)null : SimpleStringCipher.Instance.Decrypt(model.TenantId).To<int>();
            var userId = SimpleStringCipher.Instance.Decrypt(model.UserId).To<long>();

            _unitOfWorkManager.Current.SetTenantId(tenantId);

            var user = await _userManager.GetUserByIdAsync(userId);

            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != model.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }
            var setting = await SettingManager.GetSettingValueForUserAsync(AppSettings.Security.PasswordComplexity, tenantId, userId);
            model.PasswordComplexitySetting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(setting);

            return View(model);
        }

        [HttpPost]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordFormViewModel model)
        {
            var tenantId = model.TenantId.IsNullOrEmpty() ? (int?)null : SimpleStringCipher.Instance.Decrypt(model.TenantId).To<int>();
            var userId = Convert.ToInt64(SimpleStringCipher.Instance.Decrypt(model.UserId));

            _unitOfWorkManager.Current.SetTenantId(tenantId);

            var user = await _userManager.GetUserByIdAsync(userId);

            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != model.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }
            user.Password = new PasswordHasher().HashPassword(model.Password);
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;
            user.ShouldChangePasswordOnNextLogin = false;

            await _userManager.UpdateAsync(user);

            if (user.IsActive)
            {
                await SignInAsync(user);
            }
            return RedirectToAction("Index", "Application");
        }

        public ActionResult EmailActivation()
        {
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            ViewBag.TenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull();

            return View();
        }

        [HttpPost]
        public virtual async Task<JsonResult> SendEmailActivationLink(SendEmailActivationLinkViewModel model)
        {
            var tenantId = await GetTenantIdOrDefault(model.TenancyName);

            UnitOfWorkManager.Current.SetTenantId(tenantId);

            var user = await GetUserByChecking(model.EmailAddress);

            user.SetNewEmailConfirmationCode();
            await _userEmailer.SendEmailActivationLinkAsync(user);

            return Json(new AjaxResponse());
        }

        public virtual async Task<ActionResult> EmailConfirmation(EmailConfirmationViewModel model)
        {
            var tenantId = model.TenantId.IsNullOrEmpty() ? (int?)null : SimpleStringCipher.Instance.Decrypt(model.TenantId).To<int>();
            var userId = Convert.ToInt64(SimpleStringCipher.Instance.Decrypt(model.UserId));

            _unitOfWorkManager.Current.SetTenantId(tenantId);

            var user = await _userManager.GetUserByIdAsync(userId);

            if (user == null || user.EmailConfirmationCode.IsNullOrEmpty() || user.EmailConfirmationCode != model.ConfirmationCode)
            {
                throw new UserFriendlyException(L("InvalidEmailConfirmationCode"), L("InvalidEmailConfirmationCode_Detail"));
            }
            user.IsEmailConfirmed = true;
            user.EmailConfirmationCode = null;

            await _userManager.UpdateAsync(user);

            var tenancyName = user.TenantId.HasValue
                ? (await _tenantManager.GetByIdAsync(user.TenantId.Value)).TenancyName
                : "";

            return RedirectToAction(
                "Login",
                new
                {
                    successMessage = L("YourEmailIsConfirmedMessage"),
                    tenancyName = tenancyName,
                    userNameOrEmailAddress = user.UserName
                });
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // 要求用户已通过使用用户名/密码或外部登录名登录
            if (!await _signInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
            // 如果用户输入错误代码的次数达到指定的次数，则会将
            // 该用户帐户锁定指定的时间。
            // 可以在 IdentityConfig 中配置帐户锁定设置
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "代码无效。");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View();
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // 生成令牌并发送该令牌
            if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //[ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(
                provider,
                Url.Action(
                    "ExternalLoginCallback",
                    "Account",
                    new
                    {
                        ReturnUrl = returnUrl
                    })
                );
        }

        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl, string tenancyName = "")
        {
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
           
            //Try to find tenancy name
            if (tenancyName.IsNullOrEmpty())
            {
                var tenants = await FindPossibleTenantsOfUserAsync(loginInfo.Login);
                
                switch (tenants.Count)
                {
                    case 0:
                        tenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull();

                        if (string.IsNullOrEmpty(tenancyName))
                        {
                            return await RegisterView(loginInfo);
                        }
                        break;
                    case 1:
                        tenancyName = tenants[0].TenancyName;
                        break;
                    default:
                        return View("TenantSelection", new TenantSelectionViewModel
                        {
                            Action = Url.Action("ExternalLoginCallback", "Account", new { returnUrl }),
                            Tenants = tenants.MapTo<List<TenantSelectionViewModel.TenantInfo>>()
                        });
                }
            }
            var loginResult = await _logInManager.LoginAsync(loginInfo.Login, tenancyName);

            switch (loginResult.Result)
            {
                case LoginResultType.Success:
                    await SignInAsync(loginResult.User, loginResult.Identity, false);
                    await UpdateUser(loginInfo,tenancyName);

                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        returnUrl = Url.Action("Index", "Home");
                    }
                    return Redirect(returnUrl);
                case LoginResultType.UnknownExternalLogin:
                    return await RegisterView(loginInfo, tenancyName);
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, loginInfo.Email ?? loginInfo.DefaultUserName, tenancyName);
            }
        }

        private async Task UpdateUser(ExternalLoginInfo loginInfo,string tenancyName)
        {
            string nickName= loginInfo.ExternalIdentity.Claims.Where(c => c.Type == CommonClaimType.NickName).FirstOrDefault().Value;
            string avatar = loginInfo.ExternalIdentity.Claims.Where(model => model.Type == CommonClaimType.Avatar).FirstOrDefault().Value;

            if (!string.IsNullOrEmpty(nickName) || !string.IsNullOrEmpty(avatar))
            {
                Tenant tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
                var user = await _userManager.Store.FindAsync(tenant.Id, loginInfo.Login);
                user.NickName = loginInfo.ExternalIdentity.Claims.Where(c => c.Type == CommonClaimType.NickName).FirstOrDefault().Value;
                user.Avatar = loginInfo.ExternalIdentity.Claims.Where(model => model.Type == CommonClaimType.Avatar).FirstOrDefault().Value;
                await _userManager.Store.UpdateAsync(user);
            }
        }

        private async Task<ActionResult> RegisterView(ExternalLoginInfo loginInfo, string tenancyName = null)
        {
            var name = loginInfo.DefaultUserName;
            var surname = loginInfo.DefaultUserName;

            var viewModel = new RegisterViewModel
            {
                TenancyName = tenancyName,
                EmailAddress = loginInfo.Email,
                Name = loginInfo.ExternalIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value,
                Surname = loginInfo.ExternalIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value,
                NickName = loginInfo.ExternalIdentity.Claims.Where(c => c.Type == CommonClaimType.NickName).FirstOrDefault().Value,
                Avatar =loginInfo.ExternalIdentity.Claims.Where(model=>model.Type==CommonClaimType.Avatar).FirstOrDefault().Value,
                IsExternalLogin = true
            };

            if (!tenancyName.IsNullOrEmpty())
            {
                return await Register(viewModel);
            }
            return RegisterView(viewModel);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}