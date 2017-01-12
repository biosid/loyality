using RapidSoft.Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService;
using RapidSoft.Loaylty.ProductCatalog.Entities;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using API;
    using API.InputParameters;
    using API.OutputResults;

    using Moq;

    using ProductCatalog.DataSources;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Fake;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.PromoAction.WsClients.MechanicsService;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    using CheckOrderResult = RapidSoft.Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.CheckOrderResult;
    using CommitOrderResult = RapidSoft.Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.CommitOrderResult;
    using ConfirmedStatuses = RapidSoft.Loaylty.PartnersConnector.WsClients.PartnersOrderManagementService.ConfirmedStatuses;
    using DeliveryGroup = PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryGroup;
    using DeliveryVariant = PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryVariant;
    using PickupPoint = PartnersConnector.WsClients.PartnersOrderManagementService.PickupPoint;

    public class MockFactory
    {
        private static readonly string userId = TestDataStore.TestUserId;

        private const string PriceColumnAlias = "p.PriceRUR";
        private const decimal PromoFactor = 0.1m;

        private const decimal BaseMultiplicationFactor = 3.33m;
        
        private static readonly FakeMechanic fakeMech = new FakeMechanic
        {
            BaseAdd = "CASE WHEN p.PartnerId=1 THEN 1000.00000 ELSE 0 END",
            BaseMult = "1",
            Add = "CASE WHEN p.vendor='Издательский Дом &quot;Равновесие&quot;' THEN -500 ELSE 0.00000 END+CASE WHEN p.vendor='Новый Диск / ID Company' THEN -600 ELSE 0.00000 END",
            Mult = "1"
        };

        public static Mock<IImportTaskRepository> GetImportTaskRepository(int taskId)
        {
            var productImportTask = new ProductImportTask
                                    {
                                        CountFail = 5,
                                        PartnerId = 6,
                                        Id = taskId,
                                        CountSuccess = 9
                                    };
            var deliveryRateImportTask = new DeliveryRateImportTask
                                         {
                                             CountFail = 5,
                                             PartnerId = 6,
                                             Id = taskId.ToString(CultureInfo.InvariantCulture),
                                             CountSuccess = 9
                                         };
            var retProductImportTaskPage = new Page<ProductImportTask>(productImportTask.MakeArray());
            var retDeliveryRateImportTaskPage = new Page<DeliveryRateImportTask>(deliveryRateImportTask.MakeArray());

            var mock = new Mock<IImportTaskRepository>();
            mock.Setup(x => x.GetPageProductImportTask(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool?>())).Returns(retProductImportTaskPage);
            mock.Setup(x => x.GetPageDeliveryRateImportTask(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool?>())).Returns(retDeliveryRateImportTaskPage);
            mock.Setup(x => x.SaveProductImportTask(It.IsAny<ProductImportTask>())).Returns(productImportTask);
            return mock;
        }

        public static Mock<ICategoryPermissionRepository> GetCategoryPermissionRepository()
        {
            var mock = new Mock<ICategoryPermissionRepository>();
            mock.Setup(x => x.GetByPartner(It.IsAny<int>())).Returns(
                new List<CategoryPermission>
                {
                    new CategoryPermission
                    {
                        CategoryId = 50
                    }
                });
            mock.Setup(x => x.Save(userId, It.IsAny<IList<CategoryPermission>>()));
            mock.Setup(x => x.Delete(userId, It.IsAny<int>(), It.IsAny<IList<int>>()));
            return mock;
        }

        public static Mock<IPartnerProductCateroryLinkRepository> GetPartnerProductCateroryLinkRepository(
                int partnerId, int cat1, int cat2)
        {
            var mock = new Mock<IPartnerProductCateroryLinkRepository>();
            var link1 = new Entities.PartnerProductCategoryLink
            {
                Id = -5,
                IncludeSubcategories = true,
                NamePath = "Путь 1 тестовый категория 1",
                PartnerId = partnerId,
                ProductCategoryId = cat1
            };
            var link2 = new Entities.PartnerProductCategoryLink
            {
                Id = -6,
                IncludeSubcategories = false,
                NamePath = "Путь 2 тестовый категория 1",
                PartnerId = partnerId,
                ProductCategoryId = cat1
            };
            var link3 = new Entities.PartnerProductCategoryLink
            {
                Id = -6,
                IncludeSubcategories = true,
                NamePath = "Путь 3 тестовый категория 2",
                PartnerId = partnerId,
                ProductCategoryId = cat2
            };
            var links = new[]
                        {
                            link1, link2, link3
                        };

            mock.Setup(x => x.GetPartnerProductCateroryLinks(partnerId, null)).Returns(links);
            return mock;
        }

        public static Mock<IUserService> GetUserService()
        {
            var mock = new Mock<IUserService>();
            var superUser = TestHelper.BuildSuperUser();
            mock.Setup(x => x.GetUserPrincipalByName(TestDataStore.TestUserId)).Returns(superUser);
            return mock;
        }

        public static Mock<IClientProfileProvider> GetClientProfileProvider()
        {
            var mockClientProfile = new Mock<IClientProfileProvider>();
            mockClientProfile.Setup(x => x.GetClientProfile(TestDataStore.TestClientId))
                             .Returns(
                                 new GetClientProfileFullResponseTypeClientProfile
                                 {
                                     ClientId = TestDataStore.TestClientId,
                                     ClientStatus = (int)ClientProfileStatuses.Active,
                                     ClientLocationKladr = "7700000000000",
                                     FirstName = "TestFirstName",
                                 });

            mockClientProfile.Setup(x => x.GetClientProfile(TestDataStore.TestClientId + "1"))
                             .Returns(
                                 new GetClientProfileFullResponseTypeClientProfile
                                 {
                                     ClientId = TestDataStore.TestClientId + "1",
                                     ClientStatus = (int)ClientProfileStatuses.Active,
                                     ClientLocationKladr = "9800000000000",
                                     FirstName = "TestFirstName",
                                     MiddleName = "TestMiddleName",
                                 });
            return mockClientProfile;
        }

        public static Mock<IPartnerConnectorProvider> GetPartnerConnectorProvider(int? @checked = null, ConfirmedStatuses? confirmed = null, decimal? fixPriceActualPrice = null)
        {
            var mock = new Mock<IPartnerConnectorProvider>();
            
            if (!@checked.HasValue)
            {
                @checked = 1;
            }

            if (!confirmed.HasValue)
            {
                confirmed = ConfirmedStatuses.Committed;
            }

            if (!fixPriceActualPrice.HasValue)
            {
                fixPriceActualPrice = 100m;
            }

            mock.Setup(x => x.CheckOrder(It.IsAny<Order>()))
                .Returns(new CheckOrderResult { Checked = @checked.Value, ResultCode = 0, Success = true });
            
            mock.Setup(x => x.CommitOrder(It.IsAny<Order>()))
                .Returns(new CommitOrderResult { Confirmed = confirmed.Value, ResultCode = 0, Success = true });

            mock.Setup(x => x.GetDeliveryVariants(It.IsAny<Location>(), It.IsAny<OrderItem>(), It.IsAny<string>(), It.IsAny<int>())).Returns(
                GetDeliveryVariantResultObject());

            mock.Setup(x => x.GetDeliveryVariants(It.IsAny<Location>(), It.IsAny<OrderItem[]>(), It.IsAny<string>(), It.IsAny<int>())).Returns(
                GetDeliveryVariantResultObject());

            mock.Setup(x => x.FixBasketItemPrice(It.IsAny<FixBasketItemPriceParam>()))
                .Returns(new FixBasketItemPriceResult { ResultCode = 0, Success = true, Confirmed = 1, ActualPrice = fixPriceActualPrice });
            
            return mock;
        }

        private static PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult GetDeliveryVariantResultObject()
        {
            return new PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult()
                {
                    Success = true,
                    ResultCode = ResultCodes.SUCCESS,
                    Location = new PartnersConnector.WsClients.PartnersOrderManagementService.VariantsLocation()
                        {
                            LocationName = "Москва",
                            KladrCode = "7700000000000"
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
                                                    DeliveryVariantName = "Доставка до двери",
                                                    ExternalDeliveryVariantId = TestDataStore.ExternalDeliveryVariantId,
                                                    DeliveryCost = 500
                                                }
                                        }
                                },
                            new DeliveryGroup()
                                {
                                    GroupName = "Пункты самовывоза",
                                    DeliveryVariants = new[]
                                        {
                                            new DeliveryVariant()
                                                {
                                                    DeliveryVariantName = "м. Щукинская",
                                                    ExternalDeliveryVariantId = TestDataStore.PickupPointVariantId,
                                                    DeliveryCost = 10,
                                                    PickupPoints = new[]
                                                        {
                                                            new PickupPoint
                                                                {
                                                                    DeliveryVariantName =
                                                                        "Почтомат м. Щукинская в ТРК Щука",
                                                                    ExternalDeliveryVariantId =
                                                                        TestDataStore.PickupPointVariantId,
                                                                    ExternalPickupPointId = TestDataStore.PickupPointId
                                                                }
                                                        }
                                                }
                                        }
                                }

                        }
                };
        }

        public static Mock<IGeoPointProvider> GetGeoPointProvider()
        {
            var geoMock = new Mock<IGeoPointProvider>();
            geoMock.Setup(x => x.GetAddressByKladrCode(It.IsAny<string>())).Returns(new TestGeoPointProvider().GetAddressByKladrCode("notset"));
            geoMock.Setup(x => x.IsKladrCodeExists(It.IsAny<string>())).Returns(true);
            return geoMock;
        }

        public static Mock<IDeliveryTypeSpec> GetDeliverySpecification()
        {
            var geoMock = new Mock<IDeliveryTypeSpec>();
            geoMock.Setup(x => x.BuildDeliveryInfo(It.IsAny<DeliveryDto>(), TestDataStore.PartnerId))
                   .Callback(
                       (DeliveryAddress di) =>
                       {
                           di.AddressText = "г. Москва, Полесский проезд д 16";
                           di.RegionTitle = "Москва";
                       });
            return geoMock;
        }

        public static Mock<IProcessingProvider> GetProcessingProvider(decimal retVal)
        {
            var mockProcessing = new Mock<IProcessingProvider>();
            mockProcessing.Setup(x => x.GetUserBalance(It.IsAny<string>())).Returns(retVal);
            return mockProcessing;
        }

        public static Mock<IMechanicsProvider> GetMechanicsProvider()
        {
            var baseSql = "p.PriceRUR" + " * " + fakeMech.BaseMult + " + " + fakeMech.BaseAdd;
            var actionSql = "(" + baseSql + ") * " + fakeMech.Mult + " + " + fakeMech.Add;
            var priceSql = new GenerateResult { BaseSql = baseSql, ActionSql = actionSql };

            var deliveryBaseSql = "delivery.Price" + " * " + fakeMech.BaseMult + " + " + fakeMech.BaseAdd;
            var deliveryActionSql = "(" + deliveryBaseSql + ") * " + fakeMech.Mult + " + " + fakeMech.Add;
            var deliveryPriceSql = new GenerateResult { BaseSql = deliveryBaseSql, ActionSql = deliveryActionSql };

            var deliveryBaseQuantitySql = "deliveryQuantity.QuantityPrice" + " * " + fakeMech.BaseMult + " + " + fakeMech.BaseAdd;
            var deliveryActionQuantitySql = "(" + deliveryBaseQuantitySql + ") * " + fakeMech.Mult + " + " + fakeMech.Add;
            var deliveryPriceQuantitySql = new GenerateResult { BaseSql = deliveryBaseQuantitySql, ActionSql = deliveryActionQuantitySql };

            var mechMock = new Mock<IMechanicsProvider>();

            mechMock.Setup(x => x.GetPriceSql(It.IsAny<Dictionary<string, string>>())).Returns(priceSql);
            mechMock.Setup(x => x.CalculateProductPrice(It.IsAny<Dictionary<string, string>>(), It.IsAny<decimal>(), It.IsAny<Product>())).
                Returns<Dictionary<string, string>, decimal, Product>((a, b, c) => GetCalculateResult(b));
            mechMock.Setup(x => x.CalculateDeliveryPrice(It.IsAny<Dictionary<string, string>>(), It.IsAny<decimal>(), It.IsAny<int>())).
                Returns<Dictionary<string, string>, decimal, int>((a, b, c) => GetCalculatedPrice(b));
            mechMock.Setup(x => x.CalculateDeliveryPrices(It.IsAny<Dictionary<string, string>>(), It.IsAny<decimal[]>(), It.IsAny<int>())).
                Returns<Dictionary<string, string>, decimal[], int>((a, b, c) => GetCalculatedPrices(b));
            mechMock.Setup(x => x.GetOnlineProductFactors(It.IsAny<Dictionary<string, string>>())).Returns(new FactorsResult()
            {
                AdditionFactor = 0,
                MultiplicationFactor = 1,
                BaseAdditionFactor = 0,
                BaseMultiplicationFactor = 3.33m
            });

            return mechMock;
        }

        public static Mock<IProductsSearcher> GetProductsSearcher(Product product)
        {
            return GetProductsSearcher(new[]
            {
                product
            });
        }

        public static Mock<IProductsSearcher> GetProductsSearcher(Product[] products)
        {
            var mockSearcher = new Mock<IProductsSearcher>();
            mockSearcher.Setup(s => s.GetProductById(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                        .Returns<string, Dictionary<string, string>>((productId, a) => GetProductById(products, productId));

            mockSearcher.Setup(s => s.GetProductsByIds(It.IsAny<string[]>(), It.IsAny<Dictionary<string, string>>()))
                        .Returns(products.Select(p => new GetProductByIdItem
                        {
                            Product = p,
                            AvailabilityStatus = ProductAvailabilityStatuses.Available
                        }).ToArray());

            mockSearcher.Setup(
                s =>
                s.SearchPublicProducts(It.IsAny<SearchProductsParameters>())).Returns<SearchProductsParameters>(p => new SearchProductsResult()
                {
                    Products = products.Where(i => p.ProductIds.Contains(i.ProductId)).ToArray()
                });

            return mockSearcher;
        }

        private static GetProductByIdItem GetProductById(Product[] products, string productId)
        {
            return new GetProductByIdItem
            {
                Product = products.SingleOrDefault(pid => pid.ProductId == productId),
                AvailabilityStatus = ProductAvailabilityStatuses.Available
            };
        }

        public static OrderManagementService GetOrderManagementService(IBasketService basketService = null)
        {
            var service = new OrderManagementService(
                new OrdersDataSource(),
                basketService ?? GetBasketService().Object);
            return service;
        }

        public static Mock<IBasketService> GetBasketService(int? priceDelivery = null, ProductAvailabilityStatuses? availabilityStatus = null)
        {
            var service = new Mock<IBasketService>();

            var result = new GetBasketItemResult
            {
                Item = new BasketItem
                {
                    AvailabilityStatus = availabilityStatus ?? ProductAvailabilityStatuses.Available,
                    Product = new Product
                    {
                        PartnerId = TestDataStore.PartnerId,                        
                    }
                }
            };

            var multipleResult = new GetBasketItemsResult
                {
                    Items = new[]
                        {
                            new BasketItem
                                {
                                    AvailabilityStatus = availabilityStatus ?? ProductAvailabilityStatuses.Available,
                                    Product = new Product
                                        {
                                            PartnerId = TestDataStore.PartnerId,
                                        }
                                }
                        }
                };


            service.Setup(ds => ds.GetBasketItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(result);
            service.Setup(ds => ds.GetBasketItem(It.IsAny<Guid>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(result);
            service.Setup(ds => ds.GetBasketItems(It.IsAny<Guid[]>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(multipleResult);
            return service;
        }

        public static Mock<IOrdersDataSource> OrdersDataSource(Order order)
        {
            var mock = new Mock<IOrdersDataSource>();
            mock.Setup(m => m.GetOrder(It.IsAny<int>(), It.IsAny<string>())).Returns(order);
            //mock.Setup(
            //    m => m.UpdateExternalOrderStatus(
            //        It.IsAny<ExternalOrdersStatus>(), It.IsAny<string>()))
            //    .Returns(new ChangeExternalOrderStatusResult { Success = true });
            mock.Setup(
                m => m.UpdateExternalOrdersStatuses(
                    It.IsAny<ExternalOrdersStatus[]>(), It.IsAny<string>()))
                .Returns<ExternalOrdersStatus[], string>(
                    (statuses, user) =>
                    Enumerable.Repeat(new ChangeExternalOrderStatusResult { Success = true }, statuses.Length).ToArray());
            return mock;
        }

        public static Mock<IOrdersRepository> GetOrdersRepository(Order order)
        {
            var mock = new Mock<IOrdersRepository>();
            mock.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<string>())).Returns(order);
            return mock;
        }

        public static Mock<IPartnerRepository> GetPartnersRepository(Partner partner)
        {
            var mock = new Mock<IPartnerRepository>();
            mock.Setup(m => m.GetById(It.IsAny<int>())).Returns(partner);
            mock.Setup(m => m.GetActivePartner(It.IsAny<int>())).Returns(partner);
            return mock;
        }

        public static Mock<IPartnerRepository> GetPartnersRepository(params Partner[] partners)
        {
            var mock = new Mock<IPartnerRepository>();
            mock.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>((pId) => partners.FirstOrDefault(p => p.Id == pId));
            mock.Setup(m => m.GetActivePartner(It.IsAny<int>()))
                .Returns<int>((pId) => partners.FirstOrDefault(p => p.Id == pId));
            return mock;
        }


        public static Mock<IProductsDataSource> GetProductsDataSource(Product product)
        {
            var mock = new Mock<IProductsDataSource>();
            mock.Setup(
                m =>
                m.SearchPublicProducts(
                    It.IsAny<SearchProductsParameters>(), It.IsAny<GenerateResult>()))
                .Returns(new SearchProductsResult { Products = new[] { product }, Success = true });
            return mock;
        }

        public static Mock<IMechanicServiceClient> GetMechanicsClientProviderMock()
        {
            var mock = new Mock<IMechanicServiceClient>();
            mock.Setup(m => m.GenerateSql(It.IsAny<GenerateSqlParameters>())).Returns(GetSqlMock());
            mock.Setup(m => m.CalculateSingleValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<Dictionary<string, string>>())).Returns(GetCalculateResult());
            mock.Setup(m => m.CalculateFactors(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(GetCalculateFactors());

            return mock;
        }

        private static FactorsResult GetCalculateFactors()
        {
            return new FactorsResult()
            {
                Success = true,
                AdditionFactor = 0,
                MultiplicationFactor = 1,
                BaseAdditionFactor = 0,
                BaseMultiplicationFactor = BaseMultiplicationFactor
            };
        }

        private static CalculateResult GetCalculateResult(decimal priceRur = 300)
        {
            return new CalculateResult()
            {
                Success = true,
                RuleApplyStatus = RuleApplyStatuses.RulesExecuted,
                BaseResult = CalcBonus(priceRur),
                PromoResult = CalcPromo(CalcBonus(priceRur))
            };
        }

        private static CalculatedPrice[] GetCalculatedPrices(decimal[] pricesRur = null)
        {
            pricesRur = pricesRur ?? new decimal[]
            {
                300
            };

            return pricesRur.Select(GetCalculatedPrice).ToArray();
        }

        private static CalculatedPrice GetCalculatedPrice(decimal priceRur = 300)
        {
            return new CalculatedPrice()
            {
                Price = new Price()
                {
                    Bonus = CalcBonus(priceRur),
                    Rur = priceRur
                },
                PartnerId = TestDataStore.GetProduct().PartnerId
            };
        }

        private static decimal CalcBonus(decimal basePrice)
        {
            return basePrice * BaseMultiplicationFactor;
        }

        private static decimal CalcPromo(decimal basePrice)
        {
            return basePrice * PromoFactor;
        }

        private static GenerateResult GetSqlMock()
        {
            var baseSql = PriceColumnAlias + " * " + fakeMech.BaseMult + " + " + fakeMech.BaseAdd;
            var actionSql = "(" + baseSql + ") * " + fakeMech.Mult + " + " + fakeMech.Add;
            return new GenerateResult
            {
                BaseSql = baseSql,
                ActionSql = actionSql
            };
        }

        public static Mock<ISecurityChecker> GetSecurityChecker()
        {
            var mock = new Mock<ISecurityChecker>();

            mock.Setup(m => m.CheckPermissions(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string[]>()));
            mock.Setup(m => m.CheckPermissions(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<string[]>()));
            mock.Setup(m => m.CheckPermissions(It.IsAny<string>(), It.IsAny<string[]>()));

            return mock;
        }

        public static ICatalogAdminService GetCatalogAdminService()
        {
            var checker = MockFactory.GetSecurityChecker();
            return new CatalogAdminService(securityChecker: checker.Object, geoPointProvider: GetGeoPointProvider().Object);
        }

        public static Mock<IDeliveryRatesRepository> GetDeliveryRatesRepository()
        {
            var mock = new Mock<IDeliveryRatesRepository>();

            mock.Setup(m => m.GetMinPriceRate(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new PartnerDeliveryRate()
            {

            });

            return mock;
        }
    }
}