using Infrastructure.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain.Entities;

namespace Application.Channel.ChannelAgents
{
    public class ChannelAgent: FullAuditedEntity,IMustHaveTenant
    {
        public int TenantId { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public float RebateRatio { get; set; }

        public decimal Price { get; set; }

        public string MasterQrcode { get; set; }
    }
}
