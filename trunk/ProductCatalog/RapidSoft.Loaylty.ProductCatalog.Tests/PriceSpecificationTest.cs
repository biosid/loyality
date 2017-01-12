namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Services;

    [TestClass]
    public class PriceSpecificationTest
    {
        [TestMethod]
        public void CanCalcOrderPriceTest()
        {
            const string productId = "testProd";

            var testProduct = new Product
            {
                ProductId = productId,
                PriceRUR = 10,
                PriceBase = 100,
                Price = 25
            };

            var testOrder = new Order
            {
                Items = new[]
                {
                    new OrderItem
                    {
                        Amount = 2,
                        Product = testProduct
                    }
                }
            };

            var testOrderPrices = new[]
            {
                new OrderItemPrice
                {
                    ProductId = productId,
                    ProductPriceRur = 11,
                    ProductPrice = 55
                }
            };

            new PriceSpecification().FillOrderPrice(testOrder, testOrderPrices, 7, 22, 0, 0);

            Assert.AreEqual(testOrder.ItemsCost, 22);
            Assert.AreEqual(testOrder.BonusItemsCost, 110);

            Assert.AreEqual(testOrder.DeliveryCost, 7);
            Assert.AreEqual(testOrder.BonusDeliveryCost, 22);
            Assert.AreEqual(testOrder.DeliveryAdvance, 0);

            Assert.AreEqual(testOrder.TotalCost, 29);
            Assert.AreEqual(testOrder.BonusTotalCost, 132);
        }
    }
}