namespace Vtb24.Site.Services.GeoService.Models
{
    public class GeoLocation
    {
        const string MOSCOW_KLADR = "7700000000000";
        const string PITER_KLADR = "7800000000000";

        public string Name { get; set; }

        public string Toponym { get; set; }

        public string KladrCode { get; set; }

        public string RegionName { get; set; }

        public string RegionToponym { get; set; }

        public string RegionKladrCode { get; set; }

        public string DistrictToponim { get; set; }

        public string DistrictName { get; set; }

        public string DistrictKladrCode { get; set; }

        public GeoLocationType Type { get; set; }

        public bool IsCity()
        {
            return Type == GeoLocationType.City 
                || KladrCode == MOSCOW_KLADR
                || KladrCode == PITER_KLADR;
        }

        public string GetFullName()
        {
            return NormalizeName(Toponym, Name, Type);
        }

        public string GetFullRegionName()
        {
            return NormalizeName(RegionToponym, RegionName, GeoLocationType.Region);
        }

        public string GetFullDistrictName()
        {
            return NormalizeName(DistrictToponim, DistrictName, GeoLocationType.District);
        }

        private static string NormalizeName(string toponym, string name, GeoLocationType type)
        {
            if (type == GeoLocationType.Region)
            {
                switch (toponym)
                {
                    case "обл":
                        return string.Format("{0} область", name);
                    case "Респ":
                        return string.Format("Республика {0}", name);
                    case "край":
                        return string.Format("{0} край", name);
                    case "АО":
                        return string.Format("{0} автономный округ", name);
                    case "Аобл":
                        return string.Format("{0} автономная область", name);
                    case "г":
                        return name;
                }
            } 
            else if (type == GeoLocationType.District)
            {
                switch (toponym)
                {
                    case "р-н":
                        return string.Format("{0} район", name);
                    case "тер":
                        return name;
                    case "у":
                        return string.Format("{0} улус", name);
                    case "кожуун":
                        return string.Format("{0} кожуун", name);
                    case "п":
                        return string.Format("{0} поселение", name);
                }
            }

            return string.Format("{0} {1}", toponym, name);
        }
    }
}