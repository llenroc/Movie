using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;
using Application.Entities;

namespace Application.Wallets.Entities
{
    public class Wallet:FullAuditedEntity, IUserIdentifierEntity
    {
        public int TenantId { get; set; }

        public long UserId { get; set; }

        public string PayPassword { get; set; }

        public decimal Money { get; set; }

        public string Name { get; set; }
    }
}
