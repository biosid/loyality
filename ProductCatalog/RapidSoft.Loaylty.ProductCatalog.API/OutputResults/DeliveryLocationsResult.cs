namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class DeliveryLocationsResult : ResultBase
    {
        [DataMember]
        public DeliveryLocation[] DeliveryLocations { get; set; }

        [DataMember]
        public int? TotalCount { get; set; }

        public static DeliveryLocationsResult BuildSuccess(IEnumerable<DeliveryLocation> locations, int? totalCount = null)
        {
            var asArray = locations.ToArray();
            return new DeliveryLocationsResult
                       {
                           DeliveryLocations = asArray,
                           TotalCount = totalCount,
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           Success = true
                       };
        }
    }
}