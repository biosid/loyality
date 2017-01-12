namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class CatalogImporterTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var mock = MockFactory.GetUserService();
            var service = mock.Object;
            ArmSecurity.UserServiceCreator = () => service;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ArmSecurity.UserServiceCreator = null;
        }

        [TestMethod]
        public void ShouldImportTestPrice()
        {
            var mechMock = MockFactory.GetMechanicsProvider();
            var productsSearcher = new ProductsSearcher(
                new ProductsDataSource(), new ProductAttributeRepository(), mechMock.Object);

            var searcher = new CatalogAdminService(
                new CategoriesSearcher(),
                productsSearcher,
                new ProductCategoriesDataSource(),
                new PartnerProductCateroryLinkRepository(),
                new CategoryPermissionRepository(),
                logEmailSender: new StubLogEmailSender());

            var parameters = new AdminSearchProductsParameters
                             {
                                 UserId = TestDataStore.TestUserId
                             };

            new ProductsDataSource().UpdateProductsFromAllPartners();
            
            var result = searcher.SearchProducts(parameters);
            Assert.IsTrue(result.Products.Length > 0);
        }
    }
}
