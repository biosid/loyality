namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    using System;
    using System.Globalization;
    using System.Threading;

    using Quartz;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;

    [GroupDisallowConcurrentExecution]
    public class NotifyIncompleteOrdersJob : IInterruptableJob
    {
        #region IInterruptableJob Members

        public void Execute(IJobExecutionContext context)
        {
            var fromDate = DateTime.Now.Date;

            var assigments = new[]
            {
                new EtlVariableAssignment("FromDate", fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
            };

            var executionParameters = JobExecutionParametersBuilder.BuildExecutionParameters(context, assigments);

            new WrapperBase(executionParameters).Execute();
        }

        public void Interrupt()
        {
            Thread.CurrentThread.Abort();
        }

        #endregion
    }
}
