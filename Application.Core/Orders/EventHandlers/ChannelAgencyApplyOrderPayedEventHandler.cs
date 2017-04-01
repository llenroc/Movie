using Application.Orders.Entities;
using Application.Orders.Events;
using Infrastructure.Dependency;
using Infrastructure.Event.Bus.Handlers;

namespace Application.Orders.EventHandlers
{
    public class ChannelAgencyApplyOrderPayedEventHandler : IEventHandler<OrderPayedEventData>, ITransientDependency
    {
        public ChannelAgencyApplyOrderManager OrderManager { get; set; }

        public void HandleEvent(OrderPayedEventData eventData)
        {
            if(eventData.Order is ChannelAgencyApplyOrder)
            {
                OrderManager.ProcessAfterPayed(eventData.Order as ChannelAgencyApplyOrder);
            }
        }
    }
}
