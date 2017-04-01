using Application.Channel.ChannelAgencies;
using Application.Channel.End.Dto;
using Infrastructure.Application.Services;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Application.DTO;
using Application.Channel.ChananlAgencys;

namespace Application.Channel.End
{
    public class ChannelAgencyApplyForEndAppService : CrudAppService<ChannelAgencyApply, ChannelAgencyApplyDto>, 
        IChannelAgencyApplyForEndAppService
    {
        public ChannelAgencyManager ChannelAgencyManager { get; set; }

        public ChannelAgencyApplyForEndAppService(IRepository<ChannelAgencyApply> respository) :base(respository)
        {

        }

        public async Task PassChannelAgencyApply(IdInput input)
        {
            ChannelAgencyApply channelAgencyApply = Repository.Get(input.Id);
            ChannelAgencyManager.PassChannelAgencyApply(channelAgencyApply);        }
    }
}
