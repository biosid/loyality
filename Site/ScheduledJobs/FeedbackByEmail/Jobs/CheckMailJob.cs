using System;
using System.Linq;
using System.Net.Mail;
using Quartz;
using ScheduledJobs.Infrastructure.Mail;
using Topshelf.Quartz;

namespace ScheduledJobs.FeedbackByEmail.Jobs
{
    [DisallowConcurrentExecution]
    public class CheckMailJob : IJob
    {
        public CheckMailJob(ReplyMessageHandler replyMessageHandler)
        {
            _replyMessageHandler = replyMessageHandler;
        }

        private readonly ReplyMessageHandler _replyMessageHandler;


        #region API

        public void Execute(IJobExecutionContext context)
        {
            // Шаг 1: Получить новые сообщения
            var messages = FetchMessages();

            // Шаг 2: Для всех новых сообщений
            foreach (var fetched in messages)
            {
                // Шаг 2.1: Обработать сообщение
                try
                {
                    ProcessMessage(fetched);
                }
                // Шаг 2.1.1: Если при обработке произошла ошибка, послать ответное сообщение с описанием
                catch (Exception e)
                {
                    if (e is ReplyMessageHandler.HandlerException)
                    {
                        SendErrorMessage(e.Message, fetched.Message);
                    }
                    else
                    {
                        SendErrorMessage("Ошибка!\n\nНеизвестная ошибка при обработке сообщения", fetched.Message);
                    }
                }
                // Шаг 2.2: Пометить как прочитаное
                DeleteMessage(fetched);
            }
        }

        public static void Schedule(QuartzConfigurator configurator)
        {
            var interval = Settings.CheckIntervalInMinutes;

            configurator
                .WithJob(() =>
                {
                    var details = JobBuilder.Create<CheckMailJob>().Build();
                    return details;
                })
                .AddTrigger(() =>
                    TriggerBuilder
                        .Create()
                        .WithSimpleSchedule(builder => builder.WithIntervalInMinutes(interval).RepeatForever())
                        .Build()
                 );
        }

        #endregion


        #region Steps

        private FetchedMessage[] FetchMessages()
        {
            using (var mailFetcher = CreateMailFetcher())
            {
                return mailFetcher.GetAll().ToArray();
            }
        }

        private void ProcessMessage(FetchedMessage message)
        {
            _replyMessageHandler.Execute(message.Message);
        }

        private void SendErrorMessage(string error, MailMessage originalMessage)
        {
            var message = new MailMessage(Settings.MailUser, Settings.MailTo)
            {
                Subject = "Ошибка при ответе на обращение: " + originalMessage.Subject,
                Body = error
            };
            message.Attachments.Add(originalMessage, "Полученное сообщение");

            CreateSmtpClient().Send(message);
        }

        private void DeleteMessage(FetchedMessage message)
        {
            using (var mailFetcher = CreateMailFetcher())
            {
                mailFetcher.Delete(message);
            }
        }

        #endregion


        #region Injections

        public Func<IMailFetcher> CreateMailFetcher = () => 
            new ImapMailFetcher(Settings.MailHost, Settings.MailPort, Settings.MailUser, Settings.MailPassword);

        public Func<SmtpClient> CreateSmtpClient = () => 
            SmtpFactory.CreateSmtpClient(Settings.MailUser, Settings.MailPassword);

        #endregion
    }
}