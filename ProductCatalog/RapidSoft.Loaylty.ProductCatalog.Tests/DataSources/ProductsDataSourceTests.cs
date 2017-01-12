namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using API.Entities;
    using API.OutputResults;

    using Common;

    using Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ProductCatalog.DataSources.Repositories;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.ImportTests;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.PromoAction.WsClients.MechanicsService;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class ProductsDataSourceTests
    {
        private static readonly List<string> Ids = new List<string>(5);
        private string clientId = TestDataStore.TestClientId;
        private readonly Dictionary<string, string> clientContext = TestDataStore.GetClientContext();
        private SqlDeliveryPrice sqlDeliverySql;
        private GenerateResult priceSql;
        private readonly ProductsDataSource productsDataSource = new ProductsDataSource();
        private string userId = TestDataStore.TestUserId;

        [TestInitialize]
        public void MyTestInitialize()
        {
            TestHelper.CreateDeliveryMatrix();
            Ids.Add(TestHelper.CreateTestProduct(1, 3000, 100, true)); // 3 кг
            Ids.Add(TestHelper.CreateTestProduct(2, 4200, 500)); // 3 кг 200 гр
            Ids.Add(TestHelper.CreateTestProduct(3, 5200, 1000)); // 5 кг 200 гр
            Ids.Add(TestHelper.CreateTestProduct(4, 15000, 5000)); // 15 кг

            var mechanicsProvider = MockFactory.GetMechanicsProvider().Object;
            priceSql = mechanicsProvider.GetPriceSql(clientContext);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            TestHelper.DeleteTestProduct();
        }        

        [TestMethod]
        public void ShouldGetCarrierRateForPartner()
        {
            string kladr = "2300000200000";
            decimal partnerPrice = new decimal(1234.56);
            decimal carrierPrice = new decimal(11.22);

            var partnerRepo = new PartnerRepository();
            var partner = partnerRepo.GetById(TestDataStore.NoDeliveryRatesPartnerID);
            var carrier = partnerRepo.CreateOrUpdate(TestDataStore.TestUserId,
                new Partner
                {
                Name = RandomHelper.RandomString(10),
                InsertedUserId = RandomHelper.RandomString(10),
                Status = PartnerStatus.Active,
                InsertedDate = DateTime.Now
            });

            partner.Carrier = carrier;
            partner.CarrierId = carrier.Id;
            partner.UpdatedUserId = RandomHelper.RandomString(10);
            partner.UpdatedDate = DateTime.Now;
            partnerRepo.CreateOrUpdate(TestDataStore.TestUserId, partner, settings: null);

            var drRepo = new TestDeliveryRatesRepository();

            DeliveryRate drPartner = RandomHelper.RandomDeliveryRate();
            drPartner.Kladr = kladr;
            drPartner.PriceRUR = partnerPrice;
            drPartner.MinWeightGram = 0;
            drPartner.PartnerId = partner.Id;
            drRepo.Create(drPartner);

            DeliveryRate drCarrier = RandomHelper.RandomDeliveryRate();
            drCarrier.Kladr = kladr;
            drCarrier.PriceRUR = carrierPrice;
            drCarrier.MinWeightGram = 0;
            drCarrier.PartnerId = carrier.Id;
            drRepo.Create(drCarrier);

            var rate = GetDeliveryRate(partner, kladr);

            Assert.IsNotNull(rate);
            Assert.AreEqual((double)partnerPrice, (double)rate.PriceRur, 0.01);

            #region cleanup
            drRepo.Delete(drPartner.Id);
            drRepo.Delete(drCarrier.Id);
            partner.Carrier = null;
            partner.CarrierId = null;
            partner.UpdatedDate = DateTime.Now;
            partner.UpdatedUserId = RandomHelper.RandomString(10);
            partnerRepo.CreateOrUpdate(TestDataStore.TestUserId, partner, settings: null);
            partnerRepo.Delete(carrier.Id);
            #endregion
        }

        private static PartnerDeliveryRate GetDeliveryRate(Partner partner, string kladr)
        {
            var rate = new DeliveryRatesRepository().GetMinPriceRate(partner.Id, kladr, 10);
            return rate;
        }

        [TestMethod]
        public void ShouldSearchProductForPartnerWithCarrierDeliveryRateOnly()
        {
            var kladr = "3300000200000";
            var checkPrice = 4321.21m;

            var partnerRepo = new PartnerRepository();
            var partner = partnerRepo.GetById(TestDataStore.NoDeliveryRatesPartnerID);
            var carrier = partnerRepo.CreateOrUpdate(TestDataStore.TestUserId,
                                                     new Partner
                                                     {
                                                         Name = RandomHelper.RandomString(10),
                                                         InsertedUserId = RandomHelper.RandomString(10),
                                                         Status = PartnerStatus.Active,
                                                         InsertedDate = DateTime.Now
                                                     });

            partner.Carrier = carrier;
            partner.CarrierId = carrier.Id;
            partner.UpdatedUserId = RandomHelper.RandomString(10);
            partner.UpdatedDate = DateTime.Now;
            partnerRepo.CreateOrUpdate(TestDataStore.TestUserId, partner);

            var drRepo = new TestDeliveryRatesRepository();

            var drCarrier = RandomHelper.RandomDeliveryRate();
            drCarrier.Kladr = kladr;
            drCarrier.PriceRUR = checkPrice;
            drCarrier.MinWeightGram = 0;
            drCarrier.PartnerId = carrier.Id;
            drRepo.Create(drCarrier);

            new ProductsDataSource().UpdateProductsFromAllPartners();

            // необходимо "промодерировать" товар, чтобы он мог быть найден методом SearchPublicProducts
            var products = productsDataSource.AdminSearchProducts(new AdminSearchProductsParameters
            {
                UserId = TestDataStore.TestUserId,
                CountToTake = 1,
                Status = ProductStatuses.Active,
                PartnerIds = new[] { TestDataStore.NoDeliveryRatesPartnerID }
            });
            var productId = products.Products.First().ProductId;

            productsDataSource.ChangeProductsModerationStatus(new ChangeModerationStatusParameters
            {
                UserId = TestDataStore.TestUserId,
                ProductIds = new[] { productId },
                ProductModerationStatus = ProductModerationStatuses.Applied
            });

            var clientIdValue = Guid.NewGuid().ToString();

            var clientContextValue = new Dictionary<string, string>
            {
                { ClientContextParser.ClientIdKey, clientIdValue },
                { ClientContextParser.LocationKladrCodeKey, kladr }
            };

            var parameters = new SearchProductsParameters
            {
                ClientContext = clientContextValue,
                PartnerIds = new[] { TestDataStore.NoDeliveryRatesPartnerID }
            };

            productsDataSource.DeleteCache();
            var result = SearchProducts(parameters);
            var product = result.Products
                                .FirstOrDefault(t => t.PartnerId == TestDataStore.NoDeliveryRatesPartnerID);

            Assert.IsNotNull(product);

            #region cleanup
            drRepo.Delete(drCarrier.Id);
            partner.Carrier = null;
            partner.CarrierId = null;
            partner.UpdatedDate = DateTime.Now;
            partner.UpdatedUserId = RandomHelper.RandomString(10);
            partnerRepo.CreateOrUpdate(TestDataStore.TestUserId, partner);
            partnerRepo.Delete(carrier.Id);
            #endregion
        }

        [TestMethod]
        public void ShouldNotSearchProductForDeactivatedPartner()
        {
            var ds = new ProductsDataSource();

            var parameters = new SearchProductsParameters
            {
                ClientContext = clientContext,
                PartnerIds = new[] { TestDataStore.DeactivatedPartnerID }
            };

            var result = SearchProducts(parameters);
            var product = result.Products.FirstOrDefault(t=>t.PartnerId == TestDataStore.DeactivatedPartnerID);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void ShouldSearchProductByPartnerProductId()
        {
            var parameters = new SearchProductsParameters
            {
                PartnerIds = new[] { TestDataStore.PartnerId },
                ClientContext = clientContext
            };

            // NOTE: Ищем продукты для города и только доставляемые!
            var result = SearchProducts(parameters);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success, result.ResultDescription);
            Assert.IsTrue(result.Products.Any());

            parameters.PartnerProductId = result.Products.First().PartnerProductId;

            var result2 = SearchProducts(parameters);

            Assert.IsTrue(result2.Success, result2.ResultDescription);
            Assert.AreEqual(result.Products.First().PartnerProductId, result2.Products.First().PartnerProductId);
        }

        [TestMethod]
        public void ShouldSearchProductWithTargetAudiences()
        {
            ProductTargetAudience productAudience;

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var productId = "1_offerLinkedToTargetAudience";                

                ctx.ProductTargetAudiences.Add(new ProductTargetAudience()
                {
                    InsertedDate = DateTime.Now,
                    InsertedUserId = TestDataStore.TestUserId,
                    ProductId = productId,
                    TargetAudienceId = Guid.NewGuid().ToString()
                });

                ctx.SaveChanges();

                productAudience = ctx.ProductTargetAudiences.First(ta => ta.ProductId == productId);
            }

            // Если AudiencesKey не указан, то из-за таргетирования продукта мы его не найдем
            var parameters = new SearchProductsParameters
            {
                ProductIds = new string[] { productAudience.ProductId },
                ClientContext = clientContext
            };

            productsDataSource.DeleteCache();
            var result = SearchProducts(parameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Success, true);
            Assert.IsTrue(!result.Products.Any());

            // Если AudiencesKey указан пустой, то из-за таргетирования продукта мы его не найдем
            var context = TestDataStore.GetClientContext();
            context[ClientContextParser.AudiencesKey] = string.Empty;
            
            parameters = new SearchProductsParameters
            {
                ProductIds = new string[] { productAudience.ProductId },
                ClientContext = context
            };

            productsDataSource.DeleteCache();
            result = SearchProducts(parameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Success, true);
            Assert.IsTrue(!result.Products.Any());

            // Если в AudiencesKey указана конкретная ЦА, то вернуться товары привязанные к этой ЦА
            context = TestDataStore.GetClientContext();
            context[ClientContextParser.AudiencesKey] = productAudience.TargetAudienceId;

            parameters = new SearchProductsParameters
            {
                ProductIds = new string[] { productAudience.ProductId },
                ClientContext = context
            };

            productsDataSource.DeleteCache();
            result = SearchProducts(parameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Success, true);
            Assert.IsTrue(result.Products.Any(p => p.ProductId == productAudience.ProductId));

            // NOTE: Подчищаем за некоторыми
            using (var ctx1 = new LoyaltyDBEntities())
            {
                const string SQLFormat = "DELETE FROM [prod].[ProductTargetAudiences] WHERE [TargetAudienceId] = '{0}' AND [ProductId] = '{1}'";
                var sql = string.Format(SQLFormat, productAudience.TargetAudienceId, productAudience.ProductId);
                ctx1.Database.ExecuteSqlCommand(sql);
            }
        }

        private SearchProductsResult AdminSearchProducts(AdminSearchProductsParameters parameters)
        {
            return productsDataSource.AdminSearchProducts(parameters);
        }

        private SearchProductsResult SearchProducts(SearchProductsParameters parameters, bool returnNotAvailable = false)
        {
            productsDataSource.UpdateProductsFromAllPartners();
            return productsDataSource.SearchPublicProducts(parameters, priceSql);
        }

        [TestMethod]
        public void ShouldSearchProductByTargetIds()
        {
            const string query = @"select * from [prod].[ProductTargetAudiences] where ProductId in (select value from dbo.ParamParserString(@productIds,','))";

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var products = ctx.ProductSortProjections.Where(p => p.PartnerId == 1).Take(2).ToList();
                var productIds = products.Select(p => p.ProductId).ToArray();

                var existing =
                    ctx.Database.SqlQuery<ProductTargetAudience>(query, new SqlParameter("productIds", string.Join(",", productIds))).ToList();

                if (existing.Any())
                {
                    foreach (var obj in existing.Select(productTargetAudience => ctx.ProductTargetAudiences
                        .First(p => p.ProductId == productTargetAudience.ProductId && p.TargetAudienceId == productTargetAudience.TargetAudienceId)))
                    {
                        ctx.ProductTargetAudiences.Remove(obj);
                    }
                    ctx.SaveChanges();
                }

                var targetAudienceId0 = "333";

                var productId0 = products[0].ProductId;
                var audience = new ProductTargetAudience()
                               {
                                   ProductId = productId0,
                                   TargetAudienceId = targetAudienceId0,
                                   InsertedUserId = TestDataStore.TestUserId,
                                   InsertedDate = DateTime.Now
                               };
                
                ctx.ProductTargetAudiences.Add(audience);
                ctx.SaveChanges();

                var clientContext = TestDataStore.GetClientContext();
                clientContext[ClientContextParser.AudiencesKey] = targetAudienceId0;

                var ds = new ProductsDataSource();
                var parameters = new SearchProductsParameters
                                 {
                                     ProductIds = products.Select(p => p.ProductId).ToArray(),
                                     ClientContext = clientContext
                                 };

                
                productsDataSource.DeleteCache();
                var result = SearchProducts(parameters);

                Assert.IsTrue(result.Success);
                Assert.IsTrue(result.Products.Any());
                Assert.AreEqual(2, result.Products.Count());
                Assert.AreEqual(result.Products[0].ProductId, products[0].ProductId);
                Assert.AreEqual(result.Products[1].ProductId, products[1].ProductId);

                var targetAudienceId1 = Guid.NewGuid().ToString();
                var productId1 = products[1].ProductId;
                audience = new ProductTargetAudience()
                {
                    ProductId = productId1,
                    TargetAudienceId = targetAudienceId1,
                    InsertedUserId = TestDataStore.TestUserId,
                    InsertedDate = DateTime.Now
                };

                ctx.ProductTargetAudiences.Add(audience);
                ctx.SaveChanges();

                productsDataSource.DeleteCache();
                result = SearchProducts(parameters);

                Assert.IsTrue(result.Success);
                Assert.IsTrue(result.Products.Any());
                Assert.AreEqual(1, result.Products.Count());
                Assert.AreEqual(result.Products[0].ProductId, products[0].ProductId);

                // NOTE: Подчищаем за некоторыми
                using (var ctx1 = new LoyaltyDBEntities())
                {
                    const string SQLFormat = "DELETE FROM [prod].[ProductTargetAudiences] WHERE [TargetAudienceId] = '{0}' AND [ProductId] = '{1}'";
                    var sql = string.Format(SQLFormat, targetAudienceId0, productId0);
                    ctx1.Database.ExecuteSqlCommand(sql);
                    sql = string.Format(SQLFormat, targetAudienceId1, productId1);
                    ctx1.Database.ExecuteSqlCommand(sql);
                }
            }
        }

        [TestMethod]
        public void ShouldSearchProductByProductParams()
        {         
            var parameters = new SearchProductsParameters
            {
                ClientContext = clientContext
            };

            parameters.ProductParams = new ProductParam[]
                                       {
                                          new ProductParam() { Name = "Объём жесткого диска", Unit = "Гб", Value = "500" },
                                          new ProductParam() { Name = "Размер оперативной памяти", Unit = "Мб", Value = "2048" }
                                       };

            var result = SearchProducts(parameters);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Products.Any());

            parameters.ProductParams = new ProductParam[]
                                       {
                                          new ProductParam() { Name = "Тип процессора", Value = "Atom" }
                                       };

            result = SearchProducts(parameters);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Products.Any());
        }

        [TestMethod]
        public void ShouldSearchProductByProductParamsValueMore256()
        {
            var parameters = new SearchProductsParameters
            {
                ClientContext = clientContext
            };

            var paramSearchValue =
                @"67 фишек, 15 белых шашек и 15 черных шашек, 5 кубиков, 5 кубиков для игры в ""Покер"", 1 двойной кубик, домино из 28 камней, 32 шахматные фигуры, 4 игровых поля (""Oh Pardon / Скачки"", ""Backgammon / Лестница"", ""Halma / Лиса + Курицы"", ""Мельница / Шашки + Шахматы""), 2 колоды игральных карт, игра ""Квартет"", игра ""Schwarzer Peter"", стаканчик для игры с кубиками, блок для записи ""Eskalero"", блок для записи ""Yatze"".";

            parameters.ProductParams = new[] { new ProductParam { Name = "Материал", Value = paramSearchValue }, };

            var result = SearchProducts(parameters);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Products.Any());
        }

        [TestMethod]
        public void ShouldNotSearchProductByProductParams()
        {
            var parameters = new SearchProductsParameters
            {
                PartnerIds = new[] { 3 },
                ClientContext = clientContext
            };

            parameters.ProductParams = new ProductParam[]
                                       {
                                          new ProductParam() { Name = "Размер2", Value = "20482" },
                                          new ProductParam() { Name = "Размер", Value = "20482" }
                                       };

            var result = SearchProducts(parameters);

            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.Products.Any());
        }

        [TestMethod]
        public void ShouldSearchTargetProducts()
        {
            var ta1 = "ЦАTest1";
            var ta2 = "ЦАTest2";
            var ta3 = "ЦАTest3";

            CleanProductTargetAudience(new []{ta1, ta2, ta3});

            var repo = new ProductsRepository();

            // NOTE: Находим не таргетированные продукты
            var parameters = new SearchProductsParameters
                                 {
                                     ClientContext = clientContext,
                                     CountToSkip = 10,
                                     CountToTake = 10
                                 };

            productsDataSource.DeleteCache();
            var result = SearchProducts(parameters);
            
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Products.Length > 5);

            // NOTE: Запихиваем продукт 0 в ЦА1
            var productId0 = result.Products[0].ProductId;
            repo.AddProductTargetAudiences(TestDataStore.TestUserId, productId0.MakeArray(), ta1.MakeArray());

            // NOTE: Запихиваем продукт 1 в ЦА1 и ЦА2
            var productId1 = result.Products[1].ProductId;
            repo.AddProductTargetAudiences(TestDataStore.TestUserId, productId1.MakeArray(), ta1.MakeArray(ta2));

            // NOTE: Запихиваем продукт 2 в ЦА3
            var productId2 = result.Products[2].ProductId;
            repo.AddProductTargetAudiences(TestDataStore.TestUserId, productId2.MakeArray(), ta3.MakeArray());

            // NOTE: Продукт 3 не запихиваем никуда, он публичный
            var productId3 = result.Products[3].ProductId;

            // NOTE: Для простоты поиск только по 4 продуктам
            var productIds = productId0.MakeArray(productId1, productId2, productId3);

            // NOTE: Поиск без TargetAudiences
            var parameters1 = new SearchProductsParameters
                                  {
                                      ProductIds = productIds,
                                      ClientContext = clientContext,
                                      CountToTake = 10
                                  };
            productsDataSource.DeleteCache();
            var result1 = SearchProducts(parameters1);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result1.Products);
            Assert.AreEqual(1, result1.Products.Length, "Только один из 4 товар публичный");
            Assert.IsTrue(result1.Products.Any(x => x.ProductId == productId3));

            // NOTE: Поиск для ЦА1
            var clientContext2 = TestDataStore.GetClientContext();
            clientContext2[ClientContextParser.AudiencesKey] = ta1;
            var parameters2 = new SearchProductsParameters
                                  {
                                      ProductIds = productIds,
                                      ClientContext = clientContext2,
                                      CountToTake = 10
                                  };
            var result2 = SearchProducts(parameters2);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result2.Products);
            Assert.AreEqual(3, result2.Products.Length, "Должно быть 3 продукта: 1 публичный и 2 из ЦА1");
            Assert.IsTrue(result2.Products.Any(x => x.ProductId == productId3));
            Assert.IsTrue(result2.Products.Any(x => x.ProductId == productId0));
            Assert.IsTrue(result2.Products.Any(x => x.ProductId == productId1));

            // NOTE: Поиск для ЦА2
            var clientContext3 = TestDataStore.GetClientContext();
            clientContext3[ClientContextParser.AudiencesKey] = ta2;
            var parameters3 = new SearchProductsParameters
            {
                ProductIds = productIds,
                ClientContext = clientContext3,
                CountToTake = 10
            };
            var result3 = SearchProducts(parameters3);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result3.Products);
            Assert.AreEqual(2, result3.Products.Length, "Должно быть 2 продукта: 1 публичный и 1 из ЦА2");
            Assert.IsTrue(result3.Products.Any(x => x.ProductId == productId3));
            Assert.IsTrue(result3.Products.Any(x => x.ProductId == productId1));

            // NOTE: Поиск для ЦА1 и ЦА3
            var clientContext4 = TestDataStore.GetClientContext();
            clientContext4[ClientContextParser.AudiencesKey] = ta1 + ";" + ta3;
            var parameters4 = new SearchProductsParameters
            {
                ProductIds = productIds,
                ClientContext = clientContext4,
                CountToTake = 10
            };
            var result4 = SearchProducts(parameters4);
            Assert.IsNotNull(result4);
            Assert.IsNotNull(result4.Products);
            Assert.AreEqual(4, result4.Products.Length, "Должно быть 4 продукта: 1 публичный, 2 из ЦА1, 1 из ЦА3");
            Assert.IsTrue(result4.Products.Any(x => x.ProductId == productId3));
            Assert.IsTrue(result4.Products.Any(x => x.ProductId == productId0));
            Assert.IsTrue(result4.Products.Any(x => x.ProductId == productId1));
            Assert.IsTrue(result4.Products.Any(x => x.ProductId == productId2));

            // NOTE: Поиск для ЦА1 и ЦА2
            var clientContext5 = TestDataStore.GetClientContext();
            clientContext5[ClientContextParser.AudiencesKey] = ta1 + ";" + ta2;

            var parameters5 = new SearchProductsParameters
            {
                ProductIds = productIds,
                ClientContext = clientContext5,
                CountToTake = 10
            };

            var result5 = SearchProducts(parameters5);
            Assert.IsNotNull(result5);
            Assert.IsNotNull(result5.Products);
            Assert.AreEqual(3, result5.Products.Length, "Должно быть3 продукта: 1 публичный, 1 из ЦА1 и ЦА2, 1 из ЦА2");
            Assert.IsTrue(result5.Products.Any(x => x.ProductId == productId0));
            Assert.IsTrue(result5.Products.Any(x => x.ProductId == productId1));
            Assert.IsTrue(result5.Products.Any(x => x.ProductId == productId3));

            // NOTE: Убраем за собой
            CleanProductTargetAudience(new[] { ta1, ta2, ta3 });
        }

        [TestMethod]
        public void ShouldSearchRecommendedProducts()
        {
            var repo = new ProductsRepository();

            // NOTE: Находим продукты
            var parameters = new AdminSearchProductsParameters
            {
                CountToSkip = 10,
                CountToTake = 10
            };

            var result = productsDataSource.AdminSearchProducts(parameters);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Products.Length > 5);

            // NOTE: "рекомендуем" продукты 1 и 2
            var productId0 = result.Products[0].ProductId;
            var productId1 = result.Products[1].ProductId;
            repo.ChangeStatuses(new RecommendParameters
            {
                UserId = clientId,
                ProductIds = new[] { productId0, productId1 },
                IsRecommended = true
            });

            // NOTE: "не рекомендуем" продукты 3 и 4
            var productId2 = result.Products[2].ProductId;
            var productId3 = result.Products[3].ProductId;
            repo.ChangeStatuses(new RecommendParameters
            {
                UserId = clientId,
                ProductIds = new[] { productId2, productId3 },
                IsRecommended = false
            });

            var productIds = new[] { productId0, productId1, productId2, productId3 };

            // NOTE: Поиск без IsRecommended
            var parameters1 = new AdminSearchProductsParameters
            {
                ProductIds = productIds,
                CountToTake = 10
            };
            var result1 = productsDataSource.AdminSearchProducts(parameters1);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result1.Products);
            Assert.AreEqual(4, result1.Products.Length, "Должно быть 4 продукта");

            // NOTE: Поиск рекомендованных
            var parameters2 = new AdminSearchProductsParameters
            {
                ProductIds = productIds,
                IsRecommended = true,
                CountToTake = 10
            };
            var result2 = productsDataSource.AdminSearchProducts(parameters2);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result2.Products);
            Assert.AreEqual(2, result2.Products.Length, "Должно быть 2 продукта");
            Assert.IsTrue(result2.Products.Any(x => x.ProductId == productId0));
            Assert.IsTrue(result2.Products.Any(x => x.ProductId == productId1));

            // NOTE: Поиск нерекомендованных
            var parameters3 = new AdminSearchProductsParameters
            {
                ProductIds = productIds,
                IsRecommended = false,
                CountToTake = 10
            };
            var result3 = productsDataSource.AdminSearchProducts(parameters3);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result3.Products);
            Assert.AreEqual(2, result3.Products.Length, "Должно быть 2 продукта");
            Assert.IsTrue(result3.Products.Any(x => x.ProductId == productId2));
            Assert.IsTrue(result3.Products.Any(x => x.ProductId == productId3));
        }

        private static void CleanProductTargetAudience(string[] Ids)
        {
            using (var ctx = new LoyaltyDBEntities())
            {
                foreach (var id in Ids)
                {
                var sql =
                    string.Format("DELETE FROM [prod].[ProductTargetAudiences] WHERE [TargetAudienceId] = N'{0}'", id);
                ctx.Database.ExecuteSqlCommand(sql);                    
                }
            }
        }

        [TestMethod]
        public void ShouldReturnNotAvailableProductInDeactivatedCategory()
        {
            var category = TestHelper.NewCategory(null, ProductCategoryStatuses.NotActive);
            
            var parameters = new CreateProductParameters()
            {
                PartnerId = TestDataStore.OzonPartnerId,
                PartnerProductId = "deactivatedCategoryOffer_" + Guid.NewGuid(),
                UserId = userId,
                CategoryId = category.Id,
                Name = "Товар в деактевированной категории",
                PriceRUR = 160                
            };

            var product = TestHelper.NewPublicProduct(parameters);

            var searchParam = new AdminSearchProductsParameters()
            {
                ProductIds = new string[] { product.ProductId }
            };

            new ProductsDataSource().DeleteCache();
            new ProductsDataSource().UpdateProductsFromAllPartners();

            var res = AdminSearchProducts(searchParam);

            Assert.IsTrue(res.Success, res.ResultDescription);
            Assert.IsNotNull(res.Products);
            Assert.AreEqual(1, res.Products.Length);
        }

        [TestMethod]
        public void ShouldNotReturnProductIfParentCategoryIsDeactivated()
        {
            var parentCat = TestHelper.NewCategory(null, ProductCategoryStatuses.NotActive);
            var category = TestHelper.NewCategory(parentCat.Id, ProductCategoryStatuses.Active);

            var parameters = new CreateProductParameters()
            {
                UserId = userId,
                PartnerId = TestDataStore.OzonPartnerId,
                PartnerProductId = "deactivatedCategoryOffer_" + Guid.NewGuid(),                
                CategoryId = category.Id,
                Name = "Товар в категории, а родительская деактивирована",
                PriceRUR = 160
            };

            var product = TestHelper.NewPublicProduct(parameters);

            var searchParam = new SearchProductsParameters
            {
                ProductIds = new string[] { product.ProductId },
                ClientContext = clientContext
            };

            new ProductsDataSource().DeleteCache();

            var res = SearchProducts(searchParam, false);

            Assert.IsTrue(res.Success, res.ResultDescription);
            Assert.IsNotNull(res.Products);
            Assert.AreEqual(0, res.Products.Length, "Товар не должен возвращатся при поиске если он в категории, родитель которой деактивирован");
        }

        [TestMethod]
        public void ShouldReturnNotAvailableProductIfParentCategoryIsDeactivated()
        {
            var parentCat = TestHelper.NewCategory(null, ProductCategoryStatuses.NotActive);
            var category = TestHelper.NewCategory(parentCat.Id, ProductCategoryStatuses.Active);

            var parameters = new CreateProductParameters()
            {
                UserId = userId,
                PartnerId = TestDataStore.OzonPartnerId,
                PartnerProductId = "deactivatedCategoryOffer_" + Guid.NewGuid(),
                CategoryId = category.Id,
                Name = "Товар в категории, а родительская деактивирована",
                PriceRUR = 160
            };

            var product = TestHelper.NewPublicProduct(parameters);

            var searchParam = new AdminSearchProductsParameters()
            {
                ProductIds = new string[] { product.ProductId }
            };

            new ProductsDataSource().DeleteCache();
            new ProductsDataSource().UpdateProductsFromAllPartners();

            var res = AdminSearchProducts(searchParam);

            Assert.IsTrue(res.Success, res.ResultDescription);
            Assert.IsNotNull(res.Products);
            Assert.AreEqual(1, res.Products.Length, "Не доступный товар должен возвращатся");
        }

        [TestMethod]
        public void ShouldReturnCalculatedProductsPrices()
        {
            var productsIds = new[]
            {
                TestHelper.CreateTestProduct(10, 100, 10),
                TestHelper.CreateTestProduct(11, 200, 20),
                TestHelper.CreateTestProduct(12, 400, 33),
                TestHelper.CreateTestProduct(13, 700, 11),
            };

            productsDataSource.UpdateProductsFromAllPartners();

            var prices = productsDataSource.CalculateProductsPrices(productsIds, priceSql);

            Assert.IsNotNull(prices);
            Assert.AreEqual(productsIds.Length, prices.Length);

            var matchCount = prices.Count(p => productsIds.Contains(p.ProductId));
            Assert.AreEqual(productsIds.Length, matchCount);
        }
    }
}
