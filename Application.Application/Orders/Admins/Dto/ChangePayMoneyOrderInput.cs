using Application.Orders.Entities;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Admins.Dto
{
    [AutoMap(typeof(Order))]
    public class ChangePayMoneyOrderInput : EntityDto
    {
        [Required]
        public decimal PayMoney { get; set; }
    }
}
