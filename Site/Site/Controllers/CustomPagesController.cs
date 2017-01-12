using System;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Vtb24.Site.Content.Pages;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Controllers
{
    public class CustomPagesController : BaseController
    {
        public CustomPagesController(IPagesManagement pages)
        {
            _pages = pages;
        }

        private readonly IPagesManagement _pages;

        [HttpGet]
        public ActionResult ShowPlain(Guid id)
        {

            var page = _pages.GetPlainPageById(id);

            if (page == null)
            {
                throw new HttpException(404, "страница не найдена");
            }

            return View("Show", page.CurrentVersion.Data);
        }

        [HttpGet]
        public ActionResult ShowOffer(int id)
        {
            var page = _pages.GetOfferPageByPartnerId(id, false);
            if (page == null)
            {
                throw new HttpException(404, "страница не найдена");
            }

            return View("Show", page.CurrentVersion.Data);
        }

        [HttpGet]
        public ActionResult ShowVersion(Guid id)
        {
            var pageData = _pages.GetPageVersionById(id);
            if (pageData == null)
            {
                throw new HttpException(404, "страница не найдена");
            }

            return View("Show", pageData);
        }

        #if DEBUG
        [HttpGet]
        public ActionResult Local(string id)
        {
            var path = Server.MapPath(string.Format("~/App_Data/LocalCustomPages/{0}.html", id));
            var scriptsPath = Server.MapPath(string.Format("~/App_Data/LocalCustomPages/{0}_scripts.html", id));

            if (!Request.IsLocal || !System.IO.File.Exists(path))
            {
                throw new HttpException(404, "страница не найдена");
            }

            var model = new Content.Pages.Models.PageData()
            {
                Title = string.Format("Страница {0}", id),
                Content = System.IO.File.ReadAllText(path) ,
                Script = System.IO.File.Exists(scriptsPath) ? System.IO.File.ReadAllText(scriptsPath) : "",
            };

            return View("Show", model);
        }
        #endif
    }
}
