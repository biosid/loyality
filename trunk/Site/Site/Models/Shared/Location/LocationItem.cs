using Vtb24.Site.Services.GeoService.Models;

namespace Vtb24.Site.Models.Shared.Location
{
    public class LocationItem
    {
        public string Name { get; set; }
        public string KladrCode { get; set; }

        public static LocationItem FromGeoLocation(GeoLocation original)
        {
            return new LocationItem
            {
                KladrCode = original.KladrCode,
                Name = original.GetFullName()
            };
        }
    }
}