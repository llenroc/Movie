using Application.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Entities
{
    public class MemberCardPackageOrder:Order
    {
        public virtual MemberCardPackage MemberCardPackage { get; set; }

        public bool HasProcessMemberCardPackage { get; set; } = false;

        public MemberCardPackageOrder()
        {
            HasProcessMemberCardPackage = false;
        }
    }
}
