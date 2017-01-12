using System.Collections.Generic;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GeoService.Models.Outputs
{
    public class GeoLocationsResult : PagedResult<GeoLocation>
    {
        public GeoLocationsResult(IEnumerable<GeoLocation> result, int totalCount, PagingSettings paging) 
            : base(result, totalCount, paging)
        {
        }
    }
}