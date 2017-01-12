using System.ServiceModel;
using RapidSoft.Loaylty.Monitoring;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;

namespace Rapidsoft.Loyalty.NotificationSystem.API
{
    /// <summary>
    /// Интерфейс сервиса для переписки с клиентами.
    /// </summary>
    [ServiceContract]
    public interface IAdminFeedbackService : ISupportService
    {
        [OperationContract]
        AdminGetThreadsResult GetThreads(AdminGetThreadsParameters parameters);

        [OperationContract]
        GetThreadMessagesResult GetThreadMessages(AdminGetThreadMessagesParameters parameters);

        [OperationContract]
        ReplyResult Reply(AdminReplyParameters parameters);
        
        [OperationContract]
        ChangeAnsweredStatusResult ChangeAnsweredStatus(ChangeAnsweredStatusParameters parameters);

        [OperationContract]
        GetThreadByIdResult GetThreadById(AdminGetThreadByIdParameters parameters);
    }
}
