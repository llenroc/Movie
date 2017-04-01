using Application.Channel.ChannelAgencies;
using Application.Channel.ChannelAgents;
using Application.Orders.BoughtContexts;
using Application.Orders.Entities;
using Application.Orders.Fronts.ChannelAgencyApplys.Dto;
using Application.Orders.Fronts.Dto;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Fronts.ChannelAgencyApplys
{
    public class ChannelAgencyApplyOrderAppService : ApplicationAppServiceBase, IChannelAgencyApplyOrderAppService
    {
        public ChannelAgencyApplyOrderManager OrderManager { get; set; }
        public IRepository<ChannelAgencyApply> ChannelAgencyApplyRepository { get; set; }
        public IRepository<ChannelAgent> ChannelAgentRepository { get; set; }


        public async Task<ChannelAgencyApplyOrderDto> CreateOrder(ChannelAgencyApplyOrderCreateInput input)
        {
            ChannelAgencyApplyBoughtContext boughtContext = new ChannelAgencyApplyBoughtContext()
            {
                ChannelAgent = ChannelAgentRepository.Get(input.ChannelAgentId)
            };
            ChannelAgencyApplyOrder order =await OrderManager.CreateOrder(boughtContext);
            return ObjectMapper.Map<ChannelAgencyApplyOrderDto>(order);
        }
    }
}
