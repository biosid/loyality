using RapidSoft.GeoPoints.Entities;
using RapidSoft.Loaders.Geocoder.Entities;
using RapidSoft.Loaders.Geocoder.Entities.Yandex;

namespace RapidSoft.Loaders.Geocoder.Service.Yandex
{
    public class YandexResolvedAddress : IResolvedAddress
    {
        private readonly string _address;
        private readonly GeoCoordinate _coordinate;
        private readonly AddressKind _kind;
        private readonly GeocodingPrecision _precision;

        public YandexResolvedAddress(string addr, GeoCoordinate coord, AddressKind kind, GeocodingPrecision precision)
        {
            _address = addr;
            _coordinate = coord;
            _kind = kind;
            _precision = precision;
        }

        public string Address
        {
            get
            {
                return _address;
            }
        }

        public GeoCoordinate Coordinate
        {
            get
            {
                return _coordinate;
            }
        }

        GeoCodingAccuracy IResolvedAddress.Accuracy
        {
            get
            {
                switch (Kind)
                {
                    case AddressKind.House:
                        return GeoCodingAccuracy.House;
                    case AddressKind.Street:
                    case AddressKind.Bridge:
                    case AddressKind.Metro:
                        return GeoCodingAccuracy.Street;
                    case AddressKind.District:
                    case AddressKind.Cemetery:
                        return GeoCodingAccuracy.District;
                    case AddressKind.City:
                        return GeoCodingAccuracy.City;
                    case AddressKind.Area:
                        return GeoCodingAccuracy.Area;
                    case AddressKind.Province:
                        return GeoCodingAccuracy.Province;
                    case AddressKind.Country:
                        return GeoCodingAccuracy.Country;
                    case AddressKind.Hydro:
                    case AddressKind.Railway:
                    case AddressKind.Route:
                    case AddressKind.Vegetation:
                    case AddressKind.Km:
                    case AddressKind.Other:
                        return GeoCodingAccuracy.Unknown;
                    default:
                        return GeoCodingAccuracy.Unknown;
                }
            }
        }

        public AddressKind Kind
        {
            get
            {
                return _kind;
            }
        }

        public GeocodingPrecision Precision
        {
            get
            {
                return _precision;
            }
        }
    }
}