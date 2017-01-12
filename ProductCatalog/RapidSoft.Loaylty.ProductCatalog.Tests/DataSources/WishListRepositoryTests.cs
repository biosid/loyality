namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using ClientProfile.ClientProfileService;

    using Interfaces;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using ProductCatalog.DataSources.Repositories;
    using ProductCatalog.Services;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;

    ////using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientInboxService;

    using MockFactory = RapidSoft.Loaylty.ProductCatalog.Tests.MockFactory;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class WishListRepositoryTests
    {
        private readonly string connectionString;
        
        public WishListRepositoryTests()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["LoyaltyProductCatalogDB"].ConnectionString;
        }

        [TestMethod]
        public void WishListRepository_GetWishList_ShouldNotReturnNull()
        {
            var repository = new WishListRepository();

            var result = repository.GetWishList(Guid.NewGuid().ToString() + Guid.NewGuid().ToString());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void WishListRepository_GetWishList_ShouldReturn()
        {
            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();

            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var item = new WishListItem { CreatedDate = DateTime.Now, ClientId = userId, ProductId = productId };
                ctx.Entry(item).State = EntityState.Added;

                ctx.SaveChanges();
            }

            var repository = new WishListRepository();

            var result = repository.GetWishList(userId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count >= 1);
        }

        [TestMethod]
        public void WishListRepository_Add_WithHistory()
        {
            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();

            var repository = new WishListRepository();
            repository.IncreaseQuantityOrCreate(productId, userId);

            var ctx = new LoyaltyDBEntities(this.connectionString);

            Assert.IsTrue(ctx.WishListItems.Any(wli => wli.ProductId == productId && wli.ClientId == userId));
        }

        [TestMethod]
        public void WishListRepository_Remove_WithHistory()
        {
            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();

            var repository = new WishListRepository();
            repository.IncreaseQuantityOrCreate(productId, userId);

            repository.Remove(productId, userId);

            var ctx = new LoyaltyDBEntities(this.connectionString);

            Assert.IsTrue(!ctx.WishListItems.Any(wli => wli.ProductId == productId && wli.ClientId == userId));
        }

        [TestMethod]
        public void WishListRepository_NotUpdate_WishListItem()
        {
            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();

            var repository = new WishListRepository();

            var result = repository.CreateOrUpdate(productId, userId, 5);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void WishListRepository_Update_WishListItem()
        {
            const int Quantity = 5;
            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();

            var repository = new WishListRepository();
            repository.IncreaseQuantityOrCreate(productId, userId);

            var result = repository.CreateOrUpdate(productId, userId, Quantity);

            Assert.IsNotNull(result);

            var ctx = new LoyaltyDBEntities(this.connectionString);
            Assert.IsTrue(
                ctx.WishListItems.Any(
                    wli => wli.ProductId == productId && wli.ClientId == userId && wli.ProductsQuantity == Quantity));
        }

        [TestMethod]
        public void WishListRepository_ReturnCount()
        {
            var userId = Guid.NewGuid().ToString();
            string productId1, productId2;
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var products = ctx.ProductSortProjections.Take(2).ToArray();
                productId1 = products[0].ProductId;
                productId2 = products[1].ProductId;
            }

            var repository = new WishListRepository();
                repository.IncreaseQuantityOrCreate(productId1, userId);
                repository.IncreaseQuantityOrCreate(productId2, userId, 50);

                var result = repository.GetCountByClientId(userId);

                Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void WishListRepository_GetItem()
        {
            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();

            var repository = new WishListRepository();
            repository.IncreaseQuantityOrCreate(productId, userId, 60);

            var item = repository.Get(productId, userId);

            Assert.AreEqual(item.ProductId, productId);
            Assert.AreEqual(item.ClientId, userId);
            Assert.AreEqual(item.ProductsQuantity, 60);
        }

        [TestMethod]
        public void GetWishListNotifications()
        {
            var userId = "testUser";
            string productId;

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                ctx.Database.ExecuteSqlCommand("truncate table [prod].[WishListItemNotifications]");
                productId = (from p in ctx.ProductSortProjections select p.ProductId).First();

                var wishListItem = new WishListItem()
                {
                    CreatedDate = DateTime.Now,
                    ProductId = productId,
                    ProductsQuantity = 2,
                    ClientId = userId
                };

                ctx.WishListItems.Add(wishListItem);
                ctx.SaveChanges();

                var obj = ctx.WishListItemNotifications.Add(new WishListItemNotification()
                    {
                        CreatedDate = DateTime.Now,
                        ProductId = productId,
                        ProductsQuantity = 1,
                        ClientId = userId,
                        FirstName = "TestFirstName",
                        MiddleName = "TestMiddleName",
                        ItemBonusCost = 123,
                        TotalBonusCost = 234
                    });

                ctx.SaveChanges();

                Assert.IsNotNull(obj);
                Assert.IsNull(obj.NotificationDate);
           }

            var repo = new WishListRepository();
            var res = repo.GetItemsToNotify(userId);

            Assert.IsTrue(res.Any());
            Assert.IsTrue(res.Any(e => e.ProductId == productId));

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var resObj = ctx.WishListItemNotifications.FirstOrDefault(n => n.ProductId == productId);

                Assert.IsNotNull(resObj);
                Assert.IsNotNull(resObj.NotificationDate);
            }
        }
    }
}

