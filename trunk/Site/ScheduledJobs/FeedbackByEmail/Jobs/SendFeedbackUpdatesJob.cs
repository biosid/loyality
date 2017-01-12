using System;
using System.Net.Mail;
using Quartz;
using ScheduledJobs.Infrastructure.Mail;
using Topshelf.Quartz;
using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;
using IAdminFeedbackService = Vtb24.Arms.AdminServices.IAdminFeedbackService;


namespace ScheduledJobs.FeedbackByEmail.Jobs
{
    [DisallowConcurrentExecution]
    public class SendFeedbackUpdatesJob : IJob
    {
        public SendFeedbackUpdatesJob(IAdminFeedbackService feedbackService, NotificationQueueService notificationQueue, ThreadMessageGenerator messageGenerator)
        {
            _feedbackService = feedbackService;
            _messageGenerator = messageGenerator;
            _notificationQueue = notificationQueue;
        }

        private readonly IAdminFeedbackService _feedbackService;
        private readonly NotificationQueueService _notificationQueue;
        private readonly ThreadMessageGenerator _messageGenerator;


        #region API

        public void Execute(IJobExecutionContext context)
        {
            // Шаг 1: Получить список идентификторов обновлённых веток
            var ids = GetUpdatedThreads();

            // Шаг 2: Для каждого идентификатора
            foreach (var id in ids)
            {
                // Шаг 2.1: Получить ветку
                var thread = GetThread(id);
                if (thread == null)
                {
                    continue;
                }
                // Шаг 2.2: Сформировать содержимое письма по шаблону
                var message = CreateMessage(thread);
                // Шаг 2.3: Отослать письмо оператору
                SendMessage(message);
                // Шаг 2.4: Пометить ветку как отправленную
                MarkThreadAsSent(id, thread.TotalCount);
            }
        }

        public static void Schedule(QuartzConfigurator configurator)
        {
            var interval = Settings.SendIntervalInMinutes;

            configurator
                .WithJob(() => JobBuilder.Create<SendFeedbackUpdatesJob>().Build())
                .AddTrigger(() =>
                    TriggerBuilder
                        .Create()
                        .WithSimpleSchedule(builder => builder.WithIntervalInMinutes(interval).RepeatForever())
                        .Build()
                 );
        }

        #endregion


        #region Steps

        private Guid[] GetUpdatedThreads()
        {
            return _notificationQueue.GetMessagesToNotify();
        }

        private GetThreadMessagesResult GetThread(Guid id)
        {
            try
            {
                var options = new AdminGetThreadMessagesParameters
                {
                    UserId = Settings.UserId,
                    ThreadId = id,
                    CountToSkip = 0,
                    CountToTake = 1000
                };
                return _feedbackService.GetThreadMessages(options);
            }
            catch (Exception e)
            {
                //TODO: залогировать
                return null;
            }
        }

        private MailMessage CreateMessage(GetThreadMessagesResult thread)
        {
            return _messageGenerator.Execute(thread);
        }

        private  void SendMessage(MailMessage message)
        {
            CreateSmtpClient().Send(message);
        }

        private void MarkThreadAsSent(Guid id, int messageIndex)
        {
           _notificationQueue.MarkMessageAsNotified(id, messageIndex);
        }

        #endregion


        #region Injections

        public Func<SmtpClient> CreateSmtpClient = () =>
            SmtpFactory.CreateSmtpClient(Settings.MailUser, Settings.MailPassword);

        #endregion
    }
}