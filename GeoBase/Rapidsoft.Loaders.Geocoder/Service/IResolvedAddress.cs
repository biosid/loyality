using RapidSoft.GeoPoints.Entities;
using RapidSoft.Loaders.Geocoder.Entities;

namespace RapidSoft.Loaders.Geocoder.Service
{
    public interface IResolvedAddress
    {
        string Address { get; }

        GeoCoordinate Coordinate { get; }

        GeoCodingAccuracy Accuracy { get; }
    }
}