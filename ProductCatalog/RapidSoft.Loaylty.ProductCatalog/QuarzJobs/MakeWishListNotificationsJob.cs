namespace RapidSoft.Loaylty.ProductCatalog.QuarzJobs
{
    using System;

    using Logging;

    using Quartz;

    using Services;

    [GroupDisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class MakeWishListNotificationsJob : IJob
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MakeWishListNotificationsJob));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                log.Info("MakeWishListNotificationsJob Start");
                
                new WishListService().MakeWishListNotifications();
                
                log.Info("MakeWishListNotificationsJob End Success");
            }
            catch (Exception e)
            {
                log.Error("MakeWishListNotificationsJob Fail", e);
                throw;
            }
        }
    }
}