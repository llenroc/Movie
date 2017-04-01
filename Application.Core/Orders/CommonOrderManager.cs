using Application.Authorization.Users;
using Application.Expresses;
using Application.Orders.BoughtContexts;
using Application.Orders.Entities;
using Application.Orders.SalePriceProviders;
using Infrastructure.Domain.Repositories;

namespace Application.Orders
{
    public class CommonOrderManager : OrderManager<Order, BoughtContext>
    {
        public CommonOrderManager(
            IRepository<Order> orderRepository,
            IRepository<User, long> userRepository,
            IRepository<ExpressCompany> expressCompanyRepository,
            SalePriceProvider salePriceProvider
            ) :base(orderRepository, userRepository, expressCompanyRepository,salePriceProvider)
        {

        }
    }
}
