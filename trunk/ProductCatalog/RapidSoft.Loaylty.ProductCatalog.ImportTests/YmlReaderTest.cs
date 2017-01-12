using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System.Linq;

    using YML;

    [TestClass]
    public class YmlReaderTest
    {
        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\div_soft.xml", @"TestCatalogXmls")]
        public void CanParseTestCatalogTest()
        {
            var fileName = @"TestCatalogXmls\div_soft.xml";

            var reader = new YmlReader(fileName);

            var offersCount = 0;
            var categoriesCount = 0;
            foreach (var offer in reader.Offers)
            {
                offersCount++;
                Assert.IsNotNull(offer);
            }

            foreach (var category in reader.Categories)
            {
                categoriesCount++;
                Assert.IsNotNull(category);
            }
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\div_soft_with_weight.xml", @"TestCatalogXmls")]
        public void ShouldReadWeight()
        {
            var fileName = @"TestCatalogXmls\div_soft_with_weight.xml";

            var reader = new YmlReader(fileName);

            var offers = reader.Offers.ToArray();

            Assert.IsTrue(offers.Any(x => x.Id == "withWeight" && x.Weight.HasValue));
            Assert.IsTrue(offers.Any(x => x.Id == "withoutWeight" && !x.Weight.HasValue));
        }
    }

    public enum TestEn
    {
        A = 0,
        B = 1
    }
}