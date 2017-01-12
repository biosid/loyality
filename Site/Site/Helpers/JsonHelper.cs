using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Vtb24.Site.Helpers
{
    public static class JsonHelper
    {
        public static MvcHtmlString Json(this HtmlHelper htmlHelper, object obj)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(obj);
            return MvcHtmlString.Create(json);
        }
    }
}