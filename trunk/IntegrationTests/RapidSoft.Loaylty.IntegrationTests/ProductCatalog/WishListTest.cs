using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.IntegrationTests.ProductCatalog
{
    using System.Collections.Generic;

    using Loaylty.ProductCatalog.WsClients.WishListService;

    [TestClass]
    public class WishListTest
    {
        private Dictionary<string, string> clientContext = TestHelper.GetClientContext();

        [TestMethod]
        public void CanPingWishListTest()
        {
            using (var client = new WishListServiceClient())
            {
                var res = client.GetWishListItem("123", "123", clientContext);
                Assert.IsNotNull(res);
            }
        } 
    }
}