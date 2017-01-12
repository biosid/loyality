using System;
using System.Data;
using System.Linq;

using Rapidsoft.Loyalty.NotificationSystem.API.Entities;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess.Models;
using Rapidsoft.Loyalty.NotificationSystem.Services;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess.Repositories
{
    public class ThreadsRepository : IThreadsRepository
    {
        public const int THREAD_MESSAGES_START_INDEX = 0;

        #region IThreadsRepository Members

        public Guid Add(Thread thread)
        {
            using (var ctx = new NotificationSystemContext())
            {
                if (thread.Id == Guid.Empty)
                {
                    thread.Id = Guid.NewGuid();
                }

                ctx.Threads.Add(thread);
                ctx.SaveChanges();
            }

            return thread.Id;
        }

        public AdminGetThreadsResult GetThreadsPage(AdminGetThreadsParameters parameters)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var query = ctx.Threads
                               .Where(t => !t.IsDeleted &&
                                           (t.Type == ThreadTypes.Issue ||
                                            t.Type == ThreadTypes.Suggestion ||
                                            t.Type == ThreadTypes.OrderIncident))
                               .Join(ctx.ThreadMessages,
                                     thread => thread.Id,
                                     message => message.ThreadId,
                                     (thread, message) => new { thread, message });

                // Условия
                if (!string.IsNullOrEmpty(parameters.ClientId))
                {
                    query = query.Where(t => t.thread.ClientId == parameters.ClientId);
                }

                if (parameters.Filter == AnsweredFilters.Answered)
                {
                    query = query.Where(t => t.thread.IsAnswered);
                }

                if (parameters.Filter == AnsweredFilters.Unanswered)
                {
                    query = query.Where(t => !t.thread.IsAnswered);
                }

                if (parameters.FeedbackType.HasValue)
                {
                    var threadType = ApiHelper.ConvertToThreadType(parameters.FeedbackType.Value);
                    query = query.Where(t => t.thread.Type == threadType);
                }

                if (parameters.MinDate.HasValue)
                {
                    query = query.Where(t => parameters.MinDate <= t.message.InsertedDate);
                }

                if (parameters.MaxDate.HasValue)
                {
                    query = query.Where(t => t.message.InsertedDate <= parameters.MaxDate);
                }

                if (parameters.ClientType.HasValue)
                {
                    query = query.Where(t => t.thread.ClientType == parameters.ClientType);
                }

                if (!string.IsNullOrEmpty(parameters.ClientEmail))
                {
                    query = query.Where(t => t.thread.ClientEmail == parameters.ClientEmail);
                }

                if (!string.IsNullOrEmpty(parameters.SearchTerm))
                {
                    query = query.Where(t => t.message.MessageBody.Contains(parameters.SearchTerm) ||
                                             t.message.AuthorFullName.Contains(parameters.SearchTerm));
                }

                if (!string.IsNullOrEmpty(parameters.OperatorLogin))
                {
                    query = query.Where(t => t.message.AuthorId == parameters.OperatorLogin &&
                                             t.message.MessageType == MessageTypes.OperatorMessage);
                }

                // Вычисление общего колличества записей
                var totalCount = query.Select(join => join.thread).Distinct().Count();

                var threadQuery = query.Select(join => join.thread).Distinct();

                if (parameters.Filter == AnsweredFilters.UnansweredFirst)
                {
                    threadQuery = threadQuery.OrderByDescending(join => !join.IsAnswered)
                                             .ThenByDescending(t => t.LastMessageTime);
                }
                else
                {
                    threadQuery = threadQuery.OrderByDescending(join => join.LastMessageTime);
                }

                var threads = threadQuery.Skip(parameters.CountToSkip).Take(parameters.CountToTake).ToArray();
                var threadIds = threads.Select(t => t.Id).ToArray();

                var foundedMessages = query.Where(tm => threadIds.Contains(tm.message.ThreadId))
                                           .Select(t => t.message)
                                           .Distinct()
                                           .ToArray();

                foreach (var thread in threads)
                {
                    thread.TopicMessage = ctx.ThreadMessages
                                             .SingleOrDefault(tm => tm.ThreadId == thread.Id &&
                                                                    tm.Index == THREAD_MESSAGES_START_INDEX);
                }

                var messageSearchPerformed = !(string.IsNullOrEmpty(parameters.SearchTerm) &&
                                               string.IsNullOrEmpty(parameters.OperatorLogin));

                var searchResults = threads.Select(t => new ThreadSearchResult
                {
                    Thread = t,
                    MessageMatchIndexes = messageSearchPerformed
                                              ? foundedMessages.Where(tm => tm.ThreadId == t.Id)
                                                               .Select(tm => tm.Index)
                                                               .ToArray()
                                              : null
                }).ToArray();

                return new AdminGetThreadsResult
                {
                    Result = searchResults, 
                    TotalCount = totalCount
                };
            }
        }

        public ThreadMessagesPage GetThreadMessagesPage(GetThreadMessagesParameters parameters)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var query = ctx.ThreadMessages.Where(tm => tm.ThreadId == parameters.ThreadId);

                query = query.OrderBy(tm => tm.InsertedDate);

                var totalCount = query.Count();

                var messages = query.Skip(parameters.CountToSkip).Take(parameters.CountToTake).ToArray();

                var messageIds = messages.Select(m => m.Id).ToArray();

                var messageAttachments = ctx.Attachments.Where(a => messageIds.Contains(a.MessageId)).ToArray();

                foreach (var message in messages)
                {
                    message.Attachments = messageAttachments.Where(a => a.MessageId == message.Id).ToArray();
                }

                return new ThreadMessagesPage
                {
                    ThreadMessages = messages, 
                    TotalCount = totalCount
                };
            }
        }

        public Thread UpToDateThread(Guid threadId, bool? isAnswered)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var thread = ctx.Threads.Single(t => !t.IsDeleted && t.Id == threadId);

                if (isAnswered.HasValue)
                {
                    thread.IsAnswered = isAnswered.Value;
                }

                thread.MessagesCount = ctx.ThreadMessages.Count(tm => tm.ThreadId == threadId);
                thread.UnreadMessagesCount = ctx.ThreadMessages.Count(tm => tm.ThreadId == threadId && tm.IsUnread);

                var firstMessage =
                    ctx.ThreadMessages.Where(tm => tm.ThreadId == threadId).OrderBy(tm => tm.Index).First();
                var lastMessage =
                    ctx.ThreadMessages.Where(tm => tm.ThreadId == threadId).OrderByDescending(tm => tm.Index).First();

                thread.TopicMessage = firstMessage;

                thread.FirstMessageTime = firstMessage.InsertedDate;
                thread.FirstMessageBy = GetFirstMessageBy(firstMessage);
                thread.FirstMessageType = firstMessage.MessageType;

                thread.LastMessageTime = lastMessage.InsertedDate;
                thread.LastMessageBy = GetFirstMessageBy(lastMessage);
                thread.LastMessageType = lastMessage.MessageType;
                
                ctx.Entry(thread).State = EntityState.Modified;
                ctx.SaveChanges();
                                
                return thread;
            }
        }

        public Thread ChangeAnsweredStatus(Guid threadId, bool isAnswered)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var thread = ctx.Threads.SingleOrDefault(t => !t.IsDeleted && t.Id == threadId);

                if (thread == null)
                {
                    throw new InvalidOperationException(string.Format("Thread with id {0} not found", threadId));
                }

                thread.IsAnswered = isAnswered;

                ctx.Entry(thread).State = EntityState.Modified;
                ctx.SaveChanges();
                return thread;
            }
        }

        public ClientThreadsPage GetClientThreadsPage(ClientGetThreadsParameters parameters)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var nowDate = DateTime.Now;

                var clientThreads = GetClientThreads(ctx);

                var query = clientThreads.Join(ctx.ThreadMessages.Where(tm => tm.Index == THREAD_MESSAGES_START_INDEX),
                                               thread => thread.Id,
                                               message => message.ThreadId,
                                               (thread, message) => new { thread, message })
                                         .Where(join => join.thread.ClientId == parameters.ClientId &&
                                                        join.thread.ClientType == ThreadClientTypes.Client &&
                                                        (join.thread.ShowSince == null || join.thread.ShowSince <= nowDate) &&
                                                        (join.thread.ShowUntil == null || nowDate <= join.thread.ShowUntil));

                if (parameters.Filter == ReadFilters.Read)
                {
                    query = query.Where(t => t.thread.UnreadMessagesCount == 0);
                }

                if (parameters.Filter == ReadFilters.Unread)
                {
                    query = query.Where(t => t.thread.UnreadMessagesCount > 0);
                }

                // Вычисление общего колличества записей
                var totalCount = query.Count();

                if (parameters.Filter == ReadFilters.UnreadFirst)
                {
                    query = query.OrderByDescending(t => t.thread.UnreadMessagesCount > 0)
                                 .ThenByDescending(t => t.thread.LastMessageTime);
                }
                else
                {
                    query = query.OrderByDescending(t => t.thread.LastMessageTime);
                }

                var joinedRecords = query.Skip(parameters.CountToSkip).Take(parameters.CountToTake).ToArray();

                foreach (var record in joinedRecords)
                {
                    record.thread.TopicMessage = record.message;
                }

                return new ClientThreadsPage
                {
                    Threads = joinedRecords.Select(r => r.thread).ToArray(), 
                    TotalCount = totalCount
                };
            }
        }

        public ClientThreadStatistics GetClientThreadStatistics(string clientId)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var nowDate = DateTime.Now;

                var clientThreads = GetClientThreads(ctx);

                var threadsCount = clientThreads.Count(t => t.ClientId == clientId &&
                                                            (t.ShowSince == null || t.ShowSince <= nowDate) &&
                                                            (t.ShowUntil == null || nowDate <= t.ShowUntil));

                var unreadThreadsCount = clientThreads.Count(t => t.ClientId == clientId &&
                                                                  t.UnreadMessagesCount > 0 &&
                                                                  (t.ShowSince == null || t.ShowSince <= nowDate) &&
                                                                  (t.ShowUntil == null || nowDate <= t.ShowUntil));

                return new ClientThreadStatistics
                {
                    ThreadsCount = threadsCount, 
                    UnreadThreadsCount = unreadThreadsCount
                };
            }
        }

        public Thread GetThread(Guid id, bool getDeleted)
        {
            using (var ctx = new NotificationSystemContext())
            {
                return getDeleted
                           ? ctx.Threads.SingleOrDefault(t => t.Id == id)
                           : ctx.Threads.SingleOrDefault(t => !t.IsDeleted && t.Id == id);
            }
        }

        public Thread MarkThreadAsRead(Guid threadId)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var thread = ctx.Threads.Single(t => !t.IsDeleted && t.Id == threadId);
                thread.UnreadMessagesCount = 0;
                ctx.ThreadMessages.Where(tm => tm.ThreadId == threadId).ToList().ForEach(tm => tm.IsUnread = false);
                ctx.SaveChanges();
                return thread;
            }
        }

        public ThreadMessage GetTopicMessage(Guid threadId)
        {
            using (var ctx = new NotificationSystemContext())
            {
                return ctx.ThreadMessages.SingleOrDefault(t => t.ThreadId == threadId && t.Index == THREAD_MESSAGES_START_INDEX);
            }
        }

        public void Delete(Guid threadId)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var thread = ctx.Threads.SingleOrDefault(t => !t.IsDeleted && t.Id == threadId);

                if (thread == null)
                {
                    return;
                }

                thread.IsDeleted = true;

                ctx.SaveChanges();
            }
        }

        public bool VerifyThread(Guid threadId, string clientId)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var thread = ctx.Threads.SingleOrDefault(t => !t.IsDeleted && t.Id == threadId);

                if (thread == null)
                {
                    return true;
                }

                switch (thread.ClientType)
                {
                    case ThreadClientTypes.Client:
                        return thread.ClientId == clientId;

                    case ThreadClientTypes.Guest:
                        return string.IsNullOrEmpty(clientId);

                    default:
                        return false;
                }
            }
        }

        #endregion

        private static string GetFirstMessageBy(ThreadMessage firstMessage)
        {
            return firstMessage.MessageType == MessageTypes.OperatorMessage
                ? firstMessage.AuthorId
                : firstMessage.AuthorFullName;
        }

        private static IQueryable<Thread> GetClientThreads(NotificationSystemContext ctx)
        {
            return ctx.Threads
                      .Where(t => !t.IsDeleted &&
                                  (t.Type == ThreadTypes.Notification ||
                                   ctx.ThreadMessages
                                      .Any(tm => tm.ThreadId == t.Id &&
                                                 tm.MessageType == MessageTypes.OperatorMessage)));
        }
    }
}
