using System.Web.Mvc;
using Vtb24.Site.Services;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Filters
{
    public class SetClientActivationFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // REVIEW: надо покурить
            filterContext.Controller.ViewBag.ClientActivationRequired =
                filterContext.HttpContext.User.Identity.IsAuthenticated &&
                DependencyResolver.Current.GetService<IClientService>().GetStatus() != ClientStatus.Activated;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
