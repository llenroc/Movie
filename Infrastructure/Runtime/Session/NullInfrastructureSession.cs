using Infrastructure.MultiTenancy;

namespace Infrastructure.Runtime.Session
{
    /// <summary>
    /// Implements null object pattern for <see cref="IInfrastructureSession"/>.
    /// </summary>
    public class NullInfrastructureSession : IInfrastructureSession
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullInfrastructureSession Instance { get { return SingletonInstance; } }
        private static readonly NullInfrastructureSession SingletonInstance = new NullInfrastructureSession();

        /// <inheritdoc/>
        public long? UserId { get { return null; } }

        /// <inheritdoc/>
        public int? TenantId { get { return null; } }

        public MultiTenancySides MultiTenancySide { get { return MultiTenancySides.Tenant; } }

        public long? ImpersonatorUserId { get { return null; } }

        public int? ImpersonatorTenantId { get { return null; } }

        private NullInfrastructureSession()
        {

        }
    }
}
