using Infrastructure.Dependency;
using System.Text;
using Infrastructure.Web.Security.AntiForgery;

namespace Infrastructure.Web.Security
{
    internal class SecurityScriptManager : ISecurityScriptManager, ITransientDependency
    {
        private readonly IAntiForgeryConfiguration _AntiForgeryConfiguration;

        public SecurityScriptManager(IAntiForgeryConfiguration AntiForgeryConfiguration)
        {
            _AntiForgeryConfiguration = AntiForgeryConfiguration;
        }

        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    infrastructure.security.antiForgery.tokenCookieName = '" + _AntiForgeryConfiguration.TokenCookieName + "';");
            script.AppendLine("    infrastructure.security.antiForgery.tokenHeaderName = '" + _AntiForgeryConfiguration.TokenHeaderName + "';");
            script.Append("})();");

            return script.ToString();
        }
    }
}
