namespace Vtb24.Site.Services.GeoService.Models.Inputs
{
    public class GetLocationByIpParams
    {
        public string Ip { get; set; }

        public GeoLocationType Type { get; set; }
    }
}