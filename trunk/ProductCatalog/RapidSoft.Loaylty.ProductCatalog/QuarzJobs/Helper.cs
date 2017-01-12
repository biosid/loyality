namespace RapidSoft.Loaylty.ProductCatalog.QuarzJobs
{
    using Quartz;
    using Quartz.Impl;

    public static class Helper
    {
        public static IScheduler GetScheduler()
        {
            var shedulerFactory = new StdSchedulerFactory();
            return shedulerFactory.GetScheduler();
        }
    }
}