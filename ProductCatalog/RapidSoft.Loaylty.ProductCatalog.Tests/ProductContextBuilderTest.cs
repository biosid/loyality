using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using API.Entities;

    using ProductCatalog.DataSources;

    [TestClass]
    public class ProductContextBuilderTest
    {
        [TestMethod]
        public void ShouldCreateProductContextTest()
        {
            var existsProd = TestHelper.BuildTestProduct();

            var res = new ClientContextBuilder<Product>(ProductsDataSource.ProductColumns).GetProductContext(existsProd, "p.");

            Assert.IsNotNull(res);
            Assert.AreEqual(existsProd.PartnerId.ToString(), res["p.PartnerId"]);
            Assert.AreEqual(existsProd.CategoryId.ToString(), res["p.CategoryId"]);
            Assert.AreEqual(existsProd.ProductId.ToString(), res["p.ProductId"]);
        }
    }
}