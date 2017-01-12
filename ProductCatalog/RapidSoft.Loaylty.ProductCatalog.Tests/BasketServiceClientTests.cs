namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.VTB24.ArmSecurity;

    [TestClass]
    public class BasketServiceTests
    {
        #region Constructors

        public BasketServiceTests()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["LoyaltyProductCatalogDB"].ConnectionString;
            this.basketItemRepository = new BasketItemRepository(connectionString);
            this.ctx = new LoyaltyDBEntities(connectionString);
        }

        #endregion

        #region Fields

        private readonly BasketItemRepository basketItemRepository;

        private readonly LoyaltyDBEntities ctx;

        private string productId = TestDataStore.GetProduct().ProductId;

        private Dictionary<string, string> clientContext = TestDataStore.GetClientContext();
        private string userId = TestDataStore.TestUserId;

        #endregion

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var mock = MockFactory.GetUserService();
            var service = mock.Object;
            ArmSecurity.UserServiceCreator = () => service;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ArmSecurity.UserServiceCreator = null;
        }

        #region Tests

        [TestMethod]
        public void ShouldAddToBasket()
        {
            var clientId = Guid.NewGuid().ToString();

            var service = this.GetService();

            var result = service.Add(clientId, this.productId, 1, clientContext);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.ResultCode, ResultCodes.SUCCESS);

            Assert.IsTrue(
                this.ctx.BasketItems.Any(
                    x => x.ClientId == clientId && x.ProductId == this.productId && x.ProductsQuantity == 1));
        }

        [TestMethod]
        public void ShouldIncreaseQuantityIfAlreadyContains()
        {
            var clientId = Guid.NewGuid().ToString();

            var client = this.GetService();

            client.Add(clientId, this.productId, 5, clientContext);

            var result = client.Add(clientId, this.productId, 1, clientContext);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);

            var basketItem =
                ctx.BasketItems.SingleOrDefault(x => x.ClientId == clientId && x.ProductId == productId);
            Assert.IsNotNull(basketItem);
            Assert.AreEqual(6, basketItem.ProductsQuantity);
        }

        [TestMethod]
        public void ShouldReturnBasket()
        {
            var client = this.GetService();

            client.Add(userId, productId, 1, clientContext);

            var param = new GetBasketParameters
            {
                ClientId = userId,
                ClientContext = clientContext,
                CalcTotalCount = true
            };

            var result = client.GetBasket(param);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.ResultCode, ResultCodes.SUCCESS);
            Assert.IsTrue(result.TotalCount > 0);
            Assert.IsTrue(result.Items.Any(x => x.ProductId == productId));
            Assert.IsTrue(result.Items.All(x => x.Id != Guid.Empty));
        }

        [TestMethod]
        public void ShouldRemove()
        {
            var clientId = Guid.NewGuid().ToString();

            var client = this.GetService();

            client.Add(clientId, productId, 1, clientContext);
            Assert.IsTrue(ctx.BasketItems.Any(x => x.ClientId == clientId && x.ProductId == productId));

            var result = client.Remove(clientId, productId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.ResultCode, ResultCodes.SUCCESS);

            Assert.IsFalse(ctx.BasketItems.Any(x => x.ClientId == clientId && x.ProductId == productId));
        }

        [TestMethod]
        public void CanReturnBasketItemTest()
        {
            var clientId = Guid.NewGuid().ToString();

            AddBasketItem(clientId);

            var service = this.GetService();
            var result = service.GetBasketItem(clientId, productId, clientContext);

            Assert.IsNotNull(result);
            Assert.AreEqual(ResultCodes.SUCCESS, result.ResultCode, result.ResultDescription);
            Assert.IsNotNull(result.Item);
            Assert.IsNotNull(result.Item.Product);
            Assert.AreEqual(productId, result.Item.Product.ProductId);
        }

        private BasketManageResult AddBasketItem(string clientId)
        {
            var service = this.GetService();

            var res = service.Add(clientId, productId, 1, clientContext);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultDescription);

            return res;
        }

        [TestMethod]
        public void ShouldNotAddInvalidProductId()
        {
            var clientId = Guid.NewGuid().ToString();

            var client = this.GetService();
            var invalidProductId = Guid.NewGuid().ToString();

            var result = client.Add(clientId, invalidProductId, 1, clientContext);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.ResultCode, ResultCodes.NOT_FOUND);

            Assert.IsFalse(
                this.ctx.BasketItems.Any(
                    x => x.ClientId == clientId && x.ProductId == this.productId && x.ProductsQuantity == 1));
        }

        [TestMethod]
        public void ShouldSortByDateCreateDesc()
        {
            var userId = Guid.NewGuid().ToString();
            var clientContext = new Dictionary<string, string>
            {
                {
                    ClientContextParser.LocationKladrCodeKey, "7700000000000"
                }
            };
            var productId1 = productId;
            var productId2 = TestDataStore.GetOtherProduct().ProductId;

            var service = this.GetService(new []{
                TestDataStore.GetProduct(), 
                TestDataStore.GetOtherProduct()});

            service.Add(userId, productId1, 1, clientContext);          
            service.Add(userId, productId2, 1, clientContext);

            var result = service.GetBasket(
                new GetBasketParameters { ClientId = userId, ClientContext = clientContext });

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success, result.ResultDescription);
            Assert.AreEqual(result.ResultCode, ResultCodes.SUCCESS);
            Assert.IsNotNull(result.Items);

            var first = result.Items.First();
            var last = result.Items.Last();

            Assert.IsTrue(first.CreatedDate > last.CreatedDate);
        }

        [TestMethod]
        public void ShouldFixPriceWhenAddToBasket()
        {
            var clientId = Guid.NewGuid().ToString();

            var client = this.GetService();

            var result = client.Add(clientId, this.productId, 1, clientContext);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.ResultCode, ResultCodes.SUCCESS);
            
            var item = client.GetBasketItem(result.BasketItemId, clientContext);

            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Item);
            Assert.IsNotNull(item.Item.FixedPrice);
            
            var prodPrice = XmlSerializer.Deserialize<FixedPrice>(item.Item.FixedPrice);

            Assert.IsNotNull(prodPrice);

            Assert.IsTrue(prodPrice.PriceRUR > 0);
        }

        [TestMethod]
        public void ShouldFixPriceWhenSetQuantity()
        {
            var clientId = Guid.NewGuid().ToString();

            var client = this.GetService();
            
            var result = client.SetQuantity(clientId, this.productId, 10, clientContext);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.ResultCode, ResultCodes.SUCCESS);

            var item = client.GetBasketItem(result.BasketItemId, new Dictionary<string, string>());

            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Item);
            Assert.IsNotNull(item.Item.FixedPrice);

            var prodPrice = XmlSerializer.Deserialize<FixedPrice>(item.Item.FixedPrice);

            Assert.IsNotNull(prodPrice);

            Assert.IsTrue(prodPrice.PriceRUR > 0);
        }

        [TestMethod]
        public void ShouldStorePartnerIdInBasketItemGroupId()
        {
            var userId = Guid.NewGuid().ToString();
            var clientContext = new Dictionary<string, string>
            {
                {
                    ClientContextParser.LocationKladrCodeKey, "7700000000000"
                }
            };

            var partnerId1 = 111;
            var partner1 = new Partner
            {
                Id = partnerId1,
                Type = PartnerType.Direct,
                Settings = new Dictionary<string, string>() { {PartnerSettingsExtension.MultiPositionOrdersKey, "true"} }
            };

            var partnerId2 = 555;
            var partner2 = new Partner
            {
                Id = partnerId2,
                Type = PartnerType.Direct
            };

            var partners = new[] { partner1, partner2 };

            var product1 = TestDataStore.GetProduct();
            product1.PartnerId = partnerId1;
            var product2 = TestDataStore.GetOtherProduct();
            product2.PartnerId = partnerId2;

            var partnersMock = new Mock<IPartnerRepository>();
            partnersMock.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>((pId) => partners.FirstOrDefault(p => p.Id == pId));

            var service = new BasketService(this.basketItemRepository,
                MockFactory.GetProductsSearcher(new[] { product1, product2 }).Object,
                MockFactory.GetPartnerConnectorProvider().Object,
                MockFactory.GetMechanicsProvider().Object,
                partnersMock.Object);

            service.Add(userId, product1.ProductId, 1, clientContext);
            service.Add(userId, product2.ProductId, 1, clientContext);

            var result = service.GetBasket(
                new GetBasketParameters { ClientId = userId, ClientContext = clientContext });

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success, result.ResultDescription);
            Assert.AreEqual(result.ResultCode, ResultCodes.SUCCESS);
            Assert.IsNotNull(result.Items);

            var first = result.Items.First();
            var last = result.Items.Last();

            Assert.IsTrue(last.BasketItemGroupId == partnerId1);
            Assert.IsTrue(!first.BasketItemGroupId.HasValue);
        }

        #endregion

        #region Methods

        private BasketService GetService(Product[] products = null)
        {    
            return new BasketService(this.basketItemRepository,
                MockFactory.GetProductsSearcher(products ?? TestDataStore.GetProducts()).Object,
                MockFactory.GetPartnerConnectorProvider().Object,
                MockFactory.GetMechanicsProvider().Object,
                new PartnerRepository());
        }
        
        #endregion
    }
}