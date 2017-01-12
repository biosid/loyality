using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Vtb24.Site.Services.ClientMessage.Models.Exceptions;
using Vtb24.Site.Services.ClientMessageService;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Services.ClientMessage
{
    public class ClientMessageService : IClientMessageService
    {
        public ClientGetThreadsResult GetThreads(ClientGetThreadsParameters parameters)
        {
            using (var client = new ClientMessageServiceClient())
            {
                var response = client.GetThreads(parameters);

                AssertResponse(response);

                return response;
            }
        }

        public GetStatisticsResult GetStatistics(GetStatisticsParameters parameters)
        {
            using (var client = new ClientMessageServiceClient())
            {
                var response = client.GetStatistics(parameters);

                AssertResponse(response);

                return response;
            }
        }

        public GetThreadByIdResult GetThreadById(ClientGetThreadByIdParameters parameters)
        {
            using (var client = new ClientMessageServiceClient())
            {
                var response = client.GetThreadById(parameters);

                AssertResponse(response);

                return response;
            }
        }

        public GetThreadMessagesResult GetThreadMessages(ClientGetThreadMessagesParameters parameters)
        {
            using (var client = new ClientMessageServiceClient())
            {
                var response = client.GetThreadMessages(parameters);

                AssertResponse(response);

                return response;
            }
        }

        public ReplyResult Reply(ClientReplyParameters parameters)
        {
            using (var client = new ClientMessageServiceClient())
            {
                var response = client.Reply(parameters);

                AssertResponse(response);

                return response;
            }
        }

        public MarkThreadAsReadResult MarkThreadAsRead(MarkThreadAsReadParameters parameters)
        {
            using (var client = new ClientMessageServiceClient())
            {
                var response = client.MarkThreadAsRead(parameters);

                AssertResponse(response);

                return response;
            }
        }

        public NotifyClientsResult Notify(NotifyClientsParameters parameters)
        {
            using (var client = new ClientMessageServiceClient())
            {
                var response = client.Notify(parameters);

                AssertResponse(response);

                return response;
            }
        }

        public void Unsubscribe(string emailHash)
        {
            using (var client = new ClientMessageServiceClient())
            {
                client.Unsubscribe(emailHash);
            }
        }

        public void Delete(DeleteThreadParameters parameters)
        {
            using (var client = new ClientMessageServiceClient())
            {
                var response = client.Delete(parameters);

                AssertResponse(response);
            }
        }

        private static void AssertResponse(ResultBase response)
        {
            switch (response.ResultCode)
            {
                case 0:
                    return;
                case 2:
                    throw new ThreadNotFoundException(response.ResultCode, response.ResultDescription);
                case 1000:
                    throw new ThreadSecurityException(response.ResultCode, response.ResultDescription);
                default:
                    throw new ClientMessageServiceException(response.ResultCode, response.ResultDescription);
            }
        }
    }
}
