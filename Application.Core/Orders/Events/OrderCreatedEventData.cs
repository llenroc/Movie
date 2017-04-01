using Application.Orders.BoughtContexts;
using Application.Orders.Entities;
using Infrastructure.Event.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Events
{
    [Serializable]
    public class OrderCreatedEventData : OrderEventData
    {
        public int ProductCount { get; set; }

        public OrderCreatedEventData(Order order,int productCount) :base(order)
        {
            ProductCount = productCount;
        }
    }
}
