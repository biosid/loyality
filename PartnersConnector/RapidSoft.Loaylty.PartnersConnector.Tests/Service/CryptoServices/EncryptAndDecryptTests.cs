using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service.CryptoServices
{
    using RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices;

    [TestClass]
    public class EncryptAndDecryptTests
    {
        [TestMethod]
        public void ShouldEncryptAndDecryptFromPublicKeyToPrivateKey()
        {
            var email = "DZakharovRapidSoft@gmail.com";

            var serviceE = new XmlPublicKeyCryptoService(CryptoSettings.XmlPublicKey);

            var e = serviceE.Encrypt(email);

            Assert.AreNotEqual(email, e);

            var serviceD = new XmlPrivateKeyCryptoService(CryptoSettings.XmlPrivateKey);

            var d = serviceD.Decrypt(e);

            Assert.AreEqual(email, d);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldEncryptAndDecryptFromPrivateKeyToPublicKey()
        {
            var email = "DZakharovRapidSoft@gmail.com";

            var serviceE = new XmlPrivateKeyCryptoService(CryptoSettings.XmlPrivateKey);

            var e = serviceE.Encrypt(email);

            Assert.AreNotEqual(email, e);

            var serviceD = new XmlPublicKeyCryptoService(CryptoSettings.XmlPublicKey);

            serviceD.Decrypt(e);

            Assert.Fail("Должен быть Exception, так как расшифровать по публичному ключу нельзя");
        }

        [TestMethod]
        public void ShouldEncryptAndDecryptFromPrivateKeyToPrivateKey()
        {
            var email = "DZakharovRapidSoft@gmail.com";

            var serviceE = new XmlPrivateKeyCryptoService(CryptoSettings.XmlPrivateKey);

            var e = serviceE.Encrypt(email);

            Assert.AreNotEqual(email, e);

            var serviceD = new XmlPrivateKeyCryptoService(CryptoSettings.XmlPrivateKey);

            var d = serviceD.Decrypt(e);

            Assert.AreEqual(email, d);
        }

    }
}
