using Infrastructure.Web.Api.Modeling;

namespace Infrastructure.Web.Api.ProxyScripting.Generators
{
    public interface IProxyScriptGenerator
    {
        string CreateScript(ApplicationApiDescriptionModel model);
    }
}
