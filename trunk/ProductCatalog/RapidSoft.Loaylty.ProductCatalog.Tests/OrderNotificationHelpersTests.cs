using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidSoft.Extensions;
using RapidSoft.Loaylty.ProductCatalog.Entities;
using RapidSoft.Loaylty.ProductCatalog.OrdersNotifications.Helpers;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    [TestClass]
    public class OrderNotificationHelpersTests
    {
        [TestMethod]
        public void ShouldDeserializeDeliveryInfo()
        {
            var deliveryInfoObject = TestDataStore.GetDeliveryInfo();

            var deliveryInfo = XmlSerializer.Serialize(deliveryInfoObject);

            var orderNotification = new OrderNotification
            {
                DeliveryInfo = deliveryInfo
            };

            orderNotification.ExtractEmailAddress();

            Assert.IsNotNull(orderNotification.DeliveryInfoObject);
        }
    }
}
