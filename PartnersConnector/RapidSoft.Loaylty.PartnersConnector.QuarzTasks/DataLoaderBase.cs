namespace RapidSoft.Loaylty.PartnersConnector.Schedule
{
	using System;
	using System.IO;
	using System.Net;
	using System.Text;

	using Quartz;

	using RapidSoft.Loaylty.Logging;

	using Rapidsoft.Extensions;

	public abstract class DataLoaderBase : IInterruptableJob
	{

		public void Execute(IJobExecutionContext context)
		{
			Logger.Debug("Выполнение задачи DataLoaderBase");
			JobDataMap dataMap = context.JobDetail.JobDataMap;
			try
			{
				var partner = dataMap.GetAsString(DataKeys.PartnerId);
				var fileUrl = dataMap.GetAsString(DataKeys.FileUrl);

				var fileContent = this.LoadFileData(fileUrl);
				this.Process(partner, fileContent);
			}
			catch (ParseException exception)
			{
				var recipients = dataMap.GetAsString(DataKeys.Recipients);

				this.SendErrorMail(recipients, exception.Message, exception.ParseErrors.ToString());
			}
			catch (Exception e)
			{
				// TODO ???
				JobExecutionException e2 = new JobExecutionException(e)
					                           {
						                           RefireImmediately = true, 
												   UnscheduleAllTriggers = false,
												   UnscheduleFiringTrigger = false
					                           };
				throw e2;
			}
		}

		public void Interrupt()
		{
			throw new NotImplementedException();
		}

		protected string LoadFileData(string fileUrl)
		{
			fileUrl.ThrowIfNull("fileUrl");

			// TODO: Как на счет UTF-8 и Win-1251???
			using (var client = new WebClient())
			using (var stream = client.OpenRead(fileUrl))
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}

			// TODO: А надо ли удалить файл? или через Х минут (например, 15 минут), еще раз его закачать? Может хотя бы дату изменения файла запомнить?
		}

		protected abstract void Process(string partner, string content);

		protected void SendErrorMail(string recipients, string subject, string body)
		{
			// TODO: Отправка письма!
		}
	}
}