namespace RapidSoft.Loaylty.IntegrationTests.PartnersConnector
{
    using Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PartnersConnectorOrderManagementServiceTests
    {
        [TestMethod]
        public void CheckOrder_ShouldCheckOrder()
        {
            var partnerId = TestHelper.SecondTestPartnerId;

            var order = TestHelper.GetTestOrder(partnerId);

            using (var client = new OrderManagementServiceClient())
            {
                var result = client.CheckOrder(order);
                Assert.IsTrue(result.Success);
                Assert.AreEqual(1, result.Checked, result.Reason);
            }            
        }
        
        [TestMethod]
        public void CommitOrder_ShouldCommit()
        {
            var partnerId = TestHelper.OzonPartnerId;

            var order = TestHelper.GetTestOrder(partnerId);

            using (var client = new OrderManagementServiceClient())
            {
                var result = client.CommitOrder(order);

                Assert.IsTrue(result.Success);
                Assert.AreEqual(ConfirmedStatuses.Committed, result.Confirmed);
                Assert.IsNotNull(result.InternalOrderId);
            }            
        }
        
        [TestMethod]
        public void ShouldFixBasketItemPriceOk()
        {
            var param = new FixBasketItemPriceParam()
            {
                PartnerId = TestHelper.OzonPartnerId, 
                ClientId = "testClientId", 
                BasketItemId = "testBasketItemId",
                OfferId = "5626237",
                OfferName = "Приключения Эраста Фандорина. Сочинения в 12 томах (комплект)",
                Price = 2414, 
                Amount = 1
            };

            using (var client = new OrderManagementServiceClient())
            {
                var result = client.FixBasketItemPrice(param);

                Assert.IsTrue(result.Success, result.ErrorMessage);
                Assert.IsTrue(result.Confirmed == 1, result.ReasonCode + result.Reason);
            } 
        }
        
        [TestMethod]
        public void ShouldGetDeliveryVariantsTest()
        {
            var partnerId = TestHelper.OzonPartnerId;

            var order = TestHelper.GetTestOrder(partnerId);

            var param = new GetDeliveryVariantsParam()
            {
                PartnerId = partnerId,
                ClientId = "123",
                Location = new Location()
                {
                    PostCode = TestHelper.PostCode
                },
                Items = order.Items
            };

            using (var client = new OrderManagementServiceClient())
            {
                var result = client.GetDeliveryVariants(param);

                Assert.IsTrue(result.Success, result.ResultDescription);
            }
        }
    }
}