using System;
using System.Net;
using System.Threading;
using RapidSoft.Loaders.Geocoder;
using RapidSoft.Loaders.Geocoder.Exceptions;
using RapidSoft.Loaders.Geocoder.Service.WebServiceClient;

namespace RapidSoft.Loaders.Geocoder.Service.Yandex
{
    [GeoCodingServiceInfoAttribute(Name = "Yandex")]
    public class YandexGeocodingService : IGeocodingService
    {
        public const string GEOCODER_SERVICE_URL = "http://geocode-maps.yandex.ru/1.x/";

        private readonly IHttpServiceRequestFactory _requestFactory;
        private readonly string _key;
        private DateTime _lastRequest;
        Configuration config = new Configuration();

        public YandexGeocodingService()
        {
            _key = config.YandexKey;
            _requestFactory = new HttpServiceRequestFactory(GEOCODER_SERVICE_URL);
            RequestInterval = 1000;            
        }
        
        public YandexGeocodingService(string key, IHttpServiceRequestFactory factory)
        {
            _key = key;
            _requestFactory = factory ?? new HttpServiceRequestFactory(GEOCODER_SERVICE_URL);
            RequestInterval = 1000;
        }

        public string Key
        {
            get
            {
                return _key;
            }
        }

        public int RequestInterval
        {
            get; set;
        }

        public YandexGeocodingResponse ResolveAddress(string address, Excerpt resultsExcerpt)
        {
            if (!IsValidAdress(address))
            {
                throw new ArgumentNullException("address", "Адрес не задан");
            }

            var request = _requestFactory.Create();
            request.Content["geocode"] = address;
            request.Content["key"] = _key;
            if (resultsExcerpt != null)
            {
                request.Content["results"] = resultsExcerpt.Size.ToString();
                request.Content["skip"] = resultsExcerpt.Offset.ToString();
            }

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
                throw new YandexGeocodingException(
                    String.Format(
                        "Сервис геокодинга вернул неожиданный HttpStatusCode: {0}, {1}",
                        response.StatusCode,
                        response.StatusDescription));
            }

            var geoResponse = YandexGeocodingResponse.FromXmlString(response.GetResponseString());
            _lastRequest = DateTime.Now;
            return geoResponse;
        }

        public YandexGeocodingResponse ResolveAddress(string address)
        {
            return ResolveAddress(address, null);
        }

        public string ServiceName
        {
            get { return "Yandex"; }
        }

        public IGeocodingResponse FromXmlString(string xml)
        {
            return YandexGeocodingResponse.FromXmlString(xml);
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