using Infrastructure.Dependency;
using Microsoft.AspNet.Identity;

namespace Infrastructure.Authorization.Users
{
    public class NullUserTokenProviderAccessor : IUserTokenProviderAccessor, ISingletonDependency
    {
        public IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>() where TUser : CommonFrameUser<TUser>
        {
            return null;
        }
    }
}
