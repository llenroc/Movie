using Application.Authorization;
using Application.Authorization.Roles;
using Application.Authorization.Users;
using Application.Configuration;
using Application.MultiTenancy;
using Application.Notifications;
using Application.Web;
using Application.WebSite.Authorization;
using Application.WebSite.MultiTenancy;
using Infrastructure.Configuration;
using Infrastructure.Configuration.Startup;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Extensions;
using Infrastructure.Notifications;
using Infrastructure.Runtime.Caching;
using Infrastructure.Threading;
using Infrastructure.UI;
using Infrastructure.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Application.WebSite.Controllers
{
    public class AccountControllerBase : ApplicationControllerBase
    {
        protected readonly TenantManager _tenantManager;
        protected readonly UserManager _userManager;
        protected readonly RoleManager _roleManager;
        protected readonly LogInManager _logInManager;
        protected readonly ApplicationSignInManager _signInManager;

        protected readonly IUnitOfWorkManager _unitOfWorkManager;
        protected readonly IUserEmailer _userEmailer;
        protected readonly IMultiTenancyConfig _multiTenancyConfig;
        protected readonly IWebUrlService _webUrlService;
        protected readonly ITenancyNameFinder _tenancyNameFinder;
        protected readonly ICacheManager _cacheManager;

        protected readonly IAppNotifier _appNotifier;
        protected readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountControllerBase(
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
            )
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWorkManager = unitOfWorkManager;
            _tenancyNameFinder = tenancyNameFinder;
            _multiTenancyConfig = multiTenancyConfig;
            _userEmailer = userEmailer;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _appNotifier = appNotifier;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _cacheManager = cacheManager;
            _webUrlService = webUrlService;
        }

        public async Task<JsonResult> CheckUsername(string UsernameOrEmailAddress)
        {
            User user = await _userManager.FindByNameOrEmailAsyncOfAll(UsernameOrEmailAddress);

            if (user == null)
            {
                return Json(new AjaxResponse(false));
            }
            else
            {
                return Json(new AjaxResponse(true));
            }
        }

        protected void CheckSelfRegistrationIsEnabled()
        {
            if (!IsSelfRegistrationEnabled())
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        protected bool IsSelfRegistrationEnabled()
        {
            var tenancyName = _tenancyNameFinder.GetCurrentTenancyNameOrNull();

            if (tenancyName.IsNullOrEmpty())
            {
                return true;
            }
            var tenant = AsyncHelper.RunSync(() => GetActiveTenantAsync(tenancyName));
            return SettingManager.GetSettingValueForTenant<bool>(AppSettings.UserManagement.AllowSelfRegistration, tenant.Id);
        }

        protected async Task<User> GetUserByChecking(string emailAddress)
        {
            var user = await _userManager.Users.Where(
                u => u.EmailAddress == emailAddress
                ).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidEmailAddress"));
            }
            return user;
        }

        protected async Task<int?> GetTenantIdOrDefault(string tenancyName)
        {
            return tenancyName.IsNullOrEmpty() ? InfrastructureSession.TenantId : (await GetActiveTenantAsync(tenancyName)).Id;
        }


        protected async Task<Tenant> GetActiveTenantAsync(string tenancyName)
        {
            var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);

            if (tenant == null)
            {
                throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIsNotActive", tenancyName));
            }
            return tenant;
        }

        [UnitOfWork]
        protected virtual async Task<List<Tenant>> FindPossibleTenantsOfUserAsync(UserLoginInfo login)
        {
            List<User> allUsers;

            using (_unitOfWorkManager.Current.DisableFilter(DataFilters.MayHaveTenant))
            {
                allUsers = await _userManager.FindAllAsync(login);
            }

            return allUsers
                .Where(u => u.TenantId != null)
                .Select(u => AsyncHelper.RunSync(() => _tenantManager.FindByIdAsync(u.TenantId.Value)))
                .ToList();
        }

        protected static bool TryExtractNameAndSurnameFromClaims(List<Claim> claims, ref string name, ref string surname)
        {
            string foundName = null;
            string foundSurname = null;

            var givennameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);

            if (givennameClaim != null && !givennameClaim.Value.IsNullOrEmpty())
            {
                foundName = givennameClaim.Value;
            }

            var surnameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);

            if (surnameClaim != null && !surnameClaim.Value.IsNullOrEmpty())
            {
                foundSurname = surnameClaim.Value;
            }

            if (foundName == null || foundSurname == null)
            {
                var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (nameClaim != null)
                {
                    var nameSurName = nameClaim.Value;

                    if (!nameSurName.IsNullOrEmpty())
                    {
                        var lastSpaceIndex = nameSurName.LastIndexOf(' ');

                        if (lastSpaceIndex < 1 || lastSpaceIndex > (nameSurName.Length - 2))
                        {
                            foundName = foundSurname = nameSurName;
                        }
                        else
                        {
                            foundName = nameSurName.Substring(0, lastSpaceIndex);
                            foundSurname = nameSurName.Substring(lastSpaceIndex);
                        }
                    }
                }
            }

            if (!foundName.IsNullOrEmpty())
            {
                name = foundName;
            }

            if (!foundSurname.IsNullOrEmpty())
            {
                surname = foundSurname;
            }

            return foundName != null && foundSurname != null;
        }
    }
}