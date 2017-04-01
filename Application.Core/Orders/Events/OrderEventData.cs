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
    public class OrderEventData : EventData, IEventDataWithInheritableGenericArgument 
    {
        public Order Order { get; private set; }

        public OrderEventData(Order order)
        {
            Order = order;
        }

        public virtual object[] GetConstructorArgs()
        {
            return new object[] { Order };
        }
    }
}
