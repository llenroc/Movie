namespace Infrastructure.MultiTenancy
{
    public interface IDbMigrator
    {
        void CreateOrMigrateForHost();

        void CreateOrMigrateForTenant(TenantBase tenant);
    }
}
