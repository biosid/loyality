namespace RapidSoft.Loaylty.IntegrationTests.ProductCatalog
{
    using System.Collections.Generic;

    using Loaylty.ProductCatalog.WsClients.CatalogSearcherService;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CatalogSearcherTest
    {
        private string userId = "integrationTest";
        private readonly Dictionary<string, string> clientContext = TestHelper.GetClientContext();

        [TestMethod]
        public void CanPingSearchProductServiceTest()
        {
            using (var client = new CatalogSearcherClient())
            {
                var res = client.SearchPublicProducts(new SearchPublicProductsParameters { CountToTake = 1 });
                Assert.IsNotNull(res);
            }
        }

        [TestMethod]
        public void ShouldGetPopularProductsTest()
        {
            using (var client = new CatalogSearcherClient())
            {
                var res = client.GetPopularProducts(userId, PopularProductTypes.MostOrdered, clientContext, null);
                Assert.IsNotNull(res);
            }
        }
    }
}