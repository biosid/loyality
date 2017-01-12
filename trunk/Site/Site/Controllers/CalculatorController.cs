using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Controllers
{
    public class CalculatorController : BaseController
    {
        //
        // GET: /Calculator/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
