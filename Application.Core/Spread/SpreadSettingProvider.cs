using System.Collections.Generic;
using System.Configuration;
using Infrastructure.Configuration;
using Infrastructure.Json;
using Application.Security;

namespace Application.Configuration
{
    /// <summary>
    /// Defines settings for the application.
    /// See <see cref="SpreadSettings"/> for setting names.
    /// </summary>
    public class SpreadSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                //Tenant settings
                new SettingDefinition(
                    SpreadSettings.General.UpgradeOrderMoney,
                    ConfigurationManager.AppSettings[SpreadSettings.General.UpgradeOrderMoney] ?? "", 
                    scopes: SettingScopes.Tenant),
                new SettingDefinition(
                    SpreadSettings.General.MustBeSpreaderForSpread,
                    ConfigurationManager.AppSettings[SpreadSettings.General.MustBeSpreaderForSpread] ?? "",
                    scopes: SettingScopes.Tenant)
            };
        }
    }
}
