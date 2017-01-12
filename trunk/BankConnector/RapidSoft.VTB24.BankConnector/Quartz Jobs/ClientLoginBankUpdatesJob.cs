namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    using System.Collections.Generic;
    using System.Threading;

    using Quartz;

    using RapidSoft.Etl.Runtime;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;

    [GroupDisallowConcurrentExecution]
    public class ClientLoginBankUpdatesJob : IInterruptableJob
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