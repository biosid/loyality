using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Serilog;
using Vtb24.Logging;
using Vtb24.Site.Security.SecurityService.Models.Exceptions;

namespace Vtb24.Site.Filters
{
    public class HandleAndLogErrorFilterAttribute : HandleErrorAttribute
    {
        private readonly ILogger _log = SerilogLoggers.MainLogger.ForContext<HandleAndLogErrorFilterAttribute>();

        public override void OnException(ExceptionContext filterContext)
        {
            // сценарий с несуществующим (удалённым) пользователем.
            // необходимо удалить сессионную cookie (без чего сайт будет неработоспособным)
            if (filterContext.Exception is UserNotFoundException)
            {
                FormsAuthentication.SignOut();
                filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl);
                return;
            }

            var httpException = filterContext.Exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                _log.LogInfo("Controller_HTTP_404", filterContext.Exception, null);
            }
            else
            {
                _log.LogError("Controller_Error", filterContext.Exception, null);
            }

            filterContext.HttpContext.Items["IsErrorLogged"] = true;

            base.OnException(filterContext);
        }
    }
}
