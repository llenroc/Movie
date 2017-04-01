using Application.Authorization.Users;
using Application.Channel.ChannelAgents;
using Application.Entities;
using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.ChannelAgencies
{
    public enum ChannelAgencyApplyStatus
    {
        Applying,
        Success,
        Fail
    }

    public class ChannelAgencyApply : FullAuditedEntity, IUserIdentifierEntity
    {
        public int TenantId { get; set; }

        public int OrderId { get; set; }

        [Required]
        public long UserId { get; set; }

        public virtual User User { get;set;}

        [Required]
        public int ChannelAgentId { get; set; }

        public virtual ChannelAgent ChannelAgent { get; set; }

        public ChannelAgencyApplyStatus Status { get; set; }
    }
}
