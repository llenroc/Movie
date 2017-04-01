using Microsoft.AspNet.Identity;

namespace Infrastructure.Authorization.Users
{
    public interface IUserTokenProviderAccessor
    {
        IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>() where TUser : CommonFrameUser<TUser>;
    }
}
