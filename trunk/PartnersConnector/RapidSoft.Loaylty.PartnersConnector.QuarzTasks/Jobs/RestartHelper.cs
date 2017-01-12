using RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings;

namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs
{
	using System;
	using System.Threading;

	using Quartz;

	using PartnersConnector.Interfaces.Settings;

	/// <summary>
	/// Класс содержащий вспомогательные методы конструирования ошибки выполнения задачи
	/// </summary>
	public static class RestartHelper
	{
		/// <summary>
		/// Создает экземпляр <see cref="JobExecutionException"/> в зависимости от настроек <see cref="PartnerConnectionsSection"/>.
		/// </summary>
		/// <param name="context">
		/// Контекст выполнения задачи.
		/// </param>
		/// <param name="message">
		/// Описание ошибки.
		/// </param>
		/// <param name="exception">
		/// Возникшая ошибка.
		/// </param>
		/// <returns>
		/// Экземпляр ошибки позволяющий управлять задаче и/или триггером задачи.
		/// </returns>
		public static Exception GetRestartException(IJobExecutionContext context, string message = null, Exception exception = null)
		{
			Thread.Sleep(PartnerConnectionsConfig.RefireCountToMilisecFactor * (context.RefireCount + 1));

		    var retVal = new JobExecutionException(message, exception)
		                     {
		                         UnscheduleAllTriggers = false,
		                         UnscheduleFiringTrigger = false,
		                     };

			if (context.RefireCount < PartnerConnectionsConfig.MaxTaskRefire - 1)
			{
				retVal.RefireImmediately = true;
			}
			else
			{
				retVal.SetTriggerError = false;
				retVal.RefireImmediately = false;
			}

			return retVal;
		}
	}
}
