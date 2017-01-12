namespace RapidSoft.VTB24.BankConnector.Service
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using RapidSoft.VTB24.BankConnector.DataSource;
	using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    public abstract class ProcessorBase
    {
        protected ProcessorBase(EtlLogger.EtlLogger logger, IUnitOfWork uow)
        {
            Logger = logger;
            Uow = uow;
        }

        protected EtlLogger.EtlLogger Logger { get; private set; }

        protected IUnitOfWork Uow { get; private set; }

		protected int PerfomBatchWork<T>(IQueryable<T> targetQuery, Func<List<T>, int> batchWork)
		{
			int totalProcessed = 0;
			List<T> links;

			while ((links = targetQuery.Skip(totalProcessed).Take(ConfigHelper.BatchSize).ToList()).Any())
			{
				batchWork(links);
				totalProcessed += links.Count;
			}

			return totalProcessed;
		}
    }
}