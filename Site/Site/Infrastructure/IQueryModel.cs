using System.Web.Routing;

namespace Vtb24.Site.Infrastructure
{
    public interface IQueryModel
    {
        RouteValueDictionary ToRouteValueDictionary();
    }
}