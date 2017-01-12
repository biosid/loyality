using System;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess.Repositories;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    public class ThreadBuilder : IThreadBuilder
    {
        public ThreadBuilder() : this(null)
        {
        }

        public ThreadBuilder(IThreadsRepository threadsRepository = null,
                             IThreadMessagesRepository threadMessagesRepository = null,
                             IMessagesToNotifyRepository messagesToNotifyRepository = null,
                             IMessageNotificationService messageNotificationService = null)
        {
            _threadMessagesRepository = threadMessagesRepository ?? new ThreadMessagesRepository();
            _threadsRepository = threadsRepository ?? new ThreadsRepository();
            _messagesToNotifyRepository = messagesToNotifyRepository ?? new MessagesToNotifyRepository();
            _messageNotificationService = messageNotificationService ?? new MessageNotificationService();
        }

        private readonly IThreadMessagesRepository _threadMessagesRepository;
        private readonly IThreadsRepository _threadsRepository;
        private readonly IMessagesToNotifyRepository _messagesToNotifyRepository;
        private readonly IMessageNotificationService _messageNotificationService;

        #region IThreadBuilder Members

        public ThreadMessage CreateThreadMessage(
            Guid threadId, 
            MessageTypes messageType, 
            string body, 
            string authorId,
            string authorName, 
            string authorEmail, 
            MessageAttachment[] attachments, 
            bool isClientUnread)
        {
            var message = new ThreadMessage
            {
                ThreadId = threadId, 
                MessageBody = body, 
                IsUnread = isClientUnread, 
                MessageType = messageType, 
                AuthorFullName = authorName, 
                AuthorId = authorId, 
                AuthorEmail = authorEmail, 
                InsertedDate = DateTime.Now, 
                Attachments = attachments
            };

            message.Id = _threadMessagesRepository.Add(message);

            if (message.Attachments != null)
            {
                foreach (var attachment in message.Attachments)
                {
                    attachment.ThreadId = threadId;
                    attachment.MessageId = message.Id;
                }

                _threadMessagesRepository.AddAttachments(attachments);
            }

            if (messageType == MessageTypes.ClientMessage ||
                messageType == MessageTypes.GuestMessage ||
                messageType == MessageTypes.OperatorMessage)
            {
                _messagesToNotifyRepository.Add(threadId, message.Index, message.InsertedDate);
            }

            if (messageType == MessageTypes.OperatorMessage ||
                messageType == MessageTypes.SystemMessage)
            {
                _messageNotificationService.SendMessageNotification(message);
            }

            return message;
        }

        public Thread CreateThread(string messageTitle, string clientEmail, string clientFullName, string clientId, ThreadClientTypes clientType, string firstMessageBy, string lastMessageBy, DateTime? since, DateTime? until, FeedbackTypes type, MessageTypes firstMessageType, MessageTypes lastMessageType, string metaData = null)
        {
            var nowDateTime = DateTime.Now;

            var thread = new Thread
            {
                Id = Guid.NewGuid(), 
                Type = ApiHelper.ConvertToThreadType(type), 
                IsClosed = false, 
                Title = messageTitle, 
                ClientEmail = clientEmail, 
                ClientFullName = clientFullName, 
                ClientId = clientId, 
                ClientType = clientType, 
                InsertedDate = nowDateTime, 
                FirstMessageTime = nowDateTime, 
                LastMessageTime = nowDateTime, 
                FirstMessageBy = firstMessageBy, 
                LastMessageBy = lastMessageBy, 
                UnreadMessagesCount = 1, 
                MessagesCount = 1, 
                IsAnswered = false, 
                ShowSince = since, 
                ShowUntil = until,
                FirstMessageType = firstMessageType,
                LastMessageType = lastMessageType,
                MetaData = metaData
            };

            _threadsRepository.Add(thread);

            return thread;
        }

        public Thread CreateNotification(string clientId, DateTime timestamp, string title, string text, DateTime? since, DateTime? until)
        {
            var thread = new Thread
            {
                Id = Guid.NewGuid(),
                Type = ThreadTypes.Notification,
                IsClosed = true,
                Title = title,
                ClientId = clientId,
                ClientType = ThreadClientTypes.Client,
                InsertedDate = timestamp,
                FirstMessageTime = timestamp,
                LastMessageTime = timestamp,
                UnreadMessagesCount = 1,
                MessagesCount = 1,
                IsAnswered = false,
                ShowSince = since,
                ShowUntil = until,
                FirstMessageType = MessageTypes.SystemMessage,
                LastMessageType = MessageTypes.SystemMessage
            };

            var threadId = _threadsRepository.Add(thread);

            var message = new ThreadMessage
            {
                ThreadId = threadId,
                
                MessageBody = text,
                IsUnread = true,
                MessageType = MessageTypes.SystemMessage,
                InsertedDate = timestamp
            };

            _threadMessagesRepository.Add(message);

            _messageNotificationService.SendMessageNotification(message);

            return thread;
        }

        #endregion
    }
}