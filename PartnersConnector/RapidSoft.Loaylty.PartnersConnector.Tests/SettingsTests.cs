using System;

namespace RapidSoft.Loaylty.PartnersConnector.Tests
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings;
    using RapidSoft.Loyalty.Security;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void ShouldReadSettingsForJobRestart()
        {
            Assert.AreEqual(PartnerConnectionsConfig.MaxTaskRefire, 2);
            Assert.AreEqual(PartnerConnectionsConfig.RefireCountToMilisecFactor, 500);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnEmptyPosIdTerminalIdBankPrivateKey()
        {
            var prevPosId = ConfigurationManager.AppSettings["PosId"];
            var prevTerminalId = ConfigurationManager.AppSettings["TerminalId"];
            ConfigurationManager.AppSettings["PosId"] = null;
            ConfigurationManager.AppSettings["TerminalId"] = null;

            try
            {
                try
                {
                    var t = PartnerConnectionsConfig.PosId;
                    Assert.Fail("PosId не должен быть получен");
                }
                catch (NullReferenceException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("PosId"));
                }

                try
                {
                    var t = PartnerConnectionsConfig.TerminalId;
                    Assert.Fail("TerminalId не должен быть получен");
                }
                catch (NullReferenceException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("TerminalId"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ConfigurationManager.AppSettings["PosId"] = prevPosId;
                ConfigurationManager.AppSettings["TerminalId"] = prevTerminalId;
                throw;
            }

            ConfigurationManager.AppSettings["PosId"] = prevPosId;
            ConfigurationManager.AppSettings["TerminalId"] = prevTerminalId;
        }
    }
}
