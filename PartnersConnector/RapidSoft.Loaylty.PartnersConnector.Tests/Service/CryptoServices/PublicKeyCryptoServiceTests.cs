namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service.CryptoServices
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    public class PublicKeyCryptoServiceTests
    {
        private const string InputText = "12345";

        [TestMethod]
        public void ShouldVerifyData()
        {
            const string CheckValue = InputText;
            const string Signature = "lqBCYUjzHPuUAe4PoxMkKjz1gWoWs5SAHTW0IXEDz1/eqU5IgS6ELQ5z3LLZ6M08bhj4auuYOsTCHYlgxzSmgDAHLS4ziUiSIhM8FbWmylFOQlbrUDcGjRIE/nhyRwjObBAPbi+7v68de8vCWAIzc5nQCHqKeYiteQHBrhUwHxQ=";

            var service = new XmlPublicKeyCryptoService(CryptoSettings.XmlPublicKey);

            var result = service.VerifyData(Signature, CheckValue);
            Assert.AreEqual(true, result);

            const string BadCheckValue = CheckValue + "1";
            result = service.VerifyData(Signature, BadCheckValue);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ShouldVerifyDataFromPem()
        {
            const string CheckValue = InputText;
            const string Signature = "lqBCYUjzHPuUAe4PoxMkKjz1gWoWs5SAHTW0IXEDz1/eqU5IgS6ELQ5z3LLZ6M08bhj4auuYOsTCHYlgxzSmgDAHLS4ziUiSIhM8FbWmylFOQlbrUDcGjRIE/nhyRwjObBAPbi+7v68de8vCWAIzc5nQCHqKeYiteQHBrhUwHxQ=";

            var service = new PemPublicKeyCryptoService(CryptoSettings.PemPublicKey);

            var result = service.VerifyData(Signature, CheckValue);
            Assert.AreEqual(true, result);

            const string BadCheckValue = CheckValue + "1";
            result = service.VerifyData(Signature, BadCheckValue);
            Assert.AreEqual(false, result);
        }
    }
}