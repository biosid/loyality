using Rapidsoft.Loyalty.NotificationSystem.DataAccess;

namespace Rapidsoft.Loyalty.NotificationSystem.Tests
{
    using System;
    using System.Linq;

    using API.Entities;

    public class NotificationDB
    {
        public static int minutes = 0;
        public static DateTime NewThread2MinDate;
        public static DateTime newThread3MaxDate;
        public static string UserId = "vtbSystemUser";
        public static Thread newThread1;
        public static Thread newThread2;
        public static Thread newThread3;
        public static Thread newThread4;
        public static Thread threadGuest;
        public static Thread threadStat1;
        public static Thread threadStat2;
        public static Thread threadMarkRead;
        public static Thread threadMarkAnwsered;
        private static Thread notificationThread;
        public static Thread ThreadSinceNextMonth;
        public const string ThreadsClientId = "ThreadsClientId";
        public const string GetThreadsStatClientId = "GetThreadsStatClientId";
        public const string MarkThreadAsReadClientId = "MarkThreadAsReadClientId";
        public const string ThreadNextMonthClientId = "threadNextMonthClientId";

        public static ThreadMessage[] GetThreadMessages(Guid threadId)
        {
            using (var ctx = new NotificationSystemContext())
            {
                return ctx.ThreadMessages.Where(tm => tm.ThreadId == threadId).OrderBy(tm => tm.Index).ToArray();
            }
        }

        public static Thread GetThread(Guid threadId)
        {
            using (var ctx = new NotificationSystemContext())
            {
                return ctx.Threads.Where(tm1 => tm1.Id == threadId).SingleOrDefault();
            }
        }

        public static void CreateTestData()
        {
            notificationThread = NewThread(ThreadsClientId, type: ThreadTypes.Notification);
            NewThreadMessage(notificationThread.Id);

            newThread1 = NewThread(ThreadsClientId, "t1ClientEmail", 1, isAnwered: true);
            NewThreadMessage(newThread1.Id);

            newThread2 = NewThread(ThreadsClientId, "t2ClientEmail", 1, isAnwered: true);
            var newThread2Message1 = NewThreadMessage(newThread2.Id);
            NewThread2MinDate = newThread2Message1.InsertedDate.AddMilliseconds(-10);                
            var newThread2Message2 = NewThreadMessage(
                newThread2.Id, 
                index: 1,
                messageType: MessageTypes.OperatorMessage, 
                authorFullName: "Василий Алибабаевич", 
                authorId: "operatorvasya");
            NewAttachment(newThread2.Id, newThread2Message2.Id);
            NewAttachment(newThread2.Id, newThread2Message2.Id);                
            NewThreadMessage(newThread2.Id, index: 2);
            NewThreadMessage(newThread2.Id, index: 3);

            newThread3 = NewThread(ThreadsClientId, "t3ClientEmail", 0);
            NewThreadMessage(newThread3.Id, messageBody: "текст от андрея");
            NewThreadMessage(
                            newThread3.Id,
                            index: 1,
                            messageType: MessageTypes.OperatorMessage,
                            authorFullName: "Иван Петрович",
                            authorId: "operivan");
            var newThread3Message2 = NewThreadMessage(newThread3.Id, index: 3);
            newThread3MaxDate = newThread3Message2.InsertedDate.AddMilliseconds(10);

            newThread4 = NewThread(ThreadsClientId, "t4ClientEmail");
            NewThreadMessage(newThread4.Id);
            NewThreadMessage(newThread4.Id, index: 1, authorFullName: "Иванов Андрей Михайлович");

            threadGuest = NewThreadGuest();
            NewThreadMessage(threadGuest.Id);

            threadStat1 = NewThread(GetThreadsStatClientId, unreadMessagesCount: 1);
            NewThreadMessage(threadStat1.Id);
            NewThreadMessage(threadStat1.Id, index: 1, messageType: MessageTypes.OperatorMessage);

            threadStat2 = NewThread(GetThreadsStatClientId);
            NewThreadMessage(threadStat2.Id);
            NewThreadMessage(threadStat2.Id, index: 1, messageType: MessageTypes.OperatorMessage);

            var threadStatForNextMonth = NewThread(GetThreadsStatClientId, showSince: DateTime.Now.AddMonths(1));
            NewThreadMessage(threadStatForNextMonth.Id);
            NewThreadMessage(threadStatForNextMonth.Id, index: 1, messageType: MessageTypes.OperatorMessage);

            threadMarkRead = NewThread(MarkThreadAsReadClientId, unreadMessagesCount: 1);
            NewThreadMessage(threadMarkRead.Id);

            threadMarkAnwsered = NewThread(MarkThreadAsReadClientId, isAnwered: false);
            NewThreadMessage(threadMarkAnwsered.Id);

            ThreadSinceNextMonth = NewThread(ThreadNextMonthClientId, showSince: DateTime.Now.AddMonths(1));
            NewThreadMessage(ThreadSinceNextMonth.Id);
        }

        private static MessageAttachment NewAttachment(Guid threadId, int messageId)
        {
            var attachment = new MessageAttachment()
            {
                ThreadId = threadId, MessageId = messageId, FileName = "attach1", FileSize = 10, Id = Guid.NewGuid()
            };

            using (var ctx = new NotificationSystemContext())
            {
                ctx.Attachments.Add(attachment);
                ctx.SaveChanges();
            }

            return attachment;
        }

        public static void DeleteTestData()
        {
            using (var ctx = new NotificationSystemContext())
            {
                var sql = string.Format(
                    @"declare @deletedIds table ( id uniqueidentifier )
insert into @deletedIds
select Id from [mess].[Threads]

delete from [mess].[Attachments]
where ThreadId in (select * from @deletedIds)

delete from [mess].[ThreadMessages]
where ThreadId in (select * from @deletedIds)

delete from [mess].[Threads]
where Id in (select * from @deletedIds)");
                ctx.Database.ExecuteSqlCommand(sql);
                ctx.SaveChanges();
            }
        }

        private static ThreadMessage NewThreadMessage(
            Guid threadId,
            int index = 0,
            bool isUnread = true,
            string messageBody = "c",
            string authorFullName = "b",
            DateTime? insertedDate = null,
            MessageTypes messageType = MessageTypes.ClientMessage,
            string authorId = null)
        {
            if (!insertedDate.HasValue)
            {
                insertedDate = DateTime.Now.AddMinutes(minutes);
                minutes++;
            }

            var message = new ThreadMessage()
            {
                ThreadId = threadId,
                MessageBody = messageBody,
                Index = index,
                IsUnread = isUnread,
                MessageType = messageType,
                AuthorFullName = authorFullName,
                AuthorId = authorId,
                AuthorEmail = "a",
                InsertedDate = insertedDate.Value
            };

            using (var ctx = new NotificationSystemContext())
            {
                ctx.ThreadMessages.Add(message);
                ctx.SaveChanges();
            }

            return message;
        }

        private static Thread NewThreadGuest(int unreadMessagesCount = 0, ThreadTypes type = ThreadTypes.Suggestion)
        {
            var nowDateTime = DateTime.Now;

            var thread = new Thread()
            {
                ClientEmail = "Egor.Goncharov@rapidsoft.ru",
                ClientFullName = "Ульянов Владимир Ильич",
                ClientId = null,
                ClientType = ThreadClientTypes.Guest,
                Id = Guid.NewGuid(),
                IsClosed = false,
                Title = "GuestTitle",
                Type = type,
                InsertedDate = nowDateTime,
                MessagesCount = 1,
                UnreadMessagesCount = unreadMessagesCount,
                FirstMessageTime = nowDateTime,
                LastMessageTime = nowDateTime,
                FirstMessageBy = null,
                LastMessageBy = null
            };

            using (var ctx = new NotificationSystemContext())
            {
                ctx.Threads.Add(thread);
                ctx.SaveChanges();
            }

            return thread;
        }

        private static Thread NewThread(
            string clientId, 
            string clientEmail = "clientEmail", 
            int unreadMessagesCount = 0,
            ThreadTypes type = ThreadTypes.Suggestion,
            bool isAnwered = false, 
            DateTime? showSince = null, 
            DateTime? showUntil = null)
        {
            var nowDateTime = DateTime.Now.AddMinutes(minutes);
            minutes++;

            var thread = new Thread()
            {
                ClientEmail = clientEmail,
                ClientFullName = "ClientFullName",
                ClientId = clientId,
                ClientType = ThreadClientTypes.Client,
                Id = Guid.NewGuid(),
                IsClosed = false,
                Title = "ThreadTitle",
                Type = type,
                InsertedDate = nowDateTime,
                MessagesCount = 1,
                UnreadMessagesCount = unreadMessagesCount,
                FirstMessageTime = nowDateTime,
                LastMessageTime = nowDateTime,
                FirstMessageBy = "ClientFullName",
                LastMessageBy = "ClientFullName",
                IsAnswered = isAnwered,
                ShowSince = showSince,
                ShowUntil = showUntil
            };

            using (var ctx = new NotificationSystemContext())
            {
                ctx.Threads.Add(thread);
                ctx.SaveChanges();
            }

            return thread;
        } 
    }
}