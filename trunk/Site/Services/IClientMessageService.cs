using Vtb24.Site.Services.ClientMessageService;

namespace Vtb24.Site.Services
{
    public interface IClientMessageService
    {
        ClientGetThreadsResult GetThreads(ClientGetThreadsParameters parameters);

        GetStatisticsResult GetStatistics(GetStatisticsParameters parameters);

        GetThreadByIdResult GetThreadById(ClientGetThreadByIdParameters parameters);

        GetThreadMessagesResult GetThreadMessages(ClientGetThreadMessagesParameters parameters);

        ReplyResult Reply(ClientReplyParameters parameters);

        MarkThreadAsReadResult MarkThreadAsRead(MarkThreadAsReadParameters parameters);

        NotifyClientsResult Notify(NotifyClientsParameters parameters);

        void Unsubscribe(string emailHash);

        void Delete(DeleteThreadParameters parameters);
    }
}
