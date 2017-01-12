using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Serilog;
using Vtb24.Logging;
using Vtb24.Site.App_Start;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Security.SecurityService.Models.Exceptions;
using Vtb24.Site.Services;
using Vtb24.Site.Services.ClientService;

namespace Vtb24.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public const string IMAGE_RESIZER_URL = "/remote.jpg.ashx";

        private readonly ILogger _log = SerilogLoggers.MainLogger.ForContext<MvcApplication>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var container = UnityConfig.Initialize();
            CacheConfig.Configure(container);
            ContentConfig.Initialize();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, container);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBindersConfig.RegisterBinders(ModelBinders.Binders);

            // Убираем лишние проверки в Antiforgery
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // При старте сессии сразу определяем город пользователя и заносим его в cookie.

            var resolver = DependencyResolver.Current;
            var client = resolver.GetService<IClientService>();
            var cookie = resolver.GetService<CookieSessionStorage>();

            var location = client.GetUserLocation();
            cookie.SetLocation(location);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (IsImageResizerRequest())
            {
                return; // не логируем ошибки ресайзера
            }

            var exception = Server.GetLastError();
            var baseException = exception.GetBaseException();

            // сценарий с несуществующим (удалённым) или заблокированным пользователем.
            // необходимо удалить сессионную cookie (без чего сайт будет неработоспособным)
            if (baseException is UserNotFoundException ||
                baseException is UserDisabledException)
            {
                FormsAuthentication.SignOut();

                Context.ClearError();
                Response.Redirect(FormsAuthentication.LoginUrl);

                _log.LogWarn("Application_Warning", exception, Context, null);
                return;
            }

            if (Context.Items["IsErrorLogged"] != null)
            {
                return;
            }

            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                _log.LogInfo("Application_HTTP_404", exception, Context, null);
            }
            else
            {
                _log.LogError("Application_Error", exception, Context, null);
            }
        }

        protected void Application_BeginRequest()
        {
            if (IsImageResizerRequest())
            {
                return; // не логируем ошибки ресайзера
            }

            Context.Items[Constants.HTTP_REQUEST_ID_ITEM] = Guid.NewGuid().ToString();
        }

        protected void Application_EndRequest()
        {
            switch (Context.Response.StatusCode)
            {
                case 404:
                    // обрабатываем все ненайденные страницы
                    if (IsImageResizerRequest())
                    {
                        break; // не логируем ошибки ресайзера
                    }

                    Context.ProcessNotFoundRequest();
                    break;

                case 500:
                    // обрабатываем все ошибки, возникшие вне контроллера
                    if (IsImageResizerRequest())
                    {
                        break; // не логируем ошибки ресайзера
                    }

                    if (HttpContext.Current.Error != null)
                        Context.ProcessErrorRequest();
                    break;
            }
        }

        private bool IsImageResizerRequest()
        {
            return Request.Path == IMAGE_RESIZER_URL;
        }
    }
}
