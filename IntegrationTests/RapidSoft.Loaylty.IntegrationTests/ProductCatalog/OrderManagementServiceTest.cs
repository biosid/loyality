namespace RapidSoft.Loaylty.IntegrationTests.ProductCatalog
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Extensions;

    using Loaylty.ProductCatalog.WsClients.OrderManagementService;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OrderManagementServiceTest
    {
        private readonly string clientId = "intUser";

        private readonly Dictionary<string, string> clientContext = TestHelper.GetClientContext();

        [TestMethod]
        public void ShouldGetDeliveryVariantsTest()
        {
            var productId = TestUtils.FindProduct(TestHelper.OzonPartnerId).ProductId;
            var basketItem = TestUtils.AddBasketItem(productId);

            using (var client = new OrderManagementServiceClient())
            {
                var res = client.GetDeliveryVariants(new GetDeliveryVariantsParameters()
                {
                    Location = new Location()
                    {
                        KladrCode = TestHelper.Kladr,
                        PostCode = TestHelper.PostCode
                    },
                    BasketItems = new Guid[] { basketItem.Item.Id },
                    ClientContext = clientContext,
                    ClientId = clientId
                });

                Assert.IsNotNull(res);
                Trace.Write(res.Serialize());
                Assert.IsTrue(res.Success, res.ResultDescription);
            }
        }
    }
}