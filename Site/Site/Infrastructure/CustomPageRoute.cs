using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Vtb24.Site.Content.Pages;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Pages.Models.Inputs;

namespace Vtb24.Site.Infrastructure
{
    public class CustomPageRoute : RouteBase
    {
        public CustomPageRoute(IPagesManagement pages)
        {
            _pages = pages;
        }

        private readonly IPagesManagement _pages;

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var url = httpContext.Request.Path;

            // TODO: переписать с использованием кэша, который будет периодически обновляться
            var options = new GetPlainPagesOptions { IsBuiltin = false, Status = PageStatus.Active };
            var page = _pages.GetPageByUrl(url, options);
            if (page == null)
                return null;

            var routeData = new RouteData(this, new MvcRouteHandler());

            routeData.Values.Add("controller", "CustomPages");
            switch (page.Type)
            {
                case PageType.Plain:
                    routeData.Values.Add("action", "ShowPlain");
                    routeData.Values.Add("id", page.Id);
                    break;

                case PageType.Offer:
                    var id = int.Parse(page.ExternalId);
                    routeData.Values.Add("action", "ShowOffer");
                    routeData.Values.Add("id", id);
                    break;
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}
