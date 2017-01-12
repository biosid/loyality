using System;

namespace RapidSoft.Loaders.Geocoder.Exceptions
{
    public class YandexGeocodingException : GeocodingException
    {
        public YandexGeocodingException(string message)
            : base(message)
        {
        }

        public YandexGeocodingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}