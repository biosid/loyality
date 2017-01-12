using System.Web.Mvc;

namespace Rapidsoft.VTB24.Reports.Site.Controllers
{
    public class MainController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}
