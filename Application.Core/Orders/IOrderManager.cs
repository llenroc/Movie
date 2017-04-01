using Application.Orders.BoughtContexts;
using Application.Orders.Entities;
using Application.Wallets.Entities;
using Infrastructure.Domain.Services;
using System.Threading.Tasks;

namespace Application.Orders
{
    public interface IOrderManager<TOrder, TBoughtContext> :IDomainService
        where TOrder : Order, new()
        where TBoughtContext : IBoughtContext<TOrder>
    {
        Task<TOrder> Create(TBoughtContext boughtContext);

        TBoughtContext ProcessData(TBoughtContext boughtContext);

        TOrder CreateBaseOrderModel();

        Task<TOrder> PayCallback(string orderNumber, PayType payType);

        Task<TOrder> PayCallback(TOrder order,PayType payType);

        Task Refund(Order order, long refundFee);

        Task<Order> ShipAsync(Order order, bool autoShip = false, ExpressInfo expreeInfo = null);

        bool CheckShipProgressAndCompleteShip(Order order, ExpressInfo expreeInfo = null);

        Order CompleteShip(Order order, ExpressInfo expreeInfo = null);
    }
}
