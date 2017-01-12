namespace RapidSoft.VTB24.BankConnector.Tests.ServiceTests
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.Etl.Runtime.Agents;
    using RapidSoft.Etl.Runtime.Agents.Sql;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    [TestClass]
    public class ActivateClientsServiceTests : TestBase
    {
        private const string etlPackageId = "E23D8AF7-5916-4907-9D99-D69ED8E7D542";

        private const string testClientId = "test_client_id";

        private static readonly Guid testSessionId = new Guid("B1B96FC1-E344-4BD9-B10D-F5342C2F47D5");

        private static readonly Guid testSessionId2 = new Guid("8353F751-1519-4B3E-8714-8499A587A824");

        private static EtlContext etlContextStub;

        private static EtlContext etlContextStubSecondSession;

        private static EtlSession etlSessionStub;

        private static EtlSession etlSecondSessionStub;

        private static IEtlLogger etlLoggerStub;

        [ClassInitialize]
        public static void InitializeContext(TestContext testContext)
        {
            var etlAgentInfo = new EtlAgentInfo();
            etlAgentInfo.SchemaName = "etl";
            etlAgentInfo.ConnectionString = ConfigHelper.ConnectionString;
            var sqlAgent = new SqlEtlAgent(etlAgentInfo);

            sqlAgent.GetEtlPackage(etlPackageId);

            var loggerMock = new Mock<IEtlLogger>();

            // loggerMock.Setup(x => x.LogEtlCounter(It.IsAny<EtlCounter>()));
            loggerMock.Setup(x => x.LogEtlMessage(It.IsAny<EtlMessage>())).Callback<EtlMessage>(x => Console.WriteLine("MessageType: {0} / Text: {1}", x.MessageType, x.Text));

            // loggerMock.Setup(x => x.LogEtlSessionEnd(It.IsAny<EtlSession>()));
            // loggerMock.Setup(x => x.LogEtlSessionStart(It.IsAny<EtlSession>()));
            // loggerMock.Setup(x => x.LogEtlVariable(It.IsAny<EtlVariable>()));
            etlLoggerStub = loggerMock.Object;

            etlSessionStub = new EtlSession
                             {
                                 EtlPackageId = etlPackageId, 
                                 EtlSessionId = testSessionId.ToString(), 
                                 StartDateTime = DateTime.Now, 
                                 StartUtcDateTime = DateTime.UtcNow
                             };
            etlSecondSessionStub = new EtlSession
                                   {
                                       EtlPackageId = etlPackageId, 
                                       EtlSessionId = testSessionId2.ToString(), 
                                       StartDateTime = DateTime.Now, 
                                       StartUtcDateTime = DateTime.UtcNow
                                   };

            etlContextStub = new EtlContext(etlSessionStub);

            etlContextStubSecondSession = new EtlContext(etlSecondSessionStub);
        }

        [TestInitialize]
        public void RefreshTestData()
        {
            using (var uow = CreateUow())
            {
                var repository = uow.ClientForActivationRepository;
                repository.DeleteBySessionId(testSessionId);
                repository.DeleteBySessionId(testSessionId2);
                repository.DeleteByClientId(testClientId);
                uow.Save();
            }
        }

        [TestMethod]
        public void ShouldActivateClients()
        {
            var client = new ClientForActivation
                         {
                             BirthDate = DateTime.Now.AddYears(-25), 
                             ClientId = testClientId, 
                             EtlSessionId = testSessionId, 
                             FirstName = Path.GetRandomFileName(), 
                             LastName = Path.GetRandomFileName(), 
                             Gender = 0, 
                             MiddleName = Path.GetRandomFileName(), 
                             Segment = (int)ClientSegment.VIP
                         };

            ClientForActivation recordFromBase = null;

            using (var uow = CreateUow())
            {
                uow.ClientForActivationRepository.Add(client);
                uow.Save();
            }

            EtlInvokeHelper.ActivateClients(etlContextStub, etlLoggerStub);

            using (var uow = CreateUow())
            {
                recordFromBase = uow.ClientForActivationRepository.GetById(x => x.ClientId == testClientId && x.EtlSessionId == testSessionId);
            }

            Assert.IsNotNull(recordFromBase);
            Assert.AreEqual(ActivateClientStatus.Success, (ActivateClientStatus)recordFromBase.Status);
        }

        [TestMethod]
        public void ShouldSetAlreadyActivatedStatus()
        {
            var client = new ClientForActivation
                         {
                             BirthDate = DateTime.Now.AddYears(-25), 
                             ClientId = testClientId, 
                             EtlSessionId = testSessionId, 
                             FirstName = Path.GetRandomFileName(), 
                             LastName = Path.GetRandomFileName(), 
                             Gender = 0, 
                             MiddleName = Path.GetRandomFileName(), 
                             Segment = (int)ClientSegment.VIP
                         };
            ClientForActivation recordFromBase;

            using (var uow = CreateUow())
            {
                uow.ClientForActivationRepository.Add(client);
                uow.Save();
            }

            EtlInvokeHelper.ActivateClients(etlContextStub, etlLoggerStub);
            client = new ClientForActivation
                     {
                         BirthDate = DateTime.Now.AddYears(-25), 
                         ClientId = testClientId, 
                         EtlSessionId = testSessionId2, 
                         FirstName = Path.GetRandomFileName(), 
                         LastName = Path.GetRandomFileName(), 
                         Gender = 0, 
                         MiddleName = Path.GetRandomFileName(), 
                         Segment = (int)ClientSegment.VIP
                     };

            using (var uow = CreateUow())
            {
                uow.ClientForActivationRepository.Add(client);
                uow.Save();
            }

            EtlInvokeHelper.ActivateClients(etlContextStubSecondSession, etlLoggerStub);

            using (var uow = CreateUow())
            {
                recordFromBase = uow.ClientForActivationRepository.GetById(x => x.ClientId == testClientId && x.EtlSessionId == testSessionId2);
            }

            Assert.IsNotNull(recordFromBase);
            Assert.AreEqual(ActivateClientStatus.AlreadyActivated, (ActivateClientStatus)recordFromBase.Status);
        }

        [TestMethod]
        public void ShouldMarkClientForActivationAsClientForDeletion()
        {
            var deletionEtlSessionId = Guid.NewGuid();
            var deletionClientId = Guid.NewGuid().ToString();
            var clientForDeletion = new ClientForDeletion
                                    {
                                        InsertEtlSessionId = deletionEtlSessionId, 
                                        ExternalClientId = deletionClientId, 
                                        InsertedDate = DateTime.Now, 
                                        MobilePhone = "79279279279", 
                                        SendEtlSessionId = null, 
                                        Status = null
                                    };
            using (var uow = CreateUow())
            {
                uow.ClientForDeletionRepository.Add(clientForDeletion);
                uow.Save();
            }

            var clientForActivation = new ClientForActivation
                                      {
                                          BirthDate = DateTime.Now.AddYears(-25), 
                                          ClientId = deletionClientId, 
                                          EtlSessionId = testSessionId, 
                                          FirstName = Path.GetRandomFileName(), 
                                          LastName = Path.GetRandomFileName(), 
                                          Gender = 0, 
                                          MiddleName = Path.GetRandomFileName(), 
                                          Segment = (int)ClientSegment.VIP
                                      };
            using (var uow = CreateUow())
            {
                uow.ClientForActivationRepository.Add(clientForActivation);
                uow.Save();
            }

            EtlInvokeHelper.ActivateClients(etlContextStub, etlLoggerStub);

            using (var uow = CreateUow())
            {
                var fromDB = uow.ClientForActivationRepository.GetAll().Single(x => x.ClientId == deletionClientId);

                Assert.IsNotNull(fromDB);
                Assert.AreEqual(deletionEtlSessionId, fromDB.DeletionEtlSessionId);

                // NOTE: VTBPLK-1743: В ответе выставлять Status = 4 для клиентов, которые были не активированы по причине, что они находятся на удалении.
                Assert.AreEqual(4, fromDB.Status);

                uow.ClientForDeletionRepository.Delete(x => x.ExternalClientId == deletionClientId);
                uow.ClientForActivationRepository.Delete(x => x.ClientId == deletionClientId);
                uow.Save();
            }
        }

        [TestMethod]
        public void ShouldSetWrongClientIdStatus()
        {
            const string wrongClientId = "wrong_client_id";

            var client = new ClientForActivation
                         {
                             BirthDate = DateTime.Now.AddYears(-25), 
                             ClientId = wrongClientId, 
                             EtlSessionId = testSessionId, 
                             FirstName = Path.GetRandomFileName(), 
                             LastName = Path.GetRandomFileName(), 
                             Gender = 0, 
                             MiddleName = Path.GetRandomFileName(), 
                             Segment = (int)ClientSegment.VIP
                         };

            ClientForActivation recordFromBase;

            using (var uow = CreateUow())
            {
                uow.ClientForActivationRepository.Add(client);
                uow.Save();
            }

            EtlInvokeHelper.ActivateClients(etlContextStub, etlLoggerStub);

            using (var uow = CreateUow())
            {
                recordFromBase = uow.ClientForActivationRepository.GetById(x => x.ClientId == wrongClientId && x.EtlSessionId == testSessionId);
            }

            Assert.IsNotNull(recordFromBase);
            Assert.AreEqual(ActivateClientStatus.WrongClientId, (ActivateClientStatus)recordFromBase.Status);
        }
    }
}