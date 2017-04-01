using System;
using System.Security.Claims;
using System.Security.Principal;
using Infrastructure.Runtime.Security;
using Microsoft.AspNet.Identity;

namespace Infrastructure
{
    //TODO: Use from  after  v1.0!
    public static class ClaimsIdentityExtensions
    {
        public static int? GetTenantId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var tenantIdOrNull = claimsIdentity?.FindFirstValue(InfrastructureClaimTypes.TenantId);

            if (tenantIdOrNull == null)
            {
                return null;
            }
            return Convert.ToInt32(tenantIdOrNull);
        }
    }
}
