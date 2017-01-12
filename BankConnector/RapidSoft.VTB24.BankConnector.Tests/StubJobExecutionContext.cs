namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System;

    using Quartz;
    using Quartz.Impl;

    public class StubJobExecutionContext : IJobExecutionContext
    {
        private readonly JobDetailImpl jobDetailImpl = new JobDetailImpl()
                                              {
                                                  Key = new JobKey("test")
                                              };

        private readonly JobDataMap mergedJobDataMap = new JobDataMap();

        public IScheduler Scheduler
        {
            get;
            private set;
        }

        public ITrigger Trigger
        {
            get;
            private set;
        }

        public ICalendar Calendar
        {
            get;
            private set;
        }

        public bool Recovering
        {
            get;
            private set;
        }

        public int RefireCount
        {
            get;
            private set;
        }

        public JobDataMap MergedJobDataMap
        {
            get
            {
                return mergedJobDataMap;
            }
        }

        public IJobDetail JobDetail
        {
            get
            {
                return this.jobDetailImpl;
            }
        }

        public IJob JobInstance
        {
            get;
            private set;
        }

        public DateTimeOffset? FireTimeUtc
        {
            get;
            private set;
        }

        public DateTimeOffset? ScheduledFireTimeUtc
        {
            get;
            private set;
        }

        public DateTimeOffset? PreviousFireTimeUtc
        {
            get;
            private set;
        }

        public DateTimeOffset? NextFireTimeUtc
        {
            get;
            private set;
        }

        public string FireInstanceId
        {
            get;
            private set;
        }

        public object Result
        {
            get;
            set;
        }

        public TimeSpan JobRunTime
        {
            get;
            private set;
        }

        public void Put(object key, object objectValue)
        {
            throw new NotImplementedException();
        }

        public object Get(object key)
        {
            throw new NotImplementedException();
        }
    }
}