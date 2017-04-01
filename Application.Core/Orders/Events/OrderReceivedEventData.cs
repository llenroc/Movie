using Application.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Events
{
    [Serializable]
    public class OrderReceivedEventData : OrderEventData
    {
        public OrderReceivedEventData(Order order):base(order)
        {
        }
    }
}
