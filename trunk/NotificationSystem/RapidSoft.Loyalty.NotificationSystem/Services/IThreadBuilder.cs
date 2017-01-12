using System;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    public interface IThreadBuilder
    {
        ThreadMessage CreateThreadMessage(Guid threadId, MessageTypes messageType, string body, string authorId, string authorName, string authorEmail, MessageAttachment[] attachments, bool isClientUnread);

        Thread CreateThread(string messageTitle, string clientEmail, string clientFullName, string clientId, ThreadClientTypes clientType, string firstMessageBy, string lastMessageBy, DateTime? since, DateTime? until, FeedbackTypes type, MessageTypes firstMessageType, MessageTypes lastMessageType, string metaData);

        Thread CreateNotification(string clientId, DateTime timestamp, string title, string text, DateTime? since, DateTime? until);
    }
}
