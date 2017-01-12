namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
	using System.Threading;

	using Quartz;

	using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;

	[GroupDisallowConcurrentExecution]
	public class AppendDetachListJob : IInterruptableJob
	{
		#region IInterruptableJob Members

		public void Execute(IJobExecutionContext context)
		{
			var wrapper = new AppendDetachListWrapper();

			wrapper.Execute();
		}

		public void Interrupt()
		{
			Thread.CurrentThread.Abort();
		}

		#endregion
	}
}
