using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.IntegrationTests.ProductCatalog
{
    using System.Collections.Generic;

    using Loaylty.ProductCatalog.WsClients.BasketService;

    [TestClass]
    public class BasketListTest
    {
        private Dictionary<string, string> clientContext = TestHelper.GetClientContext();

        [TestMethod]
        public void CanPingBasketListTest()
        {
            using (var client = new BasketServiceClient())
            {
                var productId = "1_1029146";
                var userId = "123";
                var res = client.Add(userId, productId, 1, clientContext);

                Assert.IsNotNull(res);

                var bas = client.GetBasket(new GetBasketParameters()
                {
                    ClientContext = clientContext,
                    CalcTotalCount = true,
                    ClientId = userId
                });

                Assert.IsNotNull(bas);
            }
        } 
    }
}