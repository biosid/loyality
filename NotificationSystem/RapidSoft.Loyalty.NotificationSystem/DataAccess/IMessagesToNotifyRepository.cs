using System;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess
{
    public interface IMessagesToNotifyRepository
    {
        ThreadMessagesToNotify[] GetAll();

        void Add(Guid threadId, int messageIndex, DateTime messageTime);

        void Remove(ThreadMessagesToMarkAsNotified[] threadMessages);
    }
}
