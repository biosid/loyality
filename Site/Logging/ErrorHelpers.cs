using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Vtb24.Logging
{
    public static class ErrorHelpers
    {
        public class ErrorSource
        {
            public string FileName { get; set; }

            public string[] Lines { get; set; }

            public int FirstLineNumber { get; set; }

            public int ErrorLineIndex { get; set; }

            public string ErrorLine
            {
                get { return (FirstLineNumber + ErrorLineIndex).ToString("d5") + ": " + Lines[ErrorLineIndex]; }
            }

            public IEnumerable<string> PreErrorLines
            {
                get
                {
                    return Lines.Take(ErrorLineIndex)
                                .Select((line, index) => (FirstLineNumber + index).ToString("d5") + ": " + line);
                }
            }

            public IEnumerable<string> PostErrorLines
            {
                get
                {
                    return Lines.Skip(ErrorLineIndex + 1)
                                .Select((line, index) => (FirstLineNumber + ErrorLineIndex + index + 1).ToString("d5") + ": " + line);
                }
            }
        }

        public static ErrorSource GetErrorSource(this Exception exception)
        {
            const int NUMBER_OF_LINES_AROUND = 10;

            var trace = new StackTrace(exception, true);

            var frame = Enumerable.Range(0, trace.FrameCount)
                      .Select(trace.GetFrame)
                      .FirstOrDefault(f => f.GetFileName() != null && File.Exists(f.GetFileName()));

            if (frame == null)
            {
                return null;
            }

            var errorLineNumber = frame.GetFileLineNumber();
            var errorSource = new ErrorSource
            {
                FileName = frame.GetFileName(),
                ErrorLineIndex = Math.Min(errorLineNumber - 1, NUMBER_OF_LINES_AROUND),
                FirstLineNumber = Math.Max(errorLineNumber - NUMBER_OF_LINES_AROUND, 1)
            };

            errorSource.Lines = File.ReadLines(errorSource.FileName)
                                    .Skip(errorSource.FirstLineNumber - 1)
                                    .Take(NUMBER_OF_LINES_AROUND * 2 + 1)
                                    .ToArray();

            return errorSource;
        }

        public static string Format(this Exception ex, string title, HttpContext context, params Tuple<string, string>[] vars)
        {
            return string.Join("\r\n", GenerateErrorLines(ex, title, context, vars));
        }

        public static string Format(string message, string title, HttpContext context, params Tuple<string, string>[] vars)
        {
            return string.Join("\r\n", GenerateErrorLines(message, title, context, vars));
        }

        public static string Format(this Exception ex)
        {
            return string.Join(Environment.NewLine, ex.GenerateExceptionLines());
        }

        #region приватные методы

        private static IEnumerable<string> GenerateErrorLines(string message, string title, HttpContext context, Tuple<string, string>[] vars)
        {
            return new[] { title }.Concat(context.GenerateContextLines())
                                  .Concat(vars.GenerateVarsLines())
                                  .Concat( new [] { message } )
                                  .Concat(new[] { "END OF " + title, string.Empty, string.Empty });
        }

        private static IEnumerable<string> GenerateErrorLines(Exception ex, string title, HttpContext context, Tuple<string, string>[] vars)
        {
            return new[] { title }.Concat(context.GenerateContextLines())
                                  .Concat(vars.GenerateVarsLines())
                                  .Concat(ex.GenerateExceptionLines())
                                  .Concat(new[] { "END OF " + title, string.Empty, string.Empty });
        }

        private static IEnumerable<string> GenerateContextLines(this HttpContext context)
        {
            yield return " CONTEXT:";
            if (context != null)
            {
                yield return FormatVar("Method", context.Request.HttpMethod);
                yield return FormatVar("Request URL", context.Request.RawUrl);
                yield return FormatVar("client IP", GetClientIpAddress(context.Request));
                yield return FormatVar("user name", context.User != null ? context.User.Identity.Name : "N/A");
                yield return FormatVar("request ID", context.Items[Constants.HTTP_REQUEST_ID_ITEM] as string);
            }
            else
            {
                yield return "   null";
            }
        }

        private static IEnumerable<string> GenerateVarsLines(this Tuple<string, string>[] vars)
        {
            return vars != null && vars.Length > 0
                       ? vars.Select(v => FormatVar(v.Item1, v.Item2))
                       : Enumerable.Empty<string>();
        }

        private static IEnumerable<string> GenerateExceptionLines(this Exception ex)
        {
            if (ex == null)
            {
                return new[] { " EXCEPTION", "   null" };
            }

            var result = new[] { " EXCEPTION" }.Concat(ex.GenerateSingleExceptionLines());
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                result = result.Concat(new[] { " INNER EXCEPTION" })
                               .Concat(ex.GenerateSingleExceptionLines());
            }
            return result;
        }

        private static IEnumerable<string> GenerateSingleExceptionLines(this Exception ex)
        {
            var lines = new[]
            {
                "   TYPE: " + ex.GetType().FullName,
                "   MESSAGE: " + ex.Message,
                "   STACK:" 
            };
            return ex.StackTrace != null
                       ? lines.Concat(ex.StackTrace
                                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(l => "     " + l))
                       : lines.Concat(new[] { "     - N/A -" });
        }

        private static string FormatVar(string key, string value)
        {
            return (key ?? string.Empty).PadLeft(20) + " = " + value;
        }

        /// <summary>
        /// Получить IP пользователя, с учётом возможных баллансировщиков
        /// </summary>
        private static string GetClientIpAddress(HttpRequest request)
        {
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

        #endregion
    }
}
