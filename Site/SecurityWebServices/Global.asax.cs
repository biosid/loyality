using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Serilog;
using Vtb24.Logging;
using Vtb24.Site.SecurityWebServices.App_Start;

namespace Vtb24.Site.SecurityWebServices
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        private readonly ILogger _log = SerilogLoggers.MainLogger.ForContext<MvcApplication>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest()
        {
            Context.Items[Constants.HTTP_REQUEST_ID_ITEM] = Guid.NewGuid().ToString();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Context.AllErrors == null)
                return;

            foreach (var exception in Context.AllErrors)
                _log.LogError("Application_Error", exception, Context, null);
        }
    }
}