using Infrastructure.Dependency;
using Infrastructure.MultiTenancy;
using System;
using System.Linq;
using System.Security.Claims;
using Infrastructure.Configuration.Startup;
using Infrastructure.Runtime.Security;

namespace Infrastructure.Runtime.Session
{
    /// <summary>
    /// Implements <see cref="IInfrastructureSession"/> to get session properties from claims of <see cref="Thread.CurrentPrincipal"/>.
    /// </summary>
    public class ClaimsSession : IInfrastructureSession, ISingletonDependency
    {
        public virtual long? UserId
        {
            get
            {
                var userIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim?.Value))
                {
                    return null;
                }
                long userId;

                if (!long.TryParse(userIdClaim.Value, out userId))
                {
                    return null;
                }
                return userId;
            }
        }

        public virtual int? TenantId
        {
            get
            {
                if (!MultiTenancy.IsEnabled)
                {
                    return MultiTenancyConsts.DefaultTenantId;
                }
                var tenantIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == InfrastructureClaimTypes.TenantId);

                if (string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return null;
                }
                return Convert.ToInt32(tenantIdClaim.Value);
            }
        }

        public virtual long? ImpersonatorUserId
        {
            get
            {
                var impersonatorUserIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == InfrastructureClaimTypes.ImpersonatorUserId);

                if (string.IsNullOrEmpty(impersonatorUserIdClaim?.Value))
                {
                    return null;
                }
                return Convert.ToInt64(impersonatorUserIdClaim.Value);
            }
        }

        public virtual int? ImpersonatorTenantId
        {
            get
            {
                if (!MultiTenancy.IsEnabled)
                {
                    return MultiTenancyConsts.DefaultTenantId;
                }
                var impersonatorTenantIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == InfrastructureClaimTypes.ImpersonatorTenantId);

                if (string.IsNullOrEmpty(impersonatorTenantIdClaim?.Value))
                {
                    return null;
                }
                return Convert.ToInt32(impersonatorTenantIdClaim.Value);
            }
        }

        public virtual MultiTenancySides MultiTenancySide
        {
            get
            {
                return MultiTenancy.IsEnabled && !TenantId.HasValue? MultiTenancySides.Host: MultiTenancySides.Tenant;
            }
        }

        public IPrincipalAccessor PrincipalAccessor { get; set; } //TODO: Convert to constructor-injection

        protected readonly IMultiTenancyConfig MultiTenancy;

        public ClaimsSession(IMultiTenancyConfig multiTenancy)
        {
            MultiTenancy = multiTenancy;
            PrincipalAccessor = DefaultPrincipalAccessor.Instance;
        }
    }
}
