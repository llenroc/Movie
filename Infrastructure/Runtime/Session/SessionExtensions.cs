using System;

namespace Infrastructure.Runtime.Session
{
    /// <summary>
    /// Extension methods for <see cref="IInfrastructureSession"/>.
    /// </summary>
    public static class SessionExtensions
    {

        /// <summary>
        /// Gets current User's Id.
        /// Throws <see cref="Exception"/> if <see cref="IInfrastructureSession.UserId"/> is null.
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current User's Id.</returns>
        public static long GetUserId(this IInfrastructureSession session)
        {
            if (!session.UserId.HasValue)
            {
                throw new Exception("Session.UserId is null! Probably, user is not logged in.");
            }
            return session.UserId.Value;
        }

        /// <summary>
        /// Gets current Tenant's Id.
        /// Throws <see cref="Exception"/> if <see cref="IInfrastructureSession.TenantId"/> is null.
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current Tenant's Id.</returns>
        /// <exception cref="Exception"></exception>
        public static int GetTenantId(this IInfrastructureSession session)
        {
            if (!session.TenantId.HasValue)
            {
                throw new Exception("Session.TenantId is null! Possible problems: No user logged in or current logged in user in a host user (TenantId is always null for host users).");
            }
            return session.TenantId.Value;
        }

        /// <summary>
        /// Creates <see cref="UserIdentifier"/> from given session.
        /// Returns null if <see cref="IInfrastructureSession.UserId"/> is null.
        /// </summary>
        /// <param name="session">The session.</param>
        public static UserIdentifier ToUserIdentifier(this IInfrastructureSession session)
        {
            return session.UserId == null? null: new UserIdentifier(session.TenantId, session.GetUserId());
        }
    }
}
