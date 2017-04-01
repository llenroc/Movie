namespace Infrastructure.Web.Configuration
{
    public class WebLocalizationConfiguration : IWebLocalizationConfiguration
    {
        /// <summary>
        /// Default: ".Localization.CultureName".
        /// </summary>
        public string CookieName { get; set; }

        public WebLocalizationConfiguration()
        {
            CookieName = ".Localization.CultureName";
        }
    }
}
