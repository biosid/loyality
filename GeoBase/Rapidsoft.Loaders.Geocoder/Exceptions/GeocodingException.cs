using System;

namespace RapidSoft.Loaders.Geocoder.Exceptions
{
    public class GeocodingException : Exception
    {
        public GeocodingException(string message) : base(message)
        {
        }

        public GeocodingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class DailyRequestsLimitGeocodingException : GeocodingException
    {
        public DailyRequestsLimitGeocodingException(string message) : base(message)
        {
        }

        public DailyRequestsLimitGeocodingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}