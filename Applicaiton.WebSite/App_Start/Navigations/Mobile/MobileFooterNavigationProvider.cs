using Infrastructure.Application.Navigation;
using Infrastructure.Localization;

namespace Application.WebSite.Navigations.Mobile
{
    public class MobileFooterNavigationProvider : NavigationProvider
    {
        public const string MenuName = "FooterMenu";


        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = new MenuDefinition(MenuName, new FixedLocalizableString("Footer Menu"));
            context.Manager.Menus[MenuName] = menu;
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ApplicationConsts.LocalizationSourceName);
        }
    }
}