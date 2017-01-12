using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Loaylty.ProductCatalog.DataSources;
using RapidSoft.Loaylty.ProductCatalog.Entities;
using RapidSoft.Loaylty.ProductCatalog.Tests.DataSources;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using ProductCatalog.DataSources.Repositories;

    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;

    [TestClass]
    [Ignore]
    public class ProductViewStatisticRepositoryTest
    {
        private readonly IProductViewStatisticRepository _productViewStatisticDataSource = new ProductViewStatisticRepository();
        private string clientId = TestDataStore.TestClientId;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.Database.ExecuteSqlCommand(string.Format(
                    "DELETE FROM [prod].[ProductViewStatistics]"));
                ctx.SaveChanges();
            }   
        }

        [TestMethod]
        public void CanRegisterStatTest()
        {
            var testProductId = TestDataStore.ProductID;

            _productViewStatisticDataSource.RegisterProductView(clientId, testProductId);

            var statistic = _productViewStatisticDataSource.GetProductViewStatistic(clientId, testProductId);
            Assert.IsNotNull(statistic);
            Assert.AreEqual(clientId, statistic.ClientId);
            Assert.AreEqual(testProductId, statistic.ProductId);
            Assert.AreEqual(1, statistic.ViewCount);
        }

        [TestMethod]
        public void CanIncrementVewCountTest()
        {
            var testProductId = TestDataStore.ProductID;
            var clientId = this.clientId + "_2";

            _productViewStatisticDataSource.RegisterProductView(clientId, testProductId);
            _productViewStatisticDataSource.RegisterProductView(clientId, testProductId);

            var statistic = _productViewStatisticDataSource.GetProductViewStatistic(clientId, testProductId);
            Assert.IsNotNull(statistic);
            Assert.AreEqual(clientId, statistic.ClientId);
            Assert.AreEqual(testProductId, statistic.ProductId);
            Assert.AreEqual(2, statistic.ViewCount);
        }
    }
}