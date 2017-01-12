using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Loaylty.ProductCatalog.Import;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    [TestClass]
    public class PermissionsCheckerTest
    {
        [TestMethod]
        public void CanGetPublicProductStatusTest()
        {
            var partnerPermissions = new[]
            {
                "PERM_PRODCAT_NEWPROD_TRUST",
                "PERM_PRODCAT_REDIRCAT_TRUST",
                "PERM_PRODCAT_UPDPRODMISC_TRUST",
                "PERM_PRODCAT_UPDPRODNAME_TRUST",
                "PERM_PRODCAT_UPDPRODPRICE_TRUST",
                "PERM_PRODCAT_MOVEPROD_TRUST"

            };
            var status = new PermissionsChecker(partnerPermissions).GetImportGroupStatus(ImportGroupTypes.NewProductNewCategory);
        }
    }
}