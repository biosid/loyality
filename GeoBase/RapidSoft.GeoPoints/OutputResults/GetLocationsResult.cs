namespace RapidSoft.GeoPoints.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.GeoPoints.Entities;

    [DataContract]
    public class GetLocationsResult : ResultBase
    {
        [DataMember]
        public IList<Location> Locations { get; set; }
    }
}