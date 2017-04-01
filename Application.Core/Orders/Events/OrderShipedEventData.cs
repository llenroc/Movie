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
    public class OrderShipedEventData : OrderEventData
    {
        public ExpressInfo ExpressInfo { get; set; }

        public OrderShipedEventData(Order order, ExpressInfo expressInfo=null) :base(order)
        {
            ExpressInfo = expressInfo;
        }
    }
}
