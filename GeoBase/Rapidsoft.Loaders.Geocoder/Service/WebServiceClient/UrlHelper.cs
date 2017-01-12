using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    /// <summary>
    /// Класс для передачи параметров через QueryString
    /// </summary>
    public static class UrlHelper
    {

        #region Public methods
        public static string AddParameter(string source, string key, string value)
        {
            var sb = new StringBuilder(source);

            sb.Append(GetQueryGlue(source));

            sb.Append(key);
            sb.Append("=");
            sb.Append(value);

            return sb.ToString();
        }

        public static string GetParameter(NameValueCollection source, string key)
        {
            return source[key] ?? string.Empty;
        }

        public static string AddQuery(string url, IDictionary query)
        {
            var u = new StringBuilder(url);
            u.Append(GetQueryGlue(url));
            u.Append(BuildQuery(query));

            return u.ToString();
        }

        public static string AddQuery(string url, string query)
        {
            return url + GetQueryGlue(url) + query;
        }

        public static string SetQuery(string url, IDictionary query)
        {
            var u = new StringBuilder(StripQuery(url));
            u.Append("?");
            u.Append(BuildQuery(query));

            return u.ToString();
        }

        public static string SetQuery(string url, string query)
        {
            return StripQuery(url) + '?' + query;
        }

        #endregion

        #region Private methods
        private static char GetQueryGlue(string url)
        {
            return url.Contains("?") ? '&' : '?';
        }

        private static string StripQuery(string url)
        {
            var glueIndex = url.IndexOf('?');
            return glueIndex != -1 ? url.Substring(0, glueIndex) : url;
        }

        private static string BuildQuery(IDictionary query)
        {
            var q = new StringBuilder();
            var isFirst = true;
            foreach (string key in query.Keys)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    q.Append("&");
                }

                q.Append(key);
                q.Append("=");
                q.Append(query[key]);
            }
            return q.ToString();
        }
        #endregion
    }
}