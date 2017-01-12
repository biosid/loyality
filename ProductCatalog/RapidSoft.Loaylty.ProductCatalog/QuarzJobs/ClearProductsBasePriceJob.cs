namespace RapidSoft.Loaylty.ProductCatalog.QuarzJobs
{
    using System;

    using Quartz;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using Vtb24.Common.Configuration;

    [GroupDisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class ClearProductsBasePriceJob : IJob
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ClearProductsBasePriceJob));

        public void Execute(IJobExecutionContext context)
        {
            if (!FeaturesConfiguration.Instance.Site505EnableActionPrice)
            {
                return;
            }

            try
            {
                log.Info("Начало очистки истекших скидок на вознаграждения");
                new ProductService().CleanupProductBasePriceDate();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка очистки истекших скидок на вознаграждения", ex);
                throw;
            }
        }
    }
}