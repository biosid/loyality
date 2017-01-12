using System;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;

using RapidSoft.Extensions;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.Logging.Wcf;
using RapidSoft.Loaylty.Monitoring;
using RapidSoft.VTB24.ArmSecurity;
using RapidSoft.VTB24.ArmSecurity.Check;
using RapidSoft.VTB24.ArmSecurity.Interfaces;
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
    public class AdminFeedbackService : SupportService, IAdminFeedbackService
    {
        private readonly IFileProvider _fileProvider;
        private readonly ISecurityChecker _securityChecker;
        private readonly IThreadBuilder _threadBuilder;
        private readonly IThreadsRepository _threadsRepository;
        private readonly ILog _log = LogManager.GetLogger(typeof(AdminFeedbackService));

        public AdminFeedbackService() : this(null)
        {
        }

        public AdminFeedbackService(IThreadsRepository threadsRepository = null,
                                    ISecurityChecker securityChecker = null,
                                    IFileProvider fileProvider = null)
        {
            _fileProvider = fileProvider ?? new WebFileProvider();
            _securityChecker = securityChecker ?? new ADSecurityChecker();
            _threadsRepository = threadsRepository ?? new ThreadsRepository();
            _threadBuilder = _threadBuilder ?? new ThreadBuilder();
        }

        #region IAdminFeedbackService Members

        public AdminGetThreadsResult GetThreads(AdminGetThreadsParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");
                parameters.UserId.ThrowIfNull("parameters.UserId is null");

                _securityChecker.CheckPermissions(
                    parameters.UserId, 
                    ArmPermissions.ARMSecurityLogin, 
                    ArmPermissions.SecurityClientsFeedback);

                parameters.CountToTake = parameters.CountToTake.NormalizeByHeight(100);

                return _threadsRepository.GetThreadsPage(parameters);
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<AdminGetThreadsResult>(e);
            }
        }

        public GetThreadMessagesResult GetThreadMessages(AdminGetThreadMessagesParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");
                parameters.UserId.ThrowIfNull("parameters.UserId is null");

                if (parameters.ThreadId == Guid.Empty)
                {
                    throw new ArgumentException("parameters.ThreadId is empty");
                }

                _securityChecker.CheckPermissions(
                    parameters.UserId, 
                    ArmPermissions.ARMSecurityLogin, 
                    ArmPermissions.SecurityClientsFeedback);

                var thread = GetThread(parameters.ThreadId, true);
                
                parameters.CountToTake = parameters.CountToTake.NormalizeByHeight(100);                
                var messagesPage = _threadsRepository.GetThreadMessagesPage(parameters);

                return new GetThreadMessagesResult
                {
                    Thread = thread,
                    ThreadMessages = messagesPage.ThreadMessages,
                    TotalCount = messagesPage.TotalCount
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<GetThreadMessagesResult>(e);
            }
        }

        public ReplyResult Reply(AdminReplyParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");
                parameters.UserId.ThrowIfNull("parameters.UserId is null");
                parameters.MessageBody.ThrowIfNull("parameters.MessageBody is null");

                if (parameters.ThreadId == Guid.Empty)
                {
                    throw new ArgumentException("parameters.ThreadId is empty");
                }

                _securityChecker.CheckPermissions(
                    parameters.UserId, 
                    ArmPermissions.ARMSecurityLogin, 
                    ArmPermissions.SecurityClientsFeedback);

                var thread = _threadsRepository.GetThread(parameters.ThreadId, true);

                if (thread == null)
                {
                    throw new OperationException(ResultCodes.NOT_FOUND, "ветка сообщений не найдена");
                }

                if (thread.IsDeleted)
                {
                    // пришел ответ на удаленную пользователем ветку:
                    //   создадим новую ветку
                    thread = _threadBuilder.CreateThread(thread.Title,
                                                         thread.ClientEmail,
                                                         thread.ClientFullName,
                                                         thread.ClientId,
                                                         thread.ClientType,
                                                         thread.FirstMessageBy,
                                                         thread.LastMessageBy,
                                                         null,
                                                         null,
                                                         ApiHelper.ConvertToFeedbackType(thread.Type),
                                                         thread.FirstMessageType,
                                                         thread.LastMessageType,
                                                         thread.MetaData);
                }

                _threadBuilder.CreateThreadMessage(thread.Id,
                                                   MessageTypes.OperatorMessage,
                                                   parameters.MessageBody,
                                                   parameters.UserId,
                                                   null,
                                                   null,
                                                   parameters.Attachments,
                                                   true);

                thread = _threadsRepository.UpToDateThread(thread.Id, true);

                if (thread.ClientType == ThreadClientTypes.Guest)
                {
                    SendGuestNotification(thread);
                }

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

        public ChangeAnsweredStatusResult ChangeAnsweredStatus(ChangeAnsweredStatusParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");
                parameters.UserId.ThrowIfNull("parameters.UserId is null");

                if (parameters.ThreadId == Guid.Empty)
                {
                    throw new ArgumentException("parameters.ThreadId is empty");
                }

                _securityChecker.CheckPermissions(
                    parameters.UserId, 
                    ArmPermissions.ARMSecurityLogin, 
                    ArmPermissions.SecurityClientsFeedback);

                var thread = _threadsRepository.ChangeAnsweredStatus(parameters.ThreadId, parameters.IsAnswered);

                return new ChangeAnsweredStatusResult
                {
                    Thread = thread, 
                    ResultCode = ResultCodes.SUCCESS
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<ChangeAnsweredStatusResult>(e);
            }
        }

        public GetThreadByIdResult GetThreadById(AdminGetThreadByIdParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.UserId is null");

                if (parameters.ThreadId == Guid.Empty)
                {
                    throw new ArgumentException("parameters.ThreadId is empty");
                }

                _securityChecker.CheckPermissions(
                    parameters.UserId,
                    ArmPermissions.ARMSecurityLogin,
                    ArmPermissions.SecurityClientsFeedback);

                var thread = GetThread(parameters.ThreadId, true);                               

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

        #endregion

        private static MailMessage BuildMessage(Thread thread, IFileProvider fileProvider)
        {
            var from = ConfigurationManager.AppSettings["ClientFeedbackFrom"];
            var subject = BuildMessageSubject(thread);
            var body = BuildMessageBody(thread);

            var message = new MailMessage(from, thread.ClientEmail)
            {
                Subject = subject,
                IsBodyHtml = true
            };

            var alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

            alternateView.LinkedResources.Add(NewLinkedResource("content_bg", @"/images/content_bg.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("header_bg", @"/images/header_bg.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("logo", @"/images/logo.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("message_bg", @"/images/message_bg.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("message_bottom", @"/images/message_bottom.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("message_top", @"/images/message_top.png", "image/png", fileProvider));

            message.AlternateViews.Add(alternateView);

            return message;
        }

        private static LinkedResource NewLinkedResource(string contentId, string filePath, string mimeType, IFileProvider fileProvider)
        {
            var fileName = fileProvider.MapPath(filePath);

            return new LinkedResource(fileName, mimeType)
            {
                ContentId = contentId
            };
        }

        private static string BuildMessageBody(Thread thread)
        {
            var threadUrl = ConfigurationManager.AppSettings["ClientSiteUrl"] +
                            "/feedback/conversation/" +
                            thread.Id.ToString("D") +
                            "#message-" +
                            (thread.MessagesCount - 1).ToString("D");

            var bodyTemplate = new Templates.ClientNotificationBody
            {
                ThreadDate = thread.InsertedDate.ToString("d.MM.yyyy"),
                ThreadUrl = threadUrl
            };

            var msgBody = bodyTemplate.TransformText();
            return msgBody;
        }

        private static string BuildMessageSubject(Thread thread)
        {
            return string.Format("Ответ на Ваше обращение от {0:d.MM.yyyy}", thread.InsertedDate);
        }

        private static void SendMessage(MailMessage message)
        {
            var client = new SmtpClient();
            client.Send(message);
        }

        private Thread GetThread(Guid threadId, bool includeTopicMessage)
        {
            var thread = _threadsRepository.GetThread(threadId, false);

            if (thread == null)
            {
                throw new OperationException(
                    ResultCodes.NOT_FOUND, 
                    string.Format("Thread with id {0} not found", threadId));
            }

            if (includeTopicMessage)
            {
                thread.TopicMessage = _threadsRepository.GetTopicMessage(threadId);
            }

            return thread;
        }

        private void SendGuestNotification(Thread thread)
        {
            if (string.IsNullOrEmpty(thread.ClientEmail))
            {
                throw new Exception("thread.ClientEmail is empty");
            }

            if (string.IsNullOrEmpty(thread.ClientFullName))
            {
                throw new Exception("thread.ClientFullName is empty");
            }

            var message = BuildMessage(thread, _fileProvider);

            SendMessage(message);
        }
    }
}