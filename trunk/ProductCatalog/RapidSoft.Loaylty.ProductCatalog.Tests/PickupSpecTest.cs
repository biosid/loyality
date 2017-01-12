using RapidSoft.Loaylty.ProductCatalog.Services.Delivery;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;

    using API.Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ProductCatalog.Services;

    [TestClass]
    public class PickupSpecTest
    {
        [TestMethod]
        public void ShouldMapDeliveryDtoTest()
        {
            var pointId = TestDataStore.PickupPointId;
            var address = TestDataStore.PickupPointAddress;
            var partnersRepository = MockFactory.GetPartnersRepository(TestDataStore.GetPartner());

            var res = new PickupSpec(TestDataStore.GetDeliveryVariants(), partnersRepository.Object).BuildDeliveryInfo(TestDataStore.GetDeliveryDto(), TestDataStore.PartnerId);

            Assert.IsNotNull(res);
            Assert.IsNotNull(res.PickupPoint);
            Assert.AreEqual(address, res.PickupPoint.Address);
            Assert.AreEqual(pointId, res.PickupPoint.ExternalPickupPointId);
            Assert.AreEqual("DeliveryVariantDescription", res.DeliveryVariantDescription);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ShouldDenyPickUpTest()
        {
            var pointId = TestDataStore.PickupPointId;
            var address = TestDataStore.PickupPointAddress;
            var partner = TestDataStore.GetPartner();
            partner.Settings[PartnerSettingsExtension.GetDeliveryVariantsKey] = null;

            var partnersRepository = MockFactory.GetPartnersRepository(partner);

            var res = new PickupSpec(TestDataStore.GetDeliveryVariants(), partnersRepository.Object).BuildDeliveryInfo(TestDataStore.GetDeliveryDto(), TestDataStore.PartnerId);

            Assert.IsNotNull(res);
            Assert.IsNotNull(res.PickupPoint);
            Assert.AreEqual(address, res.PickupPoint.Address);
            Assert.AreEqual(pointId, res.PickupPoint.ExternalPickupPointId);
        }
    }
}