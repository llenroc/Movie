using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;
using Microsoft.AspNet.Identity;

namespace Infrastructure.Authorization.Roles
{
    /// <summary>
    /// Base class for role.
    /// </summary>
    [Table("Role")]
    public abstract class RoleBase : FullAuditedEntity<int>, IRole<int>, IMayHaveTenant
    {
        /// <summary>
        /// Maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// Tenant's Id, if this role is a tenant-level role. Null, if not.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Unique name of this role.
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }
    }
}