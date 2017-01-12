using System;
using System.Threading.Tasks;
using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientMessageService;

namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
    public class ClientMessageServiceStub : StubBase, IClientMessageService
    {
        public string Echo(string message)
        {
            throw new NotImplementedException();
        }

        public Task<string> EchoAsync(string message)
        {
            throw new NotImplementedException();
        }

        public ClientGetThreadsResult GetThreads(ClientGetThreadsParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ClientGetThreadsResult> GetThreadsAsync(ClientGetThreadsParameters parameters)
        {
            throw new NotImplementedException();
        }

        public GetStatisticsResult GetStatistics(GetStatisticsParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<GetStatisticsResult> GetStatisticsAsync(GetStatisticsParameters parameters)
        {
            throw new NotImplementedException();
        }

        public GetThreadByIdResult GetThreadById(ClientGetThreadByIdParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<GetThreadByIdResult> GetThreadByIdAsync(ClientGetThreadByIdParameters parameters)
        {
            throw new NotImplementedException();
        }

        public GetThreadMessagesResult GetThreadMessages(ClientGetThreadMessagesParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<GetThreadMessagesResult> GetThreadMessagesAsync(ClientGetThreadMessagesParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ReplyResult Reply(ClientReplyParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ReplyResult> ReplyAsync(ClientReplyParameters parameters)
        {
            throw new NotImplementedException();
        }

        public MarkThreadAsReadResult MarkThreadAsRead(MarkThreadAsReadParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<MarkThreadAsReadResult> MarkThreadAsReadAsync(MarkThreadAsReadParameters parameters)
        {
            throw new NotImplementedException();
        }

        public NotifyClientsResult Notify(NotifyClientsParameters parameters)
        {
            return new NotifyClientsResult
            {
                Threads = new[]
                {
                    new Thread
                    {
                        ClientId = "1"
                    },
                    new Thread
                    {
                        ClientId = "2"
                    },
                    new Thread
                    {
                        ClientId = "1"
                    }
                }
            };
        }

        public Task<NotifyClientsResult> NotifyAsync(NotifyClientsParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}