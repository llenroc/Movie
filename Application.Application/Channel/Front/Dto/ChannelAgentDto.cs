using Application.Channel.ChannelAgents;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.Front.Dto
{
    [AutoMap(typeof(ChannelAgent))]
    public class ChannelAgentDto:FullAuditedEntityDto
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public float RebateRatio { get; set; }

        public decimal Price { get; set; }

        public string MasterQrcode { get; set; }
    }
}
