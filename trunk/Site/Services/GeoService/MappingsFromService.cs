using Vtb24.Site.Services.GeoService.Models;
using Location = Vtb24.Site.Services.GeoPointsService.Location;

namespace Vtb24.Site.Services.GeoService
{
    internal static class MappingsFromService
    {

        public static GeoLocation ToGeoLocation(Location original)
        {
            if (original == null)
            {
                return null;
            }

            var location =  new GeoLocation
            {
                KladrCode = original.KladrCode,
                Name = Normalize(original.Name),
                Toponym = Normalize(original.Toponym),
                RegionName = Normalize(original.RegionName),
                RegionToponym = Normalize(original.RegionToponym),
                RegionKladrCode = original.RegionKladrCode,
                DistrictName = Normalize(original.DistrictName),
                DistrictToponim = Normalize(original.DistrictToponym),
                DistrictKladrCode = original.DistrictKladrCode,
                Type = ToGeoLocationType(original.LocationType)
            };

            return location;
        }

        public static GeoLocationType ToGeoLocationType(int original)
        {
            switch (original)
            {
                case 1:
                    return GeoLocationType.Region;
                case 2:
                    return GeoLocationType.District;
                case 3:
                    return GeoLocationType.City;
                case 4:
                    return GeoLocationType.Town;
                default:
                    return GeoLocationType.Unknown;
            }
        }

        private static string Normalize(string original)
        {
            if (original == null)
            {
                return null;
            }

            return original.Trim(new[] {' ', '-'});
        }
    }
}
