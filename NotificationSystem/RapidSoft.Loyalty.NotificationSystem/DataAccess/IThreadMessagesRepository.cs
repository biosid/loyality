using System;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess
{
    public interface IThreadMessagesRepository
    {
        int Add(ThreadMessage message);

        void AddAttachments(MessageAttachment[] attachments);
    }
}
