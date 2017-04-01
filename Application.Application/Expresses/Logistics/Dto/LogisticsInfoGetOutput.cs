using Application.Expresses.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Expresses.Logistics.Dto
{
    public class LogisticsInfoGetOutput
    {
        public ExpressCompanyDto ExpressCompany { get; set; }
        public string Logistics { get; set; }
    }
}
