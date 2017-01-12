using System.Linq;
using System.Web;

namespace Vtb24.Logging
{
    public static class HttpHelpers
    {
        public static string GetRequestId()
        {
            var id = HttpContext.Current != null
                         ? HttpContext.Current.Items[Constants.HTTP_REQUEST_ID_ITEM] as string
                         : "N/A";
            return !string.IsNullOrEmpty(id) ? id : "N/A";
        }

        public static string GetRequestId(this HttpContext context)
        {
            var id = context.Items[Constants.HTTP_REQUEST_ID_ITEM] as string;
            return !string.IsNullOrEmpty(id) ? id : "N/A";
        }

        public static string GetUserName(this HttpContext context)
        {
            return context.User != null ? context.User.Identity.Name : "N/A";
        }

        /// <summary>
        /// Получить IP пользователя, с учётом возможных баллансировщиков
        /// </summary>
        public static string GetClientIpAddress(this HttpContext context)
        {
            var request = context.Request;

            string ip = null;

            if (request.ServerVariables.AllKeys.Contains("HTTP_X_FORWARDED_FOR"))
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new[] { ',', '\0', ' ', '\t' }).FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = request.UserHostAddress;
            }

            return ip == "::1" ? "127.0.0.1" : ip;
        }
    }
}
