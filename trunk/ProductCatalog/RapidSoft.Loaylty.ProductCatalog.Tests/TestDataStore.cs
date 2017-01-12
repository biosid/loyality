using RapidSoft.Loaylty.ProductCatalog.Services.Delivery;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;
    using System.Collections.Generic;

    using API.Entities;
    using API.InputParameters;

    using Common;

    using Entities;

    using PartnersConnector.WsClients.PartnersOrderManagementService;

    using ProductCatalog.Services;

    using PromoAction.WsClients.MechanicsService;

    using Contact = API.Entities.Contact;
    using CreateOrderFromOnlinePartnerItem = API.InputParameters.CreateOrderFromOnlinePartnerItem;
    using DeliveryAddress = API.Entities.DeliveryAddress;
    using DeliveryGroup = API.Entities.DeliveryGroup;
    using DeliveryInfo = API.Entities.DeliveryInfo;
    using DeliveryVariant = API.Entities.DeliveryVariant;
    using Location = API.InputParameters.Location;
    using Order = API.Entities.Order;
    using OrderItem = API.Entities.OrderItem;
    using PartnerType = API.Entities.PartnerType;
    using PhoneNumber = API.Entities.PhoneNumber;
    using PickupPoint = API.InputParameters.PickupPoint;
    using Product = API.Entities.Product;

    public class TestDataStore
    {
        public const int OzonPartnerId = 1;
        public const int BiletixPartnerID = 5;
        public static readonly string PartnerProductID = "145833";
        public static readonly string ProductID = OzonPartnerId + "_" + PartnerProductID;
        public const string KladrCode = "7700000000000";
        public const int TestCategoryId = 1;
        public const string TestCategoryName = "TestCat";
        public const string TestPartnerCategoryID = "1166784";
        public const string TestParentPartnerCategoryID = "1165495";
        public const int PartnerId = 1;
        public const int QueuedCommitPartnerId = 2;
        public const int NoDeliveryRatesPartnerID = 3;
        public const int DeactivatedPartnerID = 4;

        public const string TestUserId = "vtbSystemUser";
        public const string SecondTestUserId = "unitTestsUser";
        public const string TestClientId = "vtb_test";
        public const string PickupPointId = "001";
        public const string ExternalDeliveryVariantId = "001";
        public const string PickupPointVariantId = "002";
        public const string PickupPointAddress = "г. Москва, адресс точки самовывоза";
        
        public static Product GetProduct()
        {
            return new Product()
            {
                PartnerId = OzonPartnerId,
                ProductId = ProductID,
                PartnerProductId = PartnerProductID,
                PartnerCategoryId = TestPartnerCategoryID,
                CategoryId = TestCategoryId,
                Description = "test product description",
                Name = "Тестовый товар на базе (Мистические повести (подарочное издание))",
                PriceRUR = 12662
            };
        }

        public static Product GetOtherProduct()
        {
            return new Product()
            {
                PartnerId = OzonPartnerId,
                ProductId = "1_1090445",
                PartnerProductId = PartnerProductID,
                PartnerCategoryId = TestPartnerCategoryID,
                CategoryId = TestCategoryId,
                Description = "other test product description",
                Name = "Тестовый товар на базе (Мода и стиль)",
                PriceRUR = 99
            };
        }

        public static PartnerProductCategory GetParentPartnerProductCategory()
        {
            return new PartnerProductCategory()
            {
                Id = TestPartnerCategoryID,
                Name = "Бытовая техника",
                NamePath = @"\Бытовая техника\"
            };
        }

        public static PartnerProductCategory GetPartnerProductCategory()
        {
            return new PartnerProductCategory()
            {
                Id = TestPartnerCategoryID,
                Name = "Утюги",
                NamePath = @"\Бытовая техника\Утюги\",
                ParentId = TestPartnerCategoryID
            };
        }

        public static DeliveryInfo GetDeliveryInfo(string kladrCode = null)
        {
            return new DeliveryInfo
                   {
                       DeliveryVariantName = "Доставка до двери",
                       DeliveryType = API.Entities.DeliveryTypes.Delivery, 
                       DeliveryVariantsLocation = new API.Entities.VariantsLocation()
                           {
                               ExternalLocationId = "ExternalLocationId",
                               KladrCode = kladrCode,
                               LocationName = "LocationName",
                               PostCode = "123456"
                           },
                       Address = new DeliveryAddress()
                       {
                           PostCode = "123123",
                           StreetTitle = "ул. Островитянова",
                           Flat = "23",
                           House = "4"   
                       },
                       DeliveryDate = DateTime.Now.AddDays(7),
                       DeliveryTimeFrom = DateTime.Now.TimeOfDay,
                       DeliveryTimeTo = DateTime.Now.TimeOfDay,
                       Comment = "без опозданий",
                       AddText = "Поздравляю",
                       Contact = new Contact
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
                            }
                                  
                   };
        }
        
        public static DeliveryInfo GetPickUpDeliveryInfo()
        {
            return new DeliveryInfo
            {
                DeliveryVariantName = "Самовывоз",
                DeliveryType = API.Entities.DeliveryTypes.Pickup, 
                PickupPoint = new API.Entities.PickupPoint()
                    {
                        Address = PickupPointAddress,
                        Description = "Point Description",
                        ExternalDeliveryVariantId = PickupPointVariantId,
                        ExternalPickupPointId = PickupPointId,
                        Name = "ПВЗ 13",
                        OperatingHours = new string[]
                            {
                                "10:00",
                                "20:00"
                            },
                       Phones = new []
                           {
                               "79101234567"
                           }
                    },
                DeliveryDate = DateTime.Now.AddDays(7),
                DeliveryTimeFrom = DateTime.Now.TimeOfDay,
                DeliveryTimeTo = DateTime.Now.TimeOfDay,
                Comment = "без опозданий",
                AddText = "Поздравляю",
                Contact = new Contact
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
                }

            };
        }
        
        public static DeliveryDto GetDeliveryDto(string kladrCode = null, string externalDeliveryVariantId = ExternalDeliveryVariantId)
        {
            var addressKladrCode = kladrCode ?? KladrCode;
            return new DeliveryDto
            {
                ExternalDeliveryVariantId = externalDeliveryVariantId,
                DeliveryVariantLocation = new Location()
                {
                    KladrCode = addressKladrCode,
                    PostCode = "123456"
                },
                Address = new DeliveryAddress()
                {
                    PostCode = "123123",
                    //AddressKladrCode = addressKladrCode,
                    StreetTitle = "ул. Островитянова",
                    Flat = "23",
                    House = "4"
                },
                Comment = "без опозданий",
                Contact = new Contact
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
                },
                PickupPoint = new PickupPoint()
                {
                    ExternalPickupPointId = PickupPointId
                }
            };
        }

        public static Order GetOrder()
        {
            var order = new Order
                        {
                            PartnerId = PartnerId,
                            DeliveryInfo = GetDeliveryInfo(),
                            BonusDeliveryCost = 100,
                            DeliveryCost = 10,
                            TotalCost = 11,
                            BonusTotalCost = 110
                        };

            order.ClientId = TestClientId;
            order.UpdatedUserId = TestUserId;
            order.Items = GetOrderItems();

            return order;
        }

        public static OrderItem[] GetOrderItems()
        {
            return new[]
            {
                GetOrderItem()
            };
        }

        public static OrderItem GetOrderItem()
        {
            return new OrderItem()
            {
                Product = GetProduct()
            };
        }

        public static Dictionary<string, string> GetClientContext()
        {
            return new Dictionary<string, string>
            {
                {
                    ClientContextParser.ClientIdKey, TestClientId
                },
                {
                    ClientContextParser.LocationKladrCodeKey,
                    KladrCode
                }
            };
        }

        public static CreateOrderFromOnlinePartnerItem[] GetCreateOrderFromOnlinePartnerItems()
        {
            return new[]
            {
                new CreateOrderFromOnlinePartnerItem()
                {
                    Amount = 1,
                    Price = 100.30m,
                    BonusPrice = 334
                }
            };
        }

        public static Partner GetOZONPartner()
        {
            return new Partner
                       {
                           Id = OzonPartnerId,
                           Type = PartnerType.Offline,
                           Settings =
                               new Dictionary<string, string>
                                   {
                                       {
                                           PartnerSettingsExtension.CheckOrderUrlKey,
                                           "некий url"
                                       }
                                   }
                       };
        }

        public static Product[] GetProducts()
        {
            return new[]
            {
                GetProduct(),
                GetOtherProduct()
            };
        }

        public static GenerateResult GetSqlPrice()
        {
            var serviceClientMock = MockFactory.GetMechanicsClientProviderMock();
            var mechanicsProvider = new MechanicsProvider(serviceClientMock.Object);
            Dictionary<string, string> clientContext = TestDataStore.GetClientContext();
            return mechanicsProvider.GetPriceSql(clientContext);            
        }

        public static PartnerDeliveryVariants GetDeliveryVariants(string pointId = null, string address = null)
        {
            return new PartnerDeliveryVariants()
            {
                Variants = new GetDeliveryVariantsResult()
                {
                    Location = new PartnersConnector.WsClients.PartnersOrderManagementService.VariantsLocation()
                    {
                        ExternalLocationId = "ExternalLocationId",
                        LocationName = "LocationName",
                        PostCode = "PostCode"
                    },
                    DeliveryGroups = new[]
                    {
                        new PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryGroup()
                        {
                            DeliveryVariants = new[]
                            {
                                new PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryVariant()
                                {
                                    ExternalDeliveryVariantId = ExternalDeliveryVariantId,
                                    Description = "DeliveryVariantDescription",
                                    PickupPoints = new[]
                                    {
                                        new PartnersConnector.WsClients.PartnersOrderManagementService.PickupPoint()
                                        {
                                            ExternalDeliveryVariantId = ExternalDeliveryVariantId,
                                            ExternalPickupPointId = pointId ?? PickupPointId,
                                            Address = address ?? PickupPointAddress,
                                            Description = "PickupPointDescription"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public static Partner GetPartner()
        {
            return new Partner()
            {
                Settings = new Dictionary<string, string>()
                {
                    { PartnerSettingsExtension.GetDeliveryVariantsKey, "https://ogrishchenko-w7.rapidsoft.local:643/Actions/GetDeliveryVariants" }
                }
            };
        }
    }
}