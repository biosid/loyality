using System;
using System.IO;
using System.Net;
using Vtb24.OnlineCategories.Client.Exceptions;

namespace Vtb24.OnlineCategories.Client.Internal
{
    internal static class HttpHelpers
    {
        public static byte[] Get(string url)
        {
            return SendRequest(url, "GET", null);
        }

        public static byte[] Post(string url, byte[] bodyBytes)
        {
            return SendRequest(url, "POST", bodyBytes);
        }

        private static byte[] SendRequest(string url, string method, byte[] bodyBytes)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = method;

                if (bodyBytes != null && bodyBytes.Length > 0)
                {
                    request.ContentLength = bodyBytes.Length;

                    using (var dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(bodyBytes, 0, bodyBytes.Length);
                    }
                }

                var response = (HttpWebResponse) request.GetResponse();

                var responseBytes = GetResponseBytes(response);

                return responseBytes;
            }
            catch (Exception ex)
            {
                throw new FailedToPerformHttpRequest(ex);
            }
        }

        private static byte[] GetResponseBytes(WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            using (var memory = new MemoryStream())
            {
                if (stream != null)
                {
                    stream.CopyTo(memory);
                }

                return memory.ToArray();
            }
        }
    }
}
