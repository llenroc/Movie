using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Infrastructure.Authorization.Roles;
using Infrastructure.Authorization.Users;
using Infrastructure.Configuration;
using Infrastructure.Dependency;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Extensions;
using Infrastructure.MultiTenancy;
using Infrastructure.Runtime.Security;
using Infrastructure.CommonFrame.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Infrastructure.Authorization
{
    public abstract class CommonFrameSignInManager<TTenant, TRole, TUser> : SignInManager<TUser, long>, ITransientDependency
          where TTenant : CommonFrameTenant<TUser>
          where TRole : CommonFrameRole<TUser>, new()
          where TUser : CommonFrameUser<TUser>
    {
        private readonly ISettingManager _settingManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        protected CommonFrameSignInManager(
            CommonFrameUserManager<TRole, TUser> userManager,
            IAuthenticationManager authenticationManager,
            ISettingManager settingManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(userManager, authenticationManager)
        {
            _settingManager = settingManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// This method can return two results:
        /// <see cref="SignInStatus.Success"/> indicates that user has successfully signed in.
        /// <see cref="SignInStatus.RequiresVerification"/> indicates that user has successfully signed in.
        /// </summary>
        /// <param name="loginResult">The login result received from <see cref="LogInManager{TTenant,TRole,TUser}"/> Should be Success.</param>
        /// <param name="isPersistent">True to use persistent cookie.</param>
        /// <param name="rememberBrowser">Remember user's browser (and not use two factor auth again) or not.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">loginResult.Result should be success in order to sign in!</exception>
        [UnitOfWork]
        public virtual async Task<SignInStatus> SignInOrTwoFactor(LoginResult<TTenant, TUser> loginResult, bool isPersistent, bool? rememberBrowser = null)
        {
            if (loginResult.Result != LoginResultType.Success)
            {
                throw new ArgumentException("loginResult.Result should be success in order to sign in!");
            }

            using (_unitOfWorkManager.Current.SetTenantId(loginResult.Tenant?.Id))
            {
                if (IsTrue(CommonFrameSettingNames.UserManagement.TwoFactorLogin.IsEnabled, loginResult.Tenant?.Id))
                {
                    UserManager.As<CommonFrameUserManager<TRole, TUser>>().RegisterTwoFactorProviders(loginResult.Tenant?.Id);

                    if (await UserManager.GetTwoFactorEnabledAsync(loginResult.User.Id))
                    {
                        if ((await UserManager.GetValidTwoFactorProvidersAsync(loginResult.User.Id)).Count > 0)
                        {
                            if (!await AuthenticationManager.TwoFactorBrowserRememberedAsync(loginResult.User.Id.ToString()) ||
                                rememberBrowser == false)
                            {
                                var claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.TwoFactorCookie);

                                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginResult.User.Id.ToString()));

                                if (loginResult.Tenant != null)
                                {
                                    claimsIdentity.AddClaim(new Claim(InfrastructureClaimTypes.TenantId, loginResult.Tenant.Id.ToString()));
                                }
                                AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);
                                return SignInStatus.RequiresVerification;
                            }
                        }
                    }
                }
                SignIn(loginResult, isPersistent, rememberBrowser);
                return SignInStatus.Success;
            }
        }

        /// <param name="loginResult">The login result received from <see cref="LogInManager{TTenant,TRole,TUser}"/> Should be Success.</param>
        /// <param name="isPersistent">True to use persistent cookie.</param>
        /// <param name="rememberBrowser">Remember user's browser (and not use two factor auth again) or not.</param>
        [UnitOfWork]
        public virtual void SignIn(LoginResult<TTenant, TUser> loginResult, bool isPersistent, bool? rememberBrowser = null)
        {
            if (loginResult.Result != LoginResultType.Success)
            {
                throw new ArgumentException("loginResult.Result should be success in order to sign in!");
            }

            using (_unitOfWorkManager.Current.SetTenantId(loginResult.Tenant?.Id))
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie,DefaultAuthenticationTypes.TwoFactorCookie);

                if (rememberBrowser == null)
                {
                    rememberBrowser = IsTrue(CommonFrameSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled, loginResult.Tenant?.Id);
                }

                if (rememberBrowser == true)
                {
                    var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(loginResult.User.Id.ToString());
                    AuthenticationManager.SignIn(
                        new AuthenticationProperties
                        {
                            IsPersistent = isPersistent
                        },
                        loginResult.Identity,
                        rememberBrowserIdentity
                    );
                }
                else
                {
                    AuthenticationManager.SignIn(
                        new AuthenticationProperties
                        {
                            IsPersistent = isPersistent
                        },
                        loginResult.Identity
                    );
                }
            }
        }

        public virtual async Task<int?> GetVerifiedTenantIdAsync()
        {
            var authenticateResult = await AuthenticationManager.AuthenticateAsync( DefaultAuthenticationTypes.TwoFactorCookie );

            return authenticateResult?.Identity?.GetTenantId();
        }

        private bool IsTrue(string settingName, int? tenantId)
        {
            return tenantId == null ? _settingManager.GetSettingValueForApplication<bool>(settingName) : _settingManager.GetSettingValueForTenant<bool>(settingName, tenantId.Value);
        }
    }
}
