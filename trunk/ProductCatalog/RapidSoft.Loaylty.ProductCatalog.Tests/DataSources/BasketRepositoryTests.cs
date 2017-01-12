namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;

    using API.Entities;

    using Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ProductCatalog.DataSources.Repositories;

    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class BasketRepositoryTests
    {
        [TestMethod]
        public void ShouldNotAddWithQuantityOverflow()
        {
            var userId = Guid.NewGuid().ToString();
            var productId = TestDataStore.ProductID;

            var basketItemRepository = new BasketItemRepository();

            try
            {
                basketItemRepository.Add(
                    clientId: userId,
                    productId: productId,
                    quantity: ApiSettings.MaxBasketItemProductsQuantity + 50,
                    newId: Guid.NewGuid());
                Assert.Fail("Должен быть exception");
            }
            catch (OperationException ex)
            {
                Assert.AreEqual(ResultCodes.QUANTITY_OVERFLOW, ex.ResultCode);
            }
            catch (Exception)
            {
                Assert.Fail("Должен быть exception типа OperationException");
            }
        }

        [TestMethod]
        public void ShouldNotUpdateWithQuantityOverflow()
        {
            var userId = Guid.NewGuid().ToString();
            var productId = TestHelper.GetAnyProductId();

            var basketItemRepository = new BasketItemRepository();

            basketItemRepository.Add(clientId: userId, productId: productId, quantity: 1, newId: Guid.NewGuid());

            try
            {
                basketItemRepository.Update(
                    clientId: userId, productId: productId, quantity: ApiSettings.MaxBasketItemProductsQuantity + 50, fixedPrice: null);
                Assert.Fail("Должен быть exception");
            }
            catch (OperationException ex)
            {
                Assert.AreEqual(ResultCodes.QUANTITY_OVERFLOW, ex.ResultCode);
            }
            catch (Exception)
            {
                Assert.Fail("Должен быть exception типа OperationException");
            }

            basketItemRepository.Remove(clientId: userId, productId: productId);
        }

        [TestMethod]
        public void ShouldDeserealizeFixPriceTest()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                foreach (var basketItem in ctx.BasketItems)
                {
                    var price = XmlSerializer.Deserialize<FixedPrice>(basketItem.FixedPrice);
                }
            }
        }
    }
}