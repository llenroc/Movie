using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.BoughtContexts
{
    public interface IBoughtContext<TOrder>
    {
        TOrder Order { get; set; }

        decimal Money { get; set; }

        int ProductCount { get; }
    }
}
