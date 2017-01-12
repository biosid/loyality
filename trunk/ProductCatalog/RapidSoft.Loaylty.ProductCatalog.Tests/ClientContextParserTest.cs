namespace RapidSoft.Loaylty.ProductCatalog.Tests.Services
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.Services;

    [TestClass]
    public class ClientContextParserTest
    {
        [TestMethod]
        public void ShouldParseNullContext()
        {
            var parser = new ClientContextParser();

            var location = parser.GetLocationKladrCode(null);
            var audience = parser.GetAudienceIds(null);

            Assert.IsNull(location);
            Assert.IsNull(audience);
        }

        [TestMethod]
        public void ShouldParseNullClientContext()
        {
            var parser = new ClientContextParser();

            var parameter = new GetCatalogParameters();
            parameter.ClientContext = null;

            var location = parser.GetLocationKladrCode(parameter.ClientContext);
            var audience = parser.GetAudienceIds(parameter.ClientContext);

            Assert.IsNull(location);
            Assert.IsNull(audience);
        }

        [TestMethod]
        public void ShouldParseEmptyClientContext()
        {
            var parser = new ClientContextParser();

            var parameter = new GetCatalogParameters();
            parameter.ClientContext = new Dictionary<string, string>();

            var location = parser.GetLocationKladrCode(parameter.ClientContext);
            var audience = parser.GetAudienceIds(parameter.ClientContext);

            Assert.IsNull(location);
            Assert.IsNull(audience);
        }

        [TestMethod]
        public void ShouldGetLocation()
        {
            var parser = new ClientContextParser();

            var parameter = new GetCatalogParameters();
            parameter.ClientContext = new Dictionary<string, string>();
            parameter.ClientContext.Add(ClientContextParser.LocationKladrCodeKey, "770000000000");

            var location = parser.GetLocationKladrCode(parameter.ClientContext);
            var audience = parser.GetAudienceIds(parameter.ClientContext);

            Assert.IsTrue(location.Equals("770000000000"));
            Assert.IsNull(audience);
        }

        [TestMethod]
        public void ShouldGetAudiences()
        {
            var parser = new ClientContextParser();

            var parameter = new GetCatalogParameters();
            parameter.ClientContext = new Dictionary<string, string>();
            parameter.ClientContext.Add(ClientContextParser.AudiencesKey, "Vip");

            var location = parser.GetLocationKladrCode(parameter.ClientContext);
            var audience = parser.GetAudienceIds(parameter.ClientContext);

            Assert.IsNull(location);
            Assert.IsTrue(audience.Equals("Vip"));
        }

        [TestMethod]
        public void ShouldGetClientId()
        {
            var parser = new ClientContextParser();

            var parameter = new GetCatalogParameters();
            parameter.ClientContext = new Dictionary<string, string>();
            parameter.ClientContext.Add(ClientContextParser.ClientIdKey, "1234567");

            var clietnId = parser.GetClientId(parameter.ClientContext);
            var location = parser.GetLocationKladrCode(parameter.ClientContext);
            var audience = parser.GetAudienceIds(parameter.ClientContext);

            Assert.IsTrue(clietnId.Equals("1234567"));
            Assert.IsNull(location);
            Assert.IsNull(audience);
        }
    }
}
