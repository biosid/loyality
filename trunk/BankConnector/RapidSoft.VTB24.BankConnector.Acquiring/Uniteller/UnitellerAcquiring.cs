using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.Logging.Interaction;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Infrastructure;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Inputs;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Outputs;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Xml;
using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller
{
    public class UnitellerAcquiring : IUnitellerAcquiring
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(UnitellerAcquiring));

        public string PaymentFormUrl
        {
            get { return ConfigHelper.AcquiringUnitellerPayFormUrl; }
        }

        public Dictionary<string, string> GetPaymentFormParameters(UnitellerPayParameters payParameters)
        {
            var signature = payParameters.Sign(ConfigHelper.AcquiringUnitellerPassword);

            var parameters = new Dictionary<string, string>
            {
                { "Signature", signature },
                { "Shop_IDP", payParameters.ShopId },
                { "Order_IDP", payParameters.OrderId },
                { "Subtotal_P", payParameters.Subtotal.ToString("F2", CultureInfo.InvariantCulture) },
                { "URL_RETURN_OK", payParameters.ReturnUrlSuccess },
                { "URL_RETURN_NO", payParameters.ReturnUrlFail }
            };

            if (payParameters.Lifetime.HasValue)
            {
                parameters["Lifetime"] = payParameters.Lifetime.Value.ToString("D");
            }

            if (!string.IsNullOrWhiteSpace(payParameters.CustomerId))
            {
                parameters["Customer_IDP"] = payParameters.CustomerId;
            }

            if (payParameters.Preauth)
            {
                parameters["Preauth"] = "1";
            }

            return parameters;
        }

        public UnitellerPaymentInfo GetPaymentInfo(string shopId, string orderId)
        {
            var request = new GetAuthorizationResultsRequest
            {
                ShopId = shopId,
                Login = ConfigHelper.AcquiringUnitellerLogin,
                Password = ConfigHelper.AcquiringUnitellerPassword,
                Format = GetAuthorizationResultsRequest.ResponseFormat.Xml,
                ShopOrderNumber = orderId
            };

            var requestContent = MappingsToService.ToFormUrlEncodedContent(request);

            var logEntry = StartInteraction.With("Uniteller").For("GetPaymentInfo");

            var responseXmlString = GetResponse(ConfigHelper.AcquiringUnitellerResultsUrl, requestContent, logEntry);

            var response = DeserializeXml<XmlResponse>(responseXmlString);

            if (response == null ||
                response.Items == null ||
                response.Items.Length != 1 ||
                response.Items[0].OrderNumber != orderId)
            {
                return null;
            }

            var responseCode = MappingsFromService.ToAuthorizeResponseCode(response.Items[0].ResponseCode);

            return new UnitellerPaymentInfo
            {
                OrderId = orderId,
                BillNumber = response.Items[0].BillNumber,
                IsAuthorized = responseCode == AuthorizeResponseCode.AS000
            };
        }

        public void ConfirmPayment(string shopId, string billNumber)
        {
            var request = new ConfirmPaymentRequest
            {
                ShopId = shopId,
                BillNumber = billNumber,
                Login = ConfigHelper.AcquiringUnitellerLogin,
                Password = ConfigHelper.AcquiringUnitellerPassword,
                Format = ConfirmPaymentRequest.ResponseFormat.Xml
            };

            var requestContent = MappingsToService.ToFormUrlEncodedContent(request);

            var logEntry = StartInteraction.With("Uniteller").For("ConfirmPayment");

            GetResponse(ConfigHelper.AcquiringUnitellerConfirmUrl, requestContent, logEntry);
        }

        public void CancelPayment(string shopId, string billNumber)
        {
            var request = new CancelPaymentRequest
            {
                ShopId = shopId,
                BillNumber = billNumber,
                Login = ConfigHelper.AcquiringUnitellerLogin,
                Password = ConfigHelper.AcquiringUnitellerPassword,
                Format = CancelPaymentRequest.ResponseFormat.Xml
            };

            var requestContent = MappingsToService.ToFormUrlEncodedContent(request);

            var logEntry = StartInteraction.With("Uniteller").For("CancelPayment");

            GetResponse(ConfigHelper.AcquiringUnitellerCancelUrl, requestContent, logEntry);
        }

        private string GetResponse(string url, HttpContent content, IInteractionLogEntry logEntry)
        {
            try
            {
                var correlationId = Guid.NewGuid().ToString();

                var requestString = content.ReadAsStringAsync().Result;
                logEntry.Info["Request"] = requestString;

                _log.InfoFormat("Отправка запроса к Uniteller ({0}): POST {1} {2}",
                                correlationId,
                                url,
                                requestString);


                HttpStatusCode responseStatus;
                string responseString = null;

                using (var client = new HttpClient())
                using (var response = client.PostAsync(url, content).Result)
                {
                    responseStatus = response.StatusCode;
                    if (responseStatus == HttpStatusCode.OK)
                    {
                        responseString = response.Content.ReadAsStringAsync().Result;
                    }
                }

                _log.InfoFormat("Получен ответ от Uniteller ({0}): [status - {1}] {2}",
                                correlationId,
                                responseStatus,
                                responseString);

                if (responseString == null)
                {
                    throw new Exception("не удалось получить ответ от uniteller");
                }

                logEntry.Info["Response"] = responseString;

                return responseString;
            }
            catch (Exception e)
            {
                logEntry.Failed("ошибка взаимодействия", e);
                throw;
            }
            finally
            {
                logEntry.FinishAndWrite(_log);
            }
        }

        private static T DeserializeXml<T>(string xmlString)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(xmlString))
            {
                var obj = (T) serializer.Deserialize(reader);

                return obj;
            }
        }
    }
}
