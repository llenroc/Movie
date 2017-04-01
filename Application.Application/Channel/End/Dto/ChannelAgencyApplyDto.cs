using Application.Channel.ChannelAgencies;
using Application.Group.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.End.Dto
{
    [AutoMap(typeof(ChannelAgencyApply))]
    public class ChannelAgencyApplyDto: FullAuditedEntityDto
    {
        public int OrderId { get; set; }

        public long UserId { get; set; }

        public CustomerDto User { get; set; }

        public int ChannelAgentId { get; set; }

        public ChannelAgentDto ChannelAgent { get; set; }

        public ChannelAgencyApplyStatus Status { get; set; }
    }
}
