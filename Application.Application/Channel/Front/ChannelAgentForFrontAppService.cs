using Application.Channel.ChananlAgencys;
using Application.Channel.ChannelAgents;
using Application.Channel.Front.Dto;
using Infrastructure.Application.Services;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.Front
{
    public class ChannelAgentForFrontAppService : CrudAppService<ChannelAgent, ChannelAgentDto>, IChannelAgentForFrontAppService
    {
        public ChannelAgentForFrontAppService(IRepository<ChannelAgent> respository) :base(respository)
        {

        }
    }
}
