using System;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaylty.IntegrationTests
{
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Xml;

    using Loaylty.PartnersConnector.WsClients.PartnerSecurityService;
    using Loaylty.ProductCatalog.WsClients.OrderManagementService;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BiletixWorkflowTests
    {
        private const int BiletixId = 7;

        private readonly string validateUserUrlFormat = ConfigurationManager.AppSettings["ValidateUserUrlFormat"];

        private const string NotifyOrderFormat = @"<?xml version=""1.0"" encoding=""utf-8""?>
<NotifyOrder UserTicket=""{3}"" ShopId=""{4}"" OrderId=""{0}"" TotalCost=""45"" OrderStatus=""{1}"" InternalStatusCode=""InternalStatusCode"" DateTime=""2013-06-11T14:31:22.8712316+04:00"" UtcDateTime=""2013-06-11T10:31:22.8712316Z"" Signature=""{2}"">
<Item Id=""1"" ArticleName=""Хороший товар"" ArticleId=""ArticleId"" Amount=""1"" Price=""45"" BonusPrice=""150"" Weight=""500"" />
</NotifyOrder>";

        private readonly string notifyOrderUrl = ConfigurationManager.AppSettings["NotifyOrderUrl"];

        [TestMethod]
        public void ShouldExecuteWorkFlow()
        {
            // NOTE: 1: Запрашиваем данные пользователя по тикету
            const string Ticket = "ticket";

            var paramss = string.Concat(BiletixId.ToString(CultureInfo.InvariantCulture), Ticket);
            var validateUserSign = this.Sing(paramss);

            Console.WriteLine("ValidateUserUrlFormat: " + this.validateUserUrlFormat);
            Console.WriteLine("BiletixId: " + BiletixId);
            Console.WriteLine("Ticket: " + Ticket);
            Console.WriteLine("validateUserSign: " + validateUserSign);
            var validateUserUrl = string.Format(
                this.validateUserUrlFormat, BiletixId, HttpUtility.UrlEncode(Ticket), HttpUtility.UrlEncode(validateUserSign));

            var validateUserBody = GetResponseXml(validateUserUrl);

            Assert.IsNotNull(validateUserBody);

            var validateUserResponse = new XmlDocument();
            validateUserResponse.LoadXml(validateUserBody);

            Assert.IsNotNull(validateUserResponse.DocumentElement, "Полученный ответ: " + validateUserBody);
            Assert.AreEqual("ValidateUserResult", validateUserResponse.DocumentElement.Name, "Полученный ответ: " + validateUserBody);
            Assert.AreEqual("0", validateUserResponse.DocumentElement.Attributes["Status"].Value, "Полученный ответ: " + validateUserBody);

            var lastNameSec = validateUserResponse.DocumentElement.Attributes["LastName"].Value;

            Assert.IsTrue(lastNameSec.EndsWith("="), "LastName в ответе должно быть зашифровано, а следовательно содержать \"=\" в конце.");

            // NOTE: 2.1: Создание заказа
            var partnerOrderId = Guid.NewGuid().ToString();
            const int OrderStatus = 1; // NOTE: 1 – Заказ создан, см. 4.4.2.	Формат запроса
            var notifyCreateOrderSing = string.Empty;
            var notifyCreateOrder = string.Format(NotifyOrderFormat, partnerOrderId, OrderStatus, notifyCreateOrderSing, Ticket, BiletixId);

            notifyCreateOrderSing = this.Sing(notifyCreateOrder);
            var notifyCreateOrderWithSing = string.Format(NotifyOrderFormat, partnerOrderId, OrderStatus, notifyCreateOrderSing, Ticket, BiletixId);

            var notifyCreateOrderBody = GetResponseXml(notifyOrderUrl, "POST", notifyCreateOrderWithSing);

            Assert.IsNotNull(notifyCreateOrderBody);

            var notifyCreateOrderResponse = new XmlDocument();
            notifyCreateOrderResponse.LoadXml(notifyCreateOrderBody);

            Assert.IsNotNull(notifyCreateOrderResponse.DocumentElement);
            Assert.AreEqual("NotifyOrderResult", notifyCreateOrderResponse.DocumentElement.Name);
            Assert.AreEqual("0", notifyCreateOrderResponse.DocumentElement.Attributes["Status"].Value);

            // NOTE: 3.1 ССБ запрашивает данные заказа
            var parameters = new GetOrderByExternalIdParameters
                                 {
                                     ClientId = null,
                                     ExternalOrderId = partnerOrderId,
                                     PartnerId = BiletixId
                                 };
            GetOrderResult getOrderResult;
            using (var cl = new OrderManagementServiceClient())
            {
                getOrderResult = cl.GetOrderByExternalId(parameters);
            }

            Assert.IsNotNull(getOrderResult);
            Assert.AreEqual(true, getOrderResult.Success, getOrderResult.ResultDescription);
            Assert.IsNotNull(getOrderResult.Order);
            Assert.AreEqual(partnerOrderId, getOrderResult.Order.ExternalOrderId);

            var orderId = getOrderResult.Order.Id;
            var orderStatus = getOrderResult.Order.Status;
            var orderClientId = getOrderResult.Order.ClientId;

            // NOTE: 5. ССБ подтверждает заказ
            ClientCommitOrderResult clientCommitOrder;
            using (var cl = new OrderManagementServiceClient())
            {
                clientCommitOrder = cl.ClientCommitOrder(orderClientId, orderId);
            }

            Assert.IsNotNull(clientCommitOrder);
            Assert.AreEqual(true, clientCommitOrder.Success);
            Assert.IsNotNull(clientCommitOrder.Order);
            Assert.AreEqual(partnerOrderId, clientCommitOrder.Order.ExternalOrderId);
            Assert.AreNotEqual(orderStatus, clientCommitOrder.Order.Status);

            using (var cl = new OrderManagementServiceClient())
            {
                getOrderResult = cl.GetOrderById(orderId, orderClientId);
            }

            Assert.IsNotNull(getOrderResult);
            Assert.AreEqual(true, getOrderResult.Success, getOrderResult.ResultDescription);
            Assert.IsNotNull(getOrderResult.Order);
            Assert.AreNotEqual(orderStatus, getOrderResult.Order.Status, "После ClientCommitOrder статус заказа должен измениться");

            // NOTE: X. Партнер переводит заказа в другой статус
            var nextStatus = getOrderResult.NextOrderStatuses.First();
            var nextStatusAsInt = (int)nextStatus;
            var notifyUpdateOrderSing = string.Empty;
            var notifyUpdateOrder = string.Format(NotifyOrderFormat, partnerOrderId, nextStatusAsInt, notifyUpdateOrderSing, Ticket, BiletixId);

            notifyUpdateOrderSing = this.Sing(notifyUpdateOrder);
            var notifyUpdateOrderWithSing = string.Format(NotifyOrderFormat, partnerOrderId, nextStatusAsInt, notifyUpdateOrderSing, Ticket, BiletixId);

            var notifyUpdateOrderBody = GetResponseXml(notifyOrderUrl, "POST", notifyUpdateOrderWithSing);

            Assert.IsNotNull(notifyUpdateOrderBody);

            var notifyUpdateOrderResponse = new XmlDocument();
            notifyUpdateOrderResponse.LoadXml(notifyUpdateOrderBody);

            Assert.IsNotNull(notifyUpdateOrderResponse.DocumentElement, "Ответ: " + notifyUpdateOrderBody);
            Assert.AreEqual("NotifyOrderResult", notifyUpdateOrderResponse.DocumentElement.Name);
            Assert.AreEqual("0", notifyUpdateOrderResponse.DocumentElement.Attributes["Status"].Value, notifyUpdateOrderResponse.DocumentElement.Attributes["Error"].Value);

            using (var cl = new OrderManagementServiceClient())
            {
                getOrderResult = cl.GetOrderById(orderId, orderClientId);
            }

            Assert.IsNotNull(getOrderResult);
            Assert.AreEqual(true, getOrderResult.Success, getOrderResult.ResultDescription);
            Assert.IsNotNull(getOrderResult.Order);
            Assert.AreEqual(nextStatus, getOrderResult.Order.Status);

            // NOTE: X+! Партнер не может перевести заказ в "старый" статус, например OrderStatuses.Registration.
            const OrderStatuses InvalidNextStatus = OrderStatuses.Registration;
            const int InvalidNextStatusAsInt = (int)InvalidNextStatus;
            var notifyUpdateOrderSing2 = string.Empty;
            var notifyUpdateOrder2 = string.Format(
                NotifyOrderFormat, partnerOrderId, InvalidNextStatusAsInt, notifyUpdateOrderSing2, Ticket, BiletixId);

            notifyUpdateOrderSing2 = this.Sing(notifyUpdateOrder2);
            var notifyUpdateOrderWithSing2 = string.Format(
                NotifyOrderFormat, partnerOrderId, InvalidNextStatusAsInt, notifyUpdateOrderSing2, Ticket, BiletixId);

            var notifyUpdateOrderBody2 = GetResponseXml(notifyOrderUrl, "POST", notifyUpdateOrderWithSing2);

            Assert.IsNotNull(notifyUpdateOrderBody2);

            var notifyUpdateOrderResponse2 = new XmlDocument();
            notifyUpdateOrderResponse2.LoadXml(notifyUpdateOrderBody2);

            Assert.IsNotNull(notifyUpdateOrderResponse2.DocumentElement);
            Assert.AreEqual("NotifyOrderResult", notifyUpdateOrderResponse2.DocumentElement.Name);
            Assert.AreNotEqual("0", notifyUpdateOrderResponse2.DocumentElement.Attributes["Status"].Value);

            using (var cl = new OrderManagementServiceClient())
            {
                getOrderResult = cl.GetOrderById(orderId, orderClientId);
            }

            Assert.IsNotNull(getOrderResult);
            Assert.AreEqual(true, getOrderResult.Success, getOrderResult.ResultDescription);
            Assert.IsNotNull(getOrderResult.Order);
            Assert.AreEqual(nextStatus, getOrderResult.Order.Status, "Статус должен остать как до не верного обновления!");
        }

        private static string GetResponseXml(string url, string method = "GET", string postData = null)
        {
            var request = WebRequest.Create(url);
            request.Method = method;
            if (!string.IsNullOrWhiteSpace(postData))
            {
                var byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                var dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();

            return ReadResponseBody(response);
        }

        private static string ReadResponseBody(HttpWebResponse response)
        {
            var encode = Encoding.UTF8;
            using (var receiveStream = response.GetResponseStream())
            using (var readStream = new StreamReader(receiveStream, encode))
            {
                var body = readStream.ReadToEnd();

                return body;
            }
        }

        private string Sing(string text)
        {
            using (var client = new PartnerSecurityServiceClient())
            {
                var signature = client.CreateOnlinePartnerSignature(BiletixId, text);

                Assert.IsNotNull(signature);

                if (signature.Signature == null)
                {
                    throw new InvalidOperationException("PartnerSecurityService.CreateOnlinePartnerSignature return null");
                }

                return signature.Signature;
            }
        }
    }
}
