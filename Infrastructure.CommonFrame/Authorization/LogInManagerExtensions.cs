using Infrastructure.Authorization.Roles;
using Infrastructure.Authorization.Users;
using Infrastructure.MultiTenancy;
using Infrastructure.Threading;

namespace Infrastructure.Authorization
{
    public static class LogInManagerExtensions
    {
        public static LoginResult<TTenant, TUser> Login<TTenant, TRole, TUser>(
            this LogInManager<TTenant, TRole, TUser> logInManager,
            string userNameOrEmailAddress,
            string plainPassword,
            string tenancyName = null,
            bool shouldLockout = true)
                where TTenant : CommonFrameTenant<TUser>
                where TRole : CommonFrameRole<TUser>, new()
                where TUser : CommonFrameUser<TUser>
        {
            return AsyncHelper.RunSync(
                () => logInManager.LoginAsync(
                    userNameOrEmailAddress,
                    plainPassword,
                    tenancyName,
                    shouldLockout
                )
            );
        }
    }
}
