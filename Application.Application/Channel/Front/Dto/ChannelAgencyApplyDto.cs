using Application.Channel.ChannelAgencies;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.Front.Dto
{
    [AutoMap(typeof(ChannelAgencyApply))]
    public class ChannelAgencyApplyDto
    {
        public int OrderId { get; set; }

        public long UserId { get; set; }

        public int ChannelAgentId { get; set; }

        public ChannelAgencyApplyStatus Status { get; set; }
    }
}
