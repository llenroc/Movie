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
    [AutoMap(typeof(OrderItem))]
    public class OrderItemDto:EntityDto
    {
        public long UserId { get; set; }

        public int SpecificationId { get; set; }

        public int? CartItemId { get; set; }

        public int Count { get; private set; }

        public decimal Price { get; set; }

        public decimal Money { get; set; }

        public virtual SpecificationForOrderItemDto Specification { get; set; }
    }
}
