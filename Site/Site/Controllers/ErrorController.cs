using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Controllers
{
    public class ErrorController : BaseController
    {
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Error()
        {
            return View("Error", new HandleErrorInfo(HttpContext.Error, "Error", "Error"));
        }
    }
}
