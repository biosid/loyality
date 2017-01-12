using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    public class HttpServiceRequest : IHttpServiceRequest
    {
        #region Конструкторы

        public HttpServiceRequest(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("Невалидный URL", "url");
            }

            Url = url;
            Timeout = 10000;
            Content = new QueryString();
        }

        public HttpServiceRequest(string url, X509Certificate2 certificate)
            : this(url)
        {
            Certificate = certificate;
        }

        #endregion

        #region API

        /// <summary>
        /// URL запроса
        /// </summary>
        public string Url
        {
            get;
            protected set;
        }

        /// <summary>
        /// SSL сертификат для защищённых запросов
        /// </summary>
        public X509Certificate2 Certificate
        {
            get;
            set;
        }

        /// <summary>
        /// Таймаут соединения в миллисекундах.
        /// По умолчанию 10000мс (10 секунд)
        /// </summary>
        public int Timeout
        {
            get;
            set;
        }

        /// <summary>
        /// Данные запроса, ключ-значение
        /// </summary>
        public QueryString Content
        {
            get;
            protected set;
        }

        /// <summary>
        /// Отправить запрос через GET.
        /// Данные запроса передаются через query string.
        /// </summary>
        public IHttpServiceResponse Get()
        {
            // подготовим URL для GET запроса
            var urlBuilder = new UrlBuilder(Url);
            urlBuilder.Query.Add(Content);
            var url = urlBuilder.Url;

            var request = CreateRequest(url, WebRequestMethods.Http.Get);

            try
            {
                var webResponse = (HttpWebResponse) request.GetResponse();
                return new HttpServiceResponse(webResponse);
            } catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.ProtocolError)
                {
                    throw;
                }

                var webResponse = (HttpWebResponse) e.Response;
                return new HttpServiceResponse(webResponse);
            }
        }

        /// <summary>
        /// Отправить запрос через POST.
        /// Данные запроса отправляются как application/x-www-form-urlencoded.
        /// </summary>
        public IHttpServiceResponse Post()
        {

            var request = CreateRequest(Url, WebRequestMethods.Http.Post);

            // подготовим данные для POST запроса
            var content = Content.ToString();
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;

            // сделаем запрос
            using (var requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.UTF8))
            {
                requestWriter.Write(content);
            }

            var webResponse = (HttpWebResponse) request.GetResponse();
            return new HttpServiceResponse(webResponse);
        }

        #endregion

        #region Вспомогательные методы

        private HttpWebRequest CreateRequest(string url, string method)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = Timeout;
            request.Method = method;
            if (Certificate != null)
            {
                request.ClientCertificates.Add(Certificate);
            }
            return request;
        }

        #endregion
    }
}