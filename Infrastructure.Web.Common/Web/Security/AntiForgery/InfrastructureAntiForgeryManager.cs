using Castle.Core.Logging;
using Infrastructure.Dependency;
using System;

namespace Infrastructure.Web.Security.AntiForgery
{
    public class InfrastructureAntiForgeryManager : IInfrastructureAntiForgeryManager, IAntiForgeryValidator, ITransientDependency
    {
        public ILogger Logger { protected get; set; }

        public IAntiForgeryConfiguration Configuration { get; }

        public InfrastructureAntiForgeryManager(IAntiForgeryConfiguration configuration)
        {
            Configuration = configuration;
            Logger = NullLogger.Instance;
        }

        public virtual string GenerateToken()
        {
            return Guid.NewGuid().ToString("D");
        }

        public virtual bool IsValid(string cookieValue, string tokenValue)
        {
            return cookieValue == tokenValue;
        }
    }
}
