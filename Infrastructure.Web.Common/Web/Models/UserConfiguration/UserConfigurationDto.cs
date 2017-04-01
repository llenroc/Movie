namespace Infrastructure.Web.Models.UserConfiguration
{
    public class UserConfigurationDto
    {
        public MultiTenancyConfigDto MultiTenancy { get; set; }

        public UserSessionConfigDto Session { get; set; }

        public UserLocalizationConfigDto Localization { get; set; }

        public UserFeatureConfigDto Features { get; set; }

        public UserAuthConfigDto Auth { get; set; }

        public UserNavConfigDto Nav { get; set; }

        public UserSettingConfigDto Setting { get; set; }

        public UserClockConfigDto Clock { get; set; }

        public UserTimingConfigDto Timing { get; set; }

        public UserSecurityConfigDto Security { get; set; }
    }
}