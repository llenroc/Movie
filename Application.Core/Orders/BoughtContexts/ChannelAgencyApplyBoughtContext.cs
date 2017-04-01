using Application.Channel.ChannelAgencies;
using Application.Channel.ChannelAgents;
using Application.Orders.Entities;
using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.BoughtContexts
{
    public class ChannelAgencyApplyBoughtContext : IBoughtContext<ChannelAgencyApplyOrder>, ISingletonDependency
    {
        public ChannelAgencyApplyOrder Order { get; set; }

        public ChannelAgent ChannelAgent { get; set; }

        public ChannelAgencyApply ChannelAgencyApply { get; set; }

        public decimal Money { get; set; } = 0;

        public int ProductCount
        {
            get
            {
                int count = 0;
                return count;
            }
        }
    }
}
