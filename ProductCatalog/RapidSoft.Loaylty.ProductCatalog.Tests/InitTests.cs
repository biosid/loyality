namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InitTests
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            //ProductCatalogDB.DropProductsInTestCategories();
            //ProductCatalogDB.DropTestCategories();
        }
    }
}