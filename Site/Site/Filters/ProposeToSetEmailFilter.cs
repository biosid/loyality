using System.Web.Mvc;
using Vtb24.Site.Helpers;

namespace Vtb24.Site.Filters
{
    public class ProposeToSetEmailFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Controller.TempData.ShowProposalToSetEmail())
            {
                filterContext.Controller.ViewBag.ProposeToSetEmail = true;
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
