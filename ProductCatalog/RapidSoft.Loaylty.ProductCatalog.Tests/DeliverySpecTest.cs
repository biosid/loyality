using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidSoft.Loaylty.ProductCatalog.Services.Delivery;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using ProductCatalog.Services;

    [TestClass]
    public class DeliverySpecTest
    {
        [TestMethod]
        public void ShouldGetVariantByIdTest()
        {
            var res = new DeliverySpec(TestDataStore.GetDeliveryVariants()).GetDeliveryCost(TestDataStore.ExternalDeliveryVariantId, null);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void ShouldBuildDeliveryTest()
        {
            var partnersRepository = MockFactory.GetPartnersRepository(TestDataStore.GetPartner());
            var geoPointProvider = MockFactory.GetGeoPointProvider().Object;
            
            var res = new DeliverySpec(TestDataStore.GetDeliveryVariants(), geoPointProvider, partnersRepository.Object).
                BuildDeliveryInfo(TestDataStore.GetDeliveryDto(), TestDataStore.PartnerId);

            Assert.IsNotNull(res);
            Assert.IsNull(res.PickupPoint);
            Assert.IsNotNull(res.Address);
            Assert.IsNotNull(res.ExternalDeliveryVariantId);
            Assert.AreEqual("DeliveryVariantDescription", res.DeliveryVariantDescription);
        }
    }
}