namespace RapidSoft.Loaylty.PartnersConnector.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;

    using Contact = Interfaces.Entities.Contact;
    using DeliveryGroup = Common.DTO.GetDeliveryVariants.DeliveryGroup;
    using DeliveryInfo = Interfaces.Entities.DeliveryInfo;
    using DeliveryVariant = Common.DTO.GetDeliveryVariants.DeliveryVariant;
    using GetDeliveryVariantsResult = Common.DTO.GetDeliveryVariants.GetDeliveryVariantsResult;
    using Order = Interfaces.Entities.Order;
    using OrderItem = Interfaces.Entities.OrderItem;
    using PickupPoint = Common.DTO.GetDeliveryVariants.PickupPoint;
    using VariantsLocation = Common.DTO.GetDeliveryVariants.VariantsLocation;

    public static class TestHelper
    {
        public static int FirstTestPartnerId = 1;
        public static int SecondTestPartnerId = 2;

        public const int TestPartnerID = 1;
        public const string AddressText = "г. Москва, тестовый адрес";

        public static GetDeliveryVariantsResult GetEmptyDeliveryVariantsResult()
        {
            return new GetDeliveryVariantsResult()
            {
                ResultCode = 0,
                Location = new VariantsLocation()
                {
                    LocationName = "г. Москва",
                    PostCode = "123456"
                }
            };
        }

        public static GetDeliveryVariantsResult GetDeliveryVariantsResult()
        {
            return new GetDeliveryVariantsResult()
            {
                ResultCode = 0,
                Location = new VariantsLocation()
                {                   
                    LocationName = "г. Москва",
                    PostCode = "197755"
                },
                DeliveryGroups = new[]
                {
                    new DeliveryGroup()
                    {
                        GroupName = "Курьерская доставка",
                        DeliveryVariants = new[]
                        {
                            new DeliveryVariant()
                            {
                                DeliveryVariantName = "DeliveryVariantName",
                                ExternalDeliveryVariantId = "ExternalDeliveryVariantId",
                                ItemsCost = 1,
                                DeliveryCost = 2,
                                TotalCost = 3,
                                PickupPoints = new[]
                                {
                                    new PickupPoint()
                                    {
                                        Name = "Name",
                                        Address = "Address",
                                        ExternalPickupPointId = "ExternalPickupPointId",
                                        DeliveryVariantName = "DeliveryVariantName",
                                        ExternalDeliveryVariantId = "ExternalDeliveryVariantId",
                                        ItemsCost = 1,
                                        DeliveryCost = 2,
                                        TotalCost = 3,
                                        Phones = new[]
                                        {
                                            "81234567891"
                                        },
                                        OperatingHours = new[]
                                        {
                                            "10:00"
                                        }
                                    },
                                    null
                                }
                            }
                        }
                    }
                }
            };
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

        public static Dictionary<string, string> GetSettings(bool useBatch = true)
        {
            return new Dictionary<string, string>
                       {
                           { "FixBasketItemPrice", "https://ows.ozon.ru/VTB24TestAPI/api/FixPrice" },
                           { "GetDeliveryVariants", "https://ows.ozon.ru/VTB24TestAPI/api/GetDeliveryVariants"},
                           { "Check", "https://ows.ozon.ru/VTB24TestAPI/api/CheckOrder" },
                           { "UseBatch", useBatch.ToString() },
                           { "Confirmation", "https://ows.ozon.ru/VTB24TestAPI/api/ConfirmOrder" },
                           { "BatchConfirmation", "https://ows.ozon.ru/VTB24TestAPI/api/BatchConfirmOrder" },
                           { "CertificateThumbprint", "‎bf c1 29 a8 5c 0f 14 c9 38 45 a8 94 46 5c a3 05 c3 58 d7 e2" },
                           { "PublicKey", @"-----BEGIN PUBLIC KEY-----
      MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC8jnuyFyf0J1mlTm0b1KwHbSyG
      1D7vDC1ipl9YxuFcXIsg/wc4Kwv/kH7GvlvK2zxwwEJzsv2HHbWPr/EkJjmjsQ87
      ZDhzIyFyKTr/2z12nWBIY0s/xuKodke/yyT7PqjmeKyhWb3M/QqK7U5QtSDmylmU
      KyPjDLh/xQsztqCv5wIDAQAB
      -----END PUBLIC KEY-----" },
                       };
        }

        public static int GetJobCount(string jobGroup)
        {
            const string SQL = "SELECT COUNT(*) FROM [dbo].[QRTZ_JOB_DETAILS] WHERE [JOB_GROUP] = '{0}'";

            var props = (NameValueCollection)ConfigurationManager.GetSection("quartz");
            var connectionStr = props["quartz.dataSource.Quartz.connectionString"];

            using (var conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = string.Format(SQL, jobGroup);

                    var retVal = Convert.ToInt32(comm.ExecuteScalar());
                    return retVal;
                }
            }
        }

        public static long GetTestPhoneNumber()
        {
            return 999881234567890;
        }

        public static Order GetOrder(int partnerId, int orderId = -555)
        {
            var orderItem = GetOrderItem();

            var order = new Order
                            {
                                BonusDeliveryCost = 5,
                                BonusItemsCost = 6,
                                BonusTotalCost = 7,
                                DeliveryCost = 300,
                                DeliveryInfo = TestHelper.GetDeliveryInfo(),
                                Id = orderId,
                                ClientId = "67",
                                InsertedDate = DateTime.Now,
                                InsertedUserId = "I",
                                Items = new[] { orderItem },
                                ItemsCost = orderItem.Price * orderItem.Amount,                                
                                Status = OrderStatuses.Processing,
                                TotalCost = (orderItem.Price * orderItem.Amount) + 300,
                                TotalWeight = orderItem.Weight * orderItem.Amount,
                                UpdatedUserId = "You",
                                PartnerId = partnerId                                
                            };
            return order;
        }

        public static DeliveryInfo GetDeliveryInfo(DateTime? nowDate = null)
        {
            nowDate = nowDate ?? DateTime.MinValue;

            return new DeliveryInfo
            {
                DeliveryVariantsLocation = new Interfaces.Entities.VariantsLocation()
                {
                    ExternalLocationId = "ExternalLocationId"
                },
                ExternalDeliveryVariantId = "ExternalDeliveryVariantId",
                DeliveryVariantName = "DeliveryVariantName",
                Address = new DeliveryAddress()
                {
                    PostCode = "197755",
                    StreetTitle = "Street",
                    AddressText = AddressText,
                },
                PickupPoint = new Interfaces.Entities.PickupPoint()
                {
                    ExternalPickupPointId = "ExternalPickupPointId"
                },
                AddText = "AddText",
                Comment = "Comment",
                Contact = GetTestContact()
            };
        }

        public static OrderItem GetOrderItem()
        {
            return new OrderItem
            {
                Amount = 1,
                BasketItemId = "2",
                OfferId = "12837910",
                OfferName = "BBC: Акулы (4 DVD)",
                Price = 709.123456m,
                Weight = 111,
            };
        }

        public static Common.DTO.CheckOrder.Order GetTestDTOOrder(int orderId)
        {
            var contact = new Common.DTO.CommitOrder.Contact
                              {
                                  Email = "ri@ru.ru",
                                  FirstName = "FirstName",
                                  LastName = "LastName",
                                  MiddleName = "MiddleName",
                                  PhoneNumber = 79995555555
                              };

            var deliveryInfo = new Common.DTO.CheckOrder.DeliveryInfo
                                   {
                                       Address = "Address",
                                       Comment = "Comment",
                                       CountryCode = "RU",
                                       CountryName = "Раша",
                                       PostCode = "446200",
                                   };

            var item = new Common.DTO.CheckOrder.OrderItem
                           {
                               Amount = 1,
                               OfferId = "utih",
                               OfferName = "Утюх",
                               Price = 500.55m,
                               Weight = 5000,
                           };

            var order = new Common.DTO.CheckOrder.Order
                            {
                                DeliveryCost = 100m,
                                DeliveryInfo = deliveryInfo,
                                Items = new[] { item },
                                ItemsCost = 750m,
                                OrderId = orderId.ToString(CultureInfo.InvariantCulture),
                                TotalCost = 5000m,
                                TotalWeight = 7000,
                            };

            return order;
        }
    }
}
