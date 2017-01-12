using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Kladr.Model;
using RapidSoft.Loaylty.ProductCatalog.API.Entities;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    [TestClass]
    public class OrderExtensionsTests
    {
        #region Tests

        [TestMethod]
        public void ShouldResetTitleAndAddress1()
        {
            var di = new DeliveryAddress()
            {
                //AddressKladrCode = "6300000000000",
                DistrictTitle = "На деревню",
                TownTitle = "Дедушке",
                House = "второй слева",
                Flat = "единственная",
                AddressText = "хукня"
            };

            var klaa = new KladrAddress
            {
                AddressLevel = AddressLevel.Region,
                Region =
                    new AddressElement
                    {
                        Name = "Самарсика насяльника",
                        Prefix = "А?",
                        Code = "6300000000000",
                        Level = AddressLevel.Region
                    },
                FullText = "Самарсика"
            };

            di.AddressText = di.BuildAddressText(klaa);

            Assert.AreEqual(di.AddressText, "Самарсика, На деревню, Дедушке, второй слева, единственная");
        }

        [TestMethod]
        public void ShouldResetTitleAndAddress2()
        {
            var di = new DeliveryAddress
            {
                //AddressKladrCode = "6300000000000",
                DistrictTitle = "На деревню",
                CityTitle = "Родному",
                TownTitle = "Дедушке",
                House = "второй слева",
                AddressText = "хукня"
            };
            var klaa = new KladrAddress
            {
                AddressLevel = AddressLevel.Region,
                Region =
                    new AddressElement
                    {
                        Name = "Самарсика насяльника",
                        Prefix = "А?",
                        Code = "6300000000000",
                        Level = AddressLevel.Region
                    },
                FullText = "Самарсика"
            };

            di.AddressText = di.BuildAddressText(klaa);

            Assert.AreEqual(di.AddressText, "Самарсика, На деревню, Родному, Дедушке, второй слева");
        }

        [TestMethod]
        public void ShouldResetTitleAndAddress3()
        {
            var di = new DeliveryAddress
            {
                //AddressKladrCode = "6399900077700",
                House = "второй слева",
                AddressText = "хукня"
            };
            var klaa = new KladrAddress
            {
                AddressLevel = AddressLevel.Town,
                Region =
                    new AddressElement
                    {
                        Name = "Самарсика насяльника",
                        Prefix = "А?",
                        Code = "6300000000000",
                        Level = AddressLevel.Region
                    },
                District =
                    new AddressElement
                    {
                        Name = "Ройна",
                        Prefix = "А?",
                        Code = "6399900000000",
                        Level = AddressLevel.District
                    },
                Town =
                    new AddressElement
                    {
                        Name = "Какой town так... хуторок",
                        Prefix = "А?",
                        Code = "6399900077700",
                        Level = AddressLevel.District
                    },
                FullText = "Адрес полученный из геобазы"
            };

            di.ResetTitles(klaa);
            di.AddressText = di.BuildAddressText(klaa);

            Assert.AreEqual("А? Самарсика насяльника", di.RegionTitle);
            Assert.AreEqual("А? Ройна", di.DistrictTitle);
            Assert.AreEqual("А? Какой town так... хуторок", di.TownTitle);
            Assert.AreEqual("Адрес полученный из геобазы, второй слева", di.AddressText);
        }

        [TestMethod]
        public void Test()
        {
            var klaa = new KladrAddress
            {
                AddressLevel = AddressLevel.Town,
                Region =
                    new AddressElement
                    {
                        Name = "Самарсика насяльника",
                        Prefix = "А?",
                        Code = "6300000000000",
                        Level = AddressLevel.Region
                    },
                District =
                    new AddressElement
                    {
                        Name = "Ройна",
                        Prefix = "А?",
                        Code = "6399900000000",
                        Level = AddressLevel.District
                    },
                Town =
                    new AddressElement
                    {
                        Name = "Какой town так... хуторок",
                        Prefix = "А?",
                        Code = "6399900077700",
                        Level = AddressLevel.District
                    },
                FullText = "Адрес полученный из геобазы"
            };

            var t = AddressStringConverter.GetAddressText(klaa);
            Console.WriteLine(t);
        }

        [TestMethod]
        public void ShouldFillAddressTextFromTownKLADRTest()
        {
            var di = GetTestDeliveryInfo("3300500000900").Address;

            var klaa = new KladrAddress
            {
                AddressLevel = AddressLevel.Town,
                Region =
                    new AddressElement
                    {
                        Name = "Владимирская",
                        Prefix = "обл",
                        Code = "3300000000000",
                        Level = AddressLevel.Region
                    },
                District =
                    new AddressElement
                    {
                        Name = "Гусь-Хрустальный",
                        Prefix = "р-н",
                        Code = "3300500000000",
                        Level = AddressLevel.District
                    },
                Town =
                    new AddressElement
                    {
                        Name = "Анопино",
                        Prefix = "п",
                        Code = "3300500000900",
                        Level = AddressLevel.Town
                    },
                FullText = "обл. Владимирская, р-н Гусь-Хрустальный, п. Анопино"
            };

            di.AddressText = di.BuildAddressText(klaa);

            Assert.AreEqual(di.AddressText, "обл. Владимирская, р-н Гусь-Хрустальный, п. Анопино, StreetName, 5, 6");
        }

        [TestMethod]
        public void ShouldFillAddressTextFromCityKLADRTest()
        {
            var di = GetTestDeliveryInfo("3300000300000").Address;

            var klaa = new KladrAddress
            {
                AddressLevel = AddressLevel.City,
                Region =
                    new AddressElement
                    {
                        Name = "Владимирская",
                        Prefix = "обл",
                        Code = "3300000000000",
                        Level = AddressLevel.Region
                    },
                City =
                    new AddressElement
                    {
                        Name = "Гусь-Хрустальный",
                        Prefix = "г",
                        Code = "3300000300000",
                        Level = AddressLevel.City
                    },
                FullText = "обл. Владимирская, г. Гусь-Хрустальный"
            };

            di.AddressText = di.BuildAddressText(klaa);

            Assert.AreEqual("обл. Владимирская, г. Гусь-Хрустальный, TownTitle, StreetName, 5, 6", di.AddressText);
        }

        #endregion

        #region Methods

        private static DeliveryInfo GetTestDeliveryInfo(string kladrCode)
        {
            var di = new DeliveryInfo
            {
                Address = new DeliveryAddress()
                {
                    PostCode = "601530",
                    //AddressKladrCode = kladrCode,
                    RegionTitle = "RegionTitle",
                    DistrictTitle = "DistrictTitle",
                    CityTitle = "CityTitle",
                    TownTitle = "TownTitle",
                    StreetTitle = "StreetName",
                    House = "5",
                    Flat = "6",
                    AddressText = "AddressText"                    
                }
            };
            return di;
        }

        #endregion
    }
}