using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Serilog;
using Vtb24.Logging;
using Vtb24.Logging.Interaction;
using Vtb24.Site.Services;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Infrastructure
{
    public abstract class BaseController : Controller
    {
        private readonly ILogger _log = SerilogLoggers.MainLogger.ForContext<BaseController>();

        #region Логирование

        public void LogError(Exception e, object context = null)
        {
            _log.LogError("Logged_Error", e, context);
        }

        public void LogError(string message, object context = null)
        {
            _log.LogError("Logged_Error", message, context);
        }

        public void LogWarn(Exception e, object context = null)
        {
            _log.LogWarn("Logged_Warning", e, context);
        }

        public void LogWarn(string message, object context = null)
        {
            _log.LogWarn("Logged_Warning", message, context);
        }

        public void LogInfo(Exception e, object context = null)
        {
            _log.LogInfo("Application_Info", e, context);
        }

        public void LogInfo(string message, object context = null)
        {
            _log.LogInfo("Application_Info", message, context);
        }

        public void LogInteraction(IInteractionLogEntry logEntry)
        {
            logEntry.FinishAndWrite(_log);
        }

        #endregion

        /// <summary>
        /// Редирект с фрагментом (#) в URL
        /// </summary>
        public ActionResult RedirectToActionWithFragment(string action, string controller, object routeValues = null, string fragment = null)
        {
            var routeValueDict = routeValues == null ? null : HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            var url = UrlHelper.GenerateUrl(
                null, // routeName
                action, // actionName
                controller, // controllerName
                null, // protocol
                null, // hostName
                fragment, // fragment
                routeValueDict, // routeValues
                RouteTable.Routes, // routeCollection
                ControllerContext.RequestContext, // requestContext
                false // includeImplicitMvcValues
            );

            return Redirect(url);
        }

        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Main");
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

        public Task<T> Async<T>(Func<T> action)
        {
            var context = System.Web.HttpContext.Current;
            return Task.Factory.StartNew(
                () =>
                {
                    System.Web.HttpContext.Current = context;
                    return action();
                }
            );
        }

        // TODO: удалить этот хак
        public void DumpActivationStatusToViewBag(IClientService client)
        {
            ViewBag.ClientActivationRequired =
                HttpContext.User.Identity.IsAuthenticated &&
                client.GetStatus() != ClientStatus.Activated;
        }
    }
}
