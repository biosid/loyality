using Vtb24.Site.Services.ClientMessageService;

namespace Vtb24.Site.Services.ClientMessage.Stubs
{
    public class ClientMessageServiceStub : IClientMessageService
    {
        public ClientGetThreadsResult GetThreads(ClientGetThreadsParameters parameters)
        {
            return new ClientGetThreadsResult
            {
                Success = true,
                Threads = new Thread[0],
                TotalCount = 0
            };
        }

        public GetStatisticsResult GetStatistics(GetStatisticsParameters parameters)
        {
            return new GetStatisticsResult
            {
                Success = true,
                ThreadsCount = 0,
                UnreadThreadsCount = 0
            };
        }

        public GetThreadByIdResult GetThreadById(ClientGetThreadByIdParameters parameters)
        {
            return new GetThreadByIdResult
            {
                Success = false
            };
        }

        public GetThreadMessagesResult GetThreadMessages(ClientGetThreadMessagesParameters parameters)
        {
            return new GetThreadMessagesResult
            {
                Success = false
            };
        }

        public ReplyResult Reply(ClientReplyParameters parameters)
        {
            return new ReplyResult
            {
                Success = false
            };
        }

        public MarkThreadAsReadResult MarkThreadAsRead(MarkThreadAsReadParameters parameters)
        {
            return new MarkThreadAsReadResult
            {
                Success = false
            };
        }

        public NotifyClientsResult Notify(NotifyClientsParameters parameters)
        {
            return new NotifyClientsResult
            {
                Success = false
            };
        }

        public void Unsubscribe(string emailHash)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(DeleteThreadParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public void Subscribe(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}
