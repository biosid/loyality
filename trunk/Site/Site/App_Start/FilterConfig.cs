using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Vtb24.Site.Filters;

namespace Vtb24.Site.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters, IUnityContainer container)
        {
            filters.Add(new HandleAndLogErrorFilterAttribute());
            filters.Add(container.Resolve<SeoFilter>());
            filters.Add(container.Resolve<SetAdvertisementFilter>());
            filters.Add(container.Resolve<ProposeToSetEmailFilter>());
            //filters.Add(new SetClientActivationFilter());
        }
    }
}
