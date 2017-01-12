namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    using System;
    using System.Globalization;
    using System.Threading;

    using Quartz;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;

    [GroupDisallowConcurrentExecution]
    public class NotifyExecutedOrdersJob : IInterruptableJob
    {
        #region IInterruptableJob Members

        public void Execute(IJobExecutionContext context)
        {
            ////var now = DateTime.Now;

            ////var monthStart = DateTime.SpecifyKind(new DateTime(now.Year, now.Month, 1), DateTimeKind.Local);

            ////var assigments = new[]
            ////{
            ////    new EtlVariableAssignment("FromDate", monthStart.AddMonths(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)), 
            ////    new EtlVariableAssignment("ToDate", monthStart.AddTicks(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
            ////};

            ////var executionParameters = JobExecutionParametersBuilder.BuildExecutionParameters(context, assigments);

            var executionParameters = JobExecutionParametersBuilder.BuildExecutionParameters(context);

            new WrapperBase(executionParameters).Execute();
        }

        public void Interrupt()
        {
            Thread.CurrentThread.Abort();
        }

        #endregion
    }
}
