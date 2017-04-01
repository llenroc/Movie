using Application.Authorization.Users;
using Application.Channel.ChananlAgencys;
using Application.Channel.ChannelAgencies;
using Application.Expresses;
using Application.Orders.BoughtContexts;
using Application.Orders.Entities;
using Application.Orders.SalePriceProviders;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class ChannelAgencyApplyOrderManager : OrderManager<ChannelAgencyApplyOrder, ChannelAgencyApplyBoughtContext>
    {
        public ChannelAgencyManager ChannelAgencyManager { get; set; }
        protected IRepository<ChannelAgencyApply> ChannelAgencyApplyRepository;

        public ChannelAgencyApplyOrderManager(
            IRepository<ChannelAgencyApply> channelAgencyApplyRepository,
            IRepository<ChannelAgencyApplyOrder> orderRepository,
            IRepository<ExpressCompany> expressCompanyRepository,
            IRepository<User, long> userRepository,
            ChannelAgencyApplySalePriceProvider salePriceProvider
            ) : base(orderRepository, userRepository, expressCompanyRepository, salePriceProvider)
        {
            ChannelAgencyApplyRepository = channelAgencyApplyRepository;
        }

        public override string BuildTitle(ChannelAgencyApplyBoughtContext boughtContext)
        {
            return boughtContext.ChannelAgent.Name + L("Apply");
        }

        public override ChannelAgencyApplyBoughtContext PreProcessOrderData(ChannelAgencyApplyBoughtContext boughtContext)
        {
            boughtContext.Order.HasProcessChannelAgencyApply = false;
            return base.PreProcessOrderData(boughtContext);
        }

        [UnitOfWork]
        public async Task<ChannelAgencyApplyOrder> CreateOrder(ChannelAgencyApplyBoughtContext boughtContext)
        {
            ChannelAgencyApplyOrder order =await Create(boughtContext);
            ChannelAgencyManager.CreateChannelAgencyApply(boughtContext.ChannelAgent.Id, boughtContext.Order.UserId, boughtContext.Order.Id);
            return order;
        }

        [UnitOfWork]
        public ChannelAgencyApplyOrder ProcessAfterPayed(ChannelAgencyApplyOrder order)
        {
            if (order.HasProcessChannelAgencyApply)
            {
                return order;
            }
            using (CurrentUnitOfWork.SetTenantId(order.TenantId))
            {
                ChannelAgencyApply channelAgencyApply = ChannelAgencyApplyRepository.GetAll().Where(model => model.OrderId == order.Id).FirstOrDefault();
                ChannelAgencyManager.PassChannelAgencyApply(channelAgencyApply);
                order.HasProcessChannelAgencyApply = true;
                OrderRepository.Update(order);
                CurrentUnitOfWork.SaveChanges();
                CompleteOrder(order);
            }
            return order;
        }
    }
}
