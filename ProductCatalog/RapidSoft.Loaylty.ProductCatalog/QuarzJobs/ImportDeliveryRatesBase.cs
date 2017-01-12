namespace RapidSoft.Loaylty.ProductCatalog.QuarzJobs
{
    using System;

    using Quartz;

    using RapidSoft.Loaylty.ProductCatalog.Import;

    public abstract class ImportDeliveryRatesBase : IJob
    {
        private const string JobGroupName = "ImportDeliveryRates";

        public static string AddJob<T>(int partnerId, string fileUrl, string userId) where T : ImportDeliveryRatesBase
        {
            var scheduler = Helper.GetScheduler();

            var guid = Guid.NewGuid().ToString();

            var job = JobBuilder.Create<T>()
                                .WithIdentity(guid, JobGroupName)
                                .RequestRecovery(false)
                                .Build();

            job.JobDataMap.Put(DataKeys.PartnerId, partnerId);
            job.JobDataMap.Put(DataKeys.FileUrl, fileUrl);
            job.JobDataMap.Put(DataKeys.UserId, userId);

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity(guid, JobGroupName)
                                        .StartNow()
                                        .Build();

            scheduler.ScheduleJob(job, trigger);

            return guid;
        }

        public abstract IDeliveryRatesImporter GetImporter(int partnerId, string fileUrl, string userId);

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Logging.Logger.Info("Начало импорта списка тарифов доставки");
                var dataMap = context.JobDetail.JobDataMap;

                int partnerId = dataMap.GetIntValue(DataKeys.PartnerId);
                string fileUrl = dataMap.GetAsString(DataKeys.FileUrl);
                string userId = dataMap.GetAsString(DataKeys.UserId);

                var importer = this.GetImporter(partnerId, fileUrl, userId);

                importer.Execute();
            }
            catch (Exception ex)
            {
                Logging.Logger.Error("Ошибка импортирования списка тарифов доставки", ex);
                throw;
            }
        }
    }
}