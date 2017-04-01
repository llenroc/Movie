using Infrastructure;

namespace Application.Entities
{
    public class UserIdentifierEntity:IUserIdentifierEntity
    {
        public int TenantId { get; set; }
        public long UserId { get; set; }

        public UserIdentifier GetUserIdentifier()
        {
            return new UserIdentifier(TenantId,UserId);
        }
    }
}
