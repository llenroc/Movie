﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Fronts.Dto
{
    public class OrderItemOutput
    {
        public int SpecificationId { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        public decimal Money { get; set; }

        public int? CartItemId { get; set; }
    }
}
