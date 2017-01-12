using Vtb24.Site.Services.GeoService.Models;
using Vtb24.Site.Services.GeoService.Models.Inputs;
using Vtb24.Site.Services.GeoService.Models.Outputs;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services
{
    public interface IGeoService
    {
        GeoLocation GetLocationByUserIp(GetLocationByIpParams parameters);

        GeoLocation GetLocationByKladr(string kladrCode);

        GeoLocationsResult Find(GeoLocationQuery query, PagingSettings paging);

        GeoLocationsResult ListRegions();
    }
}