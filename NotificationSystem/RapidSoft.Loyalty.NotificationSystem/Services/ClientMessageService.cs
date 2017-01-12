using System;
using System.Linq;

using RapidSoft.Extensions;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.Logging.Wcf;
using RapidSoft.Loaylty.Monitoring;
using Rapidsoft.Loyalty.NotificationSystem.API;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess.Repositories;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    /// <summary>
    ///     Реализация сервиса работы с сообщениями клиента.
    /// </summary>
    [LoggingBehavior]
    public class ClientMessageService : SupportService, IClientMessageService
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ClientMessageService));

        private readonly IThreadBuilder _threadBuilder;
        private readonly IThreadsRepository _threadsRepository;
        private readonly IThreadMessagesRepository _threadMessagesRepository;
        private readonly IMessageNotificationService _messageNotificationService;

        public ClientMessageService() : this(null)
        {
        }

        public ClientMessageService(IThreadsRepository threadsRepository = null,
                                    IThreadMessagesRepository threadMessagesRepository = null,
                                    IThreadBuilder threadBuilder = null,
                                    IMessageNotificationService messageNotificationService = null)
        {
            var messageNotificationServiceInstance = messageNotificationService ?? new MessageNotificationService();

            _threadsRepository = threadsRepository ?? new ThreadsRepository();
            _threadMessagesRepository = threadMessagesRepository ?? new ThreadMessagesRepository();
            _threadBuilder = threadBuilder ??
                             new ThreadBuilder(messageNotificationService: messageNotificationServiceInstance);
            _messageNotificationService = messageNotificationServiceInstance;
        }

        #region IClientMessageService Members

        public ClientGetThreadsResult GetThreads(ClientGetThreadsParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.ClientId.ThrowIfNull("ClientId");

                var threadsPage = _threadsRepository.GetClientThreadsPage(parameters);

                return new ClientGetThreadsResult
                {
                    Threads = threadsPage.Threads, 
                    TotalCount = threadsPage.TotalCount
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<ClientGetThreadsResult>(e);
            }
        }

        public GetStatisticsResult GetStatistics(GetStatisticsParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.ClientId.ThrowIfNull("ClientId");

                var clientThreadStat = _threadsRepository.GetClientThreadStatistics(parameters.ClientId);

                return new GetStatisticsResult
                {
                    ThreadsCount = clientThreadStat.ThreadsCount, 
                    UnreadThreadsCount = clientThreadStat.UnreadThreadsCount
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<GetStatisticsResult>(e);
            }
        }

        public GetThreadByIdResult GetThreadById(ClientGetThreadByIdParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");

                var thread = GetThreadSecurely(parameters.ThreadId, parameters.ClientId, true);

                return new GetThreadByIdResult
                {
                    Thread = thread, 
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<GetThreadByIdResult>(e);
            }
        }

        public GetThreadMessagesResult GetThreadMessages(ClientGetThreadMessagesParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");

                var thread = GetThreadSecurely(parameters.ThreadId, parameters.ClientId, true);

                parameters.CountToTake = parameters.CountToTake.NormalizeByHeight(100);

                var page = _threadsRepository.GetThreadMessagesPage(parameters);

                return new GetThreadMessagesResult
                {
                    Thread = thread, 
                    ThreadMessages = page.ThreadMessages, 
                    TotalCount = page.TotalCount
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<GetThreadMessagesResult>(e);
            }
        }

        public ReplyResult Reply(ClientReplyParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");
                parameters.ClientFullName.ThrowIfNull("ClientFullName is null");
                parameters.MessageBody.ThrowIfNull("parameters.MessageBody is null");

                if (parameters.ThreadId == Guid.Empty)
                {
                    throw new ArgumentException("parameters.ThreadId is empty");
                }

                var secureThread = GetThreadSecurely(parameters.ThreadId, parameters.ClientId, false);

                if (secureThread.IsClosed)
                {
                    return new ReplyResult
                    {
                        ResultCode = ResultCodes.THREAD_IS_CLOSED, 
                        ResultDescription = "Can not add message to closed thread"
                    };
                }

                var messageType = string.IsNullOrWhiteSpace(parameters.ClientId)
                    ? MessageTypes.GuestMessage
                    : MessageTypes.ClientMessage;

                _threadBuilder.CreateThreadMessage(
                    parameters.ThreadId, 
                    messageType, 
                    parameters.MessageBody, 
                    parameters.ClientId, 
                    parameters.ClientFullName, 
                    parameters.ClientEmail, 
                    parameters.Attachments, 
                    false);

                var thread = _threadsRepository.UpToDateThread(parameters.ThreadId, false);

                return new ReplyResult
                {
                    Thread = thread, 
                    ResultCode = ResultCodes.SUCCESS
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<ReplyResult>(e);
            }
        }

        public MarkThreadAsReadResult MarkThreadAsRead(MarkThreadAsReadParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");

                if (parameters.ThreadId == Guid.Empty)
                {
                    throw new ArgumentException("parameters.ThreadId is empty");
                }

                var secureThread = GetThreadSecurely(parameters.ThreadId, parameters.ClientId, true);

                var thread = _threadsRepository.MarkThreadAsRead(parameters.ThreadId);

                thread.TopicMessage = secureThread.TopicMessage;

                return new MarkThreadAsReadResult
                {
                    Thread = thread, 
                    ResultCode = ResultCodes.SUCCESS
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<MarkThreadAsReadResult>(e);
            }
        }

        public NotifyClientsResult Notify(NotifyClientsParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");
                parameters.Notifications.ThrowIfNull("Notifications is null");
                
                foreach (var notification in parameters.Notifications)
                {
                    notification.ThrowIfNull("notification is null");
                    notification.ClientId.ThrowIfNull("ClientId is null");
                    notification.Title.ThrowIfNull("Title is null");
                    notification.Text.ThrowIfNull("Text is null");
                }

                var now = DateTime.Now;

                var threads =
                    parameters.Notifications
                              .Select(notification =>
                                      _threadBuilder.CreateNotification(
                                          notification.ClientId, 
                                          now, 
                                          notification.Title, 
                                          notification.Text, 
                                          notification.ShowSince, 
                                          notification.ShowUntil))
                              .ToArray();

                var result = new NotifyClientsResult
                {
                    ResultCode = ResultCodes.SUCCESS,
                    Threads = threads
                };

                return result;
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<NotifyClientsResult>(e);
            }
        }

        public void Unsubscribe(string emailHash)
        {
            try
            {
                _messageNotificationService.Unsubscribe(emailHash);
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
            }
        }

        public ResultBase Delete(DeleteThreadParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");

                if (!_threadsRepository.VerifyThread(parameters.ThreadId, parameters.ClientId))
                {
                    return new ResultBase
                    {
                        ResultCode = ResultCodes.NOT_HAVE_PERMISSION,
                        ResultDescription = "неверный ClientId"
                    };
                }

                _threadsRepository.Delete(parameters.ThreadId);

                return new ResultBase
                {
                    ResultCode = ResultCodes.SUCCESS
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);

                return new ResultBase
                {
                    ResultCode = ResultCodes.UNKNOWN_ERROR,
                    ResultDescription = "ошибка при удалении: " + e.Message
                };
            }
        }

        #endregion

        private Thread GetThreadSecurely(Guid threadId, string clientId, bool includeTopicMessage)
        {
            var thread = _threadsRepository.GetThread(threadId, false);

            if (thread == null)
            {
                throw new OperationException
                {
                    ResultCode = ResultCodes.NOT_FOUND,
                    ResultDescription = string.Format("Thread with id {0} not found", threadId)
                };
            }

            if (thread.ClientType == ThreadClientTypes.Client &&
                (string.IsNullOrEmpty(clientId) || clientId != thread.ClientId))
            {
                {
                    throw new OperationException(
                        ResultCodes.NOT_HAVE_PERMISSION, 
                        string.Format("Thread with id {0} is client thread. Wrong clientId", thread.Id));
                }
            }

            var nowDate = DateTime.Now;

            if ((thread.ShowSince != null && nowDate < thread.ShowSince) ||
                (thread.ShowUntil != null && thread.ShowUntil < nowDate))
            {
                {
                    throw new OperationException(
                        ResultCodes.THREAD_SINCE_UNTIL_MISMATCH,
                        string.Format("Thread with id {0} not showed sinceDate:{1} untilDate:{2}", thread.Id, thread.ShowSince, thread.ShowUntil));
                }
            }
            
            if (includeTopicMessage)
            {
                thread.TopicMessage = _threadsRepository.GetTopicMessage(threadId);
            }

            return thread;
        }
    }
}