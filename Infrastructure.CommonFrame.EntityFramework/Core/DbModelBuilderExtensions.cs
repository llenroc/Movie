using System.Data.Entity;
using Infrastructure.Application.Editions;
using Infrastructure.Application.Features;
using Infrastructure.Auditing;
using Infrastructure.Authorization;
using Infrastructure.Authorization.Roles;
using Infrastructure.Authorization.Users;
using Infrastructure.BackgroundJobs;
using Infrastructure.Configuration;
using Infrastructure.Localization;
using Infrastructure.MultiTenancy;
using Infrastructure.Notifications;
using Infrastructure.Organizations;

namespace Infrastructure.CommonFrame.EntityFramework
{
    /// <summary>
    /// Extension methods for <see cref="DbModelBuilder"/>.
    /// </summary>
    public static class DbModelBuilderExtensions
    {
        /// <summary>
        /// Changes prefix for  tables (which is "" by default).
        /// Can be null/empty string to clear the prefix.
        /// </summary>
        /// <typeparam name="TTenant">The type of the tenant entity.</typeparam>
        /// <typeparam name="TRole">The type of the role entity.</typeparam>
        /// <typeparam name="TUser">The type of the user entity.</typeparam>
        /// <param name="modelBuilder">Model builder.</param>
        /// <param name="prefix">Table prefix, or null to clear prefix.</param>
        public static void ChangeTablePrefix<TTenant, TRole, TUser>(this DbModelBuilder modelBuilder, string prefix, string schemaName = null)
            where TTenant : CommonFrameTenant<TUser>
            where TRole : CommonFrameRole<TUser>
            where TUser : CommonFrameUser<TUser>
        {
            prefix = prefix ?? "";

            SetTableName<AuditLog>(modelBuilder, prefix + "AuditLog", schemaName);
            SetTableName<BackgroundJobInfo>(modelBuilder, prefix + "BackgroundJob", schemaName);
            SetTableName<Edition>(modelBuilder, prefix + "Edition", schemaName);
            SetTableName<FeatureSetting>(modelBuilder, prefix + "Feature", schemaName);
            SetTableName<TenantFeatureSetting>(modelBuilder, prefix + "Feature", schemaName);
            SetTableName<EditionFeatureSetting>(modelBuilder, prefix + "Feature", schemaName);
            SetTableName<ApplicationLanguage>(modelBuilder, prefix + "Language", schemaName);
            SetTableName<ApplicationLanguageText>(modelBuilder, prefix + "LanguageText", schemaName);
            SetTableName<NotificationInfo>(modelBuilder, prefix + "Notification", schemaName);
            SetTableName<NotificationSubscriptionInfo>(modelBuilder, prefix + "NotificationSubscription", schemaName);
            SetTableName<OrganizationUnit>(modelBuilder, prefix + "OrganizationUnit", schemaName);
            SetTableName<PermissionSetting>(modelBuilder, prefix + "Permission", schemaName);
            SetTableName<RolePermissionSetting>(modelBuilder, prefix + "Permission", schemaName);
            SetTableName<UserPermissionSetting>(modelBuilder, prefix + "Permission", schemaName);
            SetTableName<TRole>(modelBuilder, prefix + "Role", schemaName);
            SetTableName<Setting>(modelBuilder, prefix + "Setting", schemaName);
            SetTableName<TTenant>(modelBuilder, prefix + "Tenant", schemaName);
            SetTableName<UserLogin>(modelBuilder, prefix + "UserLogin", schemaName);
            SetTableName<UserLoginAttempt>(modelBuilder, prefix + "UserLoginAttempt", schemaName);
            SetTableName<TenantNotificationInfo>(modelBuilder, prefix + "TenantNotification", schemaName);
            SetTableName<UserNotificationInfo>(modelBuilder, prefix + "UserNotification", schemaName);
            SetTableName<UserOrganizationUnit>(modelBuilder, prefix + "UserOrganizationUnit", schemaName);
            SetTableName<UserRole>(modelBuilder, prefix + "UserRole", schemaName);
            SetTableName<TUser>(modelBuilder, prefix + "User", schemaName);
            SetTableName<UserAccount>(modelBuilder, prefix + "UserAccount", schemaName);
            SetTableName<UserClaim>(modelBuilder, prefix + "UserClaim", schemaName);
        }

        private static void SetTableName<TEntity>(DbModelBuilder modelBuilder, string tableName, string schemaName) where TEntity : class
        {
            if (schemaName == null)
            {
                modelBuilder.Entity<TEntity>().ToTable(tableName);
            }
            else
            {
                modelBuilder.Entity<TEntity>().ToTable(tableName, schemaName);
            }
        }
    }
}
