using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MultiTenancy
{
    public class NullTenantResolverCache : ITenantResolverCache
    {
        public TenantResolverCacheItem Value
        {
            get { return null; }
            set { }
        }
    }
}
