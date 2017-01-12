namespace RapidSoft.Loaylty.IntegrationTests.PartnersConnector
{
    using System;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loyalty.Security;

    [TestClass]
    public class NotifyOrderStatusTest
    {
        [TestMethod]
        public void CanPartnerNotifyOrderStatusTest()
        {
            var thumbprint = ConfigurationManager.AppSettings["TestPartnerThumbprint"];
            var cert = new StoreCertificateProvider().GetByThumbprint(thumbprint);

            var sender = new TextOverSslMessageDispatcher(cert);

            const string sendData = @"<?xml version=""1.0"" encoding=""utf-8""?>
<NotifyOrderStatusMessage xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <Orders>
    <Order>
      <InternalOrderId>X100500</InternalOrderId>
      <StatusCode>40</StatusCode>
      <StatusDateTime>2013-01-01T12:30:00.0</StatusDateTime>
    </Order>
    <Order>
      <InternalOrderId>X100510</InternalOrderId>
      <StatusCode>20</StatusCode>
      <StatusDateTime>2013-01-01T12:30:00.0</StatusDateTime>
      <StatusReason>Клиент не отвечает на звонки курьера в течении 3 дней</StatusReason>
    </Order>
  </Orders>
</NotifyOrderStatusMessage>
";

            var uri = new Uri(ConfigurationManager.AppSettings["NotifyOrderStatusURL"]);

            Console.WriteLine(uri);

            var result = sender.Send(uri, sendData);

            Console.WriteLine(result);
            Assert.IsNotNull(result);
        }
    }
}