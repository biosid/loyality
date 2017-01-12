using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
using RapidSoft.Loaylty.ProductCatalog.DataSources;
using RapidSoft.Loaylty.ProductCatalog.Services;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.CatalogSearcher
{
    using ProductCatalog.DataSources.Interfaces;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [TestClass]
    public class ProductsSearcherTests
    {
        [TestMethod]
        public void ShouldGetAttributes()
        {
            var repo = new Mock<IProductAttributeRepository>();
            repo.Setup(r => r.GetAll(It.IsAny<ProductAttributes>(), It.IsAny<int>(), It.IsAny<ProductModerationStatuses?>())).Returns(new[] { "A", "B" });

            var searcher = new ProductsSearcher(null, repo.Object, null);
            var attrs = searcher.GetListOfAttributeValues(ProductAttributes.Vendor, 3);

            Assert.IsTrue(attrs.Length == 2);
        }
    }
}
