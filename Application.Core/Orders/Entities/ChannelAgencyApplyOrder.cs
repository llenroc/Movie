using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Entities
{
    public class ChannelAgencyApplyOrder : Order
    {
        public bool HasProcessChannelAgencyApply { get; set; } = false;
    }
}
