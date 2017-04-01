using Application.Orders.BoughtContexts;
using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.SalePriceProviders
{
    public class MemberCardPackageSalePriceProvider : ISalePriceProvider<MemberCardPackageBoughtContext>, ISingletonDependency 
    {
        public MemberCardPackageBoughtContext Caculate(MemberCardPackageBoughtContext boughtContext)
        {
            boughtContext.Money = boughtContext.MemberCardPackage.Price;
            return boughtContext;
        }
    }
}
