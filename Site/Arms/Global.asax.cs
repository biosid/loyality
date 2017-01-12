using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Serilog;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions;
using Vtb24.Arms.App_Start;
using Vtb24.Logging;

namespace Vtb24.Arms
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private readonly ILogger _log = SerilogLoggers.MainLogger.ForContext<MvcApplication>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            UnityConfig.Initialise();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBindersConfig.RegisterBinders(ModelBinders.Binders);

            // Оставляем валидацию только с помощью дата аннотаций, убирая дефолтную на базовые типы
            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(new DataAnnotationsModelValidatorProvider());

            // Кастомизируем имя cookie для AntiForgery токена (чтобы избежать коллизий с другими сайтами в домене)
            AntiForgeryConfig.CookieName = "__ArmsRequestVerificationToken";
            // Убираем лишние проверки в Antiforgery
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
        }

        protected void Application_BeginRequest()
        {
            Context.Items[Constants.HTTP_REQUEST_ID_ITEM] = Guid.NewGuid().ToString();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            var security = DependencyResolver.Current.GetService<IAdminSecurityService>();

            if (security.VerifyCurrentUser())
                return;

            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Context.AllErrors == null)
                return;

            var accessDenied = false;

            foreach (var exception in Context.AllErrors)
            {
                var httpException = exception as HttpException;

                if (httpException != null && httpException.GetHttpCode() == 404)
                {
                    _log.LogInfo("Application_HTTP_404", exception, Context, null);
                }
                else if (exception is AccessDeniedException)
                {
                    accessDenied = true;
                    _log.LogInfo("Application_AccessDenied", exception, Context, null);
                }
                else
                {
                    _log.LogError("Application_Error", exception, Context, null);
                }
            }

            if (!accessDenied)
                return;

            Context.ClearError();
            Response.RedirectToRoute("Default",
                                     new
                                     {
                                         controller = "Home",
                                         action = "AccessDenied",
                                         area = ""
                                     });
        }
    }
}