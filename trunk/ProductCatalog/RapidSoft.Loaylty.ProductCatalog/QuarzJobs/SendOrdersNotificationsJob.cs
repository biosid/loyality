namespace RapidSoft.Loaylty.ProductCatalog.QuarzJobs
{
    using Quartz;
    using RapidSoft.Loaylty.ProductCatalog.EtlExecutionWrapper;

    public class SendOrdersNotificationsJob : IJob
    {
        private const string EtlPackageId = "4686B3B3-FC3F-460A-98E7-37A834527F38";

        public void Execute(IJobExecutionContext context)
        {
            new EtlWrapper(EtlPackageId).Execute();
        }
    }
}
