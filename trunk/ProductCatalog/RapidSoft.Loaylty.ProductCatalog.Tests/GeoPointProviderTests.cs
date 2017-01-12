using RapidSoft.Loaylty.ProductCatalog.Services.Delivery;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using API.Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.Services;

    [TestClass]
    public class GeoPointProviderTests
    {
        [TestMethod]
        public void ShouldResolveDeliveryInfo()
        {
            var kladr = TestDataStore.KladrCode;
            var address = TestHelper.BuildDeliveryInfo(kladr).Address;

            Assert.IsNull(address.AddressText);

            var service = new DeliverySpec(geoPointProvider: new TestGeoPointProvider(), partnerDeliveryVariants: new PartnerDeliveryVariants());
            service.FillDeliveryAddress(address, kladr);

            Assert.AreEqual("г. Москва, ул. Островетянова, 26б", address.AddressText);
        }
    }
}