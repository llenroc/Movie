using Infrastructure.JetBrains.Annotations;

namespace Infrastructure.RealTime
{
    public static class OnlineClientExtensions
    {
        [CanBeNull]
        public static UserIdentifier ToUserIdentifierOrNull(this IOnlineClient onlineClient)
        {
            return onlineClient.UserId.HasValue ? new UserIdentifier(onlineClient.TenantId, onlineClient.UserId.Value): null;
        }
    }
}
