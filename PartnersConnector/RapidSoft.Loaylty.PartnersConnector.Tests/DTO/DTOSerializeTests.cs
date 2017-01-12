using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.DTO
{
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Common.DTO;
    using Common.DTO.NotifyOrderStatus;

    using RapidSoft.Extensions;

    [TestClass]
    public class DTOSerializeTests
    {
        [TestMethod]
        public void ShouldSerializeFixPriceMessage()
        {
            const decimal Price = 1255.03465346577m;
            var fixPriceMessage = new FixPriceMessage
                                                  {
                                                      Amount = 1,
                                                      ClientId = "client",
                                                      BasketItemId = Guid.NewGuid().ToString(),
                                                      OfferId = 5.ToString(CultureInfo.InvariantCulture),
                                                      OfferName = "Название предложения",
                                                      Price = Price
                                                  };

            var data = fixPriceMessage.Serialize(Encoding.UTF8);

            Assert.IsTrue(data.Contains("<Price>1255.03</Price>"));

            Console.WriteLine(data);

            var des = data.Deserialize<FixPriceMessage>(Encoding.UTF8);

            Assert.AreEqual(1255.03m, des.Price);
        }

        [TestMethod]
        public void ShouldSerializeCheckOrderMessage()
        {
            const decimal Price = 1255.03465346577m;
            const decimal DeliveryCost = 15.63465345634577m;
            const decimal ItemsCost = 44.43465345634577m;
            const decimal TotalCost = 56.34465345634577m;
            
            var orderItems = new[]
                                 {
                                     new Common.DTO.CheckOrder.OrderItem
                                         {
                                             Amount = 1,
                                             Price = Price,
                                             OfferId = 5.ToString(CultureInfo.InvariantCulture),
                                             OfferName = "Название предложения",
                                             Weight = 500
                                         }
                                 };

            var deliveryInfo = new Common.DTO.CheckOrder.DeliveryInfo
                                   {
                                       ExternalLocationId = "5678",
                                       PostCode = "446200",
                                       CountryCode = "RU",
                                       CountryName = "Россия",
                                       Address =
                                           "Самарская обл., г. Новокуйбышевск, ул.50-летия НПЗ, д.3, кв.10",
                                       Comment = "Комментарий"
                                   };

            var order = new Common.DTO.CheckOrder.Order
                            {
                                OrderId = 5.ToString(CultureInfo.InvariantCulture),
                                ClientId = Guid.NewGuid().ToString(),
                                DeliveryCost = DeliveryCost,
                                ItemsCost = ItemsCost,
                                TotalCost = TotalCost,
                                TotalWeight = 5005,
                                DeliveryInfo = deliveryInfo,
                                Items = orderItems
                            };

            var message = new Common.DTO.CheckOrder.CheckOrderMessage { Order = order };

            var data = message.Serialize(Encoding.UTF8);

            Assert.IsTrue(data.Contains("<Price>1255.03</Price>"));
            Assert.IsTrue(data.Contains("<DeliveryCost>15.63</DeliveryCost>"));
            Assert.IsTrue(data.Contains("<ItemsCost>44.43</ItemsCost>"));
            Assert.IsTrue(data.Contains("<TotalCost>56.34</TotalCost>"));

            Console.WriteLine(data);

            var des = data.Deserialize<Common.DTO.CheckOrder.CheckOrderMessage>(Encoding.UTF8);

            Assert.AreEqual(44.43m, des.Order.ItemsCost);
            Assert.AreEqual(56.34m, des.Order.TotalCost);
            Assert.AreEqual(15.63m, des.Order.DeliveryCost);
            Assert.AreEqual(1255.03m, des.Order.Items.First().Price);
        }

        [TestMethod]
        public void SerializeShouldCommitOrderMessage()
        {
            const decimal Price = 1255.03465346577m;
            const decimal DeliveryCost = 15.63465345634577m;
            const decimal ItemsCost = 44.43465345634577m;
            const decimal TotalCost = 56.34465345634577m;

            var orderItems = new[]
                                 {
                                     new Common.DTO.CommitOrder.OrderItem
                                         {
                                             Amount = 1,
                                             Price = Price,
                                             OfferId = 5.ToString(CultureInfo.InvariantCulture),
                                             OfferName = "Название предложения",
                                             Weight = 500,
                                         }
                                 };

            var contacts = new[]
                               {
                                   new Common.DTO.CommitOrder.Contact
                                       {
                                           Email = "Email@Email.ru",
                                           FirstName = "Стан",
                                           LastName = "Фед",
                                           MiddleName = "Юрич",
                                           PhoneNumber = 79270001122
                                       }
                               };
            var deliveryInfo = new Common.DTO.CommitOrder.DeliveryInfo
                                   {
                                       ExternalLocationId = "5678",
                                       PostCode = "446200",
                                       CountryCode = "RU",
                                       CountryName = "Россия",
                                       Address =
                                           "Самарская обл., г. Новокуйбышевск, ул.50-летия НПЗ, д.3, кв.10",
                                       Contacts = contacts,
                                       Comment = "Комментарий"
                                   };

            var order = new Common.DTO.CommitOrder.Order
                            {
                                OrderId = 5.ToString(CultureInfo.InvariantCulture),
                                ClientId = Guid.NewGuid().ToString(),
                                DeliveryCost = DeliveryCost,
                                ItemsCost = ItemsCost,
                                TotalCost = TotalCost,
                                TotalWeight = 5005,
                                DeliveryInfo = deliveryInfo,
                                Items = orderItems
                            };

            var message = new Common.DTO.CommitOrder.CommitOrderMessage { Order = order };

            var data = message.Serialize(Encoding.UTF8);

            Assert.IsTrue(data.Contains("<Price>1255.03</Price>"));
            Assert.IsTrue(data.Contains("<DeliveryCost>15.63</DeliveryCost>"));
            Assert.IsTrue(data.Contains("<ItemsCost>44.43</ItemsCost>"));
            Assert.IsTrue(data.Contains("<TotalCost>56.34</TotalCost>"));

            Console.WriteLine(data);

            var des = data.Deserialize<Common.DTO.CommitOrder.CommitOrderMessage>(Encoding.UTF8);

            Assert.AreEqual(44.43m, des.Order.ItemsCost);
            Assert.AreEqual(56.34m, des.Order.TotalCost);
            Assert.AreEqual(15.63m, des.Order.DeliveryCost);
            Assert.AreEqual(1255.03m, des.Order.Items.First().Price);
        }

        [TestMethod]
        public void ShouldSerializeNotifyOrdersResult()
        {
            var order1 = new NotifyOrdersResultOrder
                             {
                                 InternalOrderId = "InternalOrderId-1",
                                 OrderId = "OrderId-1",
                                 Reason = "Reason-1",
                                 ResultCode = 0
                             };
            var order2 = new NotifyOrdersResultOrder
                             {
                                 InternalOrderId = "InternalOrderId-2",
                                 OrderId = "OrderId-2",
                                 Reason = "Reason-2",
                                 ResultCode = 3
                             };
            var r = new NotifyOrdersResult { Orders = new[] { order1, order2 } };
            var serialized = r.Serialize(Encoding.UTF8, true);

            Console.WriteLine(serialized);
        }
    }
}
