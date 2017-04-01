using System.Data.Common;
using System.Data.Entity;
using Infrastructure.Application.Editions;
using Infrastructure.Application.Features;
using Infrastructure.Authorization.Roles;
using Infrastructure.Authorization.Users;
using Infrastructure.BackgroundJobs;
using Infrastructure.MultiTenancy;

namespace Infrastructure.CommonFrame.EntityFramework
{
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class HostDbContext<TTenant, TRole, TUser> : CommonFrameCommonDbContext<TRole, TUser>  where TTenant : CommonFrameTenant<TUser>  where TRole : CommonFrameRole<TUser> where TUser : CommonFrameUser<TUser>
    {
        /// <summary>
        /// Tenants
        /// </summary>
        public virtual IDbSet<TTenant> Tenants { get; set; }

        /// <summary>
        /// Editions.
        /// </summary>
        public virtual IDbSet<Edition> Editions { get; set; }

        /// <summary>
        /// FeatureSettings.
        /// </summary>
        public virtual IDbSet<FeatureSetting> FeatureSettings { get; set; }

        /// <summary>
        /// TenantFeatureSetting.
        /// </summary>
        public virtual IDbSet<TenantFeatureSetting> TenantFeatureSettings { get; set; }

        /// <summary>
        /// EditionFeatureSettings.
        /// </summary>
        public virtual IDbSet<EditionFeatureSetting> EditionFeatureSettings { get; set; }

        /// <summary>
        /// Background jobs.
        /// </summary>
        public virtual IDbSet<BackgroundJobInfo> BackgroundJobs { get; set; }

        /// <summary>
        /// User accounts
        /// </summary>
        public virtual IDbSet<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// Default constructor.
        /// Do not directly instantiate this class. Instead, use dependency injection!
        /// </summary>
        protected HostDbContext()
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file</param>
        protected HostDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }

        /// <summary>
        /// This constructor can be used for unit tests.
        /// </summary>
        protected HostDbContext(DbConnection dbConnection, bool contextOwnsConnection): base(dbConnection, contextOwnsConnection)
        {

        }
    }
}
