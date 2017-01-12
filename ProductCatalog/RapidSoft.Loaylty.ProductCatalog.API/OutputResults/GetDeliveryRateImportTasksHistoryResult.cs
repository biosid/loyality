namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class GetDeliveryRateImportTasksHistoryResult : ResultBase
    {
        [DataMember]
        public DeliveryRateImportTask[] Tasks { get; set; }

        [DataMember]
        public int? TotalCount { get; set; }

        [DataMember]
        public int MaxCountToTake { get; set; }

        public static GetDeliveryRateImportTasksHistoryResult BuidlSuccess(IEnumerable<DeliveryRateImportTask> tasks, int? totalCount, int maxCountToTake)
        {
            var asArray = tasks.ToArray();
            return new GetDeliveryRateImportTasksHistoryResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       ResultDescription = null,
                       Tasks = asArray,
                       TotalCount = totalCount,
                       MaxCountToTake = maxCountToTake
                   };
        }
    }
}