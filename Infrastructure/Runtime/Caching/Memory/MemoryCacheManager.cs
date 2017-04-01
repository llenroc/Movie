using Infrastructure.Dependency;
using Infrastructure.Runtime.Caching.Configuration;

namespace Infrastructure.Runtime.Caching.Memory
{
    /// <summary>
    /// Implements <see cref="ICacheManager"/> to work with <see cref="MemoryCache"/>.
    /// </summary>
    public class MemoryCacheManager : CacheManagerBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MemoryCacheManager(IIocManager iocManager, ICachingConfiguration configuration): base(iocManager, configuration)
        {
            IocManager.RegisterIfNot<MemoryCacheBase>(DependencyLifeStyle.Transient);
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return IocManager.Resolve<MemoryCacheBase>(new { name });
        }
    }
}
