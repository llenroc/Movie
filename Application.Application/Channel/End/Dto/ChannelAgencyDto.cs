using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using Application.Channel.ChannelAgencies;
using Application.Channel.ChannelAgents;
using Application.Group.Dto;

namespace Application.Channel.End.Dto
{
    [AutoMap(typeof(ChannelAgency))]
    public class ChannelAgencyDto:FullAuditedEntityDto
    {
        public int ChannelAgentId { get; set; }

        public ChannelAgentDto ChannelAgent { get; set; }

        public int Depth { get; set; }

        public int ChildCount { get; set; }

        public long UserId { get; set; }

        public CustomerDto User { get; set; }

        public float RebateRatio { get; set; }
    }
}
