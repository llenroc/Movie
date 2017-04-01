using Infrastructure.Dependency;
using Infrastructure.Extensions;
using Infrastructure.MultiTenancy;
using Infrastructure.Text;
using Infrastructure.Web.Web.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Web.MultiTenancy
{
    public class DomainTenantResolveContributer : ITenantResolveContributer, ITransientDependency
    {
        private readonly IWebMultiTenancyConfiguration _multiTenancyConfiguration;
        private readonly ITenantStore _tenantStore;

        public DomainTenantResolveContributer(
            IWebMultiTenancyConfiguration multiTenancyConfiguration,
            ITenantStore tenantStore)
        {
            _multiTenancyConfiguration = multiTenancyConfiguration;
            _tenantStore = tenantStore;
        }

        public int? ResolveTenantId()
        {
            if (_multiTenancyConfiguration.DomainFormat.IsNullOrEmpty())
            {
                return null;
            }
            var httpContext = HttpContext.Current;

            if (httpContext == null)
            {
                return null;
            }
            var hostName = httpContext.Request.Url.Host.RemovePreFix("http://", "https://");
            var result = new FormattedStringValueExtracter().Extract(hostName, _multiTenancyConfiguration.DomainFormat, true);

            if (!result.IsMatch)
            {
                return null;
            }
            var tenancyName = result.Matches[0].Value;

            if (tenancyName.IsNullOrEmpty())
            {
                return null;
            }

            if (string.Equals(tenancyName, "www", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            var tenantInfo = _tenantStore.Find(tenancyName);

            if (tenantInfo == null)
            {
                return null;
            }
            return tenantInfo.Id;
        }
    }
}
