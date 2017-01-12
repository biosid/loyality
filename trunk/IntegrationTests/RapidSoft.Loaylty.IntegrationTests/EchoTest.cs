namespace RapidSoft.Loaylty.IntegrationTests
{
    using ClientProfile.ClientProfileService;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Processing.ProcessingService;

    using Rapidsoft.Loyalty.NotificationSystem.WsClients.AdminFeedbackService;
    using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientFeedbackService;
    using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientMessageService;
    using Rapidsoft.Loyalty.NotificationSystem.WsClients.EmailSenderService;

    using RapidSoft.GeoPoints.WsClients.GeoPointService;
    using RapidSoft.Loaylty.IntegrationTests.PromoAction;
    using RapidSoft.Loaylty.PartnersConnector.WsClients.PartnerSecurityService;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.BasketService;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogSearcherService;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.WishListService;
    using RapidSoft.Loaylty.PromoAction.WsClients.AdminMechanicsService;
    using RapidSoft.Loaylty.PromoAction.WsClients.MechanicsService;
    using RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService;
    using RapidSoft.VTB24.BankConnector.WsClients.BankConnectorService;

    using VTB24.Site.PublicProfileWebApi;
    using VTB24.Site.SecurityTokenWebApi;
    using VTB24.Site.SecurityWebApi;

    using PartnersConnectorOrderManagementServiceClient = RapidSoft.Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.OrderManagementServiceClient;
    using ProductCatalogOrderManagementServiceClient = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.OrderManagementServiceClient;

    [TestClass]
    public class EchoTest
    {
        private const string Message = "test";

        #region GeoBase

        [TestMethod]
        public void ShouldEchoGeoPointServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<GeoPointServiceClient, string>(x => x.Echo(Message)));
        }

        #endregion

        #region NotificationSystem

        [TestMethod]
        public void ShouldEchoClientMessageServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<ClientMessageServiceClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoAdminFeedbackServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<AdminFeedbackServiceClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoClientFeedbackServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<ClientFeedbackServiceClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoEmailSenderTest()
        {
            AssertEchoRes(WebClientCaller.CallService<EmailSenderClient, string>(x => x.Echo(Message)));
        }

        #endregion

        #region PartnersConnector

        [TestMethod]
        public void ShouldEchoPartnersConnectorOrderManagementServiceTest()
        {
            AssertEchoRes(
                WebClientCaller.CallService<PartnersConnectorOrderManagementServiceClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoPartnersConnectorPartnerSecurityServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<PartnerSecurityServiceClient, string>(x => x.Echo(Message)));
        }

        #endregion

        #region ProductCatalog

        [TestMethod]
        public void ShouldEchoBasketServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<BasketServiceClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoCatalogAdminServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<CatalogAdminServiceClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoCatalogSearcherServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<CatalogSearcherClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoOrderManagementServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<ProductCatalogOrderManagementServiceClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoWishListServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<WishListServiceClient, string>(x => x.Echo(Message)));
        }

        #endregion

        #region PromoAction

        [TestMethod]
        public void ShouldEchoAdminMechanicsServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<AdminMechanicsServiceClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoMechanicsServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<MechanicsServiceClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoTargetAudienceServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<TargetAudienceServiceClient, string>(x => x.Echo(Message)));
        }

        #endregion

        #region BankConnector

        [TestMethod]
        public void ShouldEchoBankConnectorServiceTest()
        {
            AssertEchoRes(WebClientCaller.CallService<BankConnectorClient, string>(x => x.Echo(Message)));
        }

        #endregion

        #region ClientProfile

        [TestMethod]
        public void ShouldEchoClientProfileTest()
        {
            var request = new GetClientProfileFullRequestType()
            {
                ClientId = "vtb_1"
            };

            var res = WebClientCaller.CallService<ClientProfileServiceClient, GetClientProfileFullResponseType>(x => x.GetClientProfileFull(request));

            Assert.IsNotNull(res, "ClientProfile ответил null");
            Assert.AreEqual(0, res.StatusCode, string.Format("Не удалось получить {0} из ClientProfile", request.ClientId));
        }

        #endregion

        #region Processing

        [TestMethod]
        public void ShouldEchoProcessingTest()
        {
            var request = new GetAccountsByClientRequestType()
            {
                ClientId = "vtb_1",
                LoyaltyProgramId = 5
            };

            var res = WebClientCaller.CallService<ProcessingServiceClient, GetAccountsByClientResponseType>(x => x.GetAccountsByClient(request));

            Assert.IsNotNull(res, "Processing ответил null");
            Assert.AreEqual(0, res.StatusCode, string.Format("Не удалось получить {0} из Processing", request.ClientId));
        }

        #endregion

        #region Site

        [TestMethod]
        public void ShouldEchoPublicProfileWebApiTest()
        {
            AssertEchoRes(WebClientCaller.CallService<PublicProfileWebApiClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoSecurityTokenWebApiTest()
        {
            AssertEchoRes(WebClientCaller.CallService<SecurityTokenWebApiClient, string>(x => x.Echo(Message)));
        }

        [TestMethod]
        public void ShouldEchoSecurityWebApiClientTest()
        {
            AssertEchoRes(WebClientCaller.CallService<SecurityWebApiClient, string>(x => x.Echo(Message)));
        }

        #endregion

        private void AssertEchoRes(string res)
        {
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Contains(Message));
        }
    }
}