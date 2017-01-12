using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AutoMapper;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
using RapidSoft.Loaylty.PartnersConnector.Services;

namespace RapidSoft.Loaylty.PartnersConnector.Tests
{
    using Common.Services;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class PartnerCommunicationTests
    {
        private static OrderManagementService c;

        static PartnerCommunicationTests()
        {
            c = new OrderManagementService();
        }

        [TestMethod]
        public void ShouldMapContactToDTOContact()
        {
            var contact = new Contact
                              {
                                  Email = "Email@Email.com",
                                  FirstName = "FirstName",
                                  LastName = "LastName",
                                  MiddleName = "MiddleName",
                                  Phone = 999881234567890
                              };
            var dtoContact = Mapper.Map<Contact, Common.DTO.CommitOrder.Contact>(contact);

            Assert.IsNotNull(dtoContact);
            Assert.AreEqual(dtoContact.Email, "Email@Email.com");
            Assert.AreEqual(dtoContact.FirstName, "FirstName");
            Assert.AreEqual(dtoContact.LastName, "LastName");
            Assert.AreEqual(dtoContact.MiddleName, "MiddleName");
            Assert.AreEqual(dtoContact.PhoneNumber, 999881234567890);
        }

        [TestMethod]
        public void ShouldMapDeliveryInfoToDTODeliveryInfo()
        {
            var now = DateTime.Now;
            var deliveryInfo = TestHelper.GetDeliveryInfo(now);

            var dtoDeliveryInfo = Mapper.Map<DeliveryInfo, Common.DTO.CheckOrder.DeliveryInfo>(deliveryInfo);

            Assert.IsNotNull(dtoDeliveryInfo);
            Assert.AreEqual("г. Москва, тестовый адрес", dtoDeliveryInfo.Address);
            Assert.AreEqual("Comment", dtoDeliveryInfo.Comment);
            Assert.AreEqual("197755", dtoDeliveryInfo.PostCode);
        }

        [TestMethod]
        public void ShouldMapDeliveryInfoAndRemoveLinkTagsFromAddressText()
        {
            var now = DateTime.Now;
            var deliveryInfo = TestHelper.GetDeliveryInfo(now);

            deliveryInfo.Address.AddressText = null;
            deliveryInfo.PickupPoint.Address = "г. Москва, <a href=\"testurl\">тестовый адрес</a>";

            var dtoDeliveryInfo = Mapper.Map<DeliveryInfo, Common.DTO.CheckOrder.DeliveryInfo>(deliveryInfo);

            Assert.IsNotNull(dtoDeliveryInfo);
            Assert.AreEqual("г. Москва, тестовый адрес", dtoDeliveryInfo.Address);
            Assert.AreEqual("Comment", dtoDeliveryInfo.Comment);
            Assert.AreEqual("197755", dtoDeliveryInfo.PostCode);
        }


        [TestMethod]
        public void ShouldMapOrderItemToDTOOrderItem()
        {
            var orderItem = new OrderItem
                                {
                                    Amount = 1,
                                    BasketItemId = "2",
                                    OfferId = "OfferId",
                                    OfferName = "OfferName",
                                    Price = 5.5m,
                                    //Product = new Product
                                    //              {
                                    //                  Adult = "Adult",
                                    //                  Artist = "Artist",
                                    //                  PartnerId = -777,
                                    //                  // ...
                                    //              },
                                    Weight = 111,
                                };
            var dtoOrderItem = Mapper.Map<OrderItem, Common.DTO.CheckOrder.OrderItem>(orderItem);
            Assert.IsNotNull(dtoOrderItem);
            Assert.AreEqual(dtoOrderItem.Amount, 1);
            Assert.AreEqual(dtoOrderItem.OfferId, "OfferId");
            Assert.AreEqual(dtoOrderItem.OfferName, "OfferName");
            Assert.AreEqual(dtoOrderItem.Price, 5.5m);
            Assert.AreEqual(dtoOrderItem.Weight, 111);
        }

        [TestMethod]
        public void ShouldMapOrderToDTOOrder()
        {
            var now = DateTime.Now;

            var deliveryInfo = TestHelper.GetDeliveryInfo();
            
            var orderItem = new OrderItem
                                {
                                    Amount = 1,
                                    BasketItemId = "2",
                                    OfferId = "OfferId",
                                    OfferName = "OfferName",
                                    Price = 5.5m,
                                    Weight = 111,
                                };

            var order = new Order
                              {
                                  BonusDeliveryCost = 5,
                                  BonusItemsCost = 6,
                                  BonusTotalCost = 7,
                                  DeliveryCost = 5.67m,
                                  DeliveryInfo = deliveryInfo,
                                  Id = -555,
                                  InsertedDate = DateTime.Now,
                                  InsertedUserId = "I",
                                  Items = new[] { orderItem },
                                  ItemsCost = 8.3m,
                                  Status = OrderStatuses.Processing,
                                  TotalCost = 346.5664m,
                                  TotalWeight = 2345,
                                  UpdatedUserId = "You"
                              };

            var dtoOrder = Mapper.Map<Order, Common.DTO.CheckOrder.Order>(order);
            Assert.IsNotNull(dtoOrder);
            Assert.AreEqual(dtoOrder.DeliveryCost, 5.67m);
            Assert.AreEqual(dtoOrder.Items.First().OfferName, "OfferName");
            Assert.AreEqual(dtoOrder.ItemsCost, 8.3m);
            Assert.AreEqual(dtoOrder.OrderId, "-555");
            Assert.AreEqual(dtoOrder.TotalCost, 346.5664m);
            Assert.AreEqual(dtoOrder.TotalWeight, 2345);
        }
    }
}
