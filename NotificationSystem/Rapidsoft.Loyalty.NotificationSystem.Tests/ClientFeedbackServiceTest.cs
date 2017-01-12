using Rapidsoft.Loyalty.NotificationSystem.DataAccess.Repositories;

namespace Rapidsoft.Loyalty.NotificationSystem.Tests.Services
{
    using System;

    using API.Entities;
    using API.InputParameters;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NotificationSystem.Services;

    [TestClass]
    public class ClientFeedbackServiceTest
    {
        [TestMethod]
        public void ShouldCreateNewThreadByClientTest()
        {
            var expMessagebody = "test MessageBody";
            var expClientid = "test ClientId";
            var expFullName = "test FullName";

            var parameters = new SendFeedbackParameters()
            {
                MessageTitle = "test Title",
                ClientFullName = expFullName,
                ClientId = expClientid, 
                MessageBody = expMessagebody, 
                Attachments = new[]
                {
                    new MessageAttachment()
                    {
                        FileName = "testFile.txt",
                        FileSize = 20000000  
                    },
                }
            };
            
            var result = GetClientFeedbackService().Send(parameters);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success, result.ResultDescription);
            
            var thread = result.Thread;
            Assert.IsNotNull(thread);
            
            var topicMessage = thread.TopicMessage;
            Assert.IsNotNull(topicMessage);

            var actualThread = new ThreadsRepository().GetThread(thread.Id, false);

            Assert.IsNotNull(actualThread);
            Assert.AreEqual(thread.Id, actualThread.Id);
            Assert.AreEqual(ThreadTypes.Suggestion, actualThread.Type);
            Assert.AreEqual(thread.Type, actualThread.Type);

            Assert.IsTrue(actualThread.Title.Contains("test Title"));
            Assert.AreEqual(thread.Title, actualThread.Title);
            
            Assert.AreEqual(ThreadClientTypes.Client, actualThread.ClientType);
            Assert.AreEqual(thread.ClientType, actualThread.ClientType);
            Assert.AreEqual(false, actualThread.IsClosed);
            Assert.AreEqual(thread.IsClosed, actualThread.IsClosed);
            Assert.AreEqual(false, actualThread.IsAnswered);

            Assert.IsNull(actualThread.ShowSince);
            Assert.IsNull(actualThread.ShowUntil);            

            Assert.AreEqual(expClientid, actualThread.ClientId);
            Assert.AreEqual(thread.ClientId, actualThread.ClientId);

            Assert.AreEqual(expFullName, actualThread.ClientFullName);
            Assert.AreEqual(thread.ClientFullName, actualThread.ClientFullName);

            Assert.AreEqual(null, actualThread.ClientEmail);
            Assert.AreEqual(thread.ClientEmail, actualThread.ClientEmail);

            Assert.AreEqual(1, actualThread.MessagesCount);
            Assert.AreEqual(thread.MessagesCount, actualThread.MessagesCount);

            Assert.AreEqual(1, actualThread.UnreadMessagesCount);
            Assert.AreEqual(thread.MessagesCount, actualThread.UnreadMessagesCount);

            Assert.AreEqual(MessageTypes.ClientMessage, actualThread.FirstMessageType);
            Assert.AreEqual(thread.FirstMessageType, actualThread.FirstMessageType);

            Assert.AreEqual(MessageTypes.ClientMessage, actualThread.LastMessageType);
            Assert.AreEqual(thread.FirstMessageType, actualThread.LastMessageType);

            var actualTopMessage = new ThreadMessagesRepository().Get(topicMessage.Id);

            Assert.IsNotNull(actualTopMessage);
            Assert.AreEqual(topicMessage.Id, actualTopMessage.Id);            
        }

        [TestMethod]
        public void ShouldCreateNewThreadByGuestTest()
        {
            var expMessagebody = "сообщение гостя";
            var expFullName = "Гость Иванович";
            var expEmail = "гость@гость.гость";

            var parameters = new SendFeedbackParameters()
            {
                MessageTitle = "test Title",
                ClientFullName = expFullName,
                ClientEmail = expEmail,
                ClientId = null,
                MessageBody = expMessagebody,
            };

            var result = GetClientFeedbackService().Send(parameters);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success, result.ResultDescription);
        }

        private static ClientFeedbackService GetClientFeedbackService()
        {
            return new ClientFeedbackService();
        }
    }
}