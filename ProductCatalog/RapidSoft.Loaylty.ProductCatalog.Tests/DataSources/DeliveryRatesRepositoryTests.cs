using System.Collections.Generic;
using System.Linq;
using RapidSoft.Loaylty.ProductCatalog.DataSources;
using RapidSoft.Loaylty.ProductCatalog.Entities;
using RapidSoft.Loaylty.ProductCatalog.ImportTests;
using RapidSoft.Loaylty.ProductCatalog.Services;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System;

    using API.Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;

    [TestClass]
    public class DeliveryRatesRepositoryTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            TestHelper.CreateDeliveryMatrix();
        }
        
        [TestMethod]
        public void ShouldGetDeliveryPriceFor1And20Items()
        {
            const int productWeight = 1000;
            const int partnerId = TestDataStore.PartnerId;
            const string kladr = "6400000000000";
            
            // NOTE: Создаем тестовые данные: 150 для весов от 0 до пяти весов продукта
            var rate0to5 = new DeliveryRate
            {
                Kladr = kladr,
                MinWeightGram = 0,
                MaxWeightGram = productWeight * 5,
                PartnerId = partnerId,
                PriceRUR = 150
            };
            // NOTE: Создаем тестовые данные: 180 для весов от пяти весов продукта + 1 грамм до пятнадцати весов продукта
            var rate6to15 = new DeliveryRate
            {
                Kladr = kladr,
                MinWeightGram = (productWeight * 5) + 1,
                MaxWeightGram = productWeight * 15,
                PartnerId = partnerId,
                PriceRUR = 180
            };

            var testRatesRepository = new TestDeliveryRatesRepository();
            testRatesRepository.Create(rate0to5);
            testRatesRepository.Create(rate6to15);

            var rate = GetRate(partnerId, productWeight, 1, kladr);

            AssertRate(rate, kladr, 150, "Для одного продуктов цена доставки должна быть {0}");

            rate = GetRate(partnerId, productWeight, 2, kladr);

            AssertRate(rate, kladr, 150, "Для одного продуктов цена доставки тоже должна быть {0}");

            rate = GetRate(partnerId, productWeight, 10, kladr);

            AssertRate(rate, kladr, 180, "Для 10 продуктов цена доставки должна быть {0}");

            rate = GetRate(partnerId, productWeight, 50, kladr);

            Assert.IsNull(rate, "Для 50 доставки быть не должно");
        }

        [TestMethod]
        public void ShouldGetRegionPriceForSmallTown()
        {
            const int productWeight = 1000;
            const int partnerId = TestDataStore.PartnerId;

            // NOTE: Создаем тестовые данные
            var rateRegion = new DeliveryRate
            {
                Kladr = "6300000000000",
                MinWeightGram = 0,
                MaxWeightGram = 1000000,
                PartnerId = partnerId,
                PriceRUR = 150
            };
            var rateSamara = new DeliveryRate
            {
                Kladr = "6300000100000",
                MinWeightGram = 0,
                MaxWeightGram = 1000000,
                PartnerId = partnerId,
                PriceRUR = 100
            };
            var rateNovokuybishevsk = new DeliveryRate
            {
                Kladr = "6300000300000",
                MinWeightGram = 0,
                MaxWeightGram = 1000000,
                PartnerId = partnerId,
                PriceRUR = 110
            };

            var testRatesRepository = new TestDeliveryRatesRepository();
            testRatesRepository.Create(rateRegion);
            testRatesRepository.Create(rateSamara);
            testRatesRepository.Create(rateNovokuybishevsk);

            var rate = GetRate(partnerId, productWeight, 1, "6300000104200");
            AssertRate(rate, rateRegion.Kladr, rateRegion.PriceRUR, "В село Рубежное должны доставить по тарифу Самарская область {0}");

            rate = GetRate(partnerId, productWeight, 1, "6300000300500");
            AssertRate(rate, rateRegion.Kladr, rateRegion.PriceRUR, "В поселок Маяк должны доставить по тарифу Самарская область {0}");

            rate = GetRate(partnerId, productWeight, 1, "6300000300000");
            AssertRate(rate, rateNovokuybishevsk.Kladr, rateNovokuybishevsk.PriceRUR, "В Новокуйбышевск доставляем по его тарифу {0}");
        }

        [TestMethod]
        public void ShouldSaveDeliveryLocation()
        {
            var repo = new DeliveryRatesRepository();

            var deliveryLocation = new DeliveryLocation { LocationName = "LocationName" + Guid.NewGuid(), Kladr = "sdfghdsfhg" };

            var saved = repo.SaveDeliveryLocation(deliveryLocation);

            Assert.IsNotNull(saved);
            Assert.AreNotEqual(0, saved.Id);

            var fromDb = repo.GetDeliveryLocation(saved.Id);

            Assert.IsTrue(fromDb.InsertDateTime > default(DateTime));

            fromDb.Kladr = "1234567890";
            fromDb.UpdateUserId = "I";
            var updated = repo.SaveDeliveryLocation(fromDb);

            fromDb = repo.GetDeliveryLocation(updated.Id);
            Assert.IsTrue(fromDb.UpdateDateTime > default(DateTime));
            Assert.AreEqual("1234567890", fromDb.Kladr);
        }

        [TestMethod]
        public void ShouldGetRatesForCityKladr()
        {
            int partnerId = TestDataStore.PartnerId;
            var cityKladr = "9800000100000";

            //// NOTE: 3 кг доставляем по городскому тарифу!
            var rate = GetRate(partnerId, 3000, 1, cityKladr);
            AssertRate(rate, cityKladr, 30m, "{0}");

            //// NOTE: 4.200 кг доставляем по областному тарифу!
            rate = GetRate(partnerId, 4200, 1, cityKladr);
            AssertRate(rate, "9800000000000", 50m, "{0}");

            //// NOTE: 5.200 кг доставляем по областному тарифу!
            rate = GetRate(partnerId, 5200, 1, cityKladr);
            AssertRate(rate, "9800000000000", 100m, "{0}");

            // NOTE: 15 кг НЕ доставляем!
            rate = GetRate(partnerId, 15000, 1, cityKladr);
            Assert.IsNull(rate, "15 кг доставлять не должны");
        }

        [TestMethod]
        public void ShouldGetRatesForRegionKladr()
        {
            int partnerId = TestDataStore.PartnerId;
            var regionKladr = "9800000000000";

            //// NOTE: 3 кг доставляем по областному тарифу!!!
            var rate = GetRate(partnerId, 3000, 1, regionKladr);
            AssertRate(rate, regionKladr, 50m, "{0}");

            //// NOTE: 4.200 кг доставляем по областному тарифу!
            rate = GetRate(partnerId, 4200, 1, regionKladr);
            AssertRate(rate, regionKladr, 50m, "{0}");

            //// NOTE: 5.200 кг доставляем по областному тарифу!
            rate = GetRate(partnerId, 5200, 1, regionKladr);
            AssertRate(rate, regionKladr, 100m, "{0}");

            // NOTE: 15 кг НЕ доставляем!
            rate = GetRate(partnerId, 15000, 1, regionKladr);
            Assert.IsNull(rate, "15 кг доставлять не должны");
        }

        private static PartnerDeliveryRate GetRate(int partnerId, int productWeight, int quantity, string kladr)
        {
            var rep = new DeliveryRatesRepository();

            var rate = rep.GetMinPriceRate(partnerId, kladr, productWeight * quantity);
            return rate;
        }

        private static void AssertRate(PartnerDeliveryRate rate, string expectedKladr, decimal expectedPrice, string message)
        {
            Assert.AreEqual(expectedKladr, rate.Kladr);
            Assert.AreEqual(expectedPrice, rate.PriceRur, string.Format(message, expectedPrice));
        }
    }
}
