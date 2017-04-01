using System;
using System.Globalization;
using System.Text;
using Infrastructure.Configuration.Startup;
using Infrastructure.Dependency;
using Infrastructure.Extensions;
using Infrastructure.MultiTenancy;

namespace Infrastructure.Web.MultiTenancy
{
    public class MultiTenancyScriptManager : IMultiTenancyScriptManager, ITransientDependency
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public MultiTenancyScriptManager(IMultiTenancyConfig multiTenancyConfig)
        {
            _multiTenancyConfig = multiTenancyConfig;
        }

        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine();

            script.AppendLine("    infrastructure.multiTenancy = infrastructure.multiTenancy || {};");
            script.AppendLine("    infrastructure.multiTenancy.isEnabled = " + _multiTenancyConfig.IsEnabled.ToString().ToLower(CultureInfo.InvariantCulture) + ";");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}