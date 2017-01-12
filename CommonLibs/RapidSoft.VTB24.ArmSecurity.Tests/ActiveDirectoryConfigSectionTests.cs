using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.VTB24.ArmSecurity.Tests
{
    [TestClass]
    public class ActiveDirectoryConfigSectionTests
    {
        [TestMethod]
        public void ShouldGetFromConfig()
        {
            var config = ActiveDirectoryConfigSection.Current;

            Assert.IsNotNull(config);
            Assert.IsNotNull(config.Connection);
            Assert.IsNotNull(config.Connection.Path);
            Assert.IsNotNull(config.Connection.UserName);
            Assert.IsNotNull(config.Connection.Password);
        }
    }
}
