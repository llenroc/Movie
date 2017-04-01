using System;
using Infrastructure.Authorization.Users;

namespace Infrastructure.Runtime.Session
{
    public static class SessionExtensions
    {
        public static bool IsUser(this IInfrastructureSession session, UserBase user)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return session.TenantId == user.TenantId &&session.UserId.HasValue && session.UserId.Value == user.Id;
        }
    }
}
