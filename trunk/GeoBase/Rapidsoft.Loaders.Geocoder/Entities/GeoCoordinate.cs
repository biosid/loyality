namespace RapidSoft.Loaders.Geocoder.Entities
{
    public class GeoCoordinate
    {
        private readonly decimal _longitude;
        private readonly decimal _latitute;

        public GeoCoordinate(decimal lat, decimal lng)
        {
            _longitude = lng;
            _latitute = lat;
        }

        public decimal Longitude
        {
            get
            {
                return _longitude;
            }
        }

        public decimal Latitude
        {
            get
            {
                return _latitute;
            }
        }

        #region Сравнения

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(GeoCoordinate) && Equals((GeoCoordinate) obj);
        }

        public bool Equals(GeoCoordinate obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj._longitude == _longitude && obj._latitute == _latitute;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_longitude.GetHashCode() * 397) ^ _latitute.GetHashCode();
            }
        }

        #endregion
    }
}