using RapidSoft.VTB24.BankConnector.Processors;
using RapidSoft.VTB24.BankConnector.Tests.Helpers;
using RapidSoft.VTB24.BankConnector.Tests.StubServices;
using RapidSoft.VTB24.Site.SecurityWebApi;

namespace RapidSoft.VTB24.BankConnector.Tests.ServiceTests
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.API.Exceptions;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.Service;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.DataSource;

    [TestClass]
    public class RegisterClientsServiceTests : TestBase
    {
        private const string TestEmail = "test@test.test";

        private const string TestFirstName = "FirstName";

        private const string TestLastName = "LastName";

        private const string TestMiddleName = "MiddleName";

        private const string TestMobilePhone = "79271234567";
        private const string NewMobilePhone = "79271234568";

        private const string TestMobilePhoneDeleted = "79277654321";

        private readonly DateTime TestBirthDate = new DateTime(1986, 10, 23);

		private const string TestClientId = "detached_client_id";

        [TestInitialize]
        public void RefreshTestData()
        {
            using (var uow = CreateUow())
            {
                var repository = uow.ClientForRegistrationRepository;
                var delete =
                    repository.GetAll()
                              .Where(
                                  x =>
                                  x.MobilePhone.Contains(TestMobilePhone)
                                  || x.MobilePhone.Contains(TestMobilePhoneDeleted) || x.ClientId == TestClientId)
                              .ToList();
                delete.ForEach(repository.Delete);

				var repositoryBankRegistration = uow.ClientForBankRegistrationRepository;
				var deleteBankRegistration = repositoryBankRegistration.GetAll().Where(x => x.MobilePhone.Contains(TestMobilePhone.Substring(1))).ToList();
				deleteBankRegistration.ForEach(repositoryBankRegistration.Delete);

				var repositoryBankRegistrationResponse = uow.ClientForBankRegistrationResponseRepository;
				var deleteBankRegistrationResponse = repositoryBankRegistrationResponse.GetAll().Where(x => x.Login.Contains(TestMobilePhone)).ToList();
				deleteBankRegistrationResponse.ForEach(repositoryBankRegistrationResponse.Delete);
				
				uow.Save();
            }

            SecurityWebApiStub.CleanUpService();
        }

        [TestMethod]
        public void ShouldReturnCorrectResponse()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var clientService = container.Resolve<IClientManagementService>();
                var result =
                    clientService.RegisterNewClient(
                        new RegisterClientRequest
                        {
                            Email = TestEmail,
                            FirstName = TestFirstName,
                            LastName = TestLastName,
                            MiddleName = TestMiddleName,
                            MobilePhone = NewMobilePhone,
                            BirthDate = TestBirthDate
                        });
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Success, result.Error);
                Assert.AreEqual(0, result.ResultCode);
                Assert.IsNull(result.Error);
            }
        }

        [TestMethod]
        public void ShouldAddRecordToDataBase()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var clientService = container.Resolve<IClientManagementService>();

                var client = new RegisterClientRequest
                {
                    Email = TestEmail,
                    FirstName = TestFirstName,
                    LastName = TestLastName,
                    MiddleName = TestMiddleName,
                    MobilePhone = TestMobilePhone,
                    BirthDate = TestBirthDate
                };

                var result = clientService.RegisterNewClient(client);

                Assert.IsTrue(result.Success, result.Error);

                using (var uow = CreateUow())
                {
                    var recordInDb = uow.ClientForRegistrationRepository.GetById(x => x.MobilePhone == TestMobilePhone);

                    Assert.IsNotNull(recordInDb);
                    Assert.AreEqual(TestMobilePhone, recordInDb.MobilePhone);
                    Assert.AreEqual(TestEmail, recordInDb.Email);
                }
            }
        }

        [TestMethod]
        public void ShouldReturnErrorIfPhoneNumberIsWaitingForConfirm()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var clientService = container.Resolve<IClientManagementService>();

                var result = clientService.RegisterNewClient(
                    new RegisterClientRequest
                    {
                        Email = TestEmail,
                        FirstName = TestFirstName,
                        LastName = TestLastName,
                        MiddleName = TestMiddleName,
                        MobilePhone = TestMobilePhone,
                        BirthDate = TestBirthDate
                    });
                result = clientService.RegisterNewClient(
                    new RegisterClientRequest
                    {
                        Email = TestEmail,
                        FirstName = TestFirstName,
                        LastName = TestLastName,
                        MiddleName = TestMiddleName,
                        MobilePhone = TestMobilePhone,
                        BirthDate = TestBirthDate
                    });

                Assert.IsNotNull(result);
                Assert.IsFalse(result.Success);
                Assert.AreEqual((int) ExceptionType.ClientAlreadyExistsException, result.ResultCode);
            }
        }

		[TestMethod]
		public void ShouldReturnErrorIfPhoneNumberAlreadyRegistered()
		{
		    using (var container = DataSourceHelper.CreateContainer())
		    {
		        var clientService = container.Resolve<IClientManagementService>();

		        using (var uow = CreateUow())
		        {
		            var client = new ClientForRegistration
		            {
		                Status = (int) RegisterClientStatus.Success,
		                MobilePhone = TestMobilePhone,
		                Email = TestEmail,
		                BirthDate = TestBirthDate,
		                FirstName = TestFirstName,
		                LastName = TestLastName,
		                Gender = (int) Gender.Other,
		                MiddleName = TestMiddleName,
		                ClientId = Guid.NewGuid().ToString(),
		                IsDeleted = false,
		            };

		            uow.ClientForRegistrationRepository.Add(client);
		            uow.Save();
		        }

		        var result = clientService.RegisterNewClient(
		            new RegisterClientRequest
		            {
		                Email = TestEmail,
		                FirstName = TestFirstName,
		                LastName = TestLastName,
		                MiddleName = TestMiddleName,
		                MobilePhone = TestMobilePhone,
		                BirthDate = TestBirthDate
		            });

		        Assert.IsNotNull(result);
		        Assert.IsFalse(result.Success);
		        Assert.AreEqual((int) ExceptionType.ClientAlreadyExistsException, result.ResultCode);
		        Assert.AreEqual(
		            "Выполнение регистрации невозможно. Данный номер уже зарегистрирован или ожидает подтверждения регистрации со стороны банка.",
		            result.Error);
		    }
		}

		[TestMethod]
        public void ShouldAllowRegistrationIfPhoneNumberRegisteredWithIsDeleted()
		{
		    using (var container = DataSourceHelper.CreateContainer())
		    {
		        var clientService = container.Resolve<IClientManagementService>();

		        using (var uow = CreateUow())
		        {
		            var client = new ClientForRegistration
		            {
		                Status = (int) RegisterClientStatus.UnknownClient,
		                MobilePhone = TestMobilePhoneDeleted,
		                Email = TestEmail,
		                BirthDate = TestBirthDate,
		                FirstName = TestFirstName,
		                LastName = TestLastName,
		                Gender = (int) Gender.Other,
		                MiddleName = TestMiddleName,
		                ClientId = Guid.NewGuid().ToString(),
		                IsDeleted = true,
		            };

		            uow.ClientForRegistrationRepository.Add(client);
		            uow.Save();
		        }

		        var result = clientService.RegisterNewClient(
		            new RegisterClientRequest
		            {
		                Email = TestEmail,
		                FirstName = TestFirstName,
		                LastName = TestLastName,
		                MiddleName = TestMiddleName,
		                MobilePhone = TestMobilePhoneDeleted,
		                BirthDate = TestBirthDate
		            });

		        Assert.IsNotNull(result);
		        Assert.IsTrue(result.Success);
		    }
		}

		[TestMethod]
		public void ShouldReturnErrorIfPhoneNumberInBankRegistrationTable()
		{
		    using (var container = DataSourceHelper.CreateContainer())
		    {
		        var clientService = container.Resolve<IClientManagementService>();

		        var bankRegistrationRequest = new ClientForBankRegistration();
		        bankRegistrationRequest.MobilePhone = TestMobilePhone.Substring(1);

		        using (var uow = CreateUow())
		        {
		            uow.ClientForBankRegistrationRepository.Add(bankRegistrationRequest);
		            uow.Save();
		        }

		        var result = clientService.RegisterNewClient(
		            new RegisterClientRequest
		            {
		                Email = TestEmail,
		                FirstName = TestFirstName,
		                LastName = TestLastName,
		                MiddleName = TestMiddleName,
		                MobilePhone = TestMobilePhone,
		                BirthDate = TestBirthDate
		            });

		        Assert.IsNotNull(result);
		        Assert.IsFalse(result.Success);
		        Assert.AreEqual("Заявка на регистрацию клиента с таким телефоном была прислана со стороны банка", result.Error);
		        Assert.AreEqual((int) ExceptionType.ClientAlreadyExistsException, result.ResultCode);
		    }
		}

		[TestMethod]
		public void ShouldReturnErrorIfPhoneNumberIsRegisteredInSecurity()
		{
		    using (var container = DataSourceHelper.CreateContainer())
		    {
		        var security = container.Resolve<ISecurityWebApi>();

		        security.CreateUser(new CreateUserOptions
		        {
		            ClientId = Guid.NewGuid().ToString(),
		            PhoneNumber = TestMobilePhone,
		            RegistrationType = RegistrationType.SiteRegistration
		        });

		        var clientService = container.Resolve<IBankConnectorService>(); //new ClientManagementService(temp);

		        var result = clientService.RegisterNewClient(
		            new RegisterClientRequest
		            {
		                Email = TestEmail,
		                FirstName = TestFirstName,
		                LastName = TestLastName,
		                MiddleName = TestMiddleName,
		                MobilePhone = TestMobilePhone,
		                BirthDate = TestBirthDate
		            });

		        Assert.IsNotNull(result);
		        Assert.IsFalse(result.Success);
		        Assert.AreEqual("Клиент c таким телефоном уже есть в системе", result.Error);
		        Assert.AreEqual((int) ExceptionType.ClientAlreadyExistsException, result.ResultCode);
		    }
		}

		[TestMethod]
		public void ShouldAllowRegistrationIfPhoneNumberIsSendedForDetach()
		{
		    using (var container = DataSourceHelper.CreateContainer())
		    {
		        var clientService = container.Resolve<IBankConnectorService>(); //new ClientManagementService(temp);

		        ClientProfileServiceStub.ResetStub();

		        var testSessionId = Guid.Parse("B1B96FC1-E344-4BD9-B10D-F5342C2F47D5");

		        using (var uow = CreateUow())
		        {
		            var clientForDeletion =
		                uow.ClientForDeletionRepository.GetAll().Where(x => x.ExternalClientId == TestClientId).ToList();

		            if (clientForDeletion.Any())
		            {
		                clientForDeletion.ForEach(uow.ClientForDeletionRepository.Delete);
		            }

		            uow.Save();
		        }

		        clientService.BlockClientToDelete(TestClientId);

		        string phone;

		        using (var uow = CreateUow())
		        {
		            var clientForDeletion =
		                uow.ClientForDeletionRepository.GetAll().Single(x => x.ExternalClientId == TestClientId);

		            phone = clientForDeletion.MobilePhone;

		            var result = clientService.RegisterNewClient(
		                new RegisterClientRequest
		                {
		                    Email = TestEmail,
		                    FirstName = TestFirstName,
		                    LastName = TestLastName,
		                    MiddleName = TestMiddleName,
		                    MobilePhone = clientForDeletion.MobilePhone,
		                    BirthDate = TestBirthDate
		                });
		            Assert.IsNotNull(result);
		            Assert.IsFalse(result.Success);

		            clientForDeletion.SendEtlSessionId = testSessionId;
		            uow.Save();
		        }

		        var resultSuccess = clientService.RegisterNewClient(
		            new RegisterClientRequest
		            {
		                Email = TestEmail,
		                FirstName = TestFirstName,
		                LastName = TestLastName,
		                MiddleName = TestMiddleName,
		                MobilePhone = phone,
		                BirthDate = TestBirthDate
		            });

		        Assert.IsNotNull(resultSuccess);
		        Assert.IsTrue(resultSuccess.Success);
		    }
		}
	}
}
