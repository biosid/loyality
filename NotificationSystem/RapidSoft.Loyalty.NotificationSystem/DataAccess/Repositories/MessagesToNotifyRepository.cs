using System;
using System.Linq;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess.Repositories
{
    public class MessagesToNotifyRepository : IMessagesToNotifyRepository
    {
        public ThreadMessagesToNotify[] GetAll()
        {
            using (var ctx = new NotificationSystemContext())
            {
                return ctx.MessagesToNotify
                          .GroupBy(m => m.ThreadId)
                          .Select(ToThreadMessagesToNotify).ToArray();
            }
        }

        public void Add(Guid threadId, int messageIndex, DateTime messageTime)
        {
            using (var ctx = new NotificationSystemContext())
            {
                ctx.MessagesToNotify.Add(new MessageToNotify
                {
                    Id = Guid.NewGuid(),
                    ThreadId = threadId,
                    MessageIndex = messageIndex,
                    MessageTime = messageTime
                });

                ctx.SaveChanges();
            }
        }

        public void Remove(ThreadMessagesToMarkAsNotified[] threadMessages)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var messages = threadMessages.SelectMany(
                    t => ctx.MessagesToNotify
                            .Where(m => m.ThreadId == t.ThreadId &&
                                        m.MessageIndex <= t.LastMessageIndex))
                                             .ToArray();

                foreach (var message in messages)
                {
                    ctx.MessagesToNotify.Remove(message);
                }

                ctx.SaveChanges();
            }
        }

        private static ThreadMessagesToNotify ToThreadMessagesToNotify(IGrouping<Guid, MessageToNotify> group)
        {
            var messages = group.ToArray();
            return new ThreadMessagesToNotify
            {
                ThreadId = group.Key,
                FirstMessageIndex = messages.Min(m => m.MessageIndex),
                LastMessageIndex = messages.Max(m => m.MessageIndex)
            };
        }
    }
}
