using Application.Orders.BoughtContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.SalePriceProviders
{
    public interface ISalePriceProvider<TBoughtContext>
    {
        TBoughtContext Caculate(TBoughtContext boughtContext);
    }
}
