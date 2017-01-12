namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    using System.Threading;

    using Quartz;

    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;

    [GroupDisallowConcurrentExecution]
    public class PersonalMessagesJob : IInterruptableJob
    {
        #region IInterruptableJob Members

        public void Execute(IJobExecutionContext context)
        {
            new WrapperBase(JobExecutionParametersBuilder.BuildExecutionParameters(context)).Execute();
        }

        public void Interrupt()
        {
            Thread.CurrentThread.Abort();
        }

        #endregion
    }
}