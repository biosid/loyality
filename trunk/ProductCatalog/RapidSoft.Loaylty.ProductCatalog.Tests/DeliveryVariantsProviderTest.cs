using RapidSoft.Loaylty.ProductCatalog.Services.Delivery;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using API.Entities;
    using API.InputParameters;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ProductCatalog.Services;

    [TestClass]
    public class DeliveryVariantsProviderTest
    {
        [TestMethod]
        public void ShouldGetSiteDeliveryVariantsFromProviderTest()
        {
            var partnerConnectorProvider = MockFactory.GetPartnerConnectorProvider().Object;
            // partnerConnectorProvider = null;

            Partner partner = TestDataStore.GetPartner();
            var partnersRepository = MockFactory.GetPartnersRepository(partner);
            var provider = new DeliveryVariantsProvider(partnerConnectorProvider, partnersRepository.Object, MockFactory.GetDeliveryRatesRepository().Object);
            var res = provider.GetSiteDeliveryVariants(partner.Id, new Location(), new OrderItem(), TestDataStore.TestClientId);

            Assert.IsNotNull(res);
            Assert.AreEqual(DeliveryTypes.Delivery, res.DeliveryGroups[0].DeliveryVariants[0].DeliveryType);
            Assert.AreEqual(DeliveryTypes.Pickup, res.DeliveryGroups[1].DeliveryVariants[0].DeliveryType);
        } 
    }
}