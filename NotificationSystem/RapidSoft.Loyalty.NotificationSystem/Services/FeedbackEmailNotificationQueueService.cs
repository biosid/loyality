using System;

using RapidSoft.Extensions;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.Logging.Wcf;
using RapidSoft.Loaylty.Monitoring;
using Rapidsoft.Loyalty.NotificationSystem.API;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess.Repositories;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    /// <summary>
    /// Реализация сервиса для работы с очередью сообщений для оповещения по email
    /// </summary>
    [LoggingBehavior]
    public class FeedbackEmailNotificationQueueService : SupportService, IFeedbackEmailNotificationQueueService
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(FeedbackEmailNotificationQueueService));

        private readonly IMessagesToNotifyRepository _messagesToNotifyRepository;

        public FeedbackEmailNotificationQueueService() : this(null)
        {
        }

        public FeedbackEmailNotificationQueueService(IMessagesToNotifyRepository messagesToNotifyRepository = null)
        {
            _messagesToNotifyRepository = messagesToNotifyRepository ?? new MessagesToNotifyRepository();
        }

        public GetMessagesToNotifyResult GetMessagesToNotify()
        {
            try
            {
                var messagesByThread = _messagesToNotifyRepository.GetAll();

                return new GetMessagesToNotifyResult
                {
                    MessagesByThreadId = messagesByThread
                };
            }
            catch (Exception e)
            {
                _log.Error("Exception", e);
                return ServiceOperationResult.BuildErrorResult<GetMessagesToNotifyResult>(e);
            }
        }

        public ResultBase MarkMessagesAsNotified(MarkMessagesAsNotifiedParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");
                parameters.MessagesByThreadId.ThrowIfNull("MessagesByThreadId is null");

                _messagesToNotifyRepository.Remove(parameters.MessagesByThreadId);

                return new ResultBase
                {
                    ResultCode = ResultCodes.SUCCESS
                };
            }
            catch (Exception e)
            {
                _log.Error("Exception", e);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(e);
            }
        }
    }
}
