using Application.Orders.BoughtContexts;
using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.SalePriceProviders
{
    public class ChannelAgencyApplySalePriceProvider : ISalePriceProvider<ChannelAgencyApplyBoughtContext>, ISingletonDependency
    {
        public ChannelAgencyApplyBoughtContext Caculate(ChannelAgencyApplyBoughtContext boughtContext)
        {
            boughtContext.Money = boughtContext.ChannelAgent.Price;
            return boughtContext;
        }
    }
}
