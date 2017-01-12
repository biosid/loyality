using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidSoft.Loaylty.ProductCatalog.OrdersNotifications.Helpers;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    [TestClass]
    public class DeliveryInfoHelperTests
    {
        [TestMethod]
        public void ShouldExtractDeliveryAddress()
        {
            var deliveryInfo = TestDataStore.GetDeliveryInfo();

            var actual = deliveryInfo.ExtractDeliveryAddress();

            Assert.AreEqual("Доставка до двери. 123123, ул. Островитянова, 4, кв. 23", actual);
        }

        [TestMethod]
        public void ShouldExtractPickUpAddress()
        {
            var deliveryInfo = TestDataStore.GetPickUpDeliveryInfo();

            var actual = deliveryInfo.ExtractDeliveryAddress();

            Assert.AreEqual("Самовывоз. г. Москва, адресс точки самовывоза", actual);
        }

        [TestMethod]
        public void ShouldExtractEmailAddress()
        {
            var deliveryInfo = TestDataStore.GetDeliveryInfo();

            var actual = deliveryInfo.ExtractEmailAddress();

            Assert.AreEqual("a@a.a", actual);
        }
    }
}
