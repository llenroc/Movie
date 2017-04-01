using System.Web;
using Infrastructure.Dependency;
using Infrastructure.MultiTenancy;

namespace Infrastructure.Web.MultiTenancy
{
    public class HttpContextTenantResolverCache : ITenantResolverCache, ITransientDependency
    {
        private const string CacheItemKey = "Infrastructure.MultiTenancy.TenantResolverCacheItem";

        public TenantResolverCacheItem Value
        {
            get
            {
                return HttpContext.Current?.Items[CacheItemKey] as TenantResolverCacheItem;
            }

            set
            {
                var httpContext = HttpContext.Current;

                if (httpContext == null)
                {
                    return;
                }
                httpContext.Items[CacheItemKey] = value;
            }
        }
    }
}
