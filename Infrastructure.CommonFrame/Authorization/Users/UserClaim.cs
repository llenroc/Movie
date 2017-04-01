using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;

namespace Infrastructure.Authorization.Users
{
    [Table("UserClaim")]
    public class UserClaim : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public virtual long UserId { get; set; }

        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }

        public UserClaim()
        {

        }

        public UserClaim(UserBase user, Claim claim)
        {
            TenantId = user.TenantId;
            UserId = user.Id;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
