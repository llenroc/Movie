using Infrastructure.MultiTenancy;

namespace Infrastructure.CommonFrame.EntityFramework
{
    public interface IMultiTenantSeed
    {
        TenantBase Tenant { get; set; }
    }
}
