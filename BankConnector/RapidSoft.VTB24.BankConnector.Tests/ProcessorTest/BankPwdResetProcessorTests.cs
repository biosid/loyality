namespace RapidSoft.VTB24.BankConnector.Tests.ProcessorTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Etl.Logging;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Processors;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.Site.SecurityWebApi;

    [TestClass]
    public class BankPwdResetProcessorTests : TestBase
    {
        class ProcessorEnv
        {
            public Guid SessionId { get; set; }
            public IUnitOfWork Uow { get; set; }
            public Mock<ISecurityWebApi> SecurityMock { get; set; }
            public BankPwdResetProcessor Processor { get; set; }
        }

        private static ProcessorEnv MakeEnv()
        {
            var env = new ProcessorEnv
                      {
                          SessionId = Guid.NewGuid(),
                          Uow = DataSourceHelper.CreateUow(),
                          SecurityMock = new Mock<ISecurityWebApi>()
                      };
            var logger = new EtlLogger.EtlLogger(new Mock<IEtlLogger>().Object, "stub", env.SessionId.ToString());
            env.Processor = new BankPwdResetProcessor(logger, env.Uow, env.SecurityMock.Object);

            return env;
        }

        [TestMethod]
        public void ShouldMakeResponseWithSuccessStatus()
        {
            // тестовые данные
            const string TestClientId = "testClientId";
            const string TestClientPhoneNumber = "79991234567";

            // создание процессора и заглушек
            var env = MakeEnv();

            env.SecurityMock.Setup(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()))
               .Returns(
                   new Dictionary<string, User>
                   {
                       {
                           TestClientId,
                           new User
                           {
                               ClientId = TestClientId,
                               PhoneNumber = TestClientPhoneNumber
                           }
                       }
                   });

            env.SecurityMock.Setup(m => m.ResetUserPassword(It.IsAny<ResetUserPasswordOptions>()))
               .Returns(new ResetUserPasswordResult { Success = true, Status = ResetUserPasswordStatus.Changed });

            // входные данные
            var pwdResetRequest = new ClientForBankPwdReset
                                  {
                                      ClientId = TestClientId,
                                      EtlSessionId = env.SessionId,
                                      InsertedDate = DateTime.Now.AddSeconds(-5)
                                  };

            env.Uow.ClientForBankPwdResetRepository.Add(pwdResetRequest);
            env.Uow.Save();

            // вызов тестируемого функционала
            env.Processor.Execute();

            // проверка результатов
            env.SecurityMock.Verify(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()), Times.Once);
            env.SecurityMock.Verify(m => m.ResetUserPassword(It.Is<ResetUserPasswordOptions>(v => v.Login == TestClientPhoneNumber)), Times.Once);

            var responses =
                env.Uow.ClientForBankPwdResetResponseRepository.GetAll()
                   .Where(r => r.EtlSessionId == env.SessionId)
                   .ToArray();

            Assert.IsTrue(responses.Length == 1);

            var response = responses[0];

            Assert.IsTrue(response.ClientId == TestClientId);
            Assert.IsTrue(response.Status == (int)ClientForBankPwdResetResponseStatus.Success);
        }

        [TestMethod]
        public void ShouldMakeResponseWithErrorStatusIfLoginResolutionIsFailed()
        {
            // тестовые данные
            const string TestClientId = "testClientId";

            // создание процессора и заглушек
            var env = MakeEnv();

            env.SecurityMock.Setup(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()))
               .Returns((Dictionary<string, User>)null);

            // входные данные
            var pwdResetRequest = new ClientForBankPwdReset
            {
                ClientId = TestClientId,
                EtlSessionId = env.SessionId,
                InsertedDate = DateTime.Now.AddSeconds(-5)
            };

            env.Uow.ClientForBankPwdResetRepository.Add(pwdResetRequest);
            env.Uow.Save();

            // вызов тестируемого функционала
            env.Processor.Execute();

            // проверка результатов
            env.SecurityMock.Verify(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()), Times.Once);
            env.SecurityMock.Verify(m => m.ResetUserPassword(It.IsAny<ResetUserPasswordOptions>()), Times.Never);

            var responses =
                env.Uow.ClientForBankPwdResetResponseRepository.GetAll()
                   .Where(r => r.EtlSessionId == env.SessionId)
                   .ToArray();

            Assert.IsTrue(responses.Length == 1);

            var response = responses[0];

            Assert.IsTrue(response.ClientId == TestClientId);
            Assert.IsTrue(response.Status == (int)ClientForBankPwdResetResponseStatus.Error);
        }

        [TestMethod]
        public void ShouldMakeResponseWithErrorStatusIfLoginResolutionThrows()
        {
            // тестовые данные
            const string TestClientId = "testClientId";

            // создание процессора и заглушек
            var env = MakeEnv();

            env.SecurityMock.Setup(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>())).Throws<Exception>();

            // входные данные
            var pwdResetRequest = new ClientForBankPwdReset
            {
                ClientId = TestClientId,
                EtlSessionId = env.SessionId,
                InsertedDate = DateTime.Now.AddSeconds(-5)
            };

            env.Uow.ClientForBankPwdResetRepository.Add(pwdResetRequest);
            env.Uow.Save();

            // вызов тестируемого функционала
            env.Processor.Execute();

            // проверка результатов
            env.SecurityMock.Verify(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()), Times.Once);
            env.SecurityMock.Verify(m => m.ResetUserPassword(It.IsAny<ResetUserPasswordOptions>()), Times.Never);

            var responses =
                env.Uow.ClientForBankPwdResetResponseRepository.GetAll()
                   .Where(r => r.EtlSessionId == env.SessionId)
                   .ToArray();

            Assert.IsTrue(responses.Length == 1);

            var response = responses[0];

            Assert.IsTrue(response.ClientId == TestClientId);
            Assert.IsTrue(response.Status == (int)ClientForBankPwdResetResponseStatus.Error);
        }

        [TestMethod]
        public void ShouldMakeResponseWithErrorStatusIfLoginIsNotResolved()
        {
            // тестовые данные
            const string TestClientId = "testClientId";

            // создание процессора и заглушек
            var env = MakeEnv();

            env.SecurityMock.Setup(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()))
               .Returns(new Dictionary<string, User>());

            // входные данные
            var pwdResetRequest = new ClientForBankPwdReset
            {
                ClientId = TestClientId,
                EtlSessionId = env.SessionId,
                InsertedDate = DateTime.Now.AddSeconds(-5)
            };

            env.Uow.ClientForBankPwdResetRepository.Add(pwdResetRequest);
            env.Uow.Save();

            // вызов тестируемого функционала
            env.Processor.Execute();

            // проверка результатов
            env.SecurityMock.Verify(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()), Times.Once);
            env.SecurityMock.Verify(m => m.ResetUserPassword(It.IsAny<ResetUserPasswordOptions>()), Times.Never);

            var responses =
                env.Uow.ClientForBankPwdResetResponseRepository.GetAll()
                   .Where(r => r.EtlSessionId == env.SessionId)
                   .ToArray();

            Assert.IsTrue(responses.Length == 1);

            var response = responses[0];

            Assert.IsTrue(response.ClientId == TestClientId);
            Assert.IsTrue(response.Status == (int)ClientForBankPwdResetResponseStatus.Error);
        }

        [TestMethod]
        public void ShouldMakeResponseWithErrorStatusIfResetPasswordIsFailed()
        {
            // тестовые данные
            const string TestClientId = "testClientId";
            const string TestClientPhoneNumber = "79991234567";

            // создание процессора и заглушек
            var env = MakeEnv();

            env.SecurityMock.Setup(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()))
               .Returns(
                   new Dictionary<string, User>
                   {
                       {
                           TestClientId,
                           new User
                           {
                               ClientId = TestClientId,
                               PhoneNumber = TestClientPhoneNumber
                           }
                       }
                   });

            env.SecurityMock.Setup(m => m.ResetUserPassword(It.IsAny<ResetUserPasswordOptions>()))
               .Returns(new ResetUserPasswordResult { Success = false, Status = ResetUserPasswordStatus.FailedToSendNotification });

            // входные данные
            var pwdResetRequest = new ClientForBankPwdReset
            {
                ClientId = TestClientId,
                EtlSessionId = env.SessionId,
                InsertedDate = DateTime.Now.AddSeconds(-5)
            };

            env.Uow.ClientForBankPwdResetRepository.Add(pwdResetRequest);
            env.Uow.Save();

            // вызов тестируемого функционала
            env.Processor.Execute();

            // проверка результатов
            env.SecurityMock.Verify(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()), Times.Once);
            env.SecurityMock.Verify(m => m.ResetUserPassword(It.Is<ResetUserPasswordOptions>(v => v.Login == TestClientPhoneNumber)), Times.Once);

            var responses =
                env.Uow.ClientForBankPwdResetResponseRepository.GetAll()
                   .Where(r => r.EtlSessionId == env.SessionId)
                   .ToArray();

            Assert.IsTrue(responses.Length == 1);

            var response = responses[0];

            Assert.IsTrue(response.ClientId == TestClientId);
            Assert.IsTrue(response.Status == (int)ClientForBankPwdResetResponseStatus.Error);
        }

        [TestMethod]
        public void ShouldMakeResponseWithErrorStatusIfResetPasswordThrows()
        {
            // тестовые данные
            const string TestClientId = "testClientId";
            const string TestClientPhoneNumber = "79991234567";

            // создание процессора и заглушек
            var env = MakeEnv();

            env.SecurityMock.Setup(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()))
               .Returns(
                   new Dictionary<string, User>
                   {
                       {
                           TestClientId,
                           new User
                           {
                               ClientId = TestClientId,
                               PhoneNumber = TestClientPhoneNumber
                           }
                       }
                   });

            env.SecurityMock.Setup(m => m.ResetUserPassword(It.IsAny<ResetUserPasswordOptions>())).Throws<Exception>();

            // входные данные
            var pwdResetRequest = new ClientForBankPwdReset
            {
                ClientId = TestClientId,
                EtlSessionId = env.SessionId,
                InsertedDate = DateTime.Now.AddSeconds(-5)
            };

            env.Uow.ClientForBankPwdResetRepository.Add(pwdResetRequest);
            env.Uow.Save();

            // вызов тестируемого функционала
            env.Processor.Execute();

            // проверка результатов
            env.SecurityMock.Verify(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()), Times.Once);
            env.SecurityMock.Verify(m => m.ResetUserPassword(It.Is<ResetUserPasswordOptions>(v => v.Login == TestClientPhoneNumber)), Times.Once);

            var responses =
                env.Uow.ClientForBankPwdResetResponseRepository.GetAll()
                   .Where(r => r.EtlSessionId == env.SessionId)
                   .ToArray();

            Assert.IsTrue(responses.Length == 1);

            var response = responses[0];

            Assert.IsTrue(response.ClientId == TestClientId);
            Assert.IsTrue(response.Status == (int)ClientForBankPwdResetResponseStatus.Error);
        }

        [TestMethod]
        public void ShouldMakeResponses()
        {
            // тестовые данные
            const string TestClientIdPrefix = "testClient";
            const string TestClientPhoneNumberPrefix = "7999123456";

            var batchResolveUsersByClientIdResult = Enumerable.Range(0, 10)
                                                              .ToDictionary(
                                                                  i => TestClientIdPrefix + i,
                                                                  i =>
                                                                  new User
                                                                  {
                                                                      ClientId = TestClientIdPrefix + i,
                                                                      PhoneNumber =
                                                                          TestClientPhoneNumberPrefix + i
                                                                  });
            var testClientIds = batchResolveUsersByClientIdResult.Keys.ToArray();

            var testClientIdsToExclude = new[] { testClientIds[1], testClientIds[2], testClientIds[3] };
            var testClientIdsFailingReset = new[] { testClientIds[5], testClientIds[6] };
            var testClientIdThrowingOnReset = testClientIds[7];

            var testClientIdsToSucceed =
                testClientIds.Except(testClientIdsToExclude)
                             .Except(testClientIdsFailingReset)
                             .Except(new[] { testClientIdThrowingOnReset })
                             .ToArray();

            foreach (var id in testClientIdsToExclude)
            {
                batchResolveUsersByClientIdResult[id] = null;
            }

            var testLoginsFailingReset =
                testClientIdsFailingReset.Select(id => batchResolveUsersByClientIdResult[id].PhoneNumber).ToArray();

            var testLoginThrowingOnReset = batchResolveUsersByClientIdResult[testClientIdThrowingOnReset].PhoneNumber;

            // создание процессора и заглушек
            var env = MakeEnv();

            env.SecurityMock.Setup(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()))
               .Returns(batchResolveUsersByClientIdResult);

            env.SecurityMock.Setup(m => m.ResetUserPassword(It.IsAny<ResetUserPasswordOptions>()))
               .Returns(new ResetUserPasswordResult { Success = true, Status = ResetUserPasswordStatus.Changed });

            env.SecurityMock.Setup(m => m.ResetUserPassword(It.Is<ResetUserPasswordOptions>(x => testLoginsFailingReset.Contains(x.Login))))
               .Returns(new ResetUserPasswordResult { Success = false, Status = ResetUserPasswordStatus.FailedToSendNotification });

            env.SecurityMock.Setup(
                m => m.ResetUserPassword(It.Is<ResetUserPasswordOptions>(x => x.Login == testLoginThrowingOnReset)))
               .Throws<Exception>();

            // входные данные
            foreach (var testClientId in testClientIds)
            {
                var pwdResetRequest = new ClientForBankPwdReset
                {
                    ClientId = testClientId,
                    EtlSessionId = env.SessionId,
                    InsertedDate = DateTime.Now.AddSeconds(-5)
                };
                env.Uow.ClientForBankPwdResetRepository.Add(pwdResetRequest);
            }
            env.Uow.Save();

            // вызов тестируемого функционала
            env.Processor.Execute();

            // проверка результатов
            env.SecurityMock.Verify(m => m.BatchResolveUsersByClientId(It.IsAny<string[]>()), Times.Once);
            env.SecurityMock.Verify(m => m.ResetUserPassword(It.IsAny<ResetUserPasswordOptions>()), Times.Exactly(testClientIds.Length - testClientIdsToExclude.Length));

            var responses =
                env.Uow.ClientForBankPwdResetResponseRepository.GetAll()
                   .Where(r => r.EtlSessionId == env.SessionId)
                   .ToArray();

            Assert.IsTrue(responses.Length == 10);

            Assert.IsTrue(
                responses.Where(r => r.Status == (int)ClientForBankPwdResetResponseStatus.Success)
                         .Select(r => r.ClientId)
                         .OrderBy(id => id)
                         .SequenceEqual(testClientIdsToSucceed.OrderBy(id => id)));
        }
    }
}
