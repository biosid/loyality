using System;
using System.Collections.Generic;
using System.Web;
using Serilog;

namespace Vtb24.Logging
{
    public static class SerilogHelpers
    {
        public static void LogError(this ILogger log, string title, string message, HttpContext httpContext, object context)
        {
            log.Error(GetTemplate(title), GetHttpContext(httpContext), context, message);
        }

        public static void LogError(this ILogger log, string title, string message, object context)
        {
            LogError(log, title, message, HttpContext.Current, context);
        }

        public static void LogError(this ILogger log, string title, Exception exception, HttpContext httpContext, object context)
        {
            LogError(log, title, exception.Format(), httpContext, context);
        }

        public static void LogError(this ILogger log, string title, Exception exception, object context)
        {
            LogError(log, title, exception.Format(), HttpContext.Current, context);
        }


        public static void LogWarn(this ILogger log, string title, string message, HttpContext httpContext, object context)
        {
            log.Warning(GetTemplate(title), GetHttpContext(httpContext), context, message);
        }

        public static void LogWarn(this ILogger log, string title, string message, object context)
        {
            LogWarn(log, title, message, HttpContext.Current, context);
        }

        public static void LogWarn(this ILogger log, string title, Exception exception, HttpContext httpContext, object context)
        {
            LogWarn(log, title, exception.Format(), httpContext, context);
        }

        public static void LogWarn(this ILogger log, string title, Exception exception, object context)
        {
            LogWarn(log, title, exception.Format(), HttpContext.Current, context);
        }


        public static void LogInfo(this ILogger log, string title, string message, HttpContext httpContext, object context)
        {
            log.Information(GetTemplate(title), GetHttpContext(httpContext), context, message);
        }

        public static void LogInfo(this ILogger log, string title, string message, object context)
        {
            LogInfo(log, title, message, HttpContext.Current, context);
        }

        public static void LogInfo(this ILogger log, string title, Exception exception, HttpContext httpContext, object context)
        {
            LogInfo(log, title, exception.Format(), httpContext, context);
        }

        public static void LogInfo(this ILogger log, string title, Exception exception, object context)
        {
            LogInfo(log, title, exception.Format(), HttpContext.Current, context);
        }

        private const string MESSAGE_TEMPLATE = @"
HTTP context: {@HttpContext}
Context: {@MsgContext}
{LogMessage}
";

        private static readonly Dictionary<string, string> Templates = new Dictionary<string, string>();

        private static string GetTemplate(string title)
        {
            string template;

            if (!Templates.TryGetValue(title, out template))
            {
                template = title + MESSAGE_TEMPLATE + "END OF " + title + Environment.NewLine;
                Templates[title] = template;
            }

            return template;
        }

        private static object GetHttpContext(HttpContext context)
        {
            if (context == null)
            {
                return null;
            }

            var referer = context.Request.UrlReferrer;

            return new
            {
                Method = context.Request.HttpMethod,
                Url = context.Request.RawUrl,
                ClientIp = context.GetClientIpAddress(),
                UserName = context.GetUserName(),
                RequestId = context.GetRequestId(),
                Referer = referer != null ? referer.ToString() : "N/A"
            };
        }
    }
}
