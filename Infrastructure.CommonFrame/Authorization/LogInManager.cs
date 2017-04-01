using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Infrastructure.Auditing;
using Infrastructure.Authorization.Roles;
using Infrastructure.Authorization.Users;
using Infrastructure.Configuration;
using Infrastructure.Configuration.Startup;
using Infrastructure.Dependency;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Extensions;
using Infrastructure.IdentityFramework;
using Infrastructure.MultiTenancy;
using Infrastructure.Timing;
using Infrastructure.CommonFrame.Configuration;
using Microsoft.AspNet.Identity;

namespace Infrastructure.Authorization
{

    //SignInManager<TUser, long>,
    public class LogInManager<TTenant, TRole, TUser> : ITransientDependency
        where TTenant : CommonFrameTenant<TUser>
        where TRole : CommonFrameRole<TUser>, new()
        where TUser : CommonFrameUser<TUser>
    {
        public IClientInfoProvider ClientInfoProvider { get; set; }

        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IRepository<TTenant> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly CommonFrameUserManager<TRole, TUser> _userManager;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<UserLoginAttempt, long> _userLoginAttemptRepository;
        private readonly IUserManagementConfig _userManagementConfig;
        private readonly IIocResolver _iocResolver;
        private readonly CommonFrameRoleManager<TRole, TUser> _roleManager;

        public LogInManager(
            CommonFrameUserManager<TRole, TUser> userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<TTenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            CommonFrameRoleManager<TRole, TUser> roleManager)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
            _userLoginAttemptRepository = userLoginAttemptRepository;
            _userManagementConfig = userManagementConfig;
            _iocResolver = iocResolver;
            _roleManager = roleManager;
            _userManager = userManager;

            ClientInfoProvider = NullClientInfoProvider.Instance;
        }

        [UnitOfWork]
        public virtual async Task<LoginResult<TTenant, TUser>> LoginAsync(UserLoginInfo login, string tenancyName = null)
        {
            var result = await LoginAsyncInternal(login, tenancyName);
            await SaveLoginAttempt(result, tenancyName, login.ProviderKey + "@" + login.LoginProvider);
            return result;
        }

        protected virtual async Task<LoginResult<TTenant, TUser>> LoginAsyncInternal(UserLoginInfo login, string tenancyName)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");
            }

            //Get and check tenant
            TTenant tenant = null;

            if (!_multiTenancyConfig.IsEnabled)
            {
                tenant = await GetDefaultTenantAsync();
            }
            else if (!string.IsNullOrWhiteSpace(tenancyName))
            {
                tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);

                if (tenant == null)
                {
                    return new LoginResult<TTenant, TUser>(LoginResultType.InvalidTenancyName);
                }

                if (!tenant.IsActive)
                {
                    return new LoginResult<TTenant, TUser>(LoginResultType.TenantIsNotActive, tenant);
                }
            }
            int? tenantId = tenant == null ? (int?)null : tenant.Id;

            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var user = await _userManager.Store.FindAsync(tenantId, login);

                if (user == null)
                {
                    return new LoginResult<TTenant, TUser>(LoginResultType.UnknownExternalLogin, tenant);
                }
                return await CreateLoginResultAsync(user, tenant);
            }
        }

        [UnitOfWork]
        public virtual async Task<LoginResult<TTenant, TUser>> LoginAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null, bool shouldLockout = true)
        {
            var result = await LoginAsyncInternal(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
            await SaveLoginAttempt(result, tenancyName, userNameOrEmailAddress);
            return result;
        }

        protected virtual async Task<LoginResult<TTenant, TUser>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            //Get and check tenant
            TTenant tenant = null;
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                if (!_multiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);

                    if (tenant == null)
                    {
                        return new LoginResult<TTenant, TUser>(LoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new LoginResult<TTenant, TUser>(LoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }
            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                //TryLoginFromExternalAuthenticationSources method may create the user, that's why we are calling it before Store.FindByNameOrEmailAsync
                var loggedInFromExternalSource = await TryLoginFromExternalAuthenticationSources(userNameOrEmailAddress, plainPassword, tenant);

                var user = await _userManager.Store.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);

                if (user == null)
                {
                    return new LoginResult<TTenant, TUser>(LoginResultType.InvalidUserNameOrEmailAddress, tenant);
                }

                if (!loggedInFromExternalSource)
                {
                    _userManager.InitializeLockoutSettings(tenantId);

                    if (await _userManager.IsLockedOutAsync(user.Id))
                    {
                        return new LoginResult<TTenant, TUser>(LoginResultType.LockedOut, tenant, user);
                    }
                    var verificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user.Password, plainPassword);

                    if (verificationResult != PasswordVerificationResult.Success)
                    {
                        if (shouldLockout)
                        {
                            if (await TryLockOutAsync(tenantId, user.Id))
                            {
                                return new LoginResult<TTenant, TUser>(LoginResultType.LockedOut, tenant, user);
                            }
                        }
                        return new LoginResult<TTenant, TUser>(LoginResultType.InvalidPassword, tenant, user);
                    }
                    await _userManager.ResetAccessFailedCountAsync(user.Id);
                }
                return await CreateLoginResultAsync(user, tenant);
            }
        }

        protected virtual async Task<LoginResult<TTenant, TUser>> CreateLoginResultAsync(TUser user, TTenant tenant = null)
        {
            if (!user.IsActive)
            {
                return new LoginResult<TTenant, TUser>(LoginResultType.UserIsNotActive);
            }

            if (await IsEmailConfirmationRequiredForLoginAsync(user.TenantId) && !user.IsEmailConfirmed)
            {
                return new LoginResult<TTenant, TUser>(LoginResultType.UserEmailIsNotConfirmed);
            }
            user.LastLoginTime = Clock.Now;

            await _userManager.Store.UpdateAsync(user);
            await _unitOfWorkManager.Current.SaveChangesAsync();

            return new LoginResult<TTenant, TUser>(
                tenant,
                user,
                await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie)
            );
        }

        protected virtual async Task SaveLoginAttempt(LoginResult<TTenant, TUser> loginResult, string tenancyName, string userNameOrEmailAddress)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var tenantId = loginResult.Tenant != null ? loginResult.Tenant.Id : (int?)null;
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var loginAttempt = new UserLoginAttempt
                    {
                        TenantId = tenantId,
                        TenancyName = tenancyName,

                        UserId = loginResult.User != null ? loginResult.User.Id : (long?)null,
                        UserNameOrEmailAddress = userNameOrEmailAddress,

                        Result = loginResult.Result,

                        BrowserInfo = ClientInfoProvider.BrowserInfo,
                        ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                        ClientName = ClientInfoProvider.ComputerName,
                    };
                    await _userLoginAttemptRepository.InsertAsync(loginAttempt);
                    await _unitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();
                }
            }
        }

        protected virtual async Task<bool> TryLockOutAsync(int? tenantId, long userId)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    (await _userManager.AccessFailedAsync(userId)).CheckErrors();

                    var isLockOut = await _userManager.IsLockedOutAsync(userId);

                    await _unitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();

                    return isLockOut;
                }
            }
        }

        protected virtual async Task<bool> TryLoginFromExternalAuthenticationSources(string userNameOrEmailAddress, string plainPassword, TTenant tenant)
        {
            if (!_userManagementConfig.ExternalAuthenticationSources.Any())
            {
                return false;
            }

            foreach (var sourceType in _userManagementConfig.ExternalAuthenticationSources)
            {
                using (var source = _iocResolver.ResolveAsDisposable<IExternalAuthenticationSource<TTenant, TUser>>(sourceType))
                {
                    if (await source.Object.TryAuthenticateAsync(userNameOrEmailAddress, plainPassword, tenant))
                    {
                        var tenantId = tenant == null ? (int?)null : tenant.Id;
                        using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                        {
                            var user = await _userManager.Store.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);

                            if (user == null)
                            {
                                user = await source.Object.CreateUserAsync(userNameOrEmailAddress, tenant);

                                user.TenantId = tenantId;
                                user.AuthenticationSource = source.Object.Name;
                                user.Password = _userManager.PasswordHasher.HashPassword(Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used

                                user.Roles = new List<UserRole>();

                                foreach (var defaultRole in _roleManager.Roles.Where(r => r.TenantId == tenantId && r.IsDefault).ToList())
                                {
                                    user.Roles.Add(new UserRole(tenantId, user.Id, defaultRole.Id));
                                }
                                await _userManager.Store.CreateAsync(user);
                            }
                            else
                            {
                                await source.Object.UpdateUserAsync(user, tenant);

                                user.AuthenticationSource = source.Object.Name;

                                await _userManager.Store.UpdateAsync(user);
                            }

                            await _unitOfWorkManager.Current.SaveChangesAsync();

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected virtual async Task<TTenant> GetDefaultTenantAsync()
        {
            var tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == CommonFrameTenant<TUser>.DefaultTenantName);

            if (tenant == null)
            {
                throw new Exception("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }
            return tenant;
        }

        protected virtual async Task<bool> IsEmailConfirmationRequiredForLoginAsync(int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return await _settingManager.GetSettingValueForTenantAsync<bool>(CommonFrameSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, tenantId.Value);
            }
            return await _settingManager.GetSettingValueForApplicationAsync<bool>(CommonFrameSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
        }
    }
}
