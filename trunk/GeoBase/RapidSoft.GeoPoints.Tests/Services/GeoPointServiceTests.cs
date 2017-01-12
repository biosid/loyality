namespace RapidSoft.GeoPoints.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.GeoPoints.Repositories;

    [TestClass]
    public class GeoPointServiceTests
    {
        #region Additional test attributes
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            RepositoriesConfig.ConnectionString = ConfigurationManager.ConnectionStrings["InformationServicesDB"].ConnectionString;
        }
        #endregion

        [TestMethod]
        public void ShouldReturnAllCountries()
        {
            var codes = this.GetAllAlpha2Code();

            var service = new GeoPointService();

            var countries = service.GetCountriesNamesByCode(codes);

            Assert.IsNotNull(countries);
            Assert.IsNotNull(countries.Countries);
            Assert.AreEqual(countries.Countries.Count, codes.Count);

            var russia = countries.Countries.FirstOrDefault(x => x.Alpha2Code == "RU");
            Assert.IsNotNull(russia);
            Assert.AreEqual(russia.NumberCode, 643);
            Assert.AreEqual(russia.Alpha3Code, "RUS");
            Assert.AreEqual(russia.Description, "Российская Федерация");
        }

        [TestMethod]
        public void ShouldReturnAddressStringByKladrCode()
        {
            const string Kladr = "7700000000000";

            var service = new GeoPointService();
            var result = service.GetAddressByKladrCode(Kladr);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.KladrAddress.FullText, "г. Москва");
        }

        [TestMethod]
        public void ShouldReturnLocationsByOneAndManyLocationType8()
        {
            var service = new GeoPointService();

            var st2 = Stopwatch.StartNew();
            var locationsByMany1 = service.GetLocationsByKladrCode(null, new[] { 8 }, null, null, null, 0, 10000);
            st2.Stop();
            Console.WriteLine(st2.ElapsedTicks);

            Assert.IsNotNull(locationsByMany1);
            Assert.AreEqual(true, locationsByMany1.Success);
            Assert.IsNotNull(locationsByMany1.Locations);
        }

        [TestMethod]
        public void ShouldReturnLocationsByOneAndManyLocationType3()
        {
            var service = new GeoPointService();

            var st2 = Stopwatch.StartNew();
            var locationsByMany1 = service.GetLocationsByKladrCode(null, new[] { 3 }, null, null, null, 0, 10000);
            st2.Stop();
            Console.WriteLine(st2.ElapsedTicks);

            Assert.IsNotNull(locationsByMany1);
            Assert.AreEqual(true, locationsByMany1.Success);
            Assert.IsNotNull(locationsByMany1.Locations);
        }

        [TestMethod]
        public void ShouldReturnLocationsByOneAndManyLocationType3AndNamePattern()
        {
            var service = new GeoPointService();

            var st2 = Stopwatch.StartNew();
            var locationsByMany1 = service.GetLocationsByKladrCode(null, new[] { 3 }, null, "са", null, 0, 10000);
            st2.Stop();
            Console.WriteLine(st2.ElapsedTicks);

            Assert.IsNotNull(locationsByMany1);
            Assert.AreEqual(true, locationsByMany1.Success);
            Assert.IsNotNull(locationsByMany1.Locations);
        }

        [TestMethod]
        public void ShouldReturnLocationsByOneAndManyLocationType1()
        {
            var service = new GeoPointService();

            var st2 = Stopwatch.StartNew();
            var locationsByMany1 = service.GetLocationsByKladrCode(null, new[] { 1 }, null, null, null, 0, 100);
            st2.Stop();
            Console.WriteLine(st2.ElapsedTicks);

            Assert.IsNotNull(locationsByMany1);
            Assert.AreEqual(true, locationsByMany1.Success);
            Assert.IsNotNull(locationsByMany1.Locations);
        }

        [TestMethod]
        public void ShouldReturnLocationsByOneAndManyLocationType1And3And8()
        {
            var service = new GeoPointService();

            var st2 = Stopwatch.StartNew();
            var locationsByMany1 = service.GetLocationsByKladrCode(null, new[] { 1, 3, 8 }, null, null, null, 0, 10000);
            st2.Stop();
            Console.WriteLine(st2.ElapsedTicks);
        }

        [TestMethod]
        public void ShouldSearchTextWithQuota()
        {
            var service = new GeoPointService();

            var res = service.GetLocationsByKladrCode(null, new[] { 1, 3, 8 }, null, "\"Бизнес-Парк \"Румянцево\"", null, 0, 10000);
            Assert.IsNotNull(res, res.ResultDescription);
            Assert.IsTrue(res.Success, res.ResultDescription);
        }


        private List<string> GetAllAlpha2Code()
        {
            using (var conn = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = @"SELECT [Alpha2Code] FROM [Geopoints].[OKSMCountries]";
                    using (var reader = comm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        var list = new List<string>();
                        while (reader.Read())
                        {
                            list.Add(reader["Alpha2Code"].ToString());
                        }

                        return list;
                    }
                }
            }
        }
    }
}
