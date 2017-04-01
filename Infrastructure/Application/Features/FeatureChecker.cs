using Infrastructure.Dependency;
using Infrastructure.Runtime.Session;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Application.Features
{
    /// <summary>
    /// Default implementation for <see cref="IFeatureChecker"/>.
    /// </summary>
    public class FeatureChecker : IFeatureChecker, ITransientDependency
    {
        /// <summary>
        /// Reference to current session.
        /// </summary>
        public IInfrastructureSession Session { get; set; }

        /// <summary>
        /// Reference to the store used to get feature values.
        /// </summary>
        public IFeatureValueStore FeatureValueStore { get; set; }

        private readonly IFeatureManager _featureManager;

        /// <summary>
        /// Creates a new <see cref="FeatureChecker"/> object.
        /// </summary>
        public FeatureChecker(IFeatureManager featureManager)
        {
            _featureManager = featureManager;

            FeatureValueStore = NullFeatureValueStore.Instance;
            Session = NullInfrastructureSession.Instance;
        }

        /// <inheritdoc/>
        public Task<string> GetValueAsync(string name)
        {
            if (!Session.TenantId.HasValue)
            {
                throw new Exception("FeatureChecker can not get a feature value by name. TenantId is not set in the ISession!");
            }

            return GetValueAsync(Session.TenantId.Value, name);
        }

        /// <inheritdoc/>
        public async Task<string> GetValueAsync(int tenantId, string name)
        {
            var feature = _featureManager.Get(name);

            var value = await FeatureValueStore.GetValueOrNullAsync(tenantId, feature);

            if (value == null)
            {
                return feature.DefaultValue;
            }
            return value;
        }
    }
}
