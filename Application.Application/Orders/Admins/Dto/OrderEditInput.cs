using Application.Orders.Entities;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Admins.Dto
{
    [AutoMapTo(typeof(Order))]
    public class OrderEditInput:EntityDto
    {
        public decimal PayMoney { get; set; }
    }
}
