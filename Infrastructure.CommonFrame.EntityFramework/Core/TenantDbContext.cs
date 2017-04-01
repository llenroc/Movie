using System.Data.Common;
using Infrastructure.Authorization.Roles;
using Infrastructure.Authorization.Users;
using Infrastructure.MultiTenancy;

namespace Infrastructure.CommonFrame.EntityFramework
{
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class TenantDbContext<TRole, TUser> : CommonFrameCommonDbContext<TRole, TUser> where TRole : CommonFrameRole<TUser>  where TUser : CommonFrameUser<TUser>
    {
        /// <summary>
        /// Default constructor.
        /// Do not directly instantiate this class. Instead, use dependency injection!
        /// </summary>
        protected TenantDbContext()
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file</param>
        protected TenantDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }

        /// <summary>
        /// This constructor can be used for unit tests.
        /// </summary>
        protected TenantDbContext(DbConnection dbConnection, bool contextOwnsConnection): base(dbConnection, contextOwnsConnection)
        {

        }
    }
}
