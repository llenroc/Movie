using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Dependency;
using Application.Orders.BoughtContexts;
using Application.Orders.SalePriceServices;

namespace Application.Orders.SalePriceProviders
{
    public class SalePriceProvider: ISalePriceProvider<BoughtContext>,ISingletonDependency
    {
        public SalePriceService ProductSalePriceService { get; set; }
        public BoughtContext Caculate(BoughtContext boughtContext)
        {
            ProductSalePriceService.Calculate(boughtContext);
            return boughtContext;
        }
    }
}
