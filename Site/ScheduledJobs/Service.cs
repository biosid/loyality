using Quartz.Impl;

namespace ScheduledJobs
{
    public class Service
    {
        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            StdSchedulerFactory.GetDefaultScheduler().Shutdown(true);
            return true;
        }
    }
}