using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Infrastructure.Dependency;
using Infrastructure.Authorization;
using Infrastructure.Localization;
using Infrastructure.Configuration;
using Infrastructure.Application.Features;
using Infrastructure.Application.Navigation;
using Infrastructure.MultiTenancy;
using Infrastructure.Runtime.Session;
using Infrastructure.Timing;
using Infrastructure.Timing.Timezone;
using Infrastructure.Extensions;
using Infrastructure.Configuration.Startup;
using Infrastructure.Web.Models.UserConfiguration;
using Infrastructure.Web.Security.AntiForgery;

namespace Infrastructure.Web.Configuration
{
    public class UserConfigurationBuilder : ITransientDependency
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ILanguageManager _languageManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly IFeatureManager _featureManager;
        private readonly IFeatureChecker _featureChecker;
        private readonly IPermissionManager _permissionManager;
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager _settingManager;
        private readonly IAntiForgeryConfiguration _AntiForgeryConfiguration;
        private readonly IInfrastructureSession _Session;
        private readonly IPermissionChecker _permissionChecker;

        public UserConfigurationBuilder(
            IMultiTenancyConfig multiTenancyConfig,
            ILanguageManager languageManager,
            ILocalizationManager localizationManager,
            IFeatureManager featureManager,
            IFeatureChecker featureChecker,
            IPermissionManager permissionManager,
            IUserNavigationManager userNavigationManager,
            ISettingDefinitionManager settingDefinitionManager,
            ISettingManager settingManager,
            IAntiForgeryConfiguration AntiForgeryConfiguration,
            IInfrastructureSession Session,
            IPermissionChecker permissionChecker)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _languageManager = languageManager;
            _localizationManager = localizationManager;
            _featureManager = featureManager;
            _featureChecker = featureChecker;
            _permissionManager = permissionManager;
            _userNavigationManager = userNavigationManager;
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
            _AntiForgeryConfiguration = AntiForgeryConfiguration;
            _Session = Session;
            _permissionChecker = permissionChecker;
        }

        public async Task<UserConfigurationDto> GetAll()
        {
            return new UserConfigurationDto
            {
                MultiTenancy = GetUserMultiTenancyConfig(),
                Session = GetUserSessionConfig(),
                Localization = GetUserLocalizationConfig(),
                Features = await GetUserFeaturesConfig(),
                Auth = await GetUserAuthConfig(),
                Nav = await GetUserNavConfig(),
                Setting = await GetUserSettingConfig(),
                Clock = GetUserClockConfig(),
                Timing = await GetUserTimingConfig(),
                Security = GetUserSecurityConfig()
            };
        }

        private MultiTenancyConfigDto GetUserMultiTenancyConfig()
        {
            return new MultiTenancyConfigDto
            {
                IsEnabled = _multiTenancyConfig.IsEnabled
            };
        }

        private UserSessionConfigDto GetUserSessionConfig()
        {
            return new UserSessionConfigDto
            {
                UserId = _Session.UserId,
                TenantId = _Session.TenantId,
                ImpersonatorUserId = _Session.ImpersonatorUserId,
                ImpersonatorTenantId = _Session.ImpersonatorTenantId,
                MultiTenancySide = _Session.MultiTenancySide
            };
        }

        private UserLocalizationConfigDto GetUserLocalizationConfig()
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture;
            var languages = _languageManager.GetLanguages();

            var config = new UserLocalizationConfigDto
            {
                CurrentCulture = new UserCurrentCultureConfigDto
                {
                    Name = currentCulture.Name,
                    DisplayName = currentCulture.DisplayName
                },
                Languages = languages.ToList()
            };

            if (languages.Count > 0)
            {
                config.CurrentLanguage = _languageManager.CurrentLanguage;
            }

            var sources = _localizationManager.GetAllSources().OrderBy(s => s.Name).ToArray();
            config.Sources = sources.Select(s => new LocalizationSourceDto
            {
                Name = s.Name,
                Type = s.GetType().Name
            }).ToList();

            config.Values = new Dictionary<string, Dictionary<string, string>>();
            foreach (var source in sources)
            {
                var stringValues = source.GetAllStrings(currentCulture).OrderBy(s => s.Name).ToList();
                var stringDictionary = stringValues
                    .ToDictionary(_ => _.Name, _ => _.Value);
                config.Values.Add(source.Name, stringDictionary);
            }

            return config;
        }

        private async Task<UserFeatureConfigDto> GetUserFeaturesConfig()
        {
            var config = new UserFeatureConfigDto()
            {
                AllFeatures = new Dictionary<string, StringValueDto>()
            };

            var allFeatures = _featureManager.GetAll().ToList();

            if (_Session.TenantId.HasValue)
            {
                var currentTenantId = _Session.GetTenantId();
                foreach (var feature in allFeatures)
                {
                    var value = await _featureChecker.GetValueAsync(currentTenantId, feature.Name);
                    config.AllFeatures.Add(feature.Name, new StringValueDto
                    {
                        Value = value
                    });
                }
            }
            else
            {
                foreach (var feature in allFeatures)
                {
                    config.AllFeatures.Add(feature.Name, new StringValueDto
                    {
                        Value = feature.DefaultValue
                    });
                }
            }

            return config;
        }

        private async Task<UserAuthConfigDto> GetUserAuthConfig()
        {
            var config = new UserAuthConfigDto();

            var allPermissionNames = _permissionManager.GetAllPermissions(false).Select(p => p.Name).ToList();
            var grantedPermissionNames = new List<string>();

            if (_Session.UserId.HasValue)
            {
                foreach (var permissionName in allPermissionNames)
                {
                    if (await _permissionChecker.IsGrantedAsync(permissionName))
                    {
                        grantedPermissionNames.Add(permissionName);
                    }
                }
            }

            config.AllPermissions = allPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");
            config.GrantedPermissions = grantedPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");

            return config;
        }

        private async Task<UserNavConfigDto> GetUserNavConfig()
        {
            var userMenus = await _userNavigationManager.GetMenusAsync(_Session.ToUserIdentifier());
            return new UserNavConfigDto
            {
                Menus = userMenus.ToDictionary(userMenu => userMenu.Name, userMenu => userMenu)
            };
        }

        private async Task<UserSettingConfigDto> GetUserSettingConfig()
        {
            var config = new UserSettingConfigDto
            {
                Values = new Dictionary<string, string>()
            };

            var settingDefinitions = _settingDefinitionManager
                .GetAllSettingDefinitions()
                .Where(sd => sd.IsVisibleToClients);

            foreach (var settingDefinition in settingDefinitions)
            {
                var settingValue = await _settingManager.GetSettingValueAsync(settingDefinition.Name);
                config.Values.Add(settingDefinition.Name, settingValue);
            }

            return config;
        }

        private UserClockConfigDto GetUserClockConfig()
        {
            return new UserClockConfigDto
            {
                Provider = Clock.Provider.GetType().Name.ToCamelCase()
            };
        }

        private async Task<UserTimingConfigDto> GetUserTimingConfig()
        {
            var timezoneId = await _settingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);

            return new UserTimingConfigDto
            {
                TimeZoneInfo = new UserTimeZoneConfigDto
                {
                    Windows = new UserWindowsTimeZoneConfigDto
                    {
                        TimeZoneId = timezoneId,
                        BaseUtcOffsetInMilliseconds = timezone.BaseUtcOffset.TotalMilliseconds,
                        CurrentUtcOffsetInMilliseconds = timezone.GetUtcOffset(Clock.Now).TotalMilliseconds,
                        IsDaylightSavingTimeNow = timezone.IsDaylightSavingTime(Clock.Now)
                    },
                    Iana = new UserIanaTimeZoneConfigDto
                    {
                        TimeZoneId = TimezoneHelper.WindowsToIana(timezoneId)
                    }
                }
            };
        }

        private UserSecurityConfigDto GetUserSecurityConfig()
        {
            return new UserSecurityConfigDto()
            {
                AntiForgery = new UserAntiForgeryConfigDto
                {
                    TokenCookieName = _AntiForgeryConfiguration.TokenCookieName,
                    TokenHeaderName = _AntiForgeryConfiguration.TokenHeaderName
                }
            };
        }
    }
}
