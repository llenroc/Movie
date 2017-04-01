using Application.Orders.Entities;
using Infrastructure.Domain.Services;

namespace Application.Orders.ShipProviders
{
    public interface IShipProvider:IDomainService
    {
        void Ship(Order order, ExpressInfo expreeInfo = null);
    }
}
