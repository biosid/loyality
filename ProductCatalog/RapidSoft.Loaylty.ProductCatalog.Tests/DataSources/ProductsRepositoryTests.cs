namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;

    [TestClass]
    public class ProductsRepositoryTests
    {
        private const string UserId = TestDataStore.TestUserId;

        [TestMethod]
        public void ShouldChangeStatuses()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var product = ctx.ProductSortProjections.First();
                var product2 = ctx.ProductSortProjections.First(p => p.PartnerId != product.PartnerId);

                var param = new ChangeStatusParameters()
                {
                    UserId = UserId,
                    ProductIds = new[]
                    {
                        product.ProductId,
                        product2.ProductId
                    },
                    ProductStatus = ProductStatuses.NotActive
                };

                var repo = new ProductsRepository();

                var result = repo.ChangeStatuses(param);

                Assert.IsNotNull(result);

                product = repo.GetById(product.ProductId);
                product2 = repo.GetById(product2.ProductId);

                Assert.IsNotNull(product);
                Assert.IsNotNull(product2);
                Assert.AreEqual(ProductStatuses.NotActive, product.Status);
                Assert.AreEqual(ProductStatuses.NotActive, product2.Status);

                param.ProductStatus = ProductStatuses.Active;

                result = repo.ChangeStatuses(param);

                Assert.IsNotNull(result);

                product = repo.GetById(product.ProductId);
                product2 = repo.GetById(product2.ProductId);

                Assert.IsNotNull(product);
                Assert.IsNotNull(product2);
                Assert.AreEqual(ProductStatuses.Active, product.Status);
                Assert.AreEqual(ProductStatuses.Active, product2.Status);
            }
        }

        [TestMethod]
        public void ShouldChangeModerationStatuses()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var product = ctx.ProductSortProjections.First();
                var product2 = ctx.ProductSortProjections.First(p => p.PartnerId != product.PartnerId);

                var param = new ChangeModerationStatusParameters()
                {
                    UserId = UserId,
                    ProductIds = new[]
                    {
                        product.ProductId,
                        product2.ProductId
                    },
                    ProductModerationStatus = ProductModerationStatuses.Applied
                };

                var repo = new ProductsRepository();

                var result = repo.ChangeStatuses(param);

                Assert.IsNotNull(result);
                
                product = repo.GetById(product.ProductId);
                product2 = repo.GetById(product2.ProductId);

                Assert.IsNotNull(product);
                Assert.IsNotNull(product2);
                Assert.AreEqual(ProductModerationStatuses.Applied, product.ModerationStatus);
                Assert.AreEqual(ProductModerationStatuses.Applied, product2.ModerationStatus);
                Assert.AreEqual(UserId, product.UpdatedUserId);
                Assert.AreEqual(UserId, product2.UpdatedUserId);
            }
        }

        [TestMethod]
        public void ShouldChangeRecommendedStatuses()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var product = ctx.ProductSortProjections.First();
                var product2 = ctx.ProductSortProjections.First(p => p.PartnerId != product.PartnerId);

                var param = new RecommendParameters
                {
                    UserId = UserId,
                    ProductIds = new[]
                    {
                        product.ProductId,
                        product2.ProductId
                    },
                    IsRecommended = true
                };

                var repo = new ProductsRepository();

                var result = repo.ChangeStatuses(param);

                Assert.IsNotNull(result);

                product = repo.GetById(product.ProductId);
                product2 = repo.GetById(product2.ProductId);

                Assert.IsNotNull(product);
                Assert.IsNotNull(product2);
                Assert.AreEqual(true, product.IsRecommended);
                Assert.AreEqual(true, product2.IsRecommended);
                Assert.AreEqual(UserId, product.UpdatedUserId);
                Assert.AreEqual(UserId, product2.UpdatedUserId);

                param.IsRecommended = false;

                result = repo.ChangeStatuses(param);

                Assert.IsNotNull(result);

                product = repo.GetById(product.ProductId);
                product2 = repo.GetById(product2.ProductId);

                Assert.IsNotNull(product);
                Assert.IsNotNull(product2);
                Assert.AreEqual(false, product.IsRecommended);
                Assert.AreEqual(false, product2.IsRecommended);
                Assert.AreEqual(UserId, product.UpdatedUserId);
                Assert.AreEqual(UserId, product2.UpdatedUserId);
            }
        }

        [TestMethod]
        public void ShouldMoveProducts()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var product = ctx.ProductSortProjections.First();
                var product2 = ctx.ProductSortProjections.First(p => p.PartnerId != product.PartnerId);

                var targetCategory = ctx.ProductCategories.FirstOrDefault();
                
                Assert.IsNotNull(product);
                Assert.IsNotNull(product2);
                Assert.IsNotNull(targetCategory);

                Assert.AreNotEqual(product.CategoryId, targetCategory.Id);
                Assert.AreNotEqual(product2.CategoryId, targetCategory.Id);

                var param = new MoveProductsParameters()
                {
                    UserId = UserId,
                    ProductIds = new[]
                    {
                        product.ProductId,
                        product2.ProductId
                    },
                    
                    TargetCategoryId = targetCategory.Id
                };

                var oldCategoryId = product.CategoryId;
                var oldCategoryId2 = product2.CategoryId;
                
                var repo = new ProductsRepository();
                
                var result = repo.MoveProducts(param);

                Assert.IsNotNull(result);
                
                product = repo.GetById(product.ProductId);
                product2 = repo.GetById(product2.ProductId);

                Assert.IsNotNull(product);
                Assert.IsNotNull(product2);
                Assert.AreEqual(product.CategoryId, targetCategory.Id);
                Assert.AreEqual(product2.CategoryId, targetCategory.Id);

                param.TargetCategoryId = oldCategoryId;
                param.ProductIds = new string[] { product.ProductId };

                result = repo.MoveProducts(param);
                Assert.IsNotNull(result);

                param.TargetCategoryId = oldCategoryId2;
                param.ProductIds = new string[] { product2.ProductId };

                result = repo.MoveProducts(param);
                Assert.IsNotNull(result);
                
                product = repo.GetById(product.ProductId);
                product2 = repo.GetById(product2.ProductId);
                
                Assert.IsNotNull(product);
                Assert.IsNotNull(product2);
                Assert.AreEqual(product.CategoryId, oldCategoryId);
                Assert.AreEqual(product2.CategoryId, oldCategoryId2);
            }
        }
    }
}
