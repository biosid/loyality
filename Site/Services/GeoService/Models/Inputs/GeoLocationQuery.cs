namespace Vtb24.Site.Services.GeoService.Models.Inputs
{
    public class GeoLocationQuery
    {
        public string SearchTerm { get; set; }

        public string ParentKladrCode { get; set; }

        public GeoLocationType[] Types { get; set; }

        public string[] Toponims { get; set; }

        public bool RegionIsCityOnly { get; set; }
    }
}