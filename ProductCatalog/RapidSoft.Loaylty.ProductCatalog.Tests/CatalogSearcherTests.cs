namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;

    using RapidSoft.Loaylty.ProductCatalog.API;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.ImportTests;
    using RapidSoft.Loaylty.ProductCatalog.Tests.DataSources;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.Loaylty.ProductCatalog.Settings;
    using Vtb24.Common.Configuration;

    [TestClass]
    public class CatalogSearcherTests
    {
        private static readonly int PartnerId = 1;
        private readonly string clientId = TestDataStore.TestClientId;
        private readonly ICatalogAdminService catalogAdminService = MockFactory.GetCatalogAdminService();
        private readonly Dictionary<string, string> clientContext = TestDataStore.GetClientContext();
        private ProductsDataSource productsDataSource;
        private ICatalogSearcher searcher;
        private ProductsSearcher productsSearcher;

        #region Tests

        [TestInitialize]
        public void TestInit()
        {
            new TestDeliveryRatesRepository().CreateIfNotExists(1, 0, 10000000, 100, "7700000000000");

            TestHelper.CreateDeliveryMatrix();
            TestHelper.CreateTestProduct(1, 3000, 100, true); // 100 руб 3 кг
            TestHelper.CreateTestProduct(2, 4200, 500); // 500 руб 3 кг 200 гр
            TestHelper.CreateTestProduct(3, 5200, 1000); // 1000 руб 5 кг 200 гр
            TestHelper.CreateTestProduct(4, 15000, 5000); // 5000 руб 15 кг

            var mechMock = MockFactory.GetMechanicsProvider();

            productsDataSource = new ProductsDataSource();
            productsDataSource.DeleteCache();
            productsDataSource.UpdateProductsFromAllPartners();

            productsSearcher = new ProductsSearcher(
                productsDataSource,
                new ProductAttributeRepository(),
                mechMock.Object);

            searcher = new ProductCatalog.Services.CatalogSearcher(
                productsSearcher,
                new PartnersSearcher(),
                new CategoriesSearcher(mechanicsProvider: mechMock.Object),
                new ProductViewStatisticRepository());
        }

        [TestMethod]
        public void GetRecomendedProductsTest()
        {
            var oldSite122EnableRecommendedProducts = FeaturesConfiguration.Instance.Site122EnableRecommendedProducts;
            FeaturesConfiguration.Instance.Site122EnableRecommendedProducts = false;

            // NOTE: находим все рекомендованные и сбрасываем у них признак рекомендованности
            var recommended = catalogAdminService.SearchProducts(new AdminSearchProductsParameters
            {
                IsRecommended = true,
                UserId = TestDataStore.TestUserId,
                CountToTake = 1000,
                CountToSkip = 0
            });
            var unrecommendResult = catalogAdminService.RecommendProducts(new RecommendParameters
            {
                IsRecommended = false,
                ProductIds = recommended.Products.Select(p => p.ProductId).ToArray(),
                UserId = TestDataStore.TestUserId
            });

            productsDataSource.UpdateProductsFromAllPartners();

            // NOTE: рекомендуем три продукта
            var toRecommend = productsSearcher.SearchPublicProducts(new SearchProductsParameters
            {
                ClientContext = clientContext,
                CountToTake = 3,
                CountToSkip = 0
            });
            var recommendResult = catalogAdminService.RecommendProducts(new RecommendParameters
            {
                IsRecommended = true,
                ProductIds = toRecommend.Products.Select(p => p.ProductId).ToArray(),
                UserId = TestDataStore.TestUserId
            });

            productsDataSource.UpdateProductsFromAllPartners();

            const int TRY_COUNT = 10;
            int? countToTake = 6;

			for (var i = 0; i < TRY_COUNT; i++)
			{
				var res = searcher.GetRecomendedProducts(clientContext, countToTake);

				Assert.IsNotNull(res);
				Assert.IsNotNull(res.Products);
				Assert.AreEqual(countToTake, res.Products.Length);
                Assert.AreEqual(3, res.Products.Count(p => p.IsRecommended));
			}

            FeaturesConfiguration.Instance.Site122EnableRecommendedProducts = oldSite122EnableRecommendedProducts;
        }

        [TestMethod]
        public void CanGetPopularProductsTest()
        {
            var res = searcher.GetPopularProducts(
                clientId,
                PopularProductTypes.MostOrdered,
                TestDataStore.GetClientContext(), 4);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultDescription);

            Assert.IsNotNull(res.PopularProducts);
            Assert.AreEqual(4, res.PopularProducts.Length);
            
            for (int i = 1; i < res.PopularProducts.Length; i++)
            {
                Assert.IsTrue(res.PopularProducts[i - 1].ProductRate >= res.PopularProducts[i].ProductRate);
                Assert.AreEqual(ProductModerationStatuses.Applied, res.PopularProducts[i].Product.ModerationStatus);
            }
        }

        [TestMethod]
        public void ShouldReturnPopularProductsWithCashesAndLocks()
        {
            var pops = new[]
                          {
                              PopularProductTypes.MostOrdered, PopularProductTypes.MostViewed,
                              PopularProductTypes.MostWished
                          };

            foreach (var type in pops)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var result = this.searcher.GetPopularProducts(this.clientId, type, TestDataStore.GetClientContext(), 4);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.PopularProducts.Any());
                sw.Stop();
                Console.WriteLine(sw.ElapsedTicks);
            }

            new ProductsDataSource().DeleteCache();

            var t1 = new Task<GetPopularProductsResult>(() => this.searcher.GetPopularProducts(
                null, PopularProductTypes.MostOrdered, TestDataStore.GetClientContext(), 4));
            var t2 = new Task<GetPopularProductsResult>(() => this.searcher.GetPopularProducts(
                null, PopularProductTypes.MostViewed, TestDataStore.GetClientContext(), 4));
            var t3 = new Task<GetPopularProductsResult>(() => this.searcher.GetPopularProducts(
                null, PopularProductTypes.MostWished, TestDataStore.GetClientContext(), 4));
            t1.Start();
            t2.Start();
            t3.Start();

            t1.Wait();
            t2.Wait();
            t3.Wait();

            Assert.IsNotNull(t1.Result);
            Assert.IsNotNull(t2.Result);
            Assert.IsNotNull(t3.Result);

            Console.WriteLine(t1.Result.ResultDescription);
            Console.WriteLine(t1.Result.ResultDescription);
            Console.WriteLine(t1.Result.ResultDescription);

            var message = string.Format(
                "Получено {0}, {1}, {2}, а должно быть не 0 для всех",
                t1.Result.PopularProducts.Length,
                t2.Result.PopularProducts.Length,
                t3.Result.PopularProducts.Length);

            Assert.IsTrue(t1.Result.PopularProducts.Any(), message);
            Assert.IsTrue(t2.Result.PopularProducts.Any(), message);
            Assert.IsTrue(t3.Result.PopularProducts.Any(), message);

            foreach (var type in pops)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var result = this.searcher.GetPopularProducts(this.clientId, type, TestDataStore.GetClientContext(), 4);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.PopularProducts.Any());
                sw.Stop();
                Console.WriteLine(sw.ElapsedTicks);
            }
        }

        [TestMethod]
        public void ShouldGetCategoryProductParamsNames()
        {
            var categoryId = 288;

            var param = new CategoryProductParamsParameters()
            {
                CategoryId = categoryId,
                ClientContext = new Dictionary<string, string>()
                {
                    {
                        ClientContextParser.LocationKladrCodeKey, "7700000000000"
                    }
                },
                ProductParams = new[]
                {
                    new CategoryProductParamsParameter()
                    {
                        Name = "Размер оперативной памяти"
                    }
                }
            };

            var result = searcher.GetCategoryProductParams(param);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.ProductParamResult.All(r => r.Name == param.ProductParams[0].Name));

            param.ProductParams[0].Unit = "Мб";

            result = searcher.GetCategoryProductParams(param);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.ProductParamResult.All(r => r.Name == param.ProductParams[0].Name));

            param.ProductParams[0].Unit = "Гб";

            result = searcher.GetCategoryProductParams(param);

            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.ProductParamResult.Any(r => r.Name == param.ProductParams[0].Name));
        }

        [TestMethod]
        public void ShouldGetCategoryAdditionalAttributesByName()
        {
            const string attributeName = "Размер оперативной памяти";
            const string attributeName2 = "Размер оперативной памяти2";

            ////using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            ////{
            ////    var result =
            ////        searcher.GetCategoryProductParamByNames(
            ////            new CategoryProductParamByNamesParameters()
            ////            {
            ////                ProductParamNames = new string[]
            ////                                 {
            ////                                     attributeName, attributeName2
            ////                                 },
            ////                                 ReturnVendors = true
            ////            });

            ////    Assert.IsTrue(result.Success);
            ////    Assert.IsTrue(result.ProductParams.Any());
            ////    Assert.IsTrue(result.ProductParams.All(p => p.Name.Equals(attributeName)));
            ////    Assert.IsTrue(result.Vendors.Any());
            ////}
        }

        #endregion

        #region Methods

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var mock = MockFactory.GetUserService();
            var service = mock.Object;
            ArmSecurity.UserServiceCreator = () => service;

            new TestDeliveryRatesRepository().Create(1, 0, 10000000, 100, "7700000000000");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString);

            new TestDeliveryRatesRepository().CreateIfNotExists(1, 0, 10000000, 100, "7700000000000");

            TestHelper.DeleteTestProduct();

            ArmSecurity.UserServiceCreator = null;
        }

        #endregion

        #region products

        [TestMethod]
        public void ShouldSearchPublicProductsByGuid()
        {
            var productId = TestHelper.GetAnyProductId();
            var parameters = new SearchProductsParameters();
            parameters.ProductIds = new[]
                                    {
                                        productId
                                    };

            parameters.ClientContext = new Dictionary<string, string>
            {
                {
                    ClientContextParser.LocationKladrCodeKey,
                    "7700000000000"
                }
            };

            var result = productsSearcher.SearchPublicProducts(parameters);

            Assert.IsTrue(result.Products[0].ProductId.Equals(productId));


            //StringBuilder sb = new StringBuilder(512);
            //// var ser = new DataContractJsonSerializer(parameters.GetType());


            XmlObjectSerializer ser = new DataContractJsonSerializer(parameters.GetType());
            MemoryStream stream = new MemoryStream();

            ser.WriteObject(stream, parameters);

            stream.Position = 0;
            var sr = new StreamReader(stream);
            string myStr = sr.ReadToEnd();

            Console.WriteLine(myStr);

            Console.WriteLine(string.Empty);

            XmlObjectSerializer ser2 = new DataContractSerializer(parameters.GetType());
            MemoryStream stream2 = new MemoryStream();

            ser2.WriteObject(stream2, parameters);

            stream2.Position = 0;
            var sr2 = new StreamReader(stream2);
            string myStr2 = sr2.ReadToEnd();

            Console.WriteLine(myStr2);


            //stream1.Position = 0;
            //StreamReader sr = new StreamReader(stream1);
            //var strR = ser.Serialize(result);

            //Console.WriteLine(strP);
            //Console.WriteLine(string.Empty);
            //Console.WriteLine(strR);
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByCategory()
        {
            var catId =
                searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters()
                {
                    ClientContext = TestDataStore.GetClientContext()
                })
                         .Categories.First(x => x.ParentId == null && x.ProductsCount > 0)
                         .Id;

            var parameters = new SearchProductsParameters()
            {
                ClientContext = TestDataStore.GetClientContext(),
                ParentCategories = new[]
                    {
                        catId
                    }
            };

            var resultCount1 = productsSearcher.SearchPublicProducts(parameters).Products.Length;
            Assert.IsTrue(resultCount1 > 0);

            parameters.IncludeSubCategory = true;
            var resultCount2 = productsSearcher.SearchPublicProducts(parameters).Products.Length;
            Assert.IsTrue(resultCount2 > 0);

            Assert.IsTrue(resultCount2 >= resultCount1);
        }

        [TestMethod]
        public void ShouldNotSearchPublicProductsByCategoryInBlackList()
        {
            var catId =
                searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters()
                    {
                        ClientContext = TestDataStore.GetClientContext()
                    })
                        .Categories.First(x => x.ParentId == null && x.ProductsCount > 0)
                        .Id;

            var parameters = new SearchProductsParameters()
            {
                ClientContext = TestDataStore.GetClientContext(),
                SortType = SortTypes.RecommendedByPriceRange,
                MinRecommendedPrice = 0,
                MaxRecommendedPrice = 999999999
            };

            var blackListCommand = String.Format(@"delete from prod.ProductCategoriesBlackList;
                                    insert into prod.ProductCategoriesBlackList(CategoryId) values({0})", catId);

            using (var ctx = new LoyaltyDBEntities())
            {
                ctx.Database.ExecuteSqlCommand(blackListCommand);
            }

            var results = productsSearcher.SearchPublicProducts(parameters).Products;
            Assert.IsTrue(results.Length > 0);
            Assert.IsTrue(results.Count(p => p.CategoryId == catId) == 0);

            parameters.IncludeSubCategory = true;
            var results2 = productsSearcher.SearchPublicProducts(parameters).Products;
            Assert.IsTrue(results2.Length > 0);
            Assert.IsTrue(results2.Count(p => p.CategoryId == catId) == 0);

            Assert.IsTrue(results2.Length >= results.Length);
        }

        [TestMethod]
        public void ShouldNotSearchPublicProductsByCategoryInBlackListWithSubcategories()
        {
            var newCategory = TestHelper.NewCategory();
            TestHelper.CreateTestProduct(5, 500, 100, category: newCategory);

            newCategory = TestHelper.NewCategory(parentId: newCategory.Id);
            TestHelper.CreateTestProduct(6, 1000, 200, category: newCategory);

            var parameters = new SearchProductsParameters()
            {
                ClientContext = TestDataStore.GetClientContext(),
                SortType = SortTypes.RecommendedByPriceRange,
                MinRecommendedPrice = 0,
                MaxRecommendedPrice = 999999999
            };

            var blackListCommand = String.Format(@"delete from prod.ProductCategoriesBlackList;
                                    insert into prod.ProductCategoriesBlackList(CategoryId) values({0})", newCategory.ParentId);

            using (var ctx = new LoyaltyDBEntities())
            {
                ctx.Database.ExecuteSqlCommand(blackListCommand);
            }

            var results = productsSearcher.SearchPublicProducts(parameters).Products;
            Assert.IsTrue(results.Length > 0);
            Assert.IsTrue(results.Count(p => p.CategoryId == newCategory.ParentId) == 0);
            Assert.IsTrue(results.Count(p => p.CategoryId == newCategory.Id) == 0);
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByPartnerId()
        {
            var parameters = new SearchProductsParameters();
            parameters.PartnerIds = new[]
            {
                PartnerId
            };
            parameters.ClientContext = new Dictionary<string, string>
            {
                {
                    ClientContextParser.LocationKladrCodeKey,
                    "7700000000000"
                }
            };

            var resultCount = productsSearcher.SearchPublicProducts(parameters).Products.Length;
            Assert.IsTrue(resultCount > 0);
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByNamePartIgnoreCase()
        {
            var parameters = new SearchProductsParameters();
            parameters.SearchTerm = "Ванная";
            parameters.ClientContext = TestDataStore.GetClientContext();

            var result = productsSearcher.SearchPublicProducts(parameters);

            var resultCount = result.Products.Length;
            Assert.IsTrue(resultCount > 0);

            var parametersLower = new SearchProductsParameters();
            parametersLower.SearchTerm = parameters.SearchTerm.ToLower();
            parametersLower.ClientContext = parameters.ClientContext;

            var resultLower = productsSearcher.SearchPublicProducts(parametersLower);
            var resultCountLower = resultLower.Products.Length;

            Assert.AreEqual(resultCount, resultCountLower);
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByDescriptionPartIgnoreCase()
        {
            var parameters = new SearchProductsParameters();
            parameters.SearchTerm = "Многих из нас";
            parameters.ClientContext = TestDataStore.GetClientContext();

            var result = productsSearcher.SearchPublicProducts(parameters);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success, result.ResultDescription);
            Assert.IsNotNull(result.Products);

            Assert.IsTrue(result.Products.Length > 0, string.Format(@"Товар с текстом ""{0}"" в Description не найден", parameters.SearchTerm));
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByVendor()
        {
            var parameters = new SearchProductsParameters();
            parameters.Vendors = new[]
            {
                "Новый Диск / IDEX CT"
            };
            parameters.ClientContext = new Dictionary<string, string>
            {
                {
                    ClientContextParser.LocationKladrCodeKey,
                    "7700000000000"
                }
            };

            var obj = productsSearcher.SearchPublicProducts(parameters);
            Assert.IsTrue(obj.Products.Length > 0);
            
            Assert.IsTrue(obj.Products[0].Price > 0);
            Assert.IsTrue(obj.Products[0].PriceBase > 0);
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByMinPrice()
        {
            var parameters = new SearchProductsParameters();

            // NOTE: Не должен найти товар с ИД "-=TEST=-1". 2600 это после применения механик и доставки.
            parameters.MinPrice = 2600;
            parameters.ClientContext = TestDataStore.GetClientContext();

            var result = productsSearcher.SearchPublicProducts(parameters);

            Assert.IsNotNull(result.Products);
            Assert.IsTrue(result.Products.Length > 0);
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByMaxPrice()
        {
            var parameters = new SearchProductsParameters();

            parameters.MaxPrice = 2000;
            parameters.ClientContext = TestDataStore.GetClientContext();

            var result = productsSearcher.SearchPublicProducts(parameters);

            Assert.IsNotNull(result.Products);
            Assert.IsTrue(result.Products.Length > 0);
        }

        [TestMethod]
        public void ShouldReturnEqualMaxPriceWithDiffentRequest()
        {
            var context = new Dictionary<string, string>
            {
                {
                    ClientContextParser.LocationKladrCodeKey, "7700000000000"
                }
            };

            var parameters = new SearchProductsParameters
            {
                ClientContext = context
            };

            var result1 = productsSearcher.SearchPublicProducts(parameters);

            parameters = new SearchProductsParameters
            {
                MinPrice = 2600,
                ClientContext = context
            };

            var result2 = productsSearcher.SearchPublicProducts(parameters);

            Assert.AreEqual(result1.Success, true);
            Assert.AreEqual(result2.Success, true);
            Assert.AreEqual(result1.MaxPrice, result2.MaxPrice);
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByMinInsertedDate()
        {
            var parameters = new SearchProductsParameters();
            parameters.SortType = SortTypes.ByInsertedDateDescByNameAsc;
            var minInsertedDate = DateTime.Now.AddDays(-10);
            parameters.MinInsertedDate = minInsertedDate;
            parameters.ClientContext = TestDataStore.GetClientContext();

            var products = productsSearcher.SearchPublicProducts(parameters).Products;

            var wrongRes = products.FirstOrDefault(p => p.InsertedDate < minInsertedDate);

            var errMessage = wrongRes == null ? "OK" : wrongRes.InsertedDate + " is less than " + minInsertedDate;

            Assert.IsNull(wrongRes, errMessage);

            var resultCount = products.Length;
            Assert.IsTrue(resultCount > 0);
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByActionPrice()
        {
            var parameters = new SearchProductsParameters();

            parameters.IsActionPrice = true;
            parameters.ClientContext = TestDataStore.GetClientContext();

            var products = productsSearcher.SearchPublicProducts(parameters).Products;

            Assert.IsTrue(products.All(p => p.PriceBase != p.Price));
            Assert.IsTrue(products.All(p => p.IsActionPrice));

            var resultCount = products.Length;
            Assert.IsTrue(resultCount > 0);
        }

        [TestMethod]
        public void ShouldSearchPublicProductsByIds()
        {
            var param = new SearchProductsParameters
            {
                CountToTake = 10,
                ClientContext = TestDataStore.GetClientContext()
            };
            var res = productsSearcher.SearchPublicProducts(param);

            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Products);
            Assert.IsTrue(res.Products.Any());

            var ids = res.Products.Select(x => x.ProductId).ToArray();

            var paramIds = new SearchProductsParameters
                           {
                               ProductIds = ids,
                               ClientContext = TestDataStore.GetClientContext()
                           };

            var resIds = productsSearcher.SearchPublicProducts(paramIds);

            Assert.IsNotNull(resIds);
            Assert.IsNotNull(resIds.Products);
            Assert.IsTrue(res.Products.Any());

            Assert.AreEqual(res.Products.Count(), resIds.Products.Count());
        }
        
        #endregion

        #region categories

        [TestMethod]
        public void ShouldGetPublicSubCategories()
        {
            var result = searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters()
            {
                ClientContext = TestDataStore.GetClientContext()
            });

            Assert.IsTrue(result.Categories.Length > 0);
            Assert.IsTrue(result.Categories.Length <= ApiSettings.MaxResultsCountCategories);
        }

        [TestMethod]
        public void ShouldCalculateTotalCategoriesCount()
        {
            var result = searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters
            {                
                ClientContext = TestDataStore.GetClientContext(),
                CalcTotalCount = true
            });

            Assert.IsTrue(result.TotalCount.HasValue);
        }

        [TestMethod]
        public void ShouldNotCalculateTotalCountTest()
        {
            var result = searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters
            {
                CalcTotalCount = false
            });

            Assert.IsTrue(!result.TotalCount.HasValue);
        }

        [TestMethod]
        public void ShouldTakeCategoriesTest()
        {
            var result = searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters
            {
                ClientContext = TestDataStore.GetClientContext(),
                CountToTake = 2
            });

            Assert.IsTrue(result.Categories.Length == 2);
        }

        [TestMethod]
        public void ShouldSkipCategoriesTest()
        {
            var result1 = searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters
            {
                ClientContext = TestDataStore.GetClientContext(),
                CountToSkip = 0,
                CountToTake = 2
            });
            var result2 = searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters
            {
                ClientContext = TestDataStore.GetClientContext(),
                CountToSkip = 1,
                CountToTake = 1
            });

            Assert.IsTrue(result1.Categories.Length == 2);
            Assert.IsTrue(result2.Categories.Length == 1);
        }

        [TestMethod]
        public void ShouldNotGetCategoryIfParentNotActive()
        {
            var createParentRes = catalogAdminService.CreateCategory(new CreateCategoryParameters()
            {
                UserId = TestDataStore.TestUserId,
                Name = TestDataStore.TestCategoryName + "Parent" + Guid.NewGuid(),
                Status = ProductCategoryStatuses.NotActive
            });

            var createChildRes = catalogAdminService.CreateCategory(new CreateCategoryParameters()
            {
                ParentCategoryId = createParentRes.Category.Id,
                UserId = TestDataStore.TestUserId,
                Name = TestDataStore.TestCategoryName + "Child" + Guid.NewGuid(),
                Status = ProductCategoryStatuses.Active
            });

            var result = searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters()
            {
                ClientContext = TestDataStore.GetClientContext()
            });

            Assert.IsTrue(result.Categories.Length > 0);

            var actualParent = result.Categories.FirstOrDefault(c => c.Id == createParentRes.Category.Id);
            var actualChild = result.Categories.FirstOrDefault(c => c.Id == createChildRes.Category.Id);

            Assert.IsNull(actualParent, "Категория предок не активна, а возвращается");
            Assert.IsNull(actualChild, "Категория содержится в не активной категории, а возвращается");
        }

        [TestMethod]
        public void ShouldGetOnlyStaticPublicCategoriesTest()
        {
            var createCatRes = catalogAdminService.CreateCategory(new CreateCategoryParameters()
            {
                Type = ProductCategoryTypes.Online,
                UserId = TestDataStore.TestUserId,
                Name = TestDataStore.TestCategoryName + Guid.NewGuid(),
                Status = ProductCategoryStatuses.Active,
                OnlineCategoryUrl = "Url"
            });

            var result = searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters()
            {
                ClientContext = TestDataStore.GetClientContext(),
                Type = ProductCategoryTypes.Static
            });

            Assert.IsTrue(result.Categories.Length > 0);

            var wrongCat = result.Categories.FirstOrDefault(c => c.Type == ProductCategoryTypes.Online);

            Assert.IsNull(wrongCat, "Присутствует динамическая категория, а запрашивались статические");
        }

        [TestMethod]
        public void ShouldGetOnlyDynamicPublicCategoriesTest()
        {
            new ProductsDataSource().DeleteCache();

            var createCatRes1 =
                catalogAdminService.CreateCategory(
                    new CreateCategoryParameters
                        {
                            Type = ProductCategoryTypes.Static,
                            UserId = TestDataStore.TestUserId,
                            Name = TestDataStore.TestCategoryName + Guid.NewGuid(),
                            Status = ProductCategoryStatuses.Active
                        });

            var createCatRes2 =
                catalogAdminService.CreateCategory(
                    new CreateCategoryParameters
                        {
                            Type = ProductCategoryTypes.Online,
                            UserId = TestDataStore.TestUserId,
                            Name = TestDataStore.TestCategoryName + Guid.NewGuid(),
                            Status = ProductCategoryStatuses.Active,
                            OnlineCategoryUrl = "www.www.ru"
                        });

            var parameters = new GetPublicSubCategoriesParameters
                                 {
                                     ClientContext = TestDataStore.GetClientContext(),
                                     Type = ProductCategoryTypes.Online
                                 };
            var result = searcher.GetPublicSubCategories(parameters);

            Assert.IsTrue(result.Categories.Length > 0);

            var wrongCat = result.Categories.FirstOrDefault(c => c.Type == ProductCategoryTypes.Static);

            Assert.IsNull(wrongCat, "Присутствует статическая категория, а запрашивались динамические");
        }

        #endregion

        #region product

        [TestMethod]
        public void ShouldGetProduct()
        {
            var productId = TestHelper.GetAnyProductId();
            var parameters = new GetProductByIdParameters
            {
                ProductId = productId,
                ClientId = clientId,
                ClientContext = TestDataStore.GetClientContext()
            };

            var result = searcher.GetProductById(parameters);

            TestHelper.AssertResult(result);
            
            var product = result.Product;

            Assert.IsNotNull(product);
            Assert.IsTrue(product.ProductId.Equals(productId));
        }

        [TestMethod]
        public void ShouldGetCategoryPath()
        {
            new ProductsDataSource().DeleteCache();

            var parameters = new GetPublicSubCategoriesParameters { ClientContext = TestDataStore.GetClientContext() };
            var result = searcher.GetPublicSubCategories(parameters);

            var category = result.Categories.Last(c => c.ParentId != null);

            var info = searcher.GetCategoryInfo(new GetCategoryInfoParameters
            {
                ClientContext = TestDataStore.GetClientContext(),
                CategoryId = category.Id
            });

            Assert.IsTrue(info.CategoryPath.Last().Id == category.Id);
            Assert.IsTrue(info.Category.Id == category.Id);
            
            // NOTE: Мне не понятно на основе чего сделаны след. проверки поэтому заменил на логичные, как мне кажется - FSY
            // Assert.IsTrue(info.Category.NestingLevel > 0, "Nesting level not > 0");
            // Assert.IsTrue(info.Category.SubCategoriesCount > 0);
            Assert.IsTrue(info.Category.NestingLevel == category.NestingLevel);
            Assert.IsTrue(info.Category.SubCategoriesCount == category.SubCategoriesCount);
        }

        [TestMethod]
        public void ShouldNotReturnNotActiveCategory()
        {
            var category = TestHelper.NewCategory(status: ProductCategoryStatuses.NotActive);

            new ProductsDataSource().DeleteCache();

            var parameters = new GetCategoryInfoParameters
                                 {
                                     CategoryId = category.Id,
                                     ClientContext = TestDataStore.GetClientContext()
                                 };
            var info = searcher.GetCategoryInfo(parameters);

            Assert.IsNotNull(info);
            Assert.AreEqual(false, info.Success);
            Assert.AreEqual(ResultCodes.CATEGORY_NOT_FOUND, info.ResultCode);
        }

        [TestMethod]
        public void ShouldNotReturnProductsInDeactivatedCategory()
        {
            var category = TestHelper.NewCategory(null, ProductCategoryStatuses.NotActive);

            var parameters = new CreateProductParameters()
            {
                UserId = TestDataStore.TestUserId,
                PartnerId = TestDataStore.OzonPartnerId,
                PartnerProductId = "deactivatedCategoryOffer_" + Guid.NewGuid(),
                CategoryId = category.Id,
                Name = "testDeactivatedCategory",
                Description = "Товар в деактивированной категории. Не возвращается из ICatalogSearcher.SearchPublicProducts",
                PriceRUR = 160
            };

            var product = TestHelper.NewPublicProduct(parameters);

            new ProductsDataSource().DeleteCache();

            var param = new SearchProductsParameters()
            {
                ClientContext = TestDataStore.GetClientContext(),
                ProductIds = new string[] { product.ProductId }
            };

            var res = productsSearcher.SearchPublicProducts(param);

            Assert.IsTrue(res.Success, res.ResultDescription);
            Assert.IsNotNull(res.Products);
            Assert.AreEqual(0, res.Products.Length, "Товар не должен быть возвращён. Категория деактивирована");
        }

        [TestMethod]
        public void ShouldSearchEmailDeliveredProductsIndependentlyOfLocation()
        {
            var clientContext = new Dictionary<string, string>();
            clientContext.Add(ClientContextParser.LocationKladrCodeKey, "9700000000000"); // <- Не существующий КЛАДР

            var result = productsSearcher.SearchPublicProducts(new SearchProductsParameters
            {
                CalcTotalCount = true,
                IncludeSubCategory = true,
                ClientContext = clientContext
            });

            Assert.IsTrue(result.TotalCount != 0);
            Assert.IsTrue(result.Products.All(p => p.IsDeliveredByEmail));
        }

        #endregion

        #region partners

        [TestMethod]
        public void ShouldGetAllPartners()
        {
            var result = searcher.GetAllPartners();

            Assert.IsTrue(result.Partners.Length > 0);
        }

        #endregion

        #region searchOptions

        [TestMethod]
        public void CatalogSearcher_ShouldCalculateMaxPrice()
        {
            var parameters = new SearchProductsParameters();
            var clientContext = new Dictionary<string, string>();
            clientContext.Add(ClientContextParser.LocationKladrCodeKey, "7700000000000");
            parameters.ClientContext = clientContext;

            var result = productsSearcher.SearchPublicProducts(parameters);

            Assert.IsTrue(result.MaxPrice > 0);
        }

        [TestMethod]
        public void CatalogSearcher_ShouldCalculateTotalCount()
        {
            var parameters = new SearchProductsParameters();
            var clientContext = new Dictionary<string, string>();
            clientContext.Add(ClientContextParser.LocationKladrCodeKey, "7700000000000");
            parameters.ClientContext = clientContext;
            parameters.CalcTotalCount = true;

            var result = productsSearcher.SearchPublicProducts(parameters);

            Assert.IsTrue(result.TotalCount > 0);
        }

        #endregion

        #region clientContext

        [TestMethod]
        public void GetPublicSubCategoriesShouldDependsOnLocation()
        {
            var context = new Dictionary<string, string>
                              {
                                  { ClientContextParser.LocationKladrCodeKey, "9700000000000" }
                              };

            var getPublicSubCategoriesParameters = new GetPublicSubCategoriesParameters { ClientContext = context };
            new ProductsDataSource().DeleteCache();
            var result = searcher.GetPublicSubCategories(getPublicSubCategoriesParameters);

            new TestDeliveryRatesRepository().Create(TestDataStore.PartnerId, 0, 10000000, 100, "7700000000000");

            Assert.IsTrue(result.Categories[0].ProductsCount == 0);

            var clientContext = new Dictionary<string, string>();
            clientContext.Add(ClientContextParser.LocationKladrCodeKey, "7700000000000");

            var result2 = searcher.GetPublicSubCategories(new GetPublicSubCategoriesParameters
            {
                ClientContext = clientContext
            });
            Assert.IsTrue(result2.Categories.Any(x => x.ParentId == null && x.ProductsCount > 0));
        }

        [TestMethod]
        public void ShouldDependsOnLocation()
        {
            var clientContext = new Dictionary<string, string>();
            clientContext.Add(ClientContextParser.LocationKladrCodeKey, "9700000000000"); // <- Не существующий КЛАДР

            var result = productsSearcher.SearchPublicProducts(new SearchProductsParameters
            {
                CalcTotalCount = true,
                IncludeSubCategory = true,
                ClientContext = clientContext
            });
            Assert.IsTrue(result.Products.Count(p => !p.IsDeliveredByEmail) == 0);

            new TestDeliveryRatesRepository().Create(TestDataStore.PartnerId, 0, 10000000, 100, "7700000000000");

            clientContext = new Dictionary<string, string>();
            clientContext.Add(ClientContextParser.LocationKladrCodeKey, "7700000000000");

            var result2 = productsSearcher.SearchPublicProducts(new SearchProductsParameters
            {
                CalcTotalCount = true,
                IncludeSubCategory = true,
                ClientContext = clientContext
            });
            Assert.IsTrue(result2.TotalCount > 0);
        }

        #endregion
        
        [TestMethod]
        public void ShouldNotChangeModarationStatusWhenChangePriceForLowTrustedPartner()
        {
            // NOTE: Партнер с низким уровнем доверия.
            var partners =
                new PartnerRepository().GetAllPartners().Where(x => x.ThrustLevel == PartnerThrustLevel.Low).ToArray();
            Assert.IsNotNull(partners);
            Assert.IsTrue(partners.Any());

            // NOTE: Находим продукт
            var param = new SearchProductsParameters()
                        {
                            PartnerIds = partners.Select(x => x.Id).ToArray(),
                            CountToSkip = 0,
                            CountToTake = 1,
                            ClientContext = TestDataStore.GetClientContext()
                        };

            SearchProductsResult products = productsSearcher.SearchPublicProducts(param);

            Assert.IsNotNull(products);
            Assert.IsNotNull(products.Products);

            var product = products.Products.FirstOrDefault();
            
            Assert.IsNotNull(product);

            var productIds = new[] { product.ProductId };

            var service = MockFactory.GetCatalogAdminService();

            // NOTE: Меняем статус модерации
            var changeModerationStatusParameters = new ChangeModerationStatusParameters
                                                   {
                                                       ProductIds = productIds,
                                                       ProductModerationStatus = ProductModerationStatuses.Applied,
                                                       UserId = Settings.ApiSettings.ClientSiteUserName
                                                   };

            var resultAfterFirst = service.ChangeProductsModerationStatus(changeModerationStatusParameters);

            Assert.IsNotNull(resultAfterFirst);
            Assert.AreEqual(true, resultAfterFirst.Success);

            // NOTE: Проверяем статус после первой модерации
            var searchProductAfterFirst = new SearchProductsParameters()
                                          {
                                              ProductIds = productIds,
                                              ClientContext = TestDataStore.GetClientContext()
                                          };

            SearchProductsResult productsAfterFirst = productsSearcher.SearchPublicProducts(searchProductAfterFirst);

            Assert.IsNotNull(productsAfterFirst);
            Assert.IsNotNull(productsAfterFirst.Products);

            var productAfterFirst = productsAfterFirst.Products.FirstOrDefault();

            Assert.IsNotNull(productAfterFirst);
            Assert.AreEqual(ProductModerationStatuses.Applied, productAfterFirst.ModerationStatus);

            // NOTE: Меняем цену
            var firstUpdate = new UpdateProductParameters
                              {
                                  ProductId = productAfterFirst.ProductId,
                                  CategoryId = productAfterFirst.CategoryId,
                                  CurrencyId = productAfterFirst.CurrencyId,
                                  Description = productAfterFirst.Description,
                                  Name = productAfterFirst.Name,
                                  Param = productAfterFirst.Param,
                                  PartnerId = productAfterFirst.PartnerId,
                                  Vendor = productAfterFirst.Vendor,
                                  Weight = productAfterFirst.Weight.HasValue ? productAfterFirst.Weight.Value : 0,
                                  UserId = Settings.ApiSettings.ClientSiteUserName,
                                  PriceRUR = productAfterFirst.PriceRUR * 2,
                                  Pictures = productAfterFirst.Pictures
                              };

            var priceUpdateFirst = service.UpdateProduct(firstUpdate);

            Assert.IsNotNull(priceUpdateFirst);
            Assert.AreEqual(true, priceUpdateFirst.Success, priceUpdateFirst.ResultDescription);

            // NOTE: Проверяем статус после первой изменения цены
            var searchProductAfterUpdateFirst = new SearchProductsParameters
            {
                ProductIds = productIds,
                ClientContext = TestDataStore.GetClientContext()
            };

            SearchProductsResult productsAfterUpdateFirst = productsSearcher.SearchPublicProducts(searchProductAfterUpdateFirst);

            Assert.IsNotNull(productsAfterUpdateFirst);
            Assert.IsNotNull(productsAfterUpdateFirst.Products);

            var productAfterUpdateFirst = productsAfterUpdateFirst.Products.FirstOrDefault();

            Assert.IsNotNull(productAfterUpdateFirst);
            Assert.AreEqual(
                ProductModerationStatuses.Applied,
                productAfterUpdateFirst.ModerationStatus,
                "Так как изменилась только цена, статус модерации должен остаться прежним");
        }
    }
}