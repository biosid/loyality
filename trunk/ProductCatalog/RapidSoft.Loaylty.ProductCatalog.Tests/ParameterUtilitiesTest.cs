using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Loaylty.ProductCatalog.Services;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    [TestClass]
    public class ParameterUtilitiesTest
    {
        [TestMethod]
        public void CanNormalizeHugeTakeCountTest()
        {
            Assert.AreEqual(50, ParameterUtilities.NormalizeByHeight(100, 50));
        }

        [TestMethod]
        public void CanNormalizeNegativeTakeCountTest()
        {
            Assert.AreEqual(50, ParameterUtilities.NormalizeByHeight(-100, 50));
        }

        [TestMethod]
        public void CanNormalizeNullTakeCountTest()
        {
            Assert.AreEqual(50, ParameterUtilities.NormalizeByHeight(null, 50));
        } 
    }
}