namespace RapidSoft.VTB24.BankConnector.Tests.ProcessorTest
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Processors;
    using RapidSoft.VTB24.Site.SecurityWebApi;

    [TestClass]
    public class LoyaltyClientUpdateProcessorTests : TestBase
    {
        private static readonly GetClientProfileFullResponseTypeClientProfilePhone TestPhone =
            new GetClientProfileFullResponseTypeClientProfilePhone
                {
                    PhoneNumber = "1234567890",
                    IsPrimary = true,
                    PhoneId = 5,
                    PhoneType = "Mobile"
                };

        private static readonly GetClientProfileFullResponseTypeClientProfile TestProfile =
            new GetClientProfileFullResponseTypeClientProfile
                {
                    BirthDate = DateTime.Now,
                    Email = "email@email.com",
                    FirstName = "FirstName",
                    LastName = "LastName",
                    MiddleName = "MiddleName",
                    Phones = new[] { TestPhone },
                    Gender = "1"
                };

        private readonly GetClientProfileFullResponse clientProfileFullResponse =
            new GetClientProfileFullResponse
                {
                    Response =
                        new GetClientProfileFullResponseType
                            {
                                Error = null,
                                StatusCode = 0,
                                ClientProfile = TestProfile
                            }
                };

        [TestMethod]
        public void ShouldSaveDirty()
        {
            var clientId = Guid.NewGuid().ToString();
            var req = new UpdateClientRequest { ClientId = clientId };

            var uow = CreateUow();

            var secMock = new Mock<ISecurityWebApi>();
            var profMock = new Mock<ClientProfileService>();

            var processor = new LoyaltyClientUpdateProcessor(uow, secMock.Object, profMock.Object);

            processor.UpdateClient(req);

            var repo = uow.LoyaltyClientUpdateRepository;

            var data = repo.GetAll().FirstOrDefault(x => x.ClientId == clientId);

            Assert.IsNotNull(data);
            
            repo.Delete(data);
            uow.Save();
        }

        [TestMethod]
        public void ShouldLoadProfile()
        {
            var clientId = Guid.NewGuid().ToString();
            var req = new UpdateClientRequest { ClientId = clientId };

            var uow = CreateUow();

            var secMock = new Mock<ISecurityWebApi>();
            var profMock = new Mock<ClientProfileService>();
            profMock.Setup(
                x =>
                x.GetClientProfileFull(It.IsAny<GetClientProfileFullRequest>()))
                    .Returns((GetClientProfileFullRequest profileReq) =>
                        {
                            if (profileReq.Request.ClientId != clientId)
                            {
                                throw new Exception("Это не наш клиент");
                            }

                            // NOTE: Важен только сам факт вызова
                            return (GetClientProfileFullResponse)null;
                        });

            var processor = new LoyaltyClientUpdateProcessor(uow, secMock.Object, profMock.Object);

            processor.UpdateClient(req);

            profMock.Verify(x => x.GetClientProfileFull(It.IsAny<GetClientProfileFullRequest>()), Times.Once());

            var repo = uow.LoyaltyClientUpdateRepository;

            var data = repo.GetAll().FirstOrDefault(x => x.ClientId == clientId);

            Assert.IsNotNull(data);
            Assert.AreEqual(
                LoyaltyClientUpdateStatuses.GetProfile_InvalidResponse,
                data.UpdateStatus,
                "Стаб вернет null, а значит должно ProfileServiceReturnInvalidData");

            repo.Delete(data);
            uow.Save();
        }

        [TestMethod]
        public void ShouldResetPropertyFromProfile()
        {
            var clientId = Guid.NewGuid().ToString();
            var req = new UpdateClientRequest
                          {
                              ClientId = clientId,
                              Email = null,
                              UpdateProperties = new[] { UpdateProperty.Email }
                          };

            var uow = CreateUow();

            var secMock = new Mock<ISecurityWebApi>();
            var profMock = new Mock<ClientProfileService>();
            profMock.Setup(x => x.GetClientProfileFull(It.IsAny<GetClientProfileFullRequest>())).Returns(
                () =>
                    {
                        clientProfileFullResponse.Response.ClientProfile.ClientId = clientId;
                        return clientProfileFullResponse;
                    });

            var processor = new LoyaltyClientUpdateProcessor(uow, secMock.Object, profMock.Object);

            processor.UpdateClient(req);

            var repo = uow.LoyaltyClientUpdateRepository;

            var data = repo.GetAll().FirstOrDefault(x => x.ClientId == clientId);

            Assert.IsNotNull(data);
            Assert.AreEqual(TestProfile.FirstName, data.FirstName);
            Assert.AreEqual(TestProfile.LastName, data.LastName);
            Assert.AreEqual(TestProfile.MiddleName, data.MiddleName);

            Assert.IsNull(data.MobilePhoneId, "Так как телефон не меняем, то и сохранять ид телефона не надо");
            Assert.AreEqual(TestPhone.PhoneNumber, data.MobilePhone);

            Assert.AreEqual(TestProfile.Gender, ((int)data.Gender.Value).ToString(CultureInfo.InvariantCulture));

            Assert.AreEqual(TestProfile.BirthDate.Value.ToString("dd.MM.yyyy"), data.BirthDate.Value.ToString("dd.MM.yyyy"));

            Assert.IsNull(data.Email, "Фактически мы сбрасываем email, поэтому должно быть null");
            
            repo.Delete(data);
            uow.Save();
        }
        
        [TestMethod]
        public void ShouldUpdateyProfile()
        {
            var clientId = Guid.NewGuid().ToString();
            var req = new UpdateClientRequest
                          {
                              ClientId = clientId,
                              Email = null,
                              UpdateProperties =
                                  new[] { UpdateProperty.Email }
                          };

            var uow = CreateUow();

            var secMock = new Mock<ISecurityWebApi>();
            var profMock = new Mock<ClientProfileService>();
            profMock.Setup(x => x.GetClientProfileFull(It.IsAny<GetClientProfileFullRequest>())).Returns(
                () =>
                {
                    clientProfileFullResponse.Response.ClientProfile.ClientId = clientId;
                    return clientProfileFullResponse;
                });
            profMock.Setup(x => x.UpdateClientProfile(It.IsAny<UpdateClientProfileRequest>()))
                    .Returns(
                        (UpdateClientProfileRequest request) =>
                            {
                                // NOTE: Проверяем что меняем только email
                                var requestClientProfile = request.Request.ClientProfile;
                                if (requestClientProfile.Email == null)
                                {
                                    throw new Exception("Меняем email => должны менять!");
                                }

                                if (requestClientProfile.FirstName != null || requestClientProfile.LastName != null
                                    || requestClientProfile.Phones != null)
                                {
                                    throw new Exception("Меняем только email!");
                                }

                                var responseType = new UpdateClientProfileResponseType { StatusCode = 0 };
                                return new UpdateClientProfileResponse(responseType);
                            });

            var processor = new LoyaltyClientUpdateProcessor(uow, secMock.Object, profMock.Object);

            processor.UpdateClient(req);

            profMock.Verify(x => x.UpdateClientProfile(It.IsAny<UpdateClientProfileRequest>()), Times.Once());

            var repo = uow.LoyaltyClientUpdateRepository;

            var data = repo.GetAll().FirstOrDefault(x => x.ClientId == clientId);
            
            Assert.AreEqual(LoyaltyClientUpdateStatuses.Success, data.UpdateStatus);

            repo.Delete(data);
            uow.Save();
        }
    }
}
