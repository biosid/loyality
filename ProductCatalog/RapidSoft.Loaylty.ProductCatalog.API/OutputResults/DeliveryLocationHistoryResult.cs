namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class DeliveryLocationHistoryResult : ResultBase
    {
        [DataMember]
        public DeliveryLocationHistory[] DeliveryLocationHistory { get; set; }

        [DataMember]
        public int? TotalCount { get; set; }

        public static DeliveryLocationHistoryResult BuildSuccess(IEnumerable<DeliveryLocationHistory> locations, int? totalCount = null)
        {
            var asArray = locations.ToArray();
            return new DeliveryLocationHistoryResult
                       {
                           DeliveryLocationHistory = asArray,
                           TotalCount = totalCount,
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           Success = true
                       };
        }
    }
}