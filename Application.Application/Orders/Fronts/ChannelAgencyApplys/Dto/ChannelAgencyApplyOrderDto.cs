using Application.Orders.Entities;
using Application.Orders.Fronts.Common.Dto;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Fronts.ChannelAgencyApplys.Dto
{
    [AutoMapFrom(typeof(ChannelAgencyApplyOrder))]
    public class ChannelAgencyApplyOrderDto : OrderDto
    {
    }
}
