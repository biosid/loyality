using System;
using System.Linq;
using Vtb24.Arms.AdminServices.FeedbackEmailNotificationQueueService;
using Vtb24.Arms.AdminServices.Models;

namespace ScheduledJobs.FeedbackByEmail
{
    public class NotificationQueueService
    {
        public Guid[] GetMessagesToNotify()
        {
            using (var service = new FeedbackEmailNotificationQueueServiceClient())
            {
                var result = service.GetMessagesToNotify();

                if (result.Success)
                {
                    return result.MessagesByThreadId.Select(m => m.ThreadId).ToArray();
                }
                throw new NotificationQueueServiceException(result.ResultCode, result.ResultDescription);
            }
        }

        public void MarkMessageAsNotified(Guid threadId, int messagesCount)
        {
            using (var service = new FeedbackEmailNotificationQueueServiceClient())
            {
                var result = service.MarkMessagesAsNotified(new MarkMessagesAsNotifiedParameters
                {
                    MessagesByThreadId = new[]
                    {
                        new ThreadMessagesToMarkAsNotified
                        {
                            ThreadId = threadId,
                            LastMessageIndex = messagesCount
                        }
                    }
                });

                if (!result.Success)
                {
                    throw new NotificationQueueServiceException(result.ResultCode, result.ResultDescription);
                }
            }
        }

        public class NotificationQueueServiceException : ComponentException
        {
            public NotificationQueueServiceException(int resultCode, string codeDescription) 
                : base("Очередь нотификаций операторов по обратной связи", resultCode, codeDescription, null)
            {
            }
        }
    }
}