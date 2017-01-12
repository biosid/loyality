using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Vtb24.Site.Services.Infrastructure.MvcHelpers
{
    public static class HighlightUrlsHelper
    {
        private const string PLACEHOLDER = "\xfff0";

        private static readonly string LocalUrl = ConfigurationManager.AppSettings["mymessages_site_url"];

        private static readonly Regex UrlRegex = new Regex(@"https?:\/\/[^\s<>!]+[^<>.,:;""')\]\s!]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex PlaceholderRegex = new Regex(PLACEHOLDER, RegexOptions.Compiled);

        public static IHtmlString HighlightLinks(this HtmlHelper htmlHelper, string text)
        {
            var sanitized = text.Replace(PLACEHOLDER, "");
            
            // извлекаем ссылки и заменяем их на символ-заглушку
            var links = new Queue<string>();
            var parsed = UrlRegex.Replace(
                sanitized,
                m => {
                    links.Enqueue(m.Value);
                    return m.Result(PLACEHOLDER);
                }
            );
            
            // экранируем HTML
            var encoded = htmlHelper.Encode(parsed);

            // возвращаем ссылки на свои места, превращая их в HTML
            var highlighted = PlaceholderRegex.Replace(
                encoded, 
                m => FormatLink(links.Dequeue())
            );

            return MvcHtmlString.Create(highlighted);
        }

        private static string FormatLink(string url)
        {
            return !string.IsNullOrEmpty(LocalUrl) && url.StartsWith(LocalUrl)
                ? string.Format("<a href=\"{0}\" >{0}</a>", url)
                : string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", url);
        }
    }
}