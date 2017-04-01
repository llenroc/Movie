using System.Text;
using Infrastructure.Dependency;
using Infrastructure.Runtime.Session;

namespace Infrastructure.Web.Sessions
{

    public class SessionScriptManager : ISessionScriptManager, ITransientDependency
    {
        public IInfrastructureSession Session { get; set; }

        public SessionScriptManager()
        {
            Session = NullInfrastructureSession.Instance;
        }

        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine();

            script.AppendLine("    infrastructure.session = infrastructure.session || {};");
            script.AppendLine("    infrastructure.session.userId = " + (Session.UserId.HasValue ? Session.UserId.Value.ToString() : "null") + ";");
            script.AppendLine("    infrastructure.session.tenantId = " + (Session.TenantId.HasValue ? Session.TenantId.Value.ToString() : "null") + ";");
            script.AppendLine("    infrastructure.session.impersonatorUserId = " + (Session.ImpersonatorUserId.HasValue ? Session.ImpersonatorUserId.Value.ToString() : "null") + ";");
            script.AppendLine("    infrastructure.session.impersonatorTenantId = " + (Session.ImpersonatorTenantId.HasValue ? Session.ImpersonatorTenantId.Value.ToString() : "null") + ";");
            script.AppendLine("    infrastructure.session.multiTenancySide = " + ((int)Session.MultiTenancySide) + ";");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}
