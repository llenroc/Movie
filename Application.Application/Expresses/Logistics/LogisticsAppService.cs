using Application.Expresses.Dto;
using Application.Expresses.Logistics.Dto;
using Application.Orders.Entities;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Expresses.Logistics
{
    public class LogisticsAppService:ApplicationAppServiceBase, ILogisticsAppService
    {
        public LogisticsManager LogisticsManager { get; set; }
        public IRepository<ExpressCompany> ExpressCompanyRepository { get; set; }

        public LogisticsInfoGetOutput GetExpresLogisticss(ExpressInfo expressInfo)
        {
            LogisticsInfoGetOutput logisticsInfoGetOutput = new LogisticsInfoGetOutput()
            {
                Logistics = LogisticsManager.GetExpressLogisticss(expressInfo),
                ExpressCompany= ExpressCompanyRepository.Get(expressInfo.ExpressCompanyId).MapTo<ExpressCompanyDto>()
            };
            return logisticsInfoGetOutput;
        }
    }
}
