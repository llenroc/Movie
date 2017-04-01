using Application.Authorization.Users;
using Application.Expresses;
using Application.Members;
using Application.Orders.BoughtContexts;
using Application.Orders.Entities;
using Application.Orders.SalePriceProviders;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class MemberCardPackageOrderManager : OrderManager<MemberCardPackageOrder, MemberCardPackageBoughtContext>
    {
        public MemberCardManager MemberCardManager { get; set; }

        public MemberCardPackageOrderManager(
            IRepository<MemberCardPackageOrder> orderRepository,
            IRepository<User, long> userRepository,
            IRepository<ExpressCompany> expressCompanyRepository,
            MemberCardPackageSalePriceProvider salePriceProvider
            ) :base(orderRepository, userRepository, expressCompanyRepository,salePriceProvider)
        {

        }

        public override string BuildTitle(MemberCardPackageBoughtContext boughtContext)
        {
            return boughtContext.MemberCardPackage.Title;
        }

        public override MemberCardPackageBoughtContext PreProcessOrderData(MemberCardPackageBoughtContext boughtContext)
        {
            boughtContext.Order.MemberCardPackage = boughtContext.MemberCardPackage;
            return base.PreProcessOrderData(boughtContext);
        }

        [UnitOfWork]
        public async Task<MemberCardPackageOrder> CreateOrder(MemberCardPackageBoughtContext boughtContext)
        {
            MemberCardManager.CheckUserMemberCard(boughtContext.MemberCardPackage.MemberLevel.Id, Session.UserId.Value);
            MemberCardPackageOrder order=await Create(boughtContext);
            return order;
        }

        public MemberCardPackage GetMemberCardPackage(Order order)
        {
            return GetMemberCardPackage(order.Id);
        }

        public MemberCardPackage GetMemberCardPackage(int orderId)
        {
            return OrderRepository.Get(orderId).MemberCardPackage;
        }

        [UnitOfWork]
        public void ProcessMemberCardPackage(int orderId)
        {
            MemberCardPackageOrder order=OrderRepository.Get(orderId);
            order.HasProcessMemberCardPackage = true;
            OrderRepository.Update(order);
        }
    }
}
