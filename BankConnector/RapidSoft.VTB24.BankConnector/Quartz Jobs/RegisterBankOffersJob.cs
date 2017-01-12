namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    using System.Threading;

    using EtlExecutionWrapper;
    using Quartz;

    [GroupDisallowConcurrentExecution]
    public class RegisterBankOffersJob : IInterruptableJob
    {
        public void Execute(IJobExecutionContext context)
        {
            new WrapperBase(JobExecutionParametersBuilder.BuildExecutionParameters(context)).Execute();
        }

        public void Interrupt()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
