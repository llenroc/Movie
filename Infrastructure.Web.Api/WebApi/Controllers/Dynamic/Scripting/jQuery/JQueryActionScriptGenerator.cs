using System.Text;
using Infrastructure.Extensions;
using Infrastructure.Web;

namespace Infrastructure.WebApi.Controllers.Dynamic.Scripting.jQuery
{
    internal class JQueryActionScriptGenerator
    {
        private readonly DynamicApiControllerInfo _controllerInfo;
        private readonly DynamicApiActionInfo _actionInfo;

        private const string JsMethodTemplate =
@"    serviceNamespace.{jsMethodName} = function({jsMethodParameterList}) {
        return infrastructure.ajax($.extend({
{ajaxCallParameters}
        }, ajaxParams));
    };";

        public JQueryActionScriptGenerator(DynamicApiControllerInfo controllerInfo, DynamicApiActionInfo actionInfo)
        {
            _controllerInfo = controllerInfo;
            _actionInfo = actionInfo;
        }

        public virtual string GenerateMethod()
        {
            var jsMethodName = _actionInfo.ActionName.ToCamelCase();
            var jsMethodParameterList = ActionScriptingHelper.GenerateJsMethodParameterList(_actionInfo.Method, "ajaxParams");

            var jsMethod = JsMethodTemplate
                .Replace("{jsMethodName}", jsMethodName)
                .Replace("{jsMethodParameterList}", jsMethodParameterList)
                .Replace("{ajaxCallParameters}", GenerateAjaxCallParameters());

            return jsMethod;
        }

        protected string GenerateAjaxCallParameters()
        {
            var script = new StringBuilder();
            
            script.AppendLine("            url: infrastructure.appPath + '" + ActionScriptingHelper.GenerateUrlWithParameters(_controllerInfo, _actionInfo) + "',");
            script.AppendLine("            type: '" + _actionInfo.Verb.ToString().ToUpperInvariant() + "',");

            if (_actionInfo.Verb == HttpVerb.Get)
            {
                script.Append("            data: " + ActionScriptingHelper.GenerateBody(_actionInfo));
            }
            else
            {
                script.Append("            data: JSON.stringify(" + ActionScriptingHelper.GenerateBody(_actionInfo) + ")");                
            }
            
            return script.ToString();
        }
    }
}