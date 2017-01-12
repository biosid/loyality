using System;

namespace RapidSoft.Loaders.Geocoder.Exceptions
{
    public class GoogleGeocodingException : GeocodingException
    {
        public GoogleGeocodingException(string message) : base(message)
        {
        }

        public GoogleGeocodingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}