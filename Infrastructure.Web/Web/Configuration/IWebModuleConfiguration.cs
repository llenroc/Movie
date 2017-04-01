using Infrastructure.Web.Security.AntiForgery;

namespace Infrastructure.Web.Configuration
{
    public interface IWebModuleConfiguration
    {
        IAntiForgeryWebConfiguration AntiForgery { get; }

        IWebLocalizationConfiguration Localization { get; }
    }
}
