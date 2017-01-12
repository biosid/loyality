namespace RapidSoft.Loaylty.IntegrationTests
{
    using System;
    using System.Collections.Generic;

    using Loaylty.ProductCatalog.WsClients.OrderManagementService;

    using Contact = Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.Contact;
    using DeliveryAddress = Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryAddress;
    using DeliveryInfo = Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryInfo;
    using DeliveryTypes = Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryTypes;
    using Location = Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.Location;
    using Order = Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.Order;
    using OrderItem = Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.OrderItem;
    using OrderStatuses = Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.OrderStatuses;
    using PickupPoint = Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.PickupPoint;

    public static class TestHelper
    {
        public static int OzonPartnerId = 1;
        public static int SecondTestPartnerId = 3;
        public static string PostCode = "125252";
        public const string UserId = "vtbSystemUser";
        public const string VtbSystemUserId = "vtbSystemUser";
        public const string Kladr = "7700000000000";

        // public const string OzonTestProductId = "1_5121404";

        public static Order GetTestOrder(int partnerId, int orderId = -555)
        {
            var deliveryInfo = GetDeliveryInfoForDelivery();
            var orderItem = new OrderItem
                                {
                                    Amount = 1,
                                    BasketItemId = "2",
                                    OfferId = "5626237",
                                    OfferName = "OfferName",
                                    Price = 999999,
                                    Weight = 111,
                                };

            var deliveryCost = 1130;

            var order = new Order
                            {
                                BonusDeliveryCost = 5,
                                BonusItemsCost = 6,
                                BonusTotalCost = 7,
                                DeliveryCost = deliveryCost,
                                DeliveryInfo = deliveryInfo,
                                Id = orderId,
                                ClientId = "67",
                                InsertedDate = DateTime.Now,
                                InsertedUserId = "I",
                                Items = new[] { orderItem },
                                ItemsCost = orderItem.Price * orderItem.Amount,                                
                                Status = OrderStatuses.Processing,
                                TotalCost = (orderItem.Price * orderItem.Amount) + deliveryCost,
                                TotalWeight = orderItem.Weight * orderItem.Amount,
                                UpdatedUserId = "You",
                                PartnerId = partnerId                                
                            };
            return order;
        }

        public static Loaylty.ProductCatalog.WsClients.OrderManagementService.Contact GetContact()
        {
            return new Loaylty.ProductCatalog.WsClients.OrderManagementService.Contact
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                Phone = new PhoneNumber
                {
                    CountryCode = "7",
                    CityCode = "495",
                    LocalNumber = "4561278"
                },
                Email = "a@a.a"
            };
        }

        private static DeliveryInfo GetDeliveryInfoForDelivery()
        {
            var deliveryInfo = new DeliveryInfo
                                   {
                                       ExternalDeliveryVariantId = "55",
                                       Address = new DeliveryAddress()
                                       {
                                            TownTitle = "TownTitle",                                            
                                            PostCode = PostCode,
                                            StreetTitle = "Street",
                                            AddressText = "AddressText"
                                       },
                                       AddText = "доставка только в указанное время",
                                       Comment = "тестовый комментарий",
                                       Contact = GetTestContact()
                                   };
            return deliveryInfo;
        }

        private static Contact GetTestContact()
        {
            var contact = new Contact
                              {
                                  Email = "Email@Email.com",
                                  FirstName = "FirstName",
                                  LastName = "LastName",
                                  MiddleName = "MiddleName",
                                  Phone = GetTestPhoneNumber()
                              };
            return contact;
        }

        public static long GetTestPhoneNumber()
        {
            return 999881234567890;
        }

        public static Dictionary<string, string> GetClientContext()
        {
            return new Dictionary<string, string>
                                    {
                                        { "ClientProfile.KLADR", Kladr },
                                        { "ClientProfile.Audiences", null }
                                    };
        }

    }
}
