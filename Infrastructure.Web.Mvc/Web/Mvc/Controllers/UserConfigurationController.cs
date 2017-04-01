using System.Threading.Tasks;
using System.Web.Mvc;
using Infrastructure.Web.Configuration;

namespace Infrastructure.Web.Mvc.Controllers
{
    public class UserConfigurationController : InfrastructureController
    {
        private readonly UserConfigurationBuilder _UserConfigurationBuilder;

        public UserConfigurationController(UserConfigurationBuilder UserConfigurationBuilder)
        {
            _UserConfigurationBuilder = UserConfigurationBuilder;
        }

        public async Task<JsonResult> GetAll()
        {
            var userConfig = await _UserConfigurationBuilder.GetAll();
            return Json(userConfig, JsonRequestBehavior.AllowGet);
        }
    }
}
