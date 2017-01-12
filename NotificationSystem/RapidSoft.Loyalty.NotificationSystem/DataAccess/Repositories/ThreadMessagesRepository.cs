using System;
using System.Linq;

using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess.Repositories
{
    public class ThreadMessagesRepository : IThreadMessagesRepository
    {
        #region IThreadMessagesRepository Members

        public int Add(ThreadMessage message)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var maxThreadIndex = ctx.ThreadMessages
                                        .Where(tm => tm.ThreadId == message.ThreadId)
                                        .Max(tm => (int?) tm.Index);

                message.Index = maxThreadIndex.HasValue
                                    ? maxThreadIndex.Value + 1
                                    : ThreadsRepository.THREAD_MESSAGES_START_INDEX;

                ctx.ThreadMessages.Add(message);
                ctx.SaveChanges();
            }

            return message.Id;
        }

        public void AddAttachments(MessageAttachment[] attachments)
        {
            using (var ctx = new NotificationSystemContext())
            {
                foreach (var attachment in attachments)
                {
                    if (attachment.Id == Guid.Empty)
                    {
                        attachment.Id = Guid.NewGuid();
                    }

                    ctx.Attachments.Add(attachment);
                }

                ctx.SaveChanges();
            }
        }

        #endregion

        public ThreadMessage Get(int id)
        {
            using (var ctx = new NotificationSystemContext())
            {
                return ctx.ThreadMessages.SingleOrDefault(m => m.Id == id);
            }
        }
    }
}
