using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.GeoPoints.Tests
{
    using RapidSoft.GeoPoints.Entities;
    using RapidSoft.Kladr.Model;

    [TestClass]
    public class KladrCodeToAddressStringConvertTests
    {
        [TestMethod]
        public void ShouldReturnTrimKladrCode()
        {
            const string Kladr = "6300400300501";
            var location = new Location { KladrCode = Kladr };

            Assert.AreEqual(location.GetKladrCode(AddressLevel.Region), "6300000000000");
            Assert.AreEqual(location.GetKladrCode(AddressLevel.District), "6300400000000");
            Assert.AreEqual(location.GetKladrCode(AddressLevel.City), "6300400300000");
            Assert.AreEqual(location.GetKladrCode(AddressLevel.Town), "6300400300500");
        }

        [TestMethod]
        public void ShouldConvertKladrCodeToAddressString()
        {
            var t1 = new Location
                              {
                                  KladrCode = "6300100300500",
                                  RegionName = "Регионная",
                                  RegionToponym = "обл",
                                  DistrictName = "Районный",
                                  DistrictToponym = "р-н",
                                  CityName = "Город",
                                  CityToponym = "гр",
                                  TownName = "Поселок",
                                  TownToponym = "пос"
                              };
            var address = t1.ToKladrAddress();

            var result1 = AddressStringConverter.GetAddressText(address, AddressLevel.Town);

            Assert.AreEqual(result1, "обл. Регионная, р-н Районный, гр. Город, пос. Поселок");

            var t2 = new Location
                              {
                                  KladrCode = "6300100300500",
                                  RegionName = "Регионная",
                                  RegionToponym = "обл",
                                  CityName = "Город",
                                  CityToponym = "гр",
                                  TownName = "Поселок",
                                  TownToponym = "пос"
                              };
            address = t2.ToKladrAddress();

            var result2 = AddressStringConverter.GetAddressText(address, AddressLevel.Town);

            Assert.AreEqual(result2, "обл. Регионная, гр. Город, пос. Поселок");

            var t3 = new Location
                         {
                             KladrCode = "6300100300500",
                             RegionName = "Краснодарский",
                             RegionToponym = "край",
                             CityName = "Город",
                             CityToponym = "гр"
                         };
            address = t3.ToKladrAddress();

            var result3 = AddressStringConverter.GetAddressText(address, AddressLevel.Town);

            Assert.AreEqual(result3, "край Краснодарский, гр. Город");
        }
    }
}
