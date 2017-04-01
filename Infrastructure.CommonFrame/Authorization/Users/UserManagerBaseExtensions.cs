using System;
using Infrastructure.Authorization.Roles;
using Infrastructure.MultiTenancy;
using Infrastructure.Threading;

namespace Infrastructure.Authorization.Users
{
    /// <summary>
    /// Extension methods for <see cref="UserManager{TRole,TUser}"/>.
    /// </summary>
    public static class UserManagerExtensions
    {
        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="manager">User manager</param>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public static bool IsGranted<TRole, TUser>(CommonFrameUserManager<TRole, TUser> manager, long userId, string permissionName) where TRole : CommonFrameRole<TUser>, new() where TUser : CommonFrameUser<TUser>
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            return AsyncHelper.RunSync(() => manager.IsGrantedAsync(userId, permissionName));
        }

        //public static UserManager<TRole, TUser> Login<TRole, TUser>(UserManager<TRole, TUser> manager, string userNameOrEmailAddress, string plainPassword, string tenancyName = null)
        //    where TRole : Role<TUser>, new()
        //    where TUser : User<TUser>
        //{
        //    if (manager == null)
        //    {
        //        throw new ArgumentNullException(nameof(manager));
        //    }

        //    return AsyncHelper.RunSync(() => manager.LoginAsync(userNameOrEmailAddress, plainPassword, tenancyName));
        //}
    }
}
