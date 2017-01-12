namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.Configuration;

    [TestClass]
    public class EtlConfigSectionTest
    {
        [TestMethod]
        public void ShouldReadEtlConfigSectionTest()
        {
            var etlConfigSection = EtlConfigSection.Current;

            Assert.IsNotNull(etlConfigSection);
            Assert.IsNotNull(etlConfigSection.Variables.All(p => p.Name != null && p.Value != null));
        }
    }
}