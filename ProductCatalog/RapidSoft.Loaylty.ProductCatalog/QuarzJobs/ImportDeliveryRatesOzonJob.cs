namespace RapidSoft.Loaylty.ProductCatalog.QuarzJobs
{
    using Quartz;

    using RapidSoft.Loaylty.ProductCatalog.Import;

    [GroupDisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class ImportDeliveryRatesOzonJob : ImportDeliveryRatesBase
    {
        /// <summary>
        /// Идентификатор ETL-пакета для Озона.
        /// </summary>
        public const string PackageId = "777ff1a8-3fbf-4127-96d3-70a0fa7fd05c";

        public override IDeliveryRatesImporter GetImporter(int partnerId, string fileUrl, string userId)
        {
            return new DeliveryRatesImporter(PackageId, partnerId, fileUrl, userId);
        }
    }
}