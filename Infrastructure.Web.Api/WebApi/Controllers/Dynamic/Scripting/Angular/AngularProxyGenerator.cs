using System.Text;

namespace Infrastructure.WebApi.Controllers.Dynamic.Scripting.Angular
{
    internal class AngularProxyGenerator : IScriptProxyGenerator
    {
        private readonly DynamicApiControllerInfo _controllerInfo;

        public AngularProxyGenerator(DynamicApiControllerInfo controllerInfo)
        {
            _controllerInfo = controllerInfo;
        }

        public string Generate()
        {
            var script = new StringBuilder();

            script.AppendLine("(function (infrastructure, angular) {");
            script.AppendLine("");
            script.AppendLine("    if (!angular) {");
            script.AppendLine("        return;");
            script.AppendLine("    }");
            script.AppendLine("    ");
            script.AppendLine("    var infrastructureModule = angular.module('infrastructure');");
            script.AppendLine("    ");
            script.AppendLine("    infrastructureModule.factory('infrastructure.services." + _controllerInfo.ServiceName.Replace("/", ".") + "', [");
            script.AppendLine("        '$http', function ($http) {");
            script.AppendLine("            return new function () {");

            foreach (var methodInfo in _controllerInfo.Actions.Values)
            {
                var actionWriter = new AngularActionScriptWriter(_controllerInfo, methodInfo);
                actionWriter.WriteTo(script);
            }

            script.AppendLine("            };");
            script.AppendLine("        }");
            script.AppendLine("    ]);");
            script.AppendLine();

            script.AppendLine();
            script.AppendLine("})((infrastructure || (infrastructure = {})), (angular || undefined));");

            return script.ToString();
        }
    }
}