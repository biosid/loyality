namespace Rapidsoft.Loyalty.NotificationSystem.Tests.Services
{
    using System;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;

    using API.OutputResults;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using API.Entities;

    using API.InputParameters;

    using Moq;

    [TestClass]
    public class AdminFeedbackServiceTest
    {
        #region GetThreads

        [TestMethod]
        public void ShouldGetUnansweredThreadsFirstTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                ClientId = NotificationDB.ThreadsClientId,
                Filter = AnsweredFilters.UnansweredFirst
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);            
            helper.AssertResult(4);
            var threads = helper.GetThreads();

            Assert.AreEqual("t4ClientEmail", threads[0].ClientEmail);
            Assert.AreEqual("t3ClientEmail", threads[1].ClientEmail);
            Assert.AreEqual("t2ClientEmail", threads[2].ClientEmail);
            Assert.AreEqual("t1ClientEmail", threads[3].ClientEmail);
        }

        [TestMethod]
        public void ShouldGetOnlyUnansweredThreadsTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                ClientId = NotificationDB.ThreadsClientId,
                Filter = AnsweredFilters.Unanswered
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult(2);
            var threads = helper.GetThreads();

            Assert.AreEqual("t4ClientEmail", threads[0].ClientEmail);
            Assert.AreEqual("t3ClientEmail", threads[1].ClientEmail);
        }

        [TestMethod]
        public void ShouldGetOnlyAnsweredThreadsTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                ClientId = NotificationDB.ThreadsClientId,
                Filter = AnsweredFilters.Answered
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult(2);
            var threads = helper.GetThreads();

            Assert.AreEqual("t2ClientEmail", threads[0].ClientEmail);
            Assert.AreEqual("t1ClientEmail", threads[1].ClientEmail);
        }

        [TestMethod]
        public void ShouldIncludeTopicMessageTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                ClientId = NotificationDB.ThreadsClientId
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult();
            var threads = helper.GetThreads();

            Assert.IsTrue(threads.All(t => t.TopicMessage != null));
        }

        [TestMethod]
        public void ShouldFilterByThreadTypeTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                ClientId = NotificationDB.ThreadsClientId,
                FeedbackType = FeedbackTypes.Suggestion
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult(4);
            var threads = helper.GetThreads();

            Assert.AreEqual("t4ClientEmail", threads[0].ClientEmail);
            Assert.AreEqual("t3ClientEmail", threads[1].ClientEmail);
            Assert.AreEqual("t2ClientEmail", threads[2].ClientEmail);
            Assert.AreEqual("t1ClientEmail", threads[3].ClientEmail);
        }

        [TestMethod]
        public void ShouldFilterByMinMaxDateTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                ClientId = NotificationDB.ThreadsClientId,
                MinDate = NotificationDB.NewThread2MinDate,
                MaxDate = NotificationDB.newThread3MaxDate
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult(2);
            var threads = helper.GetThreads();

            Assert.AreEqual("t3ClientEmail", threads[0].ClientEmail);
            Assert.AreEqual("t2ClientEmail", threads[1].ClientEmail);
        }

        [TestMethod]
        public void ShouldFilterByClientTypeTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                ClientType = ThreadClientTypes.Guest,
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult(1);
            var threads = helper.GetThreads();

            Assert.IsNull(threads[0].ClientId);
            Assert.IsNotNull(threads[0].ClientEmail);
            Assert.IsNotNull(threads[0].ClientFullName);
        }

        [TestMethod]
        public void ShouldSearchByClientEmailTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                ClientEmail = "t3ClientEmail"
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult(1);
            var threads = helper.GetThreads();

            Assert.AreEqual("t3ClientEmail", threads[0].ClientEmail);
        }

        [TestMethod]
        public void ShouldSearchBySearchTermTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                SearchTerm = "андре"
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult(2);
            var threads = helper.GetThreads();

            Assert.AreEqual("t4ClientEmail", threads[0].ClientEmail);
            Assert.AreEqual("t3ClientEmail", threads[1].ClientEmail);

            var thread4Indexes = helper.GetMessageMatchIndexes("t4ClientEmail", 1);
            Assert.AreEqual(1, thread4Indexes[0]);

            var thread3Indexes = helper.GetMessageMatchIndexes("t3ClientEmail", 1);
            Assert.AreEqual(0, thread3Indexes[0]);
        }

        [TestMethod]
        public void ShouldSearchByOperatorLoginTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                OperatorLogin = "operatorvasya"
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult(1);
            var threads = helper.GetThreads();

            Assert.AreEqual("t2ClientEmail", threads[0].ClientEmail);

            var thread4Indexes = helper.GetMessageMatchIndexes("t2ClientEmail", 1);
            Assert.AreEqual(1, thread4Indexes[0]);
        }

        [TestMethod]
        public void ShouldPageRecordsTest()
        {
            var parameters = new AdminGetThreadsParameters()
            {
                UserId = NotificationDB.UserId,
                ClientId = NotificationDB.ThreadsClientId,
                CountToSkip = 1,
                CountToTake = 2
            };

            var helper = new GetThreadsClient();
            helper.Call(parameters);
            helper.AssertResult(2);
            var threads = helper.GetThreads();

            Assert.AreEqual("t3ClientEmail", threads[0].ClientEmail);
            Assert.AreEqual("t2ClientEmail", threads[1].ClientEmail);

            helper.GetMessageMatchIndexes("t3ClientEmail", 0);
            helper.GetMessageMatchIndexes("t2ClientEmail", 0);
        }

        #endregion

        #region GetThreadMessages

        [TestMethod]
        public void ShouldGetThreadMessagesTest()
        {
            var client = new GetThreadMessagesClient();

            var parameters = new AdminGetThreadMessagesParameters()
            {
                UserId = NotificationDB.UserId,
                ThreadId = NotificationDB.newThread2.Id,
                CountToSkip = 1,
                CountToTake = 2
            };

            client.Call(parameters);
            client.AssertResult(NotificationDB.newThread2.Id, 4, 2);

            var thread = client.GetThread();

            Assert.IsNotNull(thread);
            Assert.IsNotNull(thread.TopicMessage);

            var messages = client.GetMessages();

            Assert.AreEqual(1, messages[0].Index);
            Assert.IsNotNull(messages[0].Attachments);
            Assert.AreEqual(2, messages[0].Attachments.Length);
            Assert.AreEqual("attach1", messages[0].Attachments[0].FileName);

            Assert.AreEqual(2, messages[1].Index);
        }

        #endregion
        
        #region Reply

        [DeploymentItem("Images", "Images")]
        [TestMethod]
        public void ShouldReplyToGuestThreadTest()
        {
            var client = new ReplyMethodClient();

            var operatorLogin = "operatorPetr";
            var messageBody = "учтём ваши замечания";

            var parameters = new AdminReplyParameters()
            {
                ThreadId = NotificationDB.threadGuest.Id,
                UserId = operatorLogin, 
                MessageBody = messageBody
            };
            client.Call(parameters);
            client.AssertRes();

            var actualThreadMessages = NotificationDB.GetThreadMessages(NotificationDB.threadGuest.Id);

            Assert.AreEqual(2, actualThreadMessages.Length);

            var replyMessage = actualThreadMessages[1];

            Assert.AreEqual(1, replyMessage.Index);
            Assert.AreEqual(operatorLogin, replyMessage.AuthorId);
            Assert.AreEqual(messageBody, replyMessage.MessageBody);
            Assert.AreEqual(true, replyMessage.IsUnread);

            var actualThread = NotificationDB.GetThread(NotificationDB.threadGuest.Id);
            Assert.AreEqual(true, actualThread.IsAnswered);
            Assert.AreEqual(2, actualThread.MessagesCount);
            Assert.AreEqual(2, actualThread.UnreadMessagesCount);
            Assert.AreEqual(replyMessage.InsertedDate, actualThread.LastMessageTime);
            Assert.AreEqual(operatorLogin, actualThread.LastMessageBy);
            Assert.AreEqual(replyMessage.MessageType, actualThread.LastMessageType);
        }

        #endregion

        #region ChangeAnsweredStatus

        [TestMethod]
        public void ShouldChangeThreadAnsweredStatusTest()
        {
            var serviceProvider = new ServiceProvider();
            var service = serviceProvider.GetAdminFeedbackService();

            var parameters = new ChangeAnsweredStatusParameters()
            {
                UserId = NotificationDB.UserId,
                ThreadId = NotificationDB.threadMarkAnwsered.Id,
                IsAnswered = true
            };

            var res = service.ChangeAnsweredStatus(parameters);

            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Success, res.ResultDescription);

            Assert.IsNotNull(res.Thread);
            Assert.AreEqual(true, res.Thread.IsAnswered);

            var actualThread = NotificationDB.GetThread(NotificationDB.threadMarkAnwsered.Id);
            Assert.AreEqual(true, actualThread.IsAnswered);
        }

        #endregion

        #region GetThreadById

        [TestMethod]
        public void ShouldGetThreadByIdTest()
        {
            var serviceProvider = new ServiceProvider();
            var service = serviceProvider.GetAdminFeedbackService();

            var parameters = new AdminGetThreadByIdParameters()
            {
                UserId = NotificationDB.UserId,
                ThreadId = NotificationDB.newThread1.Id
            };

            var res = service.GetThreadById(parameters);

            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Success, res.ResultDescription);
            Assert.IsNotNull(res.Thread);
            Assert.IsNotNull(res.Thread.TopicMessage);
        }

        #endregion
        
        #region Method clients

        public class GetThreadsClient
        {
            private AdminGetThreadsResult result;
            
            public void AssertResult(int? threadsCount = null, int? totalCount = null)
            {
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Success, result.ResultDescription);

                var threads = GetThreads();

                if (threadsCount.HasValue)
                {
                    Assert.AreEqual(threadsCount, threads.Length, "threads.Length не верно");
                }

                if (totalCount.HasValue)
                {
                    Assert.AreEqual(threadsCount, result.TotalCount, "res.TotalCount не верно");
                }
            }

            public Thread[] GetThreads()
            {
                return result.Result.Select(r => r.Thread).ToArray();
            }

            public int[] GetMessageMatchIndexes(string threadClientEmail, int? indexLen = null)
            {
                var indexes =
                    result.Result.Where(
                        r => r.Thread.ClientEmail == threadClientEmail && r.MessageMatchIndexes != null).
                                     SelectMany(r => r.MessageMatchIndexes).ToArray();

                if (indexLen.HasValue)
                {
                    Assert.AreEqual(indexLen, indexes.Length, "Количество индексов сообщений не совпадает");
                }

                return indexes;
            }

            public AdminGetThreadsResult Call(AdminGetThreadsParameters parameters)
            {
                var serviceProvider = new ServiceProvider();
                var service = serviceProvider.GetAdminFeedbackService();

                result = service.GetThreads(parameters);

                serviceProvider.GetSecurityMock().Verify(m => m.CheckPermissions(It.IsAny<string>(), It.IsAny<string[]>()), Times.AtLeastOnce());

                return result;
            }
        }

        public class GetThreadMessagesClient
        {
            private GetThreadMessagesResult result;

            public GetThreadMessagesResult Call(AdminGetThreadMessagesParameters parameters)
            {
                var serviceProvider = new ServiceProvider();
                var service = serviceProvider.GetAdminFeedbackService();

                result = service.GetThreadMessages(parameters);

                serviceProvider.GetSecurityMock().Verify(m => m.CheckPermissions(It.IsAny<string>(), It.IsAny<string[]>()), Times.AtLeastOnce());

                return result;
            }

            public void AssertResult(Guid threadId, int messagesTotalCount, int messagesLen)
            {
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Success, result.ResultDescription);
                Assert.IsNotNull(result.Thread);
                Assert.AreEqual(threadId, result.Thread.Id);
                Assert.AreEqual(messagesTotalCount, result.TotalCount);

                var threadMessages = result.ThreadMessages;

                Assert.IsNotNull(threadMessages);
                Assert.AreEqual(messagesLen, threadMessages.Length);
            }

            public ThreadMessage[] GetMessages()
            {
                return result.ThreadMessages;
            }

            public Thread GetThread()
            {
                return result.Thread;
            }
        }

        public class ReplyMethodClient
        {
            private ReplyResult result;

            public ReplyResult Call(AdminReplyParameters parameters)
            {
                var serviceProvider = new ServiceProvider();
                var service = serviceProvider.GetAdminFeedbackService();

                result = service.Reply(parameters);

                serviceProvider.GetSecurityMock().Verify(m => m.CheckPermissions(It.IsAny<string>(), It.IsAny<string[]>()), Times.AtLeastOnce());

                return result;
            }

            public void AssertRes()
            {
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Success, result.ResultDescription);
            }
        }

        #endregion
    }
}