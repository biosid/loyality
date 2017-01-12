using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.Import
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Import;

    [TestClass]
    public class PartnerThrustCalculatorTests
    {
        [TestMethod]
        public void ShouldCorrectCalculateModerationStatusForHighThrustPartner()
        {
            var existsProd = Tests.TestHelper.BuildTestProduct();
            existsProd.ModerationStatus = ProductModerationStatuses.Canceled;

            var calculator = new HighPartnerTrustCalculator(
                new[]
                {
                    existsProd
                });

            var newProd = Tests.TestHelper.BuildTestProduct();

            Assert.AreEqual(
                ProductModerationStatuses.Canceled,
                calculator.CalcModerationStatus(newProd),
                "Статус такой же как у существующего товара");

            newProd.Available = !newProd.Available;
            Assert.AreEqual(
                existsProd.ModerationStatus,
                calculator.CalcModerationStatus(newProd),
                "Даже при измененом Available должно быть такой же как у существующего товара");

            newProd.PriceRUR = newProd.PriceRUR + 5555.5m;
            Assert.AreEqual(
                ProductModerationStatuses.Canceled,
                calculator.CalcModerationStatus(newProd),
                "Даже при измененом PriceRUR должно такой же как у существующего товара");

            newProd.PartnerProductId = Guid.NewGuid().ToString();

            Assert.AreEqual(
                ProductModerationStatuses.Applied,
                calculator.CalcModerationStatus(newProd),
                "Для товара которого нет в текущем каталоге ProductModerationStatuses.Applied");
        }

        [TestMethod]
        public void ShouldCorrectCalculateModerationStatusForMiddleThrustPartner()
        {
            var existsProd = Tests.TestHelper.BuildTestProduct();
            existsProd.ModerationStatus = ProductModerationStatuses.Canceled;

            var calculator = new MiddlePartnerTrustCalculator(
                new[]
                {
                    existsProd
                });

            var newProd = Tests.TestHelper.BuildTestProduct();

            Assert.AreEqual(
                ProductModerationStatuses.Canceled,
                calculator.CalcModerationStatus(newProd),
                "Статус такой же как у существующего товара");

            newProd.Available = !newProd.Available;
            Assert.AreEqual(
                existsProd.ModerationStatus,
                calculator.CalcModerationStatus(newProd),
                "Даже при измененом Available должно быть такой же как у существующего товара");

            newProd.PriceRUR = newProd.PriceRUR + 5555.5m;
            Assert.AreEqual(
                ProductModerationStatuses.Canceled,
                calculator.CalcModerationStatus(newProd),
                "Даже при измененом PriceRUR должно такой же как у существующего товара");

            newProd.Name = newProd.Name + newProd.Name;
            Assert.AreEqual(
               ProductModerationStatuses.Canceled,
               calculator.CalcModerationStatus(newProd),
               "Даже при измененом от PriceRUR должно такой же как у существующего товара");

            newProd = Tests.TestHelper.BuildTestProduct();
            newProd.PartnerProductId = Guid.NewGuid().ToString();

            Assert.AreEqual(
                ProductModerationStatuses.InModeration,
                calculator.CalcModerationStatus(newProd),
                "Для товара которого нет в текущем каталоге InModeration");
        }

        [TestMethod]
        public void ShouldCorrectCalculateModerationStatusForLowThrustPartner()
        {
            var existsProd = Tests.TestHelper.BuildTestProduct();
            existsProd.ModerationStatus = ProductModerationStatuses.Canceled;

            var calculator = new LowPartnerTrustCalculator(
                new[]
                {
                    existsProd
                });

            var newProd = Tests.TestHelper.BuildTestProduct();

            Assert.AreEqual(
                ProductModerationStatuses.Canceled,
                calculator.CalcModerationStatus(newProd),
                "Статус такой же как у существующего товара");

            newProd.PriceRUR = newProd.PriceRUR + 5555.5m;
            Assert.AreEqual(
                ProductModerationStatuses.Canceled,
                calculator.CalcModerationStatus(newProd),
                "Даже при измененом PriceRUR должен быть как у существующего товара");

            newProd.Available = !newProd.Available;
            Assert.AreEqual(
                ProductModerationStatuses.InModeration,
                calculator.CalcModerationStatus(newProd),
                "При измененом совйстве отличном от PriceRUR должен быть InModeration");

            newProd.Available = !newProd.Available;
            newProd.Name = newProd.Name + newProd.Name;
            Assert.AreEqual(
               ProductModerationStatuses.InModeration,
               calculator.CalcModerationStatus(newProd),
               "При измененом совйстве отличном от PriceRUR должен быть InModeration");

            newProd = Tests.TestHelper.BuildTestProduct();
            newProd.PartnerProductId = Guid.NewGuid().ToString();

            Assert.AreEqual(
                ProductModerationStatuses.InModeration,
                calculator.CalcModerationStatus(newProd),
                "Для товара которого нет в текущем каталоге InModeration");

            newProd = Tests.TestHelper.BuildTestProduct();
            newProd.CategoryId = 0;

            Assert.AreEqual(
                ProductModerationStatuses.Canceled,
                calculator.CalcModerationStatus(newProd),
                "Категория не должна влиять на статус модерации");
        }
    }
}
