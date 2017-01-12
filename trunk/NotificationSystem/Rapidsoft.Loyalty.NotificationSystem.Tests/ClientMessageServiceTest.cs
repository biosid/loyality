namespace Rapidsoft.Loyalty.NotificationSystem.Tests.Services
{
    using System.Linq;

    using API;
    using API.InputParameters;
    using API.OutputResults;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Vtb24.Common.Configuration;
    using Moq;
    using Rapidsoft.Loyalty.NotificationSystem.Services;
    using Rapidsoft.Loyalty.NotificationSystem.Email;
    using System.Net.Mail;

    [TestClass]
    public class ClientMessageServiceTest
    {
        [TestMethod]
        public void ShouldGetThreadsTest()
        {
            var parameters = new ClientGetThreadsParameters()
            {
                ClientId = NotificationDB.ThreadsClientId,
                CountToSkip = 0,
                CountToTake = 100
            };

            var res = GetService().GetThreads(parameters);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultDescription);

            var threads = res.Threads;

            Assert.AreEqual(3, threads.Length);
            Assert.AreEqual("t3ClientEmail", threads[0].ClientEmail);
            Assert.AreEqual("t2ClientEmail", threads[1].ClientEmail);
            Assert.AreEqual("clientEmail", threads[2].ClientEmail);
        }
        
        [TestMethod]
        public void ShouldGetThreadsWithSinceUntilTest()
        {
            var parameters = new ClientGetThreadsParameters()
            {
                ClientId = NotificationDB.ThreadNextMonthClientId,
                CountToSkip = 0,
                CountToTake = 100
            };

            var res = GetService().GetThreads(parameters);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultDescription);

            var threads = res.Threads;

            var notShowedThread = threads.SingleOrDefault(t => t.Id == NotificationDB.ThreadSinceNextMonth.Id);

            Assert.IsNull(notShowedThread, "Ветка не должна быть найдена. Так как дата показа со следующего месяца");
        }
        
        [TestMethod]
        public void ShouldGetStatisticsTest()
        {
            var parameters = new GetStatisticsParameters()
            {
                ClientId = NotificationDB.GetThreadsStatClientId
            };

            var res = GetService().GetStatistics(parameters);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultDescription);

            Assert.AreEqual(2, res.ThreadsCount);
            Assert.AreEqual(1, res.UnreadThreadsCount);
        }

        [TestMethod]
        public void ShouldGetThreadByIdTest()
        {
            var parameters = new ClientGetThreadByIdParameters()
            {
                ClientId = NotificationDB.ThreadsClientId,
                ThreadId = NotificationDB.newThread3.Id
            };

            var res = GetService().GetThreadById(parameters);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultDescription);

            Assert.IsNotNull(res.Thread);
            Assert.IsNotNull(res.Thread.TopicMessage);
            Assert.AreEqual(NotificationDB.newThread3.Id, res.Thread.Id);
        }

        [TestMethod]
        public void ShouldNotGetThreadByIdShowUntilTest()
        {
            var parameters = new ClientGetThreadByIdParameters()
            {
                ClientId = NotificationDB.ThreadNextMonthClientId,
                ThreadId = NotificationDB.ThreadSinceNextMonth.Id
            };

            var res = GetService().GetThreadById(parameters);

            Assert.IsNotNull(res);
            Assert.IsFalse(res.Success);
            Assert.AreEqual(ResultCodes.THREAD_SINCE_UNTIL_MISMATCH, res.ResultCode);            
        }
        
        [TestMethod]
        public void ShouldGetThreadByIdThrowSecurityErrorTest()
        {
            var parameters = new ClientGetThreadByIdParameters()
            {
                ThreadId = NotificationDB.newThread3.Id
            };

            var res = GetService().GetThreadById(parameters);

            Assert.IsNotNull(res);
            Assert.IsFalse(res.Success);

            Assert.IsNull(res.Thread);
        }

        [TestMethod]
        public void ShouldGetThreadMessagesTest()
        {
            var parameters = new ClientGetThreadMessagesParameters()
            {
                ClientId = NotificationDB.ThreadsClientId,
                ThreadId = NotificationDB.newThread2.Id,
                CountToSkip = 1,
                CountToTake = 2
            };

            var res = GetService().GetThreadMessages(parameters);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultDescription);

            var thread = res.Thread;
            
            Assert.IsNotNull(thread);
            Assert.IsNotNull(thread.TopicMessage);

            var messages = res.ThreadMessages;

            Assert.AreEqual(1, messages[0].Index);
            Assert.AreEqual(2, messages[1].Index);
        }

        [TestMethod]
        public void ShouldGetThreadMessagesThrowSecurityErrorTest()
        {
            var parameters = new ClientGetThreadMessagesParameters()
            {
                ThreadId = NotificationDB.newThread2.Id
            };

            var res = GetService().GetThreadMessages(parameters);

            Assert.IsNotNull(res);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public void ShouldReplyTest()
        {
            var clientFio = "clientFio";

            var messageBody = "всё равно не работает";

            var clientEmail = "ClientEmail";

            var parameters = new ClientReplyParameters()
            {
                ThreadId = NotificationDB.threadStat1.Id,
                ClientId = NotificationDB.GetThreadsStatClientId,
                MessageBody = messageBody,
                ClientFullName = clientFio,
                ClientEmail = clientEmail
            };

            var res = GetService().Reply(parameters);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultDescription);

            var actualThreadMessages = NotificationDB.GetThreadMessages(NotificationDB.threadStat1.Id);

            Assert.AreEqual(3, actualThreadMessages.Length);

            var replyMessage = actualThreadMessages.Last();

            Assert.AreEqual(2, replyMessage.Index);
            Assert.AreEqual(NotificationDB.GetThreadsStatClientId, replyMessage.AuthorId);
            Assert.AreEqual(clientEmail, replyMessage.AuthorEmail);
            Assert.AreEqual(clientFio, replyMessage.AuthorFullName);
            Assert.AreEqual(messageBody, replyMessage.MessageBody);
            Assert.AreEqual(false, replyMessage.IsUnread);

            var actualThread = NotificationDB.GetThread(NotificationDB.threadStat1.Id);
            Assert.AreEqual(false, actualThread.IsAnswered);
            Assert.AreEqual(3, actualThread.MessagesCount);
            Assert.AreEqual(2, actualThread.UnreadMessagesCount);
            Assert.AreEqual(replyMessage.InsertedDate, actualThread.LastMessageTime);
            Assert.AreEqual(clientFio, actualThread.LastMessageBy);
        }

        [TestMethod]
        public void ShouldMarkThreadAsReadTest()
        {
            var parameters = new MarkThreadAsReadParameters()
            {
                ClientId = NotificationDB.MarkThreadAsReadClientId,
                ThreadId = NotificationDB.threadMarkRead.Id
            };

            var res = GetService().MarkThreadAsRead(parameters);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultDescription);

            var actualThread = NotificationDB.GetThread(NotificationDB.threadMarkRead.Id);

            Assert.AreEqual(0, actualThread.UnreadMessagesCount);
            Assert.AreEqual(1, actualThread.MessagesCount);

            var threadMessages = NotificationDB.GetThreadMessages(NotificationDB.threadMarkRead.Id);

            Assert.AreEqual(1, threadMessages.Length);
            Assert.AreEqual(false, threadMessages[0].IsUnread);
        }

        [TestMethod]
        public void ShouldNotifyTest()
        {
            var parameters = new NotifyClientsParameters()
            {
                Notifications = new []
                {
                    new Notification()
                    {
                        ClientId = NotificationDB.ThreadsClientId,
                        Text = "testTxt",
                        Title = "testTitle"
                    }
                }
            };
            
            var res = GetService().Notify(parameters);

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void ShouldSendMessageNotification()
        {
            var oldGen1070EnableEmailMessageNotification =
                FeaturesConfiguration.Instance.Gen1070EnableEmailMessageNotification;
            FeaturesConfiguration.Instance.Gen1070EnableEmailMessageNotification = true;

            var parameters = new NotifyClientsParameters()
            {
                Notifications = new[]
                {
                    new Notification()
                    {
                        ClientId = NotificationDB.ThreadsClientId,
                        Text = "testTxt",
                        Title = "testTitle"
                    }
                }
            };

            var profileMock = new Mock<IProfile>();
            profileMock.Setup(m => m.GetProfileEmail(It.IsAny<string>())).Returns((string e) => "akosinskiy@rapidsoft.ru");

            var senderMock = new Mock<ISender>();

            var messageNotificationService = new ServiceProvider().GetMessageNotificationService(profile: profileMock.Object, sender: senderMock.Object);
            var clientMessageService = new ServiceProvider().GetClientMessageService(messageNotificationService: messageNotificationService);

            messageNotificationService.RemoveFromBlackList("akosinskiy@rapidsoft.ru");

            var res = clientMessageService.Notify(parameters);

            Assert.IsNotNull(res);
            senderMock.Verify(s => s.SendEmail(It.IsAny<MailMessage>()), Times.AtLeastOnce());

            FeaturesConfiguration.Instance.Gen1070EnableEmailMessageNotification =
                oldGen1070EnableEmailMessageNotification;
        }

        [TestMethod]
        public void ShouldNotSendMessageNotification()
        {
            var oldGen1070EnableEmailMessageNotification =
                FeaturesConfiguration.Instance.Gen1070EnableEmailMessageNotification;
            FeaturesConfiguration.Instance.Gen1070EnableEmailMessageNotification = true;

            var parameters = new NotifyClientsParameters()
            {
                Notifications = new[]
                {
                    new Notification()
                    {
                        ClientId = NotificationDB.ThreadsClientId,
                        Text = "testTxt",
                        Title = "testTitle"
                    }
                }
            };

            var profileMock = new Mock<IProfile>();
            profileMock.Setup(m => m.GetProfileEmail(It.IsAny<string>())).Returns((string e) => "invalid.email@@qqq.qq");

            var senderMock = new Mock<ISender>();

            var messageNotificationService = new ServiceProvider().GetMessageNotificationService(profile: profileMock.Object, sender: senderMock.Object);
            var clientMessageService = new ServiceProvider().GetClientMessageService(messageNotificationService: messageNotificationService);

            var res = clientMessageService.Notify(parameters);

            Assert.IsNotNull(res);
            senderMock.Verify(s => s.SendEmail(It.IsAny<MailMessage>()), Times.Never);

            profileMock.Setup(m => m.GetProfileEmail(It.IsAny<string>())).Returns((string e) => "akosinskiy@rapidsoft.ru");

            messageNotificationService.AddToBlackList("akosinskiy@rapidsoft.ru");

            res = clientMessageService.Notify(parameters);

            Assert.IsNotNull(res);
            senderMock.Verify(s => s.SendEmail(It.IsAny<MailMessage>()), Times.Never);

            FeaturesConfiguration.Instance.Gen1070EnableEmailMessageNotification =
                oldGen1070EnableEmailMessageNotification;
        }

        private static IClientMessageService GetService()
        {
            return new ServiceProvider().GetClientMessageService();
        }
    }
}