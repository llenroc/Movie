using System.Security.Claims;
using Infrastructure.MultiTenancy;

namespace Infrastructure.Authorization.Users
{
    public class LoginResult<TTenant, TUser> where TTenant : CommonFrameTenant<TUser> where TUser : CommonFrameUser<TUser>
    {
        public LoginResultType Result { get; private set; }

        public TTenant Tenant { get; private set; }

        public TUser User { get; private set; }

        public ClaimsIdentity Identity { get; private set; }

        public LoginResult(LoginResultType result, TTenant tenant = null, TUser user = null)
        {
            Result = result;
            Tenant = tenant;
            User = user;
        }

        public LoginResult(TTenant tenant, TUser user, ClaimsIdentity identity) : this(LoginResultType.Success, tenant)
        {
            User = user;
            Identity = identity;
        }
    }
}
