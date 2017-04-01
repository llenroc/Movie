using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MultiTenancy
{
    public static class MultiTenancyConsts
    {
        /// <summary>
        /// Default tenant id: 1.
        /// </summary>
        public const int DefaultTenantId = 1;

        public const string TenantIdResolveKey = "Infrastructure.TenantId";
    }
}
