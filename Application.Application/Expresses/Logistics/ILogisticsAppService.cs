using Application.Expresses.Logistics.Dto;
using Application.Orders.Entities;
using Infrastructure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Expresses.Logistics
{
    public interface ILogisticsAppService:IApplicationService
    {
        LogisticsInfoGetOutput GetExpresLogisticss(ExpressInfo expressInfo);
    }
}
