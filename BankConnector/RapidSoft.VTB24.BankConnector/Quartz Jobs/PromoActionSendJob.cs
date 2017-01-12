namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Microsoft.Practices.Unity;

    using Quartz;

    using RapidSoft.Etl.Runtime;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;

    [DisallowConcurrentExecution]
    public class PromoActionSendJob : IInterruptableJob
    {
        #region IInterruptableJob Members

        public void Execute(IJobExecutionContext context)
        {
            // NOTE: "Динамические" параметры
            var date = DateTime.Now.Date;

            var index = EtlInvokeHelper.ResolveAndInvoke<IUnitOfWork, int>(
                uow => uow.PromoActionRepository
                          .GetAll()
                          .Where(x => x.DateSent == date)
                          .Select(x => x.IndexSent)
                          .DefaultIfEmpty(0)
                          .Max());

            index++;

            var assigments = new[]
            {
                new EtlVariableAssignment("FileDate", date.ToString("yyyyMMdd", CultureInfo.InvariantCulture)), 
                new EtlVariableAssignment("FileNum", index.ToString(CultureInfo.InvariantCulture))
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