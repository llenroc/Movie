using Infrastructure.Domain.Entities.Auditing;
using Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Application.Authorization.Users;
using Infrastructure.Extensions;

namespace Application.Members
{
    public class MemberCard: AuditedEntity,IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual MemberLevel Level { get; set; }

        [Required]
        [MaxLength(20)]
        public string No { get; set; }

        [Required]
        public long UserId { get; set; }

        public virtual User User { get; set; }

        public long? Expiry { get; set; }

        public DateTime? LimitTime { get; set; }

        public static string CreateNo()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }
    }
}
