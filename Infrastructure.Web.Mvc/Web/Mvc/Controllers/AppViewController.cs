using System.Web.Mvc;
using Infrastructure.Auditing;
using Infrastructure.Domain.UnitOfWork;
using Infrastructure.Extensions;
using Infrastructure.Runtime.Validation;

namespace Infrastructure.Web.Mvc.Controllers
{
    public class AppViewController : InfrastructureController
    {
        [DisableAuditing]
        [DisableValidation]
        [UnitOfWork(IsDisabled = true)]
        public ActionResult Load(string viewUrl)
        {
            return View(viewUrl.EnsureStartsWith('~'));
        }
    }
}
