using System;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Quartz;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess.Repositories;
using RapidSoft.Loaylty.PartnersConnector.Litres.Reports;

namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs
{
    [DisallowConcurrentExecution]
    public class SendLitresRemainingCodesCountReport : IInterruptableJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(SendLitresRemainingCodesCountReport));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                using (var repository = new LitresDownloadCodesRepository())
                {
                    const string SUBJECT = "Отчёт по количеству кодов Литрес";

                    var codes = repository.GetAllRemainingCodesCount();

                    var report = new RemainingCodesCountReport { Codes = codes };

                    var body = report.TransformText();

                    var message = new MailMessage
                    {
                        Subject = SUBJECT,
                        Body = body,
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8
                    };

                    message.To.Add(report.ReportRecipients);

                    using (var client = new SmtpClient())
                    {
                        client.Send(message);
                    }
                }
            }
            catch (JobExecutionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _log.Error("Ошибка при создании отчета", ex);

                throw RestartHelper.GetRestartException(context, exception: ex);
            }
        }

        public void Interrupt()
        {
            _log.Info("Прерывание выполнения");
            Thread.CurrentThread.Abort();
        }
    }
}
