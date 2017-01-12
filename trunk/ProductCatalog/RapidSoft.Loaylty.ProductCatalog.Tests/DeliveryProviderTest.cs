using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using API.Entities;
    using API.InputParameters;

    using ProductCatalog.Services;

    [TestClass]
    public class DeliveryProviderTest
    {
        //[TestMethod]
        //public void ShouldCalcDeliveryPriceTest()
        //{
        //    var partnerConnectorProvider = MockFactory.GetPartnerConnectorProvider();
        //    var res = new DeliveryProvider(partnerConnectorProvider.Object).GetDeliveryVariants(TestDataStore.PartnerID, new Location(), new OrderItem(), TestDataStore.TestClientId, DeliveryTypes.Delivery, "123");
        //    Assert.IsNotNull(res);
        //    Assert.AreEqual(100, res.PriceDeliveryRur);
        //    Assert.AreEqual(200, res.PriceDelivery);
        //} 
    }
}