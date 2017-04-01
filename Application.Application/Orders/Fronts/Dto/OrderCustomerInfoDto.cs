using Application.Orders.Entities;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Fronts.Dto
{
    [AutoMap(typeof(OrderCustomerInfo))]
    public class OrderCustomerInfoDto
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
