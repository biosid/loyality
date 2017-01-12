using System.Web.Mvc;
using Vtb24.Site.Models.PersonalVideo;

namespace Vtb24.Site.Controllers
{
    public class PersonalVideoController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new PersonalVideoModel
            {
                Url = string.Format("http://vtb24.indicomm.ru/{0}", Request.Url.Query)
            };
            return View(model);
        }

    }
}
