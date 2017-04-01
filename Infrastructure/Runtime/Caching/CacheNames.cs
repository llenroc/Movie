using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Runtime.Caching
{
    /// <summary>
    /// Names of standard caches used in .
    /// </summary>
    public static class CacheNames
    {
        /// <summary>
        /// Application settings cache: ApplicationSettingsCache.
        /// </summary>
        public const string ApplicationSettings = "ApplicationSettingsCache";

        /// <summary>
        /// Tenant settings cache: TenantSettingsCache.
        /// </summary>
        public const string TenantSettings = "TenantSettingsCache";

        /// <summary>
        /// User settings cache: UserSettingsCache.
        /// </summary>
        public const string UserSettings = "UserSettingsCache";

        /// <summary>
        /// Localization scripts cache: LocalizationScripts.
        /// </summary>
        public const string LocalizationScripts = "LocalizationScripts";
    }    
}
