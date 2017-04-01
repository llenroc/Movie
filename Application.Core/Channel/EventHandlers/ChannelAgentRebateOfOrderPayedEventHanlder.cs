using Application.Channel.ChannelAgents;
using Application.Orders.Entities;
using Application.Orders.Events;
using Infrastructure.Configuration;
using Infrastructure.Dependency;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Event.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Distributions.EventHandlers
{
    public class ChannelAgentRebateOfOrderPayedEventHanlder : IEventHandler<OrderPayedEventData>, ITransientDependency
    {
        public ChannelAgentManager ChannelAgentManager { get; set; }

        [UnitOfWork]
        public void HandleEvent(OrderPayedEventData eventData)
        {
           
        }
    }
}
