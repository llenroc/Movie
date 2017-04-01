using Application.Channel.ChannelAgencies;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.Front.Dto
{
    [AutoMap(typeof(ChannelAgency))]
    public class ChannelAgencyDto:FullAuditedEntityDto
    {
        public int ChannelAgentId { get; set; }

        public ChannelAgentDto ChannelAgent { get; set; }

        public int Depth { get; set; }

        public int ChildCount { get; set; }

        public long UserId { get; set; }
    }
}
