namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;

    using Interfaces;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using PartnersConnector.Services;
    using PartnersConnector.Services.CryptoServices;

    using ProductCatalog.WsClients.OrderManagementService;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.Logging.Interaction;
    using RapidSoft.Loaylty.PartnersConnector.Common.DTO.Online;
    using RapidSoft.VTB24.Site.SecurityTokenWebApi;

    using VTB24.Site.PublicProfileWebApi;

    using Order = ProductCatalog.WsClients.OrderManagementService.Order;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    public class BonusPaymentGatewayServiceTests
    {
        private const string TestPrivateKey =
            @"<RSAKeyValue><Modulus>vI57shcn9CdZpU5tG9SsB20shtQ+7wwtYqZfWMbhXFyLIP8HOCsL/5B+xr5byts8cMBCc7L9hx21j6/xJCY5o7EPO2Q4cyMhcik6/9s9dp1gSGNLP8biqHZHv8sk+z6o5nisoVm9zP0Kiu1OULUg5spZlCsj4wy4f8ULM7agr+c=</Modulus><Exponent>AQAB</Exponent><P>6xnZ7HzXHvtJi+KxJkUBHJt+wSPgO1CiSAYzHBy16uajaN+XUhuWXcCCEmwyYUwH18mmH726c5ZPKqT0MJcliQ==</P><Q>zVFt/FuaZIV6D0qh/vN83b0APvf8pZlKESS8hwfGUl7s1RNOL391dAu15JEZxixzNSaOH3XniL6KYTnudzs97w==</Q><DP>wpQtwkX8wZ6W21ju506bQfMMMIwhzNXKyjOfX3f/tH/Y5TaRBhrhE4z92oOEGvUTVKyHeqPLyliwAwptND4UiQ==</DP><DQ>AZaerjQbNqndrt6Z8Dn7/k8nAFW0y6cq7oUFPFowC5UWafOTSETJKNOqXZFNzL2tSnz43n9wAhvPQD9Ne/imWw==</DQ><InverseQ>izdoQEbFzZpytICetjejkuOG7JAIsJHOjjzajb00GbQcQw5GOavhyQsddKT2qQSNDzm/NW/O3m/2jCDppvw4Ew==</InverseQ><D>SPFx5sZQfYJPisSZLwAKAOF6LTqkkgOK9zPXhNeDTSC77era1x7ICHjUonv3tLU1X4Tw8CNZMtEKcOimh86F0yQGAOZEkLw5A+4OWjccGzeuhnjpVwASRmuYvuoKrxdRX4VeXs3RPBtHg/6n/vPyKKPgxOvYSMNXWXGatEze//E=</D></RSAKeyValue>";

        private const string ShopId = "1";

        [TestMethod]
        public void ShouldValidateUser()
        {
            var fakeCrypto = new XmlPrivateKeyCryptoService(TestPrivateKey);

            const string UserTicket = "userTicket";

            var parametres = string.Concat(ShopId, UserTicket);
            var sign = fakeCrypto.CreateSignature(parametres);

            var mockFactory = new Mock<ICryptoServiceFactory>();
            mockFactory.Setup(x => x.GetBankCryptoService()).Returns(fakeCrypto);
            mockFactory.Setup(x => x.GetParnterCryptoService(It.IsAny<int>())).Returns(fakeCrypto);
            
            var mockProfileWebApi = new Mock<ISecurityWebServicesProvider>();
            mockProfileWebApi.Setup(x => x.GetUserProfile(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(BuildGetPublicProfileResult());

            Console.WriteLine(string.Format("In: ShopId = {0}, UserTicket = {1}, Signature = {2}", ShopId, UserTicket, sign));
            var service = new BonusPaymentGatewayService(mockFactory.Object, securityWebServicesProvider: mockProfileWebApi.Object);

            var resultXml = service.ValidateUser(ShopId, UserTicket, sign, StartInteraction.With("Partner").For("ValidateUser"));

            Console.WriteLine("Out: " + resultXml);
            var response = resultXml.Deserialize<ValidateUserResult>(Encoding.UTF8);

            Assert.AreEqual(0, response.Status);
            Assert.AreEqual("Самара", response.City);

            Assert.AreEqual("email@email.ru", fakeCrypto.Decrypt(response.Email));
            Assert.AreEqual("LastName", fakeCrypto.Decrypt(response.LastName));
            Assert.AreEqual("FirstName", fakeCrypto.Decrypt(response.FirstName));
            Assert.AreEqual("MiddleName", fakeCrypto.Decrypt(response.MiddleName));

            mockFactory.Verify(x => x.GetBankCryptoService(), Times.Once());
            mockFactory.Verify(x => x.GetParnterCryptoService(It.IsAny<int>()), Times.Once());

            mockProfileWebApi.Verify(x => x.GetUserProfile(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ShouldUpdateOrder()
        {
            const string UserTicket = "123456";
            const int OrderId = 555;

            var fakeCrypto = new XmlPrivateKeyCryptoService(TestPrivateKey);

            var mockFactory = new Mock<ICryptoServiceFactory>();
            mockFactory.Setup(x => x.GetBankCryptoService()).Returns(fakeCrypto);
            mockFactory.Setup(x => x.GetParnterCryptoService(It.IsAny<int>())).Returns(fakeCrypto);

            var mockCatalogProvider = new Mock<IProductCatalogProvider>();

            var batchResult = BuildMockChangeExternalOrdersStatusesResult(OrderId);
            mockCatalogProvider.Setup(x => x.ChangeOrdersStatuses(It.IsAny<ExternalOrdersStatus[]>()))
                               .Returns(batchResult);

            var mockSecurityWebServices = new Mock<ISecurityWebServicesProvider>();
            var validateSecurityToken = new ValidateSecurityTokenResult { PrincipalId = "654321" };
            mockSecurityWebServices.Setup(x => x.GetClientId(UserTicket)).Returns(validateSecurityToken);

            var dt = DateTime.Now;
            var item = new NotifyOrderItem
                           {
                               Amount = 1,
                               ArticleId = "ArticleId",
                               ArticleName = "ArticleName",
                               Comment = "Comment",
                               Id = "Id",
                               Price = 45m,
                               BonusPrice = 20,
                               Weight = 500
                           };
            var notifyOrder = new NotifyOrder
                                  {
                                      ShopId = ShopId,
                                      OrderId = OrderId.ToString(CultureInfo.InvariantCulture),
                                      DateTime = dt,
                                      UtcDateTime = dt.ToUniversalTime(),
                                      OrderStatus = 30,
                                      Description = "Description",
                                      InternalStatusCode = "InternalStatusCode",
                                      TotalCost = 45m,
                                      Item = new[] { item },
                                      Signature = string.Empty,
                                      UserTicket = UserTicket
                                  };

            var signedXml = notifyOrder.SerializeWithSing(fakeCrypto);

            Console.WriteLine("In: " + signedXml);
            var service = new BonusPaymentGatewayService(
                mockFactory.Object, mockCatalogProvider.Object, mockSecurityWebServices.Object);

            var resultXml = service.NotifyOrder(signedXml, StartInteraction.With("Partner").For("NotifyOrder"));

            Console.WriteLine("Out: " + resultXml);
            var response = resultXml.Deserialize<NotifyOrderResult>(Encoding.UTF8);

            Assert.AreEqual(0, response.Status);
            Assert.AreEqual(OrderId.ToString(CultureInfo.InvariantCulture), response.OrderId);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(response.Signature));

            mockFactory.Verify(x => x.GetBankCryptoService(), Times.Once());
            mockFactory.Verify(x => x.GetParnterCryptoService(It.IsAny<int>()), Times.Once());

            mockCatalogProvider.Verify(x => x.ChangeOrdersStatuses(It.IsAny<ExternalOrdersStatus[]>()), Times.Once());
        }

        [TestMethod]
        public void ShouldCreateOrder()
        {
            const string UserTicket = "123456";
            const int OrderId = 555;

            var fakeCrypto = new XmlPrivateKeyCryptoService(TestPrivateKey);

            var mockFactory = new Mock<ICryptoServiceFactory>();
            mockFactory.Setup(x => x.GetBankCryptoService()).Returns(fakeCrypto);
            mockFactory.Setup(x => x.GetParnterCryptoService(It.IsAny<int>())).Returns(fakeCrypto);

            var mockCatalogProvider = new Mock<IProductCatalogProvider>();

            var orderResult = BuildMockCreateOrderResult(OrderId);
            mockCatalogProvider.Setup(x => x.CreateOrderForOnlinePartner(It.IsAny<CreateOrderFromOnlinePartnerParameters>()))
                               .Returns(orderResult);

            var mockSecurityWebServices = new Mock<ISecurityWebServicesProvider>();
            var validateSecurityToken = new ValidateSecurityTokenResult { PrincipalId = "654321" };
            mockSecurityWebServices.Setup(x => x.GetClientId(UserTicket)).Returns(validateSecurityToken);

            var dt = DateTime.Now;
            var item = new NotifyOrderItem
                           {
                               Amount = 1,
                               ArticleId = "ArticleId",
                               ArticleName = "ArticleName",
                               Comment = "Comment",
                               Id = "Id",
                               Price = 45m,
                               BonusPrice = 20,
                               Weight = 500
                           };
            var notifyOrder = new NotifyOrder
                                  {
                                      UserTicket = UserTicket,
                                      ShopId = ShopId,
                                      OrderId = OrderId.ToString(CultureInfo.InvariantCulture),
                                      DateTime = dt,
                                      UtcDateTime = dt.ToUniversalTime(),
                                      OrderStatus = 1, // NOTE: Заказ создан
                                      Description = "Description",
                                      InternalStatusCode = "InternalStatusCode",
                                      TotalCost = 45m,
                                      Item = new[] { item },
                                      Signature = string.Empty
                                  };

            var signedXml = notifyOrder.SerializeWithSing(fakeCrypto);

            Console.WriteLine("In: " + signedXml);
            var service = new BonusPaymentGatewayService(mockFactory.Object, mockCatalogProvider.Object, mockSecurityWebServices.Object);

            var resultXml = service.NotifyOrder(signedXml, StartInteraction.With("Partner").For("NotifyOrder"));

            Console.WriteLine("Out: " + resultXml);
            var response = resultXml.Deserialize<NotifyOrderResult>(Encoding.UTF8);

            Assert.AreEqual(0, response.Status);
            Assert.AreEqual(OrderId.ToString(CultureInfo.InvariantCulture), response.OrderId);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(response.Signature));

            mockFactory.Verify(x => x.GetBankCryptoService(), Times.Once());
            mockFactory.Verify(x => x.GetParnterCryptoService(It.IsAny<int>()), Times.Once());

            mockCatalogProvider.Verify(x => x.CreateOrderForOnlinePartner(It.IsAny<CreateOrderFromOnlinePartnerParameters>()), Times.Once());

            mockSecurityWebServices.Verify(x => x.GetClientId(UserTicket), Times.Once());
        }

        [TestMethod]
        public void ShouldNotCreateOrderWithInvalidPrice()
        {
            const string UserTicket = "123456";
            const int OrderId = 555;

            var fakeCrypto = new XmlPrivateKeyCryptoService(TestPrivateKey);

            var mockFactory = new Mock<ICryptoServiceFactory>();
            mockFactory.Setup(x => x.GetBankCryptoService()).Returns(fakeCrypto);
            mockFactory.Setup(x => x.GetParnterCryptoService(It.IsAny<int>())).Returns(fakeCrypto);

            var dt = DateTime.Now;
            var item = new NotifyOrderItem
                           {
                               Amount = 1,
                               ArticleId = "ArticleId",
                               ArticleName = "ArticleName",
                               Comment = "Comment",
                               Id = "Id",
                               Price = 45m,
                               Weight = 500
                           };
            var notifyOrder = new NotifyOrder
            {
                UserTicket = UserTicket,
                ShopId = ShopId,
                OrderId = OrderId.ToString(CultureInfo.InvariantCulture),
                DateTime = dt,
                UtcDateTime = dt.ToUniversalTime(),
                OrderStatus = 1, // NOTE: Заказ создан
                Description = "Description",
                InternalStatusCode = "InternalStatusCode",
                TotalCost = 45m,
                Item = new[] { item },
                Signature = string.Empty
            };

            var signedXml = notifyOrder.SerializeWithSing(fakeCrypto);

            Console.WriteLine("In: " + signedXml);
            var service = new BonusPaymentGatewayService(mockFactory.Object);

            var resultXml = service.NotifyOrder(signedXml, StartInteraction.With("Partner").For("NotifyOrder"));

            Console.WriteLine("Out: " + resultXml);
            var response = resultXml.Deserialize<NotifyOrderResult>(Encoding.UTF8);

            Assert.AreEqual(1, response.Status);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(response.Signature));
        }

        private static ChangeExternalOrdersStatusesResult BuildMockChangeExternalOrdersStatusesResult(int orderId)
        {
            var orderResult = new ChangeExternalOrderStatusResult
                                  {
                                      OrderId = orderId,
                                      ExternalOrderId =
                                          orderId.ToString(CultureInfo.InvariantCulture),
                                      Success = true,
                                      ResultCode = 0,
                                      ResultDescription = string.Empty
                                  };
            var batchResult = new ChangeExternalOrdersStatusesResult
                                  {
                                      Success = true,
                                      ResultCode = 0,
                                      ResultDescription = string.Empty,
                                      ChangeExternalOrderStatusResults = new[] { orderResult }
                                  };
            return batchResult;
        }

        private static CreateOrderResult BuildMockCreateOrderResult(int orderId)
        {
            var order = new Order
                            {
                                Id = 555,
                                ExternalOrderId =
                                    orderId.ToString(CultureInfo.InvariantCulture)
                            };
            var orderResult = new CreateOrderResult
                                  {
                                      Success = true,
                                      ResultCode = 0,
                                      ResultDescription = string.Empty,
                                      Order = order
                                  };

            return orderResult;
        }

        private static GetPublicProfileResult BuildGetPublicProfileResult()
        {
            return new GetPublicProfileResult
            {
                Status = 0,
                Email = "email@email.ru",
                LastName = "LastName",
                FirstName = "FirstName",
                MiddleName = "MiddleName",
                NameLang = "RU",
                City = "Самара",
                Balance = 150m,
                Rate = 0.5m,
                Delta = -10m,
                UtcDateTime = DateTime.UtcNow
            };
        }
    }
}
