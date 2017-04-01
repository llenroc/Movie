using Application.Channel.ChannelAgents;
using Application.Distributions;
using Application.Orders.Entities;
using Application.Orders.Events;
using Application.Shops;
using Application.Wallets;
using Infrastructure;
using Infrastructure.Configuration;
using Infrastructure.Dependency;
using Infrastructure.Event.Bus.Handlers;
using Infrastructure.Threading;
using System;
using System.Collections.Generic;

namespace Application.Orders.EventHandlers
{
    public class RebateProductOrderPayedEventHandler : IEventHandler<OrderPayedEventData>, ITransientDependency
    {
        public ISettingManager SettingManager { get; set; }
        protected DistributionManager DistributionManager { get; set; }
        public ChannelAgentManager ChannelAgentManager { get; set; }
        public WalletManager WalletManager { get; set; }

        public RebateProductOrderPayedEventHandler(DistributionManager distributionManager)
        {
            DistributionManager = distributionManager;
        }

        public void HandleEvent(OrderPayedEventData eventData)
        {
            if (eventData.Order is ProductOrder)
            {
                ProductOrder order = eventData.Order as ProductOrder;

                //Distribution and ChannelAgency Rebate
                if ((DistributionWhen)Enum.Parse(typeof(DistributionWhen), SettingManager.GetSettingValue(ShopSettings.Distribution.DistributionWhen)) == DistributionWhen.Payed)
                {
                    AsyncHelper.RunSync(async () =>
                    {
                        List<OrderDistribution> orderDistributions = await DistributionManager.TryAndCreateOrderDistributionAsync(order);
                        List<ChannelAgentRebate> channelAgentRebates = await ChannelAgentManager.TryAndCreateOrderChannelAgentRebatesAsync(order);

                        List<long> userList = new List<long>();

                        foreach (OrderDistribution orderDistribution in orderDistributions)
                        {
                            if (!userList.Contains(orderDistribution.UserId))
                            {
                                userList.Add(orderDistribution.UserId);
                            }
                        }
                        foreach (ChannelAgentRebate channelAgentRebate in channelAgentRebates)
                        {
                            if (!userList.Contains(channelAgentRebate.UserId))
                            {
                                userList.Add(channelAgentRebate.UserId);
                            }
                        }

                        foreach (long userId in userList)
                        {
                            WalletManager.WithdrawAllBalanceAsync(new UserIdentifier(order.TenantId, userId));
                        }
                    });
                }
            }
        }
    }
}
