using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using API.Entities;

    using Interfaces;

    using Moq;

    using ProductCatalog.DataSources;
    using ProductCatalog.DataSources.Repositories;
    using ProductCatalog.Services;

    [TestClass]
    public class WishListNotificationsBuilderTest
    {
        private readonly string clientId = TestDataStore.TestClientId;
        private readonly List<Entities.ProductSortProjection> products;
        private readonly string expensiveProductId = "1_5199254";

        public WishListNotificationsBuilderTest()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                products = ctx.ProductSortProjections.Where(p => p.PartnerId == 1).Take(2).ToList();
                products.Add(ctx.ProductSortProjections.SingleOrDefault(p => p.ProductId == expensiveProductId));
            }            
        }

        [TestInitialize]
        [TestCleanup]
        public void DeleteDataFromDb()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.Database.ExecuteSqlCommand("delete from prod.WishListItems;");

                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void MakeWishListNotifications()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.Database.ExecuteSqlCommand("truncate table [prod].[WishListItemNotifications]");                

                Assert.IsTrue(products.Count > 1, "Недостаточно продуктов с PartnerId = 1 для проведения теста");

                var wishListItem = new WishListItem()
                {
                    CreatedDate = DateTime.Now,
                    ProductId = products[0].ProductId,
                    ProductsQuantity = 2,
                    ClientId = clientId
                };

                var wishListItem2 = new WishListItem()
                {
                    CreatedDate = DateTime.Now,
                    ProductId = products[1].ProductId,
                    ProductsQuantity = 3,
                    ClientId = clientId
                };

                ctx.WishListItems.Add(wishListItem);
                ctx.WishListItems.Add(wishListItem2);
                ctx.SaveChanges();

                GetNotificationsBuilder().MakeWishListNotifications();

                var notifications = ctx.WishListItemNotifications.ToList();

                Assert.IsTrue(notifications.Any(n => n.ProductId == products[0].ProductId && n.FirstName == "TestFirstName"), "Отсутствует оповещение для продукта ({0})", products[0].ProductId);
                Assert.IsTrue(notifications.Any(n => n.ProductId == products[1].ProductId && n.FirstName == "TestFirstName"), "Отсутствует оповещение для продукта ({0})", products[1].ProductId);
                Assert.AreEqual(2, notifications.Count(x => x.NotificationDate == null), "Созданные оповещения не должны содержать даты отправки");
            }
        }

        [TestMethod]
        public void MakeWishListNotificationsShouldNotFail()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.Database.ExecuteSqlCommand("truncate table [prod].[WishListItemNotifications]");

                Assert.IsTrue(products.Count > 1, "Недостаточно продуктов с PartnerId = 1 для проведения теста");

                var wishListItem = new WishListItem()
                {
                    CreatedDate = DateTime.Now,
                    ProductId = products[0].ProductId,
                    ProductsQuantity = 2,
                    ClientId = clientId
                };

                var wishListItem2 = new WishListItem()
                {
                    CreatedDate = DateTime.Now,
                    ProductId = products[1].ProductId,
                    ProductsQuantity = 3,
                    ClientId = clientId
                };

                ctx.WishListItems.Add(wishListItem);
                ctx.WishListItems.Add(wishListItem2);
                ctx.SaveChanges();

                var mockClientProfile = MockFactory.GetClientProfileProvider();

                mockClientProfile.Setup(x => x.GetClientProfile(TestDataStore.TestClientId))
                 .Callback(() => { throw new Exception("test"); });

                GetNotificationsBuilder(mockClientProfile).MakeWishListNotifications();
            }
        }

        [TestMethod]
        public void NotMakeWishListNotificationsIfBalanceIsLessThanCost()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var product = products.SingleOrDefault(p => p.ProductId == expensiveProductId);

                var wishListItem = new WishListItem()
                {
                    CreatedDate = DateTime.Now,
                    ProductId = product.ProductId,
                    ProductsQuantity = 10,
                    ClientId = clientId
                };

                ctx.WishListItems.Add(wishListItem);
                ctx.SaveChanges();

                GetNotificationsBuilder().MakeWishListNotifications();

                var notifications = ctx.WishListItemNotifications.Where(n => n.NotificationDate == null).ToList();

                Assert.IsFalse(notifications.Any(n => n.ProductId == product.ProductId));
            }
        }

        [TestMethod]
        public void ReplaceWishNotificationIfNotYetSended()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.Database.ExecuteSqlCommand("truncate table [prod].[WishListItemNotifications]");

                var wishListItem = new WishListItem()
                {
                    CreatedDate = DateTime.Now,
                    ProductId = products[0].ProductId,
                    ProductsQuantity = 9,
                    ClientId = clientId
                };

                var wishListItem2 = new WishListItem()
                {
                    CreatedDate = DateTime.Now,
                    ProductId = products[1].ProductId,
                    ProductsQuantity = 10,
                    ClientId = clientId
                };

                ctx.WishListItems.Add(wishListItem);
                ctx.WishListItems.Add(wishListItem2);
                ctx.SaveChanges();

                GetNotificationsBuilder().MakeWishListNotifications();

                var notifications = ctx.WishListItemNotifications.Where(n => n.NotificationDate == null).ToList();

                Assert.AreEqual(notifications[0].CreatedDate, notifications[1].CreatedDate);
                notifications[0].NotificationDate = DateTime.Now;
                notifications[0].CreatedDate = notifications[0].CreatedDate.AddDays(-1);
                notifications[1].CreatedDate = notifications[1].CreatedDate.AddDays(-1);
                ctx.SaveChanges();
            }

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                GetNotificationsBuilder().MakeWishListNotifications();

                var notifications = ctx.WishListItemNotifications.ToList();
                Assert.AreEqual(2, notifications.Count);
                Assert.AreNotEqual(notifications[0].CreatedDate, notifications[1].CreatedDate);
            }
        }

        [TestMethod]
        public void GetWishListNotificationsTwiceOnItemRemove()
        {
            string productId;
            WishListItem wishListItem;

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.Database.ExecuteSqlCommand("truncate table [prod].[WishListItemNotifications]");
                productId = products[0].ProductId;

                wishListItem = new WishListItem
                {
                    CreatedDate = DateTime.Now,
                    ProductId = productId,
                    ProductsQuantity = 2,
                    ClientId = TestDataStore.TestClientId
                };

                ctx.WishListItems.Add(wishListItem);
                ctx.SaveChanges();
            }

            var repo = GetWishListRepository();
            GetNotificationsBuilder().MakeWishListNotifications();

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var res = ctx.WishListItemNotifications.Where(x => x.ClientId == TestDataStore.TestClientId);

                Assert.IsTrue(res.Any());
                Assert.IsTrue(res.Any(e => e.ProductId == productId));

                var notifications = repo.GetItemsToNotify(TestDataStore.TestClientId, int.MaxValue, false);
                Assert.IsNotNull(notifications);

                var resObj = ctx.WishListItemNotifications.FirstOrDefault(n => n.ProductId == productId);

                Assert.IsNotNull(resObj);
                Assert.IsNotNull(resObj.NotificationDate);

                // должен удалить также WishListItemNotification
                repo.Remove(productId, TestDataStore.TestClientId);

                res = ctx.WishListItemNotifications.Where(x => x.ClientId == TestDataStore.TestClientId);

                Assert.IsFalse(res.Any(), "WishListItemNotification был возвращен после удаления WishListItem");

                ctx.WishListItems.Add(wishListItem);
                ctx.SaveChanges();
            }

            GetNotificationsBuilder().MakeWishListNotifications();

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var res = ctx.WishListItemNotifications.Where(x => x.ClientId == TestDataStore.TestClientId);

                Assert.IsTrue(res.Any(), "WishListItemNotification не был возвращен после передобавления WishListItem");
                Assert.IsTrue(res.Any(e => e.ProductId == productId));
            }
        }

        private WishListRepository GetWishListRepository()
        {
            return new WishListRepository();
        }

        private WishListNotificationsBuilder GetNotificationsBuilder(Mock<IClientProfileProvider> mockClientProfile = null)
        {
            var mockProcessing = MockFactory.GetProcessingProvider(100000);

            if (mockClientProfile == null)
            {
                mockClientProfile = MockFactory.GetClientProfileProvider();
            }

            var prods = products.Select(p => new Product()
            {
                ProductId = p.ProductId,
                Price = p.ProductId == expensiveProductId ? 1000001 : 200
            }).ToArray();

            var productsSearcher = MockFactory.GetProductsSearcher(prods);

            var mechanicsProvider = new MechanicsProvider(MockFactory.GetMechanicsClientProviderMock().Object);

            var repo = new WishListNotificationsBuilder(
                new WishListRepository(), 
                mockProcessing.Object,
                mockClientProfile.Object,
                productsSearcher.Object,
                mechanicsProvider: mechanicsProvider);
            return repo;
        }
    }
}