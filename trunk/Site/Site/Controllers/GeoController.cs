using System;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Site.Models.Shared;
using Vtb24.Site.Models.Shared.Location;
using Vtb24.Site.Services;
using Vtb24.Site.Services.GeoService.Models;
using Vtb24.Site.Services.GeoService.Models.Inputs;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Controllers
{
    public class GeoController : Controller
    {
        public GeoController(IGeoService geo)
        {
            _geo = geo;
        }

        private readonly IGeoService _geo;

        [HttpPost]
        public ActionResult GetCitiesByRegion(string regionKladr)
        {
            var options = new GeoLocationQuery
            {
                SearchTerm = null,
                ParentKladrCode = regionKladr,
                Types = new[] { GeoLocationType.City, GeoLocationType.Town },
                Toponims = new[] {"г"}
            };

            var found = _geo.Find(options, PagingSettings.ByOffset(0, 200));

            var model = new CitiesModel();
            if (found != null && found.Any())
            {
                model.Cities = found.Select(LocationItem.FromGeoLocation).ToList();
                model.RegionName = found.First().GetFullRegionName();
            }

            return PartialView("Location/_Cities", model);
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult SearchCity(string term)
        {
            var options = new GeoLocationQuery
            {
                SearchTerm = term,
                Types = new[]{ GeoLocationType.Region, GeoLocationType.City, GeoLocationType.Town  },
                Toponims = new[] { "г" }
            };

            var cities = _geo.Find(options, PagingSettings.ByOffset(0, 20));

            return Json(cities.Select(MapForAutocomplete), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult SearchCityOrTown(string term, string regionKladr = null)
        {
            var options = new GeoLocationQuery
            {
                ParentKladrCode = string.IsNullOrEmpty(regionKladr) ? null : regionKladr,
                SearchTerm = term,
                Types = new[] { GeoLocationType.Region, GeoLocationType.City, GeoLocationType.Town },
                RegionIsCityOnly = true
            };

            var locations = _geo.Find(options, PagingSettings.ByOffset(0, 20));

            return Json(locations.Select(MapForAutocomplete), JsonRequestBehavior.AllowGet);
        }

        private static AutocompleteSuggestionModel MapForAutocomplete(GeoLocation location)
        {
            string label;
            if (location.Type == GeoLocationType.Region)
            {
                label = location.GetFullRegionName();
            } 
            else if (location.Type == GeoLocationType.District || string.IsNullOrWhiteSpace(location.DistrictName))
            {
                label = string.Format("{0}, {1}", location.GetFullName(), location.GetFullRegionName());
            }
            else
            {
                label = string.Format("{0}, {1}, {2}", location.GetFullName(), location.GetFullDistrictName(), location.GetFullRegionName());
            }

            return new AutocompleteSuggestionModel
            {
                text = location.Name,
                value = location.KladrCode,
                label = label
            };
        }

    }
}
