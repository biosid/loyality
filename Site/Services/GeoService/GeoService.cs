using System;
using System.Linq;
using Vtb24.Site.Services.GeoPointsService;
using Vtb24.Site.Services.GeoService.Models;
using Vtb24.Site.Services.GeoService.Models.Exceptions;
using Vtb24.Site.Services.GeoService.Models.Inputs;
using Vtb24.Site.Services.GeoService.Models.Outputs;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GeoService
{
    public class GeoService : IGeoService, IDisposable
    {
        public GeoLocation GetLocationByUserIp(GetLocationByIpParams parameters)
        {
            using (var service = new GeoPointServiceClient())
            {
                if (string.IsNullOrEmpty(parameters.Ip))
                {
                    return null;
                }
                var result = service.GetLocationByIP(parameters.Ip);

                if (result == null)
                {
                    return null;
                }

                var location = MappingsFromService.ToGeoLocation(result);

                return location;
            }
        }

        public GeoLocation GetLocationByKladr(string kladrCode)
        {
            using (var service = new GeoPointServiceClient())
            {
                var result = service.GetLocationByKladrCode(kladrCode);

                var location = MappingsFromService.ToGeoLocation(result);

                return location;
            }
        }

        public GeoLocationsResult Find(GeoLocationQuery query, PagingSettings paging)
        {
            using (var service = new GeoPointServiceClient())
            {
                var response = service.GetLocationsByKladrCode(
                    query.ParentKladrCode,
                    query.Types.Cast<int>().ToArray(),
                    query.Toponims,
                    query.SearchTerm,
                    query.RegionIsCityOnly,
                    paging.Skip,
                    paging.Take
                );

                AssertResponse(response.ResultCode, response.ResultDescription);

                if (response.Locations == null)
                {
                    return new GeoLocationsResult(Enumerable.Empty<GeoLocation>(), 0, paging);
                }
                var cities = response.Locations.Select(MappingsFromService.ToGeoLocation).ToArray();
                return new GeoLocationsResult(cities, cities.Length, paging);
            }
        }

        public GeoLocationsResult ListRegions()
        {
            return Find(new GeoLocationQuery {Types = new[] {GeoLocationType.Region}}, PagingSettings.ByOffset(0, 100));
        }

        private static void AssertResponse(int code, string message)
        {
            if (code == 0)
            {
                return;
            }

            throw new GeoServiceException(code, message);
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}