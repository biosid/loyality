using System.Web.Mvc;
using Vtb24.Site.Helpers;
using Vtb24.Site.Models.Shared;

namespace Vtb24.Site.Filters
{
    public class SetAdvertisementFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            AdvertisementModel viewModel;
            if (filterContext.Controller.TempData.TryGetActiveAdvertisement(out viewModel))
            {
                filterContext.Controller.ViewBag.ActiveAdvertisement = viewModel;
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}