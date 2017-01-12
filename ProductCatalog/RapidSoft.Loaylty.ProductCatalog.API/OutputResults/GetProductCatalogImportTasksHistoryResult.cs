namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetProductCatalogImportTasksHistoryResult : ResultBase
    {
        public ProductImportTask[] Tasks { get; set; }

        public int? TotalCount { get; set; }

        public int MaxCountToTake { get; set; }

        public static GetProductCatalogImportTasksHistoryResult BuidlSuccess(ProductImportTask[] tasks, int? totalCount, int maxCountToTake)
        {
            return new GetProductCatalogImportTasksHistoryResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       ResultDescription = null,
                       Tasks = tasks,
                       TotalCount = totalCount,
                       MaxCountToTake = maxCountToTake
                   };
        }
    }
}