using Application.Expresses.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Admins.Dto
{
    public class OrderForShipOutput
    {
        public OrderDto Order { get; set; }

        public List<ExpressCompanyDto> ExpressCompanys { get; set; }
    }
}
