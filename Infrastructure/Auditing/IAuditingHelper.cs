using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Infrastructure.Auditing
{
    public interface IAuditingHelper
    {
        bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false);

        AuditInfo CreateAuditInfo(MethodInfo method, object[] arguments);
        AuditInfo CreateAuditInfo(MethodInfo method, IDictionary<string, object> arguments);

        void Save(AuditInfo auditInfo);
        Task SaveAsync(AuditInfo auditInfo);
    }
}
