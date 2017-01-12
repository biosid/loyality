using System;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RapidSoft.Etl.Logging;
using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource;
using RapidSoft.VTB24.BankConnector.Processors;
using RapidSoft.VTB24.BankConnector.Tests.Helpers;

namespace RapidSoft.VTB24.BankConnector.Tests.ProcessorTest
{
    using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientMessageService;

    [TestClass]
    public class PersonalMessageProcessorTests : TestBase
    {
        [TestMethod]
        public void ReceiveThenSendResponseCheckBatchGetClientsByExternalIdCalled()
        {
            var sessionId = Guid.Parse("71231619-C0C1-4836-8FE1-A0A6F95944F1");
            var clientId = Guid.NewGuid().ToString();
            using (var uow = CreateUow())
            {
                uow.ClientPersonalMessageRepository.Add(new ClientPersonalMessage()
                {
                    SessionId = sessionId,
                    ClientId = clientId,
                    Message = "test"
                });
                uow.Save();
            }

            var stubClientMessageService = new Mock<IClientMessageService>();
            stubClientMessageService.Setup(t => t.Notify(It.IsAny<NotifyClientsParameters>()))
                       .Returns(new NotifyClientsResult { Threads = new Thread[]{new Thread() } });

            var mockProfile = new Mock<ClientProfileService>();
            mockProfile.Setup(t => t.BatchGetClientsByExternalId(It.IsAny<BatchGetClientsByExternalIdRequest>()))
                     .Returns(new BatchGetClientsByExternalIdResponse
                     {
                         Response = new BatchGetClientsByExternalIdResponseType()
                         {
                             StatusCode = 0,
                             ResClientsIdentifiers = new BatchGetClientsByExternalIdResponseTypeResClientIdentifier[] { }
                         }
                     });

            using (var uow = CreateUow())
            {
                var logger = new EtlLogger.EtlLogger(new Mock<IEtlLogger>().Object, "stub", sessionId.ToString());
                var proc = new PersonalMessageProcessor(logger, uow, stubClientMessageService.Object, mockProfile.Object);
                proc.Process();
            }

            mockProfile.Verify(t => t.BatchGetClientsByExternalId(It.IsAny<BatchGetClientsByExternalIdRequest>()), Times.AtLeastOnce());
        }
    }
}
