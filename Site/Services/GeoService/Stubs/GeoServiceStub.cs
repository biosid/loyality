using System.Linq;
using Vtb24.Site.Services.GeoService.Models;
using Vtb24.Site.Services.GeoService.Models.Inputs;
using Vtb24.Site.Services.GeoService.Models.Outputs;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GeoService.Stubs
{
    public class GeoServiceStub : IGeoService
    {
        public GeoLocation GetLocationByUserIp(GetLocationByIpParams parameters)
        {
            return GeoServiceStubData.DefaultCity;
        }

        public GeoLocation GetLocationByKladr(string kladrCode)
        {
            return GeoServiceStubData.Cities.FirstOrDefault(c => c.KladrCode == kladrCode);
        }

        public GeoLocationsResult Find(GeoLocationQuery query, PagingSettings paging)
        {
            return new GeoLocationsResult(GeoServiceStubData.Cities, GeoServiceStubData.Cities.Count, paging);
        }

        public GeoLocationsResult ListRegions()
        {
            return new GeoLocationsResult(GeoServiceStubData.Regions, GeoServiceStubData.Regions.Count, new PagingSettings());
        }
    }
}