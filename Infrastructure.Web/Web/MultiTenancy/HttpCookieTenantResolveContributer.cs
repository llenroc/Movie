using Infrastructure.Dependency;
using Infrastructure.Extensions;
using Infrastructure.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Web.MultiTenancy
{
    public class HttpCookieTenantResolveContributer : ITenantResolveContributer, ITransientDependency
    {
        public int? ResolveTenantId()
        {
            var cookie = HttpContext.Current?.Request.Cookies[MultiTenancyConsts.TenantIdResolveKey];

            if (cookie == null || cookie.Value.IsNullOrEmpty())
            {
                return null;
            }
            int tenantId;
            return !int.TryParse(cookie.Value, out tenantId) ? (int?)null : tenantId;
        }
    }
}
