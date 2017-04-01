using Infrastructure.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.ChannelAgents
{
    public class ChannelAgentRebate:FullAuditedEntity
    {
        public int OrderId { get; set; }

        public long UserId { get; set; }

        public int ChannlAgencyId { get; set; }

        public int ChannelAgentId { get; set; }

        public float RebateRatio { get; set; }

        public int Depth { get; set; }

        public decimal Money { get; set; }
    }
}
