namespace RapidSoft.Loaylty.IntegrationTests.ProductCatalog
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    [TestClass]
    public class CatalogAdminServiceTest
    {
        [TestMethod]
        public void ShouldGetAllPartnersTest()
        {
            using (var client = new CatalogAdminServiceClient())
            {
                var res = client.GetPartners(null, TestHelper.UserId);
                Assert.IsNotNull(res);
            }
        } 
    }
}