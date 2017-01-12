using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using API.Entities;

    [TestClass]
    public class ProductParamComparerTest
    {
        [TestMethod]
        public void ShouldNotFailWhenUnitIsNullTest()
        {
            Assert.IsTrue(new ProductParamComparer().Equals(new ProductParam(), new ProductParam()));
        }

        [TestMethod]
        public void ShouldTrueWhenUnitIsSameTest()
        {
            var testUnit = "test";
            
            Assert.IsTrue(new ProductParamComparer().Equals(new ProductParam()
            {
                Unit = testUnit
            }, new ProductParam()
            {
                Unit = testUnit
            }));
        }
    }
}