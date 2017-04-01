using Application.Orders.Fronts.ChannelAgencyApplys.Dto;
using Application.Orders.Fronts.Dto;
using Infrastructure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Fronts.ChannelAgencyApplys
{
    public interface IChannelAgencyApplyOrderAppService:IApplicationService
    {
        Task<ChannelAgencyApplyOrderDto> CreateOrder(ChannelAgencyApplyOrderCreateInput input);
    }
}
