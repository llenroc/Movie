using Infrastructure.Web.Security.AntiForgery;

namespace Infrastructure.Web.Configuration
{
    public class WebModuleConfiguration : IWebModuleConfiguration
    {
        public IAntiForgeryWebConfiguration AntiForgery { get; }
        public IWebLocalizationConfiguration Localization { get; }

        public WebModuleConfiguration( IAntiForgeryWebConfiguration antiForgery, IWebLocalizationConfiguration localization)
        {
            AntiForgery = antiForgery;
            Localization = localization;
        }
    }
}
