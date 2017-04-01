using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{
    public interface IAuthorizationHelper
    {
        Task AuthorizeAsync(IEnumerable<IInfrastructureAuthorizeAttribute> authorizeAttributes);

        Task AuthorizeAsync(MethodInfo methodInfo);
    }
}
