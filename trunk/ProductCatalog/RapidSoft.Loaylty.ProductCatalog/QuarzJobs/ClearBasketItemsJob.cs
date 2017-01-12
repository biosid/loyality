namespace RapidSoft.Loaylty.ProductCatalog.QuarzJobs
{
    using System;

    using DataSources;

    using Quartz;

    using RapidSoft.Loaylty.Logging;

    [GroupDisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class ClearBasketItemsJob : IJob
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ClearBasketItemsJob));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                log.Info("Начало очистки BasketItems");
                new BasketItemsCleaner().Execute();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка очистки BasketItems", ex);
                throw;
            }
        }
    }
}