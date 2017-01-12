using RapidSoft.GeoPoints.Entities;
using RapidSoft.Loaders.Geocoder.Entities;
using RapidSoft.Loaders.Geocoder.Entities.Google;

namespace RapidSoft.Loaders.Geocoder.Service.Google
{
    public class GoogleResolvedAddress : IResolvedAddress
    {
        private readonly GeocodingAccuracy _accuracy;
        private readonly string _address;
        private readonly GeoCoordinate _coordinate;

        public GoogleResolvedAddress(string adress, GeoCoordinate coordinate, GeocodingAccuracy accuracy)
        {
            _accuracy = accuracy;
            _address = adress;
            _coordinate = coordinate;
        }

        public GeocodingAccuracy Accuracy
        {
            get
            {
                return _accuracy;
            }
        }

        GeoCodingAccuracy IResolvedAddress.Accuracy
        {
            get
            {
                switch (Accuracy)
                {
                    case GeocodingAccuracy.Country:
                        return GeoCodingAccuracy.Country;
                    case GeocodingAccuracy.Region:
                        return GeoCodingAccuracy.Province;
                    case GeocodingAccuracy.SubRegion:
                        return GeoCodingAccuracy.Area;
                    case GeocodingAccuracy.Town:
                        return GeoCodingAccuracy.City;
                    case GeocodingAccuracy.PostCode:
                        return GeoCodingAccuracy.District;
                    case GeocodingAccuracy.Street:
                    case GeocodingAccuracy.Intersection:
                    case GeocodingAccuracy.Address:
                        return GeoCodingAccuracy.Street;
                    case GeocodingAccuracy.Premise:
                        return GeoCodingAccuracy.House;
                    default:
                        return GeoCodingAccuracy.Unknown;
                }
            }
        }

        public GeoCoordinate Coordinate
        {
            get
            {
                return _coordinate;
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
        }
    }
}