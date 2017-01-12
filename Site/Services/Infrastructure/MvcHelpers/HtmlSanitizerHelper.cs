using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Vtb24.Site.Services.Infrastructure.MvcHelpers
{
    public static class HtmlSanitizerHelper
    {
        public static string StripHtml(this HtmlHelper htmlHelper, string text)
        {
            return HttpUtility.HtmlDecode(HtmlSanitizer.StripHtmlTags(text));
        }

        public static IHtmlString SanitizeHtml(this HtmlHelper htmlHelper, string text, HtmlSanitizerPresets preset = HtmlSanitizerPresets.Default)
        {
            var whitelist = preset.GetWhitelist();

            var result = HtmlSanitizer.SanitizeHtml(text, whitelist);
            return MvcHtmlString.Create(result);
        }

    }
}