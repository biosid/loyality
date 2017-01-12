using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Controllers
{
    public class ActivationRequiredController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
