using Application.Orders.Entities;

namespace Application.Orders.ShipProviders
{
    public class PhysicalShipProvider:ApplicationDomainServiceBase, IShipProvider
    {
        protected CommonOrderManager OrderManager;

        public PhysicalShipProvider(CommonOrderManager orderManager)
        {
            OrderManager = orderManager;
        }

        public void Ship(Order order, ExpressInfo expreeInfo = null)
        {
            foreach (OrderItem orderItem in order.OrderItems)
            {
                ShipOrderItem(orderItem);
            }
            OrderManager.CheckShipProgressAndCompleteShip(order, expreeInfo);
        }

        private void ShipOrderItem(OrderItem orderItem)
        {
            if (orderItem.ShipStatus != ShipStatus.UnShip)
            {
                return;
            }

            if (orderItem.Specification.Product.Type == Products.ProductType.Physical)
            {
                orderItem.ShipStatus = ShipStatus.Shipping;
            }
        }
    }
}
