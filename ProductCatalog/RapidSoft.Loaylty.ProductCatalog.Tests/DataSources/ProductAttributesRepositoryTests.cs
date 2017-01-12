namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Services;

    [TestClass]
    public class ProductAttributesRepositoryTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
        }

        [TestMethod]
        public void ShouldGetVendors()
        {
            var categoryId = new CategoriesSearcher().GetAllSubCategories(new GetAllSubCategoriesParameters()).Categories.First(x => x.ProductsCount > 0).Id;

            var repo = new ProductAttributeRepository();
            var attrs = repo.GetAll(ProductAttributes.Vendor, categoryId, ProductModerationStatuses.Applied);

            Assert.IsTrue(attrs.Length > 0);
        }

        [TestMethod]
        public void ShouldGetVendorsForRoot()
        {
            var repo = new ProductAttributeRepository();
            var attrs = repo.GetAll(ProductAttributes.Vendor, null, ProductModerationStatuses.Applied);

            Assert.IsTrue(attrs.Length > 0);
        }
    }
}
