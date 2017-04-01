﻿using Application.Orders.Entities;
using Infrastructure.Event.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Events
{
    [Serializable]
    public class OrderPayedEventData : OrderEventData
    {

        public OrderPayedEventData(Order order):base(order)
        {
        }
    }
}
