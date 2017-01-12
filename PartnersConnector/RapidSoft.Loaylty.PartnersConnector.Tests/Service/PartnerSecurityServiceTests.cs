using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service
{
    using RapidSoft.Loaylty.PartnersConnector.Services;

    [TestClass]
    public class PartnerSecurityServiceTests
    {
        public const string TestFile = @"..\Certificates\TestBankPrivateKey.pem";

        [DeploymentItem(TestFile, @"bin")]
        [TestMethod]
        public void ShouldSignAndVerifyBankSignature()
        {
            const string Text = " = 1234567890 = ";

            var service = new PartnerSecurityService();

            var signatureResult = service.CreateBankSignature(Text);

            Assert.IsNotNull(signatureResult);
            Assert.AreEqual(true, signatureResult.Success);
            Assert.AreNotEqual(Text, signatureResult.Signature);

            var verifyResult = service.VerifyBankSignature(Text, signatureResult.Signature);

            Assert.IsNotNull(verifyResult);
            Assert.AreEqual(true, verifyResult.Success);
            Assert.AreEqual(true, verifyResult.Valid);
        }
    }
}
