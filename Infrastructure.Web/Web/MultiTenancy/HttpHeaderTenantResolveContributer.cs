using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Dependency;
using Infrastructure.Extensions;
using Infrastructure.MultiTenancy;
using Castle.Core.Logging;
using System.Web;

namespace Infrastructure.Web.MultiTenancy
{

    public class HttpHeaderTenantResolveContributer : ITenantResolveContributer, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public HttpHeaderTenantResolveContributer()
        {
            Logger = NullLogger.Instance;
        }

        public int? ResolveTenantId()
        {
            var httpContext = HttpContext.Current;

            if (httpContext == null)
            {
                return null;
            }
            var tenantIdHeader = httpContext.Request.Headers[MultiTenancyConsts.TenantIdResolveKey];

            if (tenantIdHeader.IsNullOrEmpty())
            {
                return null;
            }
            int tenantId;
            return !int.TryParse(tenantIdHeader, out tenantId) ? (int?)null : tenantId;
        }
    }
}
