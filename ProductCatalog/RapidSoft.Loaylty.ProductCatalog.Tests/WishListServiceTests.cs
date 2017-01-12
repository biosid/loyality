namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class WishListServiceTests
    {
        #region Constructors

        public WishListServiceTests()
        {
            connectionString = ConfigurationManager.ConnectionStrings["LoyaltyProductCatalogDB"].ConnectionString;
        }

        #endregion

        #region Fields

        private readonly string connectionString;
        private static Product _testProduct;
        private Dictionary<string, string> clientContext = TestDataStore.GetClientContext();
        private string userId = TestDataStore.TestUserId;
        private string secondUserId = TestDataStore.SecondTestUserId;

        #endregion

        #region Tests

        [TestMethod]
        public void WishListService_ShouldCreateItem()
        {
            var productId = GetTestProduct().ProductId;
            var userId = Guid.NewGuid().ToString();

            var service = GetWLService();

            var result = service.Add(userId, productId, 1, clientContext);

            Assert.AreEqual(result.Success, true);

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                Assert.IsTrue(
                    ctx.WishListItems.Any(
                        wli => wli.ProductId == productId && wli.ClientId == userId && wli.ProductsQuantity == 1));

                var addedObj = ctx.WishListItems.FirstOrDefault(w => w.ProductId == productId);

                Assert.IsNotNull(addedObj);
                Assert.AreEqual(productId, addedObj.ProductId);                
            }
        }

        [TestMethod]
        public void ShouldAdd()
        {
            var service = new WishListService(
                new WishListRepository(),
                new ProductsSearcher(new ProductsDataSource(), new ProductAttributeRepository(), new MechanicsProvider(MockFactory.GetMechanicsClientProviderMock().Object)));
            
            var productId = TestHelper.GetAnyProductId();
            
            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var wishItems = ctx.WishListItems.Where(w => w.ProductId == productId).ToList();

                if (wishItems.Any())
                {
                    wishItems.ForEach(p => ctx.WishListItems.Remove(p));
                    ctx.SaveChanges();
                }

                var result = service.Add(TestDataStore.TestUserId, productId, 1, clientContext);

                Assert.AreEqual(true, result.Success);

                var addedObj = ctx.WishListItems.FirstOrDefault(w => w.ProductId == productId && w.ClientId == TestDataStore.TestUserId);

                Assert.IsNotNull(addedObj);
                Assert.AreEqual(productId, addedObj.ProductId);
            }    
        }

        [TestMethod]
        public void WishListService_ShouldLimitQuantityBy99Default()
        {
            var productId = GetTestProduct().ProductId;
            var userId = Guid.NewGuid().ToString();

            var service = GetWLService();

            var result = service.Add(userId, productId, 1000, clientContext);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(ResultCodes.QUANTITY_OVERFLOW, result.ResultCode);
        }
        
        [TestMethod]
        public void WishListService_ShouldCreateItemWithQuantity()
        {
            const int Quantity = 50;
            var productId = GetTestProduct().ProductId;
            var userId = Guid.NewGuid().ToString();

            var service = this.GetWLService();

            var result = service.Add(userId, productId, quantity: Quantity, clientContext: clientContext);

            Assert.AreEqual(result.Success, true);

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                Assert.IsTrue(
                    ctx.WishListItems.Any(
                        wli => wli.ProductId == productId && wli.ClientId == userId && wli.ProductsQuantity == Quantity));
            }
        }

        [TestMethod]
        public void WishListService_ShouldNotCreateItemWithInvalidQuantity()
        {
            const int Quantity = -50;
            var productId = GetTestProduct().ProductId;
            var userId = Guid.NewGuid().ToString();

            var service = GetWLService();

            var result = service.Add(userId, productId, quantity: Quantity, clientContext: clientContext);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(-1, result.ResultCode);
            Assert.IsNotNull(result.ResultDescription);

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                Assert.IsFalse(
                    ctx.WishListItems.Any(
                        wli => wli.ProductId == productId && wli.ClientId == userId && wli.ProductsQuantity == Quantity));
            }
        }

        [TestMethod]
        public void WishListService_ShouldAddQuantity()
        {
            const int InitQuantity = 10;
            const int Quantity = 50;
            var productId = GetTestProduct().ProductId;
            var userId = Guid.NewGuid().ToString();

            var service = GetWLService();

            var addResult = service.Add(userId, productId, quantity: InitQuantity, clientContext: clientContext);
            Assert.IsNotNull(addResult);
            Assert.AreEqual(true, addResult.Success, addResult.ResultDescription);

            string date;

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                date =
                    ctx.WishListItems.Single(wli => wli.ProductId == productId && wli.ClientId == userId)
                       .CreatedDate.ToString("dd.MM.yyyy hh:mm:ss");
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }

            var result = service.Add(userId, productId, quantity: Quantity, clientContext: clientContext);

            Assert.AreEqual(result.Success, true);

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var item = ctx.WishListItems.SingleOrDefault(wli => wli.ProductId == productId && wli.ClientId == userId);
                Assert.IsNotNull(item);
                Assert.AreEqual(item.ProductsQuantity, InitQuantity + Quantity);
                Assert.AreNotEqual(date, item.CreatedDate.ToString("dd.MM.yyyy hh:mm:ss"));
            }
        }

        [TestMethod]
        public void WishListService_ShouldSetQuantity()
        {
            const int InitQuantity = 10;
            const int Quantity = 50;
            var productId = GetTestProduct().ProductId;
            var userId = Guid.NewGuid().ToString();

            var service = GetWLService();

            service.Add(userId, productId, quantity: InitQuantity, clientContext: clientContext);

            string date;

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                date =
                    ctx.WishListItems.Single(wli => wli.ProductId == productId && wli.ClientId == userId)
                       .CreatedDate.ToString("dd.MM.yyyy hh:mm:ss");
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }

            var result = service.SetQuantity(userId, productId, Quantity, clientContext);

            Assert.AreEqual(result.Success, true);

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var item = ctx.WishListItems.SingleOrDefault(wli => wli.ProductId == productId && wli.ClientId == userId);
                Assert.IsNotNull(item);
                Assert.AreEqual(item.ProductsQuantity, Quantity);
                Assert.AreEqual(date, item.CreatedDate.ToString("dd.MM.yyyy hh:mm:ss"));
            }
        }

        [TestMethod]
        public void WishListService_ShouldCreateWhenSetQuantity()
        {
            const int Quantity = 50;
            var productId = GetTestProduct().ProductId;
            var userId = Guid.NewGuid().ToString();

            var service = GetWLService();

            var result = service.SetQuantity(userId, productId, Quantity, clientContext);

            Assert.AreEqual(result.Success, true);

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var item = ctx.WishListItems.SingleOrDefault(wli => wli.ProductId == productId && wli.ClientId == userId);
                Assert.IsNotNull(item);
                Assert.AreEqual(item.ProductsQuantity, Quantity);
            }
        }

        [TestMethod]
        public void WishListService_ShouldRemove()
        {
            var productId = GetTestProduct().ProductId;
            var userId = Guid.NewGuid().ToString();

            var service = GetWLService();

            service.Add(userId, productId, 1, clientContext);

            var result = service.Remove(userId, productId);

            Assert.AreEqual(result.Success, true);

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var item = ctx.WishListItems.SingleOrDefault(wli => wli.ProductId == productId && wli.ClientId == userId);
                Assert.IsNull(item);
            }
        }

        [TestMethod]
        public void WishListService_ShouldNotRemoveWithInvalidProductId()
        {
            var productId = GetTestProduct().ProductId;
            var userId = Guid.NewGuid().ToString();

            var service = GetWLService();

            service.Add(userId, productId, 1, clientContext);

            var result = service.Remove(userId, null);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(-1, result.ResultCode);
            Assert.IsNotNull(result.ResultDescription);

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                Assert.IsTrue(ctx.WishListItems.Any(wli => wli.ProductId == productId && wli.ClientId == userId));
            }
        }

        [TestMethod]
        public void WishListService_ShouldReturnWishListItem()
        {
            var product = GetTestProduct();

            var clientId = Guid.NewGuid().ToString();       

            var mockSearcher = MockFactory.GetProductsSearcher(product);

            var service = new WishListService(new WishListRepository(), mockSearcher.Object);

            const int Quantity = 10;
            service.Add(clientId, product.ProductId, quantity: Quantity, clientContext: clientContext);

            var itemResult = service.GetWishListItem(clientId, product.ProductId, clientContext);

            Assert.AreEqual(itemResult.Success, true);
            Assert.AreEqual(itemResult.ResultCode, ResultCodes.SUCCESS);
            var wishListItem = itemResult.Item;
            Assert.IsNotNull(wishListItem);
            Assert.IsNotNull(wishListItem.Product);

            Assert.AreEqual(ProductAvailabilityStatuses.Available, wishListItem.AvailabilityStatus);
            Assert.AreEqual(wishListItem.TotalPrice, product.Price * Quantity);
            Assert.AreEqual(wishListItem.TotalPrice, wishListItem.ItemPrice);
            Assert.AreEqual(product.ProductId, wishListItem.Product.ProductId);
            Assert.AreEqual(Quantity, wishListItem.ProductsQuantity);
        }

        [TestMethod]
        public void WishListService_ShouldReturnWishList()
        {
            var product = GetTestProduct();
            product.PriceBase = 1234;
            product.Price = 1234;

            var mockSearcher = MockFactory.GetProductsSearcher(product);

            var service = new WishListService(new WishListRepository(), mockSearcher.Object);

            service.Add(secondUserId, product.ProductId, 10, clientContext);

            var itemResult = service.GetWishList(new GetWishListParameters()
            {
                ClientContext = clientContext,
                ClientId = secondUserId
            });

            Assert.IsNotNull(itemResult);
            Assert.IsNotNull(itemResult.Items);
            Assert.AreEqual(itemResult.Success, true);
            Assert.AreEqual(itemResult.ResultCode, ResultCodes.SUCCESS);
            
            var wishListItem = itemResult.Items.SingleOrDefault(i => i.Product.ProductId == product.ProductId);
            
            Assert.IsNotNull(wishListItem);
            Assert.IsNotNull(wishListItem.Product);
            Assert.IsTrue(wishListItem.TotalPrice > 0);
            Assert.IsTrue(wishListItem.ItemPrice > 0);
            Assert.AreEqual(wishListItem.Product.ProductId, product.ProductId);
            Assert.IsTrue(wishListItem.ProductsQuantity > 0);
        }

        [TestMethod]
        public void WishListService_ShouldSortByName()
        {
            var userId = Guid.NewGuid().ToString();

            var clientContext = this.clientContext;
           
            var prod1 = TestDataStore.GetProduct();
            var prod2 = TestDataStore.GetOtherProduct();

            var service = this.GetWLService();

            service.Add(userId, prod1.ProductId, quantity: 5, clientContext: clientContext);
            service.Add(userId, prod2.ProductId, quantity: 10, clientContext: clientContext);

            var result = service.GetWishList(new GetWishListParameters
            {
                ClientId = userId,
                CountToSkip = 0,
                CountToTake = 1,
                SortType = WishListSortTypes.ByProductName,
                SortDirect = SortDirections.Asc,
                ClientContext = clientContext
            });

            var i = result.Items.FirstOrDefault();

            Assert.IsNotNull(i);
            Assert.AreEqual(prod1.ProductId, i.Product.ProductId);

            result = service.GetWishList(new GetWishListParameters
            {
                ClientId = userId,
                CountToSkip = 0,
                CountToTake = 1,
                SortType = WishListSortTypes.ByProductName,
                SortDirect = SortDirections.Desc,
                ClientContext = clientContext
            });

            i = result.Items.FirstOrDefault();

            Assert.IsNotNull(i);
            Assert.AreEqual(prod2.ProductId, i.Product.ProductId);

            result =
                service.GetWishList(
                    new GetWishListParameters
                        {
                            ClientId = userId,
                            CountToSkip = 0,
                            CountToTake = 5,
                            SortType = WishListSortTypes.ByProductName,
                            SortDirect = SortDirections.Asc,
                            ClientContext = clientContext
                        });

            i = result.Items.FirstOrDefault();
            Assert.IsNotNull(i);
            Assert.AreEqual(prod1.ProductId, i.Product.ProductId);
            i = result.Items.LastOrDefault();
            Assert.IsNotNull(i);
            Assert.AreEqual(prod2.ProductId, i.Product.ProductId);
        }

        [TestMethod]
        public void WishListService_ShouldDleteInvalidWishListItem()
        {
            var clientId = Guid.NewGuid().ToString();

            var clientContext = new Dictionary<string, string>
            {
                {
                    ClientContextParser.ClientIdKey, clientId
                },
                {
                    ClientContextParser.LocationKladrCodeKey,
                    "7700000000000"
                }
            };

            ProductSortProjection prod1, prod2;
            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var prods = ctx.ProductSortProjections;
                prod1 = prods.OrderBy(x => x.Name).First();
                prod2 = prods.OrderByDescending(x => x.Name).First();
            }

            var mockSearcher = new Mock<IProductsSearcher>();
            mockSearcher.Setup(s => s.GetProductsByIds(It.IsAny<string[]>(), It.IsAny<Dictionary<string, string>>()))
                        .Returns(new GetProductByIdItem[0]);

            var service = new WishListService(new WishListRepository(), mockSearcher.Object);

            service.Add(clientId, prod1.ProductId, quantity: 5, clientContext: clientContext);
            service.Add(clientId, prod2.ProductId, quantity: 10, clientContext: clientContext);

            var result = service.GetWishList(new GetWishListParameters
            {
                ClientId = clientId,
                CountToSkip = 0,
                CountToTake = 1,
                SortType = WishListSortTypes.ByProductName,
                SortDirect = SortDirections.Asc,
                ClientContext = clientContext
            });

            Assert.IsNotNull(result.Items);
            Assert.AreEqual(0, result.Items.Length);

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var item =
                    ctx.WishListItems.SingleOrDefault(wli => wli.ProductId == prod1.ProductId && wli.ClientId == clientId);
                Assert.IsNull(item);
                item = ctx.WishListItems.SingleOrDefault(wli => wli.ProductId == prod2.ProductId && wli.ClientId == clientId);
                Assert.IsNull(item);
            }
        }

        [TestMethod]
        public void WishListServic_GetWishListCount()
        {
            var product = TestDataStore.GetProduct();
            var product2 = TestDataStore.GetOtherProduct();

            var userId = Guid.NewGuid().ToString();

            var service = GetWLService();

            service.Add(userId, product.ProductId, 1, clientContext);
            service.Add(userId, product2.ProductId, 1, clientContext); 

            Assert.AreEqual(2, new WishListRepository().GetWishListCount(userId));
        }

        #endregion

        #region Methods

        private WishListService GetWLService(Product[] products = null)
        {
            var mockSearcher = MockFactory.GetProductsSearcher(products ?? TestDataStore.GetProducts());
            return new WishListService(new WishListRepository(), mockSearcher.Object);
        }

        private static Product GetTestProduct()
        {
            return TestDataStore.GetProduct();
        }

        #endregion
    }
}