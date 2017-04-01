using Application.Authorization.Users;
using Application.Channel.ChannelAgents;
using Application.Entities;
using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.ChannelAgencies
{
    public class ChannelAgency : FullAuditedEntity, IUserIdentifierEntity
    {
        public int TenantId { get; set; }

        [Required]
        public int ChannelAgentId { get; set; }

        public virtual ChannelAgent ChannelAgent { get; set; }

        public int Depth { get; set; }

        public int ChildCount { get; set; }

        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get;set;}

        public float RebateRatio { get; set; }
    }
}
