namespace RapidSoft.Loaders.Geocoder.Entities
{
    public class GeocodingCache
    {
        protected GeocodingCache()
        {}

        public GeocodingCache(string address, string rawResponse)
        {
            Address = address;
            RawResponse = rawResponse;
        }

        public static readonly string GeocodingCacheTablePattern = "Geopoints.BUFFER_{0}GeocodingCache";

        public virtual int ID { get; set; }

        public virtual string Address { get; set; }

        public virtual string RawResponse { get; set; }
    }
}