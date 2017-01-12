namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System.Collections.Generic;

    using API.Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using ProductCatalog.Services;

    using PromoAction.WsClients.MechanicsService;

    [TestClass]
    public class MechanicsProviderTest
    {
        [TestMethod]
        public void ShouldCasheObtainedMechanicsTest()
        {
            var clientContext = GetClientContext();

            var serviceClientMock = MockFactory.GetMechanicsClientProviderMock();
            var mechanicsProvider = new MechanicsProvider(serviceClientMock.Object);
            
            mechanicsProvider.ClearCache();
            mechanicsProvider.GetPriceSql(clientContext);
            mechanicsProvider.GetPriceSql(clientContext);

            serviceClientMock.Verify(m => m.GenerateSql(It.IsAny<GenerateSqlParameters>()), Times.Exactly(1));
        }

        [TestMethod]
        public void ShouldNotCasheDifferentKeysTest()
        {
            var clientContext = GetClientContext();
            var serviceClientMock = MockFactory.GetMechanicsClientProviderMock();
            var mechanicsProvider = new MechanicsProvider(serviceClientMock.Object);

            mechanicsProvider.ClearCache();
            mechanicsProvider.GetPriceSql(clientContext);
            mechanicsProvider.GetPriceSql(TestDataStore.GetClientContext());

            serviceClientMock.Verify(m => m.GenerateSql(It.IsAny<GenerateSqlParameters>()), Times.Exactly(2));
        }

        [TestMethod]
        public void ShouldClearCasheTest()
        {
            var clientContext = GetClientContext();
            var serviceClientMock = MockFactory.GetMechanicsClientProviderMock();
            var mechanicsProvider = new MechanicsProvider(serviceClientMock.Object);
            
            mechanicsProvider.ClearCache();
            mechanicsProvider.GetPriceSql(clientContext);
            mechanicsProvider.ClearCache();
            mechanicsProvider.GetPriceSql(clientContext);

            serviceClientMock.Verify(m => m.GenerateSql(It.IsAny<GenerateSqlParameters>()), Times.Exactly(2));
        }

        [TestMethod]
        public void ShouldCalcDeliveryPriceTest()
        {
            var serviceClientMock = MockFactory.GetMechanicsClientProviderMock();
            var mechanicsProvider = new MechanicsProvider(serviceClientMock.Object);
            var product = TestDataStore.GetProduct();
            var res = mechanicsProvider.CalculateDeliveryPrice(TestDataStore.GetClientContext(), 100, product.PartnerId);
            Assert.IsNotNull(res);            
        }

        private static Dictionary<string, string> GetClientContext()
        {
            var clientContext = new Dictionary<string, string>()
            {
                {
                    "KLADR", "1234"
                }
            };
            return clientContext;
        }
    }
}