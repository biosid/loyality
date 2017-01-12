using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;

namespace Vtb24.Arms.AdminServices
{
    public interface IAdminFeedbackService
    {
        AdminGetThreadsResult GetThreads(AdminGetThreadsParameters options);
        GetThreadMessagesResult GetThreadMessages(AdminGetThreadMessagesParameters options);
        ReplyResult Reply(AdminReplyParameters options);
        ChangeAnsweredStatusResult ChangeAnsweredStatus(ChangeAnsweredStatusParameters options);
    }
}