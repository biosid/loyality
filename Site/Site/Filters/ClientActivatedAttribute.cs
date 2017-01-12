using System.Web.Mvc;
using System.Web.Routing;
using Vtb24.Site.Services;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Filters
{
    public class ClientActivatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                return;
            }

            if (IsClientActivated())
                return;

            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", "ActivationRequired" }
            });
        }

        public static bool IsClientActivated()
        {
            var clientService = DependencyResolver.Current.GetService<IClientService>();
            return clientService.GetStatus() == ClientStatus.Activated;
        }
    }
}
