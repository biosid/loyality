using System;
using System.Net;
using System.Threading;
using RapidSoft.Loaders.Geocoder.Entities.Google;
using RapidSoft.Loaders.Geocoder.Exceptions;
using RapidSoft.Loaders.Geocoder.Service;
using RapidSoft.Loaders.Geocoder.Service.WebServiceClient;

namespace RapidSoft.Loaders.Geocoder.Service.Google
{
    [GeoCodingServiceInfoAttribute(Name="Google")]
    public class GoogleGeocodingService : IGeocodingService
    {
        public const string GOOGLE_MAPS_SERVICE_URL = "http://maps.google.com/maps/geo";

        private readonly IHttpServiceRequestFactory _requestFactory;
        private DateTime _lastRequest;

        public GoogleGeocodingService() : this(null)
        {
            var config = new Configuration();
            RequestInterval = config.RequestInterval;
        }

        public GoogleGeocodingService(IHttpServiceRequestFactory requestFactory)
        {
            _requestFactory = requestFactory ?? new HttpServiceRequestFactory(GOOGLE_MAPS_SERVICE_URL);
            RequestInterval = 1000;
        }

        public int RequestInterval { get; set; }

        public GoogleGeocodingResponse ResolveAddress(string address)
        {
            if (!IsValidAdress(address))
            {
                throw new ArgumentNullException("address", "Адрес не задан");
            }

            var request = _requestFactory.Create();
            request.Content["q"] = address;
            request.Content["sensor"] = "false";    // нет датчика местоположения
            request.Content["output"] = "xml";      // формат ответа
            request.Content["oe"] = "utf8";         // кодеровка ответа

            /* var timeout = DateTime.Now.AddMilliseconds(-RequestInterval) - _lastRequest;
            if (timeout.Milliseconds > 0)
            {
                Thread.Sleep(timeout.Milliseconds);
            }*/

            if (RequestInterval > 0)
            {
                Thread.Sleep(RequestInterval);
            }

            _lastRequest = DateTime.Now;
            var response = request.Get();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new GoogleGeocodingException(String.Format("Сервис геокодинга вернул неожиданный HttpStatusCode: {0}, {1}", response.StatusCode, response.StatusDescription));
            }

            var geoResponse = GoogleGeocodingResponse.FromXMLString(response.GetResponseString());

            switch (geoResponse.StatusCode)
            {
                case StatusCode.Success:
                case StatusCode.UnavailibleAdress:
                case StatusCode.UnknownAdress:
                    break;
                case StatusCode.TooManyQueries:
                    throw new DailyRequestsLimitGeocodingException("Превышен дневной лимит запросов к сервису google maps");
                default:
                    throw new GoogleGeocodingException(String.Format("Ошибка геокодирования в google maps. StatusCode: {0}", response.StatusCode));
            }
            _lastRequest = DateTime.Now;
            return geoResponse;
        }

        public string ServiceName
        {
            get { return "Google"; }
        }

        public IGeocodingResponse FromXmlString(string xml)
        {
            return GoogleGeocodingResponse.FromXMLString(xml);
        }

        IGeocodingResponse IGeocodingService.ResolveAddress(string address)
        {
            return ResolveAddress(address);
        }

        private static bool IsValidAdress(string address)
        {
            return !String.IsNullOrEmpty((address ?? String.Empty).Trim(' ', '\n', '\r', '\t'));
        }
    }
}