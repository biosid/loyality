using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.Geocoder.Entities
{
    public class KladrLocation : ILocation
    {
        public object Id { get; set; }
        public int LocationType { get; set; }
        public string Name { get; set; }
        public string Toponym { get; set; }
        public string RegionName { get; set; }
        public string RegionToponym { get; set; }
        public string DistrictName { get; set; }
        public string DistrictToponym { get; set; }
        public string CityName { get; set; }
        public string CityToponym { get; set; }
        public string TownName { get; set; }
        public string TownToponym { get; set; }
        public string Address { get; set; }
    }
}
