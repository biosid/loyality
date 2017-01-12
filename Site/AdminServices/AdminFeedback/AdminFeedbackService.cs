using Vtb24.Arms.AdminServices.AdminFeedback.Models.Exceptions;
using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;

namespace Vtb24.Arms.AdminServices.AdminFeedback
{
    public class AdminFeedbackService : IAdminFeedbackService
    {
        public AdminGetThreadsResult GetThreads(AdminGetThreadsParameters options)
        {
            using (var client = new AdminFeedbackServiceClient())
            {
                var response = client.GetThreads(options);
                HandleResultCode(response.ResultCode, response.ResultDescription);
                return response;
            }
        }

        public GetThreadMessagesResult GetThreadMessages(AdminGetThreadMessagesParameters options)
        {
            using (var client = new AdminFeedbackServiceClient())
            {
                var response = client.GetThreadMessages(options);
                HandleResultCode(response.ResultCode, response.ResultDescription);
                return response;
            }
        }

        public ReplyResult Reply(AdminReplyParameters options)
        {
            using (var client = new AdminFeedbackServiceClient())
            {
                var response = client.Reply(options);
                HandleResultCode(response.ResultCode, response.ResultDescription);
                return response;
            }
        }

        public ChangeAnsweredStatusResult ChangeAnsweredStatus(ChangeAnsweredStatusParameters options)
        {
            using (var client = new AdminFeedbackServiceClient())
            {
                var response = client.ChangeAnsweredStatus(options);
                HandleResultCode(response.ResultCode, response.ResultDescription);
                return response;
            }
        }

        private static void HandleResultCode(int resultCode, string message)
        {
            switch (resultCode)
            {
                case 0:
                    break;
                default:
                    throw new AdminFeedbackException(resultCode, message);
            }
        }
        
    }
}