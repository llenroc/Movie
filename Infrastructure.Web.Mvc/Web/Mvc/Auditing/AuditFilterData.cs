using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Infrastructure.Auditing;

namespace Infrastructure.Web.Mvc.Auditing
{
    public class AuditFilterData
    {
        private const string AuditFilterDataHttpContextKey = "__AuditFilterData";

        public Stopwatch Stopwatch { get; }

        public AuditInfo AuditInfo { get; }

        public AuditFilterData(Stopwatch stopwatch,AuditInfo auditInfo)
        {
            Stopwatch = stopwatch;
            AuditInfo = auditInfo;
        }

        public static void Set(HttpContextBase httpContext, AuditFilterData auditFilterData)
        {
            GetAuditDataStack(httpContext).Push(auditFilterData);
        }

        public static AuditFilterData GetOrNull(HttpContextBase httpContext)
        {
            var stack = GetAuditDataStack(httpContext);
            return stack.Count <= 0? null: stack.Pop();
        }

        private static Stack<AuditFilterData> GetAuditDataStack(HttpContextBase httpContext)
        {
            var stack = httpContext.Items[AuditFilterDataHttpContextKey] as Stack<AuditFilterData>;

            if (stack == null)
            {
                stack = new Stack<AuditFilterData>();
                httpContext.Items[AuditFilterDataHttpContextKey] = stack;
            }
            return stack;
        }
    }
}
