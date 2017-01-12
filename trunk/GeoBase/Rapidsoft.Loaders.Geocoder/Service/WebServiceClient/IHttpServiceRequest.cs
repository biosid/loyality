using System.Security.Cryptography.X509Certificates;

namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    public interface IHttpServiceRequest {
        /// <summary>
        /// URL запроса
        /// </summary>
        string Url { get; }

        /// <summary>
        /// SSL сертификат для защищённых запросов
        /// </summary>
        X509Certificate2 Certificate { get; set; }

        /// <summary>
        /// Таймаут соединения в миллисекундах.
        /// По умолчанию 10000мс (10 секунд)
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Данные запроса, ключ-значение
        /// </summary>
        QueryString Content { get; }

        /// <summary>
        /// Отправить запрос через GET.
        /// Данные запроса передаются через query string.
        /// </summary>
        IHttpServiceResponse Get();

        /// <summary>
        /// Отправить запрос через POST.
        /// Данные запроса отправляются как application/x-www-form-urlencoded.
        /// </summary>
        IHttpServiceResponse Post();
    }
}