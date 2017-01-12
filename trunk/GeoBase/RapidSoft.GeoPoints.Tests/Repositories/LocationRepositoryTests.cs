namespace RapidSoft.GeoPoints.Tests.Repositories
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.GeoPoints.Repositories;

    [TestClass]
    public class LocationRepositoryTests
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

            var repo = new LocationRepository();

            var countries = repo.GetCountriesByAlpha2Codes(codes);

            Assert.IsNotNull(countries);
            Assert.AreEqual(countries.Count, codes.Count);

            var russia = countries.FirstOrDefault(x => x.Alpha2Code == "RU");
            Assert.IsNotNull(russia);
            Assert.AreEqual(russia.NumberCode, 643);
            Assert.AreEqual(russia.Alpha3Code, "RUS");
            Assert.AreEqual(russia.Description, "Российская Федерация");
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
