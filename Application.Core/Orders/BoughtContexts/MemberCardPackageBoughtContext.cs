using Application.Members;
using Application.Orders.Entities;
using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.BoughtContexts
{
    public class MemberCardPackageBoughtContext : IBoughtContext<MemberCardPackageOrder>, ISingletonDependency
    {
        public MemberCardPackageOrder Order { get; set; }

        public MemberCardPackage MemberCardPackage { get; set; }

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
