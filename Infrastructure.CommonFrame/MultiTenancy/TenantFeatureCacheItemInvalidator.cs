using Infrastructure.Dependency;
using Infrastructure.Event.Bus.Entities;
using Infrastructure.Event.Bus.Handlers;
using Infrastructure.Runtime.Caching;

namespace Infrastructure.MultiTenancy
{
    /// <summary>
    /// This class handles related events and invalidated tenant feature cache items if needed.
    /// </summary>
    public class TenantFeatureCacheItemInvalidator :IEventHandler<EntityChangedEventData<TenantFeatureSetting>>, ITransientDependency
    {
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantFeatureCacheItemInvalidator"/> class.
        /// </summary>
        /// <param name="cacheManager">The cache manager.</param>
        public TenantFeatureCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void HandleEvent(EntityChangedEventData<TenantFeatureSetting> eventData)
        {
            _cacheManager.GetTenantFeatureCache().Remove(eventData.Entity.TenantId);
        }
    }
}
