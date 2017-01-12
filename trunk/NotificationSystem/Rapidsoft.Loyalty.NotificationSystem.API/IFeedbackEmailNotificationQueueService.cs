using System.ServiceModel;
using RapidSoft.Loaylty.Monitoring;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;

namespace Rapidsoft.Loyalty.NotificationSystem.API
{
    /// <summary>
    /// Интрефейс сервиса для работы с очередью сообщений для оповещения по email
    /// </summary>
    [ServiceContract]
    public interface IFeedbackEmailNotificationQueueService : ISupportService
    {
        [OperationContract]
        GetMessagesToNotifyResult GetMessagesToNotify();

        [OperationContract]
        ResultBase MarkMessagesAsNotified(MarkMessagesAsNotifiedParameters parameters);
    }
}
