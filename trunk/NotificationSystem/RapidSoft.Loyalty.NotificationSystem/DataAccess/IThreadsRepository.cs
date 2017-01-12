using System;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess.Models;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess
{
    public interface IThreadsRepository
    {
        Guid Add(Thread thread);

        AdminGetThreadsResult GetThreadsPage(AdminGetThreadsParameters parameters);

        ThreadMessagesPage GetThreadMessagesPage(GetThreadMessagesParameters parameters);

        Thread UpToDateThread(Guid threadId, bool? isAnswered);

        Thread ChangeAnsweredStatus(Guid threadId, bool isAnswered);

        ClientThreadsPage GetClientThreadsPage(ClientGetThreadsParameters parameters);

        ClientThreadStatistics GetClientThreadStatistics(string clientId);

        Thread GetThread(Guid id, bool getDeleted);

        Thread MarkThreadAsRead(Guid threadId);

        ThreadMessage GetTopicMessage(Guid threadId);

        void Delete(Guid threadId);

        bool VerifyThread(Guid threadId, string clientId);
    }
}
