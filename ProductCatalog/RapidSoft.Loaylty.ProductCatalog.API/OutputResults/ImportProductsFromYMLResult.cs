namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public class ImportProductsFromYmlResult : ResultBase
    {
        public int? TaskId { get; set; }

        public static ImportProductsFromYmlResult BuildSuccess(int taskId)
        {
            return new ImportProductsFromYmlResult
                   {
                       TaskId = taskId,
                       ResultCode = ResultCodes.SUCCESS,
                       ResultDescription = null
                   };
        }
    }
}