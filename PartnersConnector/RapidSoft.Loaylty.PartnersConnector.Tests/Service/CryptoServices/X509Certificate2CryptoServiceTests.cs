namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service.CryptoServices
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    public class X509Certificate2CryptoServiceTests
    {
        private const string InputText = "12345";

        [TestMethod]
        public void ShouldGenerateSignatureByPrivateKeyByPfx()
        {
            var pfx = CryptoSettings.Pfx;

            var service = new X509Certificate2CryptoService(pfx);

            var result = service.CreateSignature(InputText);
            Console.WriteLine(result);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ShouldVerifyDataByPfx()
        {
            const string CheckValue = InputText;
            var pfx = CryptoSettings.Pfx;

            var service = new X509Certificate2CryptoService(pfx);

            var signature = service.CreateSignature(InputText);

            var result = service.VerifyData(signature, CheckValue);
            Assert.AreEqual(true, result);

            const string BadCheckValue = CheckValue + "1";
            result = service.VerifyData(signature, BadCheckValue);
            Assert.AreEqual(false, result);
        }
        
        [TestMethod]
        public void ShouldVerifyDataByCer()
        {
            const string CheckValue = InputText;

            var pfx = CryptoSettings.Pfx;
            Console.WriteLine(pfx);
            var pfxService = new X509Certificate2CryptoService(pfx);

            var signature = pfxService.CreateSignature(InputText);

            Console.WriteLine("====  signature  ====");

            var cert = CryptoSettings.Cert;
            Console.WriteLine(cert);
            var service = new X509Certificate2CryptoService(cert);

            var result = service.VerifyData(signature, CheckValue);
            Assert.AreEqual(true, result);

            const string BadCheckValue = CheckValue + "1";
            result = service.VerifyData(signature, BadCheckValue);
            Assert.AreEqual(false, result);
        }
    }
}
