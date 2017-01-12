using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    public interface IMessageNotificationService
    {
        void SendMessageNotification(ThreadMessage threadMessage);

        void Unsubscribe(string emailHash);

        void AddToBlackList(string email);

        void RemoveFromBlackList(string email);
    }
}
