using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service.CryptoServices
{
    using System.Diagnostics.CodeAnalysis;

    using RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    public class CryptoServiceFactoryTests
    {
        private const string InputStr = "abcde";

        [TestMethod]
        [DeploymentItem(PartnerSecurityServiceTests.TestFile, @"bin")]        
        public void ShouldCreateBankCryptoService()
        {
            var factory = new CryptoServiceFactory();

            var bankService = factory.GetBankCryptoService();

            var encrypt = bankService.Encrypt(InputStr);
            var decrypt = bankService.Decrypt(encrypt);

            Assert.AreEqual(InputStr, decrypt);
        }

        [TestMethod]
        public void ShouldCreatePartnerCryptoService()
        {
            // NOTE: Идентификатор online партнера указаный в конфиге.
            const int OnlinePartnerId = 5;
            var factory = new CryptoServiceFactory(MockFactory.GetCatalogAdminServiceProvider().Object);

            var bankService = factory.GetParnterCryptoService(OnlinePartnerId);

            var encrypt = bankService.Encrypt(InputStr);

            Assert.AreNotEqual(InputStr, encrypt);
        }
    }
}
