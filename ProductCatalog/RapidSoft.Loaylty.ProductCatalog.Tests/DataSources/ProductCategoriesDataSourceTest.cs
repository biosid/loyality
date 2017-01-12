namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ProductCatalog.Services;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.ImportTests;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class ProductCategoriesDataSourceTest
    {
        private readonly List<int> ids = new List<int>();
        private string userId = TestDataStore.TestUserId;
        private int partnerId = TestDataStore.OzonPartnerId;

        [TestCleanup]
        public void MyTestCleanup()
        {
            if (this.ids.Count > 0)
            {
                const string SQL = "DELETE FROM [prod].[ProductCategories] WHERE [Id] IN ({0})";
                using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
                {
                    conn.Open();
                    using (var comm = conn.CreateCommand())
                    {
                        comm.CommandText = string.Format(SQL, string.Join(",", this.ids));
                        comm.ExecuteNonQuery();
                    }
                }
            }
        }

        [TestMethod]
        public void ShouldUpdateChildCategory()
        {
            var ds = new ProductCategoriesDataSource();

            var categoryParametersParent = new CreateCategoryParameters
                                               {
                                                   Name = "Test1_" + Guid.NewGuid(),
                                                   Status = ProductCategoryStatuses.Active,
                                                   UserId = "FSY"
                                               };

            var catParent = ds.CreateProductCategory(categoryParametersParent);
            this.ids.Add(catParent.Id);

            var categoryParametersChild = new CreateCategoryParameters
                                              {
                                                  Name = "Test1Child_" + Guid.NewGuid(),
                                                  Status = ProductCategoryStatuses.NotActive,
                                                  UserId = "FSY",
                                                  ParentCategoryId = catParent.Id
                                              };
            var catChild = ds.CreateProductCategory(categoryParametersChild);
            this.ids.Add(catChild.Id);

            var categoryParametersChildChild = new CreateCategoryParameters
                                                   {
                                                       Name = "Test1ChildChild_" + Guid.NewGuid(),
                                                       Status = ProductCategoryStatuses.NotActive,
                                                       UserId = "FSY",
                                                       ParentCategoryId = catChild.Id
                                                   };
            var catChildChild = ds.CreateProductCategory(categoryParametersChildChild);
            this.ids.Add(catChildChild.Id);

            var updateCategoryParameters = new UpdateCategoryParameters
                                               {
                                                   NewName = "Test1ChildNewName_" + Guid.NewGuid(),
                                                   CategoryId = catChild.Id,
                                                   UserId = "FSY"
                                               };
            var updatedCatChild = ds.UpdateCategory(updateCategoryParameters);
            Assert.IsTrue(updatedCatChild.NamePath.Contains(catParent.Name), "Путь категории должен измениться");

            var childChild = ds.GetProductCategoryById(catChildChild.Id);
            Assert.IsTrue(childChild.NamePath.Contains(updatedCatChild.Name), "Пути дочерних категорий должны измениться");
        }

        [TestMethod]
        public void ShouldCorrectCalculateProductCounts()
        {
            var suffix = "ShouldCorrectCalculateProductCounts";

            // NOTE: Убраем за собой
            using (var ctx = new LoyaltyDBEntities())
            {
                var sql =
                    string.Format(
                        "DELETE FROM [prod].[ProductTargetAudiences] WHERE [TargetAudienceId] LIKE '% {0}'", suffix);
                ctx.Database.ExecuteSqlCommand(sql);
            }

            ClearCashe();
            UpdateProductsFromAllPartners();

            new TestDeliveryRatesRepository().CreateIfNotExists(
                partnerId, 0, 100000, 100, TestDataStore.KladrCode);

            var repo = new ProductsRepository();
            var ds = new ProductCategoriesDataSource();

            var categories = ds.GetPublicCategories(
                TestDataStore.GetSqlPrice(),
                TestDataStore.KladrCode,
                ProductCategoryStatuses.Active,
                countToTake: 500);

            Assert.IsNotNull(categories);
            Assert.IsNotNull(categories.Categories);
            Assert.IsTrue(categories.Categories.Length > 0);

            var cat = categories.Categories.FirstOrDefault(x => x.ProductsCount > 2);
            Assert.IsNotNull(cat, "Тест не возможен если нет ни одной категории с более чем 2 товарами");

            var catId = cat.Id;

            // NOTE: Берем чуть больше чем в категории
            var param = new SearchPublicProductsParameters()
                            {                               
                                ClientContext = TestDataStore.GetClientContext(),
                                ParentCategories = catId.MakeArray(),
                                CountToTake = (int)cat.ProductsCount + 5
                            };

            var mechMock = Tests.MockFactory.GetMechanicsProvider();
            var productsSearcher = new ProductsSearcher(new ProductsDataSource(), new ProductAttributeRepository(), mechMock.Object);
            var products = new CatalogSearcher(productsSearcher, new PartnersSearcher(), new CategoriesSearcher(), new ProductViewStatisticRepository()).SearchPublicProducts(param);

            Assert.IsNotNull(products);
            Assert.IsNotNull(products.Products);
            var product0 = products.Products.First();
            var product1 = products.Products.Skip(1).First();

            var ta1 = "ЦА1 " + suffix;
            var ta2 = "ЦА2 " + suffix;

            // NOTE: 1. Если продукты не добавлены в TA
            var categories1 = ds.GetPublicCategories(
                TestDataStore.GetSqlPrice(),
                TestDataStore.KladrCode,
                ProductCategoryStatuses.Active,
                countToTake: 500,
                audienceIds: ta1 + ";" + ta2);
            Assert.IsNotNull(categories1);
            Assert.IsNotNull(categories1.Categories);
            Assert.IsTrue(categories1.Categories.Length > 0);

            var cat1 = categories1.Categories.FirstOrDefault(x => x.Id == cat.Id);

            Assert.IsNotNull(cat1);
            Assert.AreEqual(
                cat.ProductsCount,
                cat1.ProductsCount,
                "Если продукты не добавлены в TA, то кол-во должно быть таким же как и без указания TA");

            // NOTE: 2.1 Добавляем продукт 0 в TA1
            repo.AddProductTargetAudiences(TestDataStore.TestUserId, product0.ProductId.MakeArray(), ta1.MakeArray());

            // NOTE: 2.2 Теперь для пользователя из ЦА2 продукт 0 НЕ доступен
            var categories2 = ds.GetPublicCategories(
                TestDataStore.GetSqlPrice(),
                TestDataStore.KladrCode,
                ProductCategoryStatuses.Active,
                countToTake: 500,
                audienceIds: ta2);
            Assert.IsNotNull(categories2);
            Assert.IsNotNull(categories2.Categories);
            Assert.IsTrue(categories2.Categories.Length > 0);

            var cat2 = categories2.Categories.FirstOrDefault(x => x.Id == cat.Id);

            Assert.IsNotNull(cat2);
            Assert.AreEqual(
                cat.ProductsCount - 1,
                cat2.ProductsCount,
                "Если один продукт добавлен в TA, то кол-во должно быть на ед. меньше при поиске по другой TA");

            // NOTE: 3 Для пользователя из ЦА1 продукт 0 доступен
            var categories3 = ds.GetPublicCategories(
               TestDataStore.GetSqlPrice(),
               TestDataStore.KladrCode,
               ProductCategoryStatuses.Active,
               countToTake: 500,
               audienceIds: ta1 + ";" + ta2);
            Assert.IsNotNull(categories3);
            Assert.IsNotNull(categories3.Categories);
            Assert.IsTrue(categories3.Categories.Length > 0);

            var cat3 = categories3.Categories.FirstOrDefault(x => x.Id == cat.Id);

            Assert.IsNotNull(cat3);
            Assert.AreEqual(
                cat.ProductsCount,
                cat3.ProductsCount,
                "Если один продукт добавлен в TA, но поиск и по той же TA, то кол-во должно быть таким же как и без указания TA");

            // NOTE: 4.1 Добавляем продукт 0 в TA2
            repo.AddProductTargetAudiences(TestDataStore.TestUserId, product0.ProductId.MakeArray(), ta2.MakeArray());

            ClearCashe();

            // NOTE: 4.2 Теперь для пользователя из ЦА2 продукт 0 НЕ доступен
            var categories4 = ds.GetPublicCategories(
                TestDataStore.GetSqlPrice(),
                TestDataStore.KladrCode,
                ProductCategoryStatuses.Active,
                countToTake: 500,
                audienceIds: ta2);
            Assert.IsNotNull(categories4);
            Assert.IsNotNull(categories4.Categories);
            Assert.IsTrue(categories4.Categories.Length > 0);

            var cat4 = categories4.Categories.FirstOrDefault(x => x.Id == cat.Id);

            Assert.IsNotNull(cat4);
            Assert.AreEqual(
                cat.ProductsCount,
                cat4.ProductsCount,
                "Если один продукт добавлен в TA, и поиск по той же TA, то кол-во должно быть таким же как и без указания TA");

            // NOTE: 5.1 Добавляем продукт 1 в TA1
            repo.AddProductTargetAudiences(TestDataStore.TestUserId, product1.ProductId.MakeArray(), ta1.MakeArray());

            ClearCashe();

            // NOTE: 5.2 Поиск по TA2 не должен увидить продукт
            var categories5 = ds.GetPublicCategories(
               TestDataStore.GetSqlPrice(),
                TestDataStore.KladrCode,
                ProductCategoryStatuses.Active,
               countToTake: 500,
               audienceIds: ta2);
            Assert.IsNotNull(categories5);
            Assert.IsNotNull(categories5.Categories);
            Assert.IsTrue(categories5.Categories.Length > 0);

            var cat5 = categories5.Categories.FirstOrDefault(x => x.Id == cat.Id);

            Assert.IsNotNull(cat5);
            Assert.AreEqual(
                cat.ProductsCount - 1,
                cat5.ProductsCount,
                "Если один продукт добавлен в TA, и по ДРУГОЙ той же TA, то кол-во должно быть на ед. меньше");

            // NOTE: 5.3 Поиск по TA1 не должен увидить продукт
            var categories6 = ds.GetPublicCategories(
               TestDataStore.GetSqlPrice(),
                TestDataStore.KladrCode,
                ProductCategoryStatuses.Active,
               countToTake: 500,
               audienceIds: ta1);
            Assert.IsNotNull(categories6);
            Assert.IsNotNull(categories6.Categories);
            Assert.IsTrue(categories6.Categories.Length > 0);

            var cat6 = categories6.Categories.FirstOrDefault(x => x.Id == cat.Id);

            Assert.IsNotNull(cat6);
            Assert.AreEqual(
                cat.ProductsCount,
                cat6.ProductsCount,
                "Если один продукт добавлен в TA, но поиск и по той же TA, то кол-во должно быть таким же как и без указания TA");

            // NOTE: Чистим за собой
            repo.RemoveProductTargetAudiences(userId, product1.ProductId.MakeArray());
            repo.RemoveProductTargetAudiences(userId, product0.ProductId.MakeArray());
        }

        private static void ClearCashe()
        {
            new ProductsDataSource().DeleteCache();
        }

        private static void UpdateProductsFromAllPartners()
        {
            new ProductsDataSource().UpdateProductsFromAllPartners();
        }

        [TestMethod]
        public void ShouldReturnCategoriesForAdmin()
        {
            var ds = new ProductCategoriesDataSource();

            var adminResult = ds.AdminGetCategories(null, null, null, 5000, null, true, true, null);

            Assert.IsNotNull(adminResult);
            Assert.IsNotNull(adminResult.TotalCount);
            Assert.IsNotNull(adminResult.Categories);

            const string SQLCount = "SELECT COUNT(*) FROM prod.ProductCategories";
            var count = SQLCount.ExecuteScalar(DataSourceConfig.ConnectionString);
            Assert.AreEqual(Convert.ToInt32(count), adminResult.TotalCount);

            const string SQLTopCount = "SELECT COUNT(*) FROM prod.ProductCategories WHERE ParentId IS NULL";
            var topCount = SQLTopCount.ExecuteScalar(DataSourceConfig.ConnectionString);
            Assert.AreEqual(Convert.ToInt32(topCount), adminResult.ChildrenCount);

            Assert.AreEqual(adminResult.TotalCount, adminResult.Categories.Count());

            var maxProductsCount = adminResult.Categories.Max(x => x.ProductsCount);

            Assert.IsTrue(maxProductsCount > 0);
            var catWithProducts =
                adminResult.Categories.Where(x => x.ProductsCount < maxProductsCount)
                           .OrderByDescending(x => x.ProductsCount)
                           .Take(5);

            const string SqlProductCountFormat = @"SELECT COUNT(DISTINCT p.ProductId) 
FROM prod.ProductCategories pc
JOIN prod.Products p ON pc.Id = p.CategoryId
WHERE pc.NamePath LIKE '{0}%'";
            const string SqlChildCount = @"SELECT COUNT(pc.Id)
FROM prod.ProductCategories pc
WHERE pc.ParentId = {0}";

            foreach (var productCategory in catWithProducts)
            {
                var sqlProductsCount = string.Format(SqlProductCountFormat, productCategory.NamePath);
                var sqlChildCount = string.Format(SqlChildCount, productCategory.Id);

                var productsCount = sqlProductsCount.ExecuteScalar(DataSourceConfig.ConnectionString);
                Assert.AreEqual(Convert.ToInt32(productsCount), productCategory.ProductsCount);

                var childCount = sqlChildCount.ExecuteScalar(DataSourceConfig.ConnectionString);
                Assert.AreEqual(Convert.ToInt32(childCount), productCategory.SubCategoriesCount);
            }

            var cat = adminResult.Categories.First(x => x.ProductsCount == maxProductsCount);

            var adminResultWithoutSelf = ds.AdminGetCategories(null, cat.Id, null, 5000, null, true, false, null);
            var adminResultWithSelf = ds.AdminGetCategories(null, cat.Id, null, 5000, null, true, true, null);

            Assert.IsNotNull(adminResultWithoutSelf);
            Assert.IsNotNull(adminResultWithSelf);

            Assert.AreEqual(adminResultWithoutSelf.Categories.Count() + 1, adminResultWithSelf.Categories.Count());

            Assert.AreEqual(adminResultWithoutSelf.TotalCount, adminResultWithoutSelf.TotalCount);

            foreach (var productCategory in adminResultWithSelf.Categories)
            {
                var sqlProductsCount = string.Format(SqlProductCountFormat, productCategory.NamePath);
                var sqlChildCount = string.Format(SqlChildCount, productCategory.Id);

                var productsCount = sqlProductsCount.ExecuteScalar(DataSourceConfig.ConnectionString);
                Assert.AreEqual(Convert.ToInt32(productsCount), productCategory.ProductsCount);

                var childCount = sqlChildCount.ExecuteScalar(DataSourceConfig.ConnectionString);
                Assert.AreEqual(Convert.ToInt32(childCount), productCategory.SubCategoriesCount);
            }
        }

        [TestMethod]
        public void ShouldReturnRootCategoriesForAdmin()
        {
            var ds = new ProductCategoriesDataSource();

            var adminResult = ds.AdminGetCategories(null, null, 1, 5000, null, true, true, null);

            Assert.IsNotNull(adminResult);
            Assert.IsNotNull(adminResult.TotalCount);
            Assert.IsNotNull(adminResult.Categories);

            Assert.AreEqual(adminResult.TotalCount, adminResult.Categories.Count());
            Assert.IsTrue(adminResult.Categories.All(x => x.ParentId == null));

            const string SQLRootCount = "SELECT COUNT(*) FROM prod.ProductCategories WHERE ParentId IS NULL";
            var rootCount = SQLRootCount.ExecuteScalar(DataSourceConfig.ConnectionString);
            Assert.AreEqual(Convert.ToInt32(rootCount), adminResult.Categories.Count());
        }
    }
}