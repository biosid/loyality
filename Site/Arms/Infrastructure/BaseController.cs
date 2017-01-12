using System.IO;
using System.Web.Mvc;

namespace Vtb24.Arms.Infrastructure
{
    public abstract class BaseController : Controller
    {
        public JsonResult JsonErrors(object data = null)
        {
            if (ModelState.IsValid)
            {
                return Json(new {success = true, data});
            }

            var errors = ModelState.GetAllErrors();
            //Response.StatusCode = (int) System.Net.HttpStatusCode.BadRequest;
            return Json(new { success = false, data, errors});
        }

        public JsonResult JsonSuccess(object data = null)
        {
            return Json(new { success = true, data });
        }

        public string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        public string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
