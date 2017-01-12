using System.ServiceModel;
using RapidSoft.Loaylty.Monitoring;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;

namespace Rapidsoft.Loyalty.NotificationSystem.API
{
    /// <summary>
    /// Интерфейс сервиса для работы с сообщениями клиента.
    /// </summary>
    [ServiceContract]
    public interface IClientMessageService : ISupportService
    {
        [OperationContract]
        ClientGetThreadsResult GetThreads(ClientGetThreadsParameters parameters);
        
        [OperationContract]
        GetStatisticsResult GetStatistics(GetStatisticsParameters parameters);

        [OperationContract]
        GetThreadByIdResult GetThreadById(ClientGetThreadByIdParameters parameters);

        [OperationContract]
        GetThreadMessagesResult GetThreadMessages(ClientGetThreadMessagesParameters parameters);

        [OperationContract]
        ReplyResult Reply(ClientReplyParameters parameters);

        [OperationContract]
        MarkThreadAsReadResult MarkThreadAsRead(MarkThreadAsReadParameters parameters);

        [OperationContract]
        NotifyClientsResult Notify(NotifyClientsParameters parameters);

        [OperationContract]
        void Unsubscribe(string emailHash);

        [OperationContract]
        ResultBase Delete(DeleteThreadParameters parameters);
    }
}
