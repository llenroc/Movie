using Application.Orders.Entities;
using Application.Orders.ShipProviders;
using Infrastructure.Dependency;
using Infrastructure.Domain.Services;

namespace Application.Orders
{
    public class ShipService:DomainService
    {
        private readonly IIocResolver _iocResolver;

        public ShipService(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public Order Ship(Order order,bool autoShip=false, ExpressInfo expressInfo = null)
        {
            IShipProvider[] ShipProviders = _iocResolver.ResolveAll<IShipProvider>();

            foreach (IShipProvider shipProvider in ShipProviders)
            {
                if(autoShip&& shipProvider is PhysicalShipProvider)
                {
                    continue;
                }
                shipProvider.Ship(order,expressInfo);
            }
            return order;
        }
    }
}
