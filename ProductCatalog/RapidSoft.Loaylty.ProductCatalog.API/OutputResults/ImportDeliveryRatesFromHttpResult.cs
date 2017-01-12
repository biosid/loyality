namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public class ImportDeliveryRatesFromHttpResult : ResultBase
    {
        public string JobId { get; set; }

        public static ImportDeliveryRatesFromHttpResult BuildSuccess(string jobId)
        {
            return new ImportDeliveryRatesFromHttpResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       ResultDescription = null,
                       JobId = jobId
                   };
        }
    }
}