using Application.Orders.Entities;
using Application.Orders.Events;
using Infrastructure.Dependency;
using Infrastructure.Event.Bus.Handlers;

namespace Application.Orders.EventHandlers
{
    public class ShipVirtualCardProductOrderPayedEventHandler : IEventHandler<OrderPayedEventData>, ITransientDependency
    {
        public ShipService ShipService { get; set; }

        public void HandleEvent(OrderPayedEventData eventData)
        {
            if (eventData.Order is ProductOrder)
            {
                ShipService.Ship(eventData.Order, true);
            }
        }
    }
}
