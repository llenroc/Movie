using Application.Authorization.Users;
using Application.Channel.ChannelAgencies;
using Application.Entities;
using Application.Orders.Entities;
using Application.Wallets;
using Infrastructure.Domain.Repositories;
using Infrastructure.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channel.ChannelAgents
{
    public class ChannelAgentManager: ApplicationDomainServiceBase
    {
        public IRepository<User,long> UserRepository { get; set; }
        public IRepository<Order> OrderRepository { get; set; }
        public IRepository<ChannelAgency> ChannelAgencyRepository { get; set; }
        public IRepository<ChannelAgentRebate> ChannelAgentRebateRepository{get;set;}
        public WalletManager WalletManager { get; set; }

        [UnitOfWork]
        public async Task<List<ChannelAgentRebate>> TryAndCreateOrderChannelAgentRebatesAsync(int orderId)
        {
            Order order = OrderRepository.Get(orderId);
            List<ChannelAgentRebate> channelAgentRebates=await TryAndCreateOrderChannelAgentRebatesAsync(order);
            return channelAgentRebates;
        }

        [UnitOfWork]
        public async Task<List<ChannelAgentRebate>> TryAndCreateOrderChannelAgentRebatesAsync(Order order)
        {
            using (CurrentUnitOfWork.SetTenantId(order.TenantId))
            {
                User user = order.User;
                List<ChannelAgentRebate> channelAgentRebates = new List<ChannelAgentRebate>();

                if (user.ChannelAgencyId.HasValue)
                {
                    ChannelAgency channelAgency = ChannelAgencyRepository.Get(user.ChannelAgencyId.Value);
                    float totalRebateRatio = 0;

                    await Task.Run(() =>
                    {
                        TryAndCreateOrderChannelAgentRebate(channelAgency, order, ref totalRebateRatio, user.ChannelAgentDepth, ref channelAgentRebates);
                    });
                }
                return channelAgentRebates;
            }
        }

        public void TryAndCreateOrderChannelAgentRebate(ChannelAgency channelAgency, Order order, ref float totalRebateRatio, int depth, ref List<ChannelAgentRebate> channelAgentRebates)
        {
            float rebateRatio = channelAgency.ChannelAgent.RebateRatio - totalRebateRatio;

            if (rebateRatio > 0)
            {
                ChannelAgentRebate channelAgentRebate = new ChannelAgentRebate()
                {
                    OrderId = order.Id,
                    UserId = channelAgency.UserId,
                    ChannelAgentId = channelAgency.ChannelAgentId,
                    ChannlAgencyId = channelAgency.Id,
                    RebateRatio= rebateRatio,
                    Money=order.PayMoney* (decimal)rebateRatio,
                    Depth =depth
                };
                ChannelAgentRebateRepository.Insert(channelAgentRebate);
                CurrentUnitOfWork.SaveChanges();

                WalletManager.IncomeOfOrderRebate(channelAgency.GetUserIdentifier(), channelAgentRebate.Money,"ChannelAgentRebate", order);
                totalRebateRatio += rebateRatio;
            }

            if (channelAgency.User.ChannelAgencyId.HasValue)
            {
                depth += channelAgency.User.ChannelAgentDepth;
                ChannelAgency parentChannelAgency = ChannelAgencyRepository.Get(channelAgency.User.ChannelAgencyId.Value);
                TryAndCreateOrderChannelAgentRebate(parentChannelAgency, order, ref totalRebateRatio, depth, ref channelAgentRebates);
            }
        }
    }
}
