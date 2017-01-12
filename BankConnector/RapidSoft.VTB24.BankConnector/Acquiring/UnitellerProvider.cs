namespace RapidSoft.VTB24.BankConnector.Acquiring
{
    using System;
    using System.Net.Http;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Interaction;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    public class UnitellerProvider : IUnitellerProvider
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(UnitellerProvider));

        public void Pay(string shopId, decimal sum, string client, string orderId)
        {
            var logEntry = StartInteraction.With("Uniteller").For("OrderPayment");
            logEntry.Info["ShopId"] = shopId;
            logEntry.Info["Sum"] = sum;
            logEntry.Info["ClientId"] = client;
            logEntry.Info["OrderId"] = orderId;

            try
            {
                Pay(
                    ConfigHelper.UnitellerPaymentPassword,
                    shopId,
                    sum,
                    client,
                    orderId,
                    ConfigHelper.Cv2,
                    ConfigHelper.UnitellerPaymentReturnUrl);
                logEntry.Succeeded();
            }
            catch (Exception e)
            {
                logEntry.Failed("ошибка взаимодействия", e);
                throw;
            }
            finally
            {
                logEntry.FinishAndWrite(logger);
            }
        }

        public void RegisterCard(string client)
        {
            throw new NotImplementedException();
        }

        public OperationStatusResponse GetRegistrationStatus(string orderId)
        {
            var request = new RegistrationStatusRequest(orderId);

            logger.Info("Get operation status request: " + request.GetFormContent());

            var response = GetResponse(request, ConfigHelper.UnitellerPaymentUrlResult);

            logger.Info("Get operation status response: " + response);

            return DeserializeResponse<OperationStatusResponse>(response);
        }

        private static T DeserializeResponse<T>(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                throw new ArgumentException("Can't deserialize empty response");
            }

            return JsonConvert.DeserializeObject<T>(
                response, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        private void Pay(
            string password,
            string paymentTarget,
            decimal paymentSum,
            string paymentClient,
            string paymentOrder,
            int cv2,
            string returnUrl)
        {
            const string CorrectAnswer = "pay_ok";

            var session = EstablishSession(
                password: password,
                shopId: paymentTarget,
                orderId: paymentOrder,
                customerId: paymentClient,
                subtotal: paymentSum,
                returnUrl: returnUrl);

            if (session.Result != CorrectAnswer)
            {
                throw new Exception(
                    string.Format("Произошла ошибка при создании платежа в Uniteller({0})", session.Message));
            }

            var payResult = PerformPay(session.Message, session.Cid, true, cv2);

            if (payResult.Result != CorrectAnswer)
            {
                throw new Exception(
                    string.Format("Произошла ошибка при проведении платежа в Uniteller({0})", payResult.Message));
            }

            var confirmResult = ConfirmPay(payResult.Message);

            if (confirmResult.Result != CorrectAnswer)
            {
                throw new Exception(
                    string.Format("Произошла ошибка при подтверждении платежа в Uniteller({0})", confirmResult.Message));
            }

            var cheque = GetCheque(confirmResult.Message);

            if (cheque.Result != CorrectAnswer)
            {
                throw new Exception(string.Format("Произошла ошибка при получении чека в Uniteller({0})", cheque.Message));
            }
        }

        private EstablishSessionResponse EstablishSession(string password, string shopId, string orderId, string customerId, decimal subtotal, string returnUrl)
        {
            var client = new UnitellerAuthInfo
            {
                ShopId = shopId,
                CustomerId = customerId,
                OrderId = orderId,
                Password = password,
                Subtotal = subtotal,
                ReturnUrl = returnUrl
            };

            var request = new EstablishSessionRequest(client, new UnitellerSignatureCreator());

            logger.Info("Establish session request: " + GetRawContent(request));

            var response = GetResponse(request, ConfigHelper.UnitellerPaymentUrl);

            logger.Info("Establish session response: " + response);

            return DeserializeResponse<EstablishSessionResponse>(response);
        }

        private string GetRawContent(IUnitellerRequest request)
        {
            var formUrlEncodedContent = request.GetFormContent();
            return formUrlEncodedContent.ReadAsStringAsync().Result;
        }

        private PerformPayResponse PerformPay(string context, string cid, bool doPay, int cv2)
        {
            var request = new PerformPayRequest(context, cid, doPay, cv2);

            logger.Info("Perform payment request: " + GetRawContent(request));

            var response = GetResponse(request, ConfigHelper.UnitellerPaymentUrl);

            logger.Info("Perform payment response: " + response);

            return DeserializeResponse<PerformPayResponse>(response);
        }

        private ConfirmPayResponse ConfirmPay(string context)
        {
            var request = new ConfirmPayRequest(context);

            logger.Info("Confirm payment request: " + GetRawContent(request));

            var response = GetResponse(request, ConfigHelper.UnitellerPaymentUrlConfirm);

            logger.Info("Confirm payment response: " + response);

            return DeserializeResponse<ConfirmPayResponse>(response);
        }

        private GetChequeResponse GetCheque(string context)
        {
            var request = new GetChequeRequest(context);

            logger.Info("Get cheque request: " + GetRawContent(request));

            var response = GetResponse(request, ConfigHelper.UnitellerPaymentUrlCheque);

            logger.Info("Get cheque response: " + response);

            return DeserializeResponse<GetChequeResponse>(response);
        }

        private string GetResponse(IUnitellerRequest request, string url)
        {
            var client = new HttpClient();

            var response = client.PostAsync(url, request.GetFormContent()).Result;

            var responseContent = response.Content;

            return responseContent.ReadAsStringAsync().Result;
        }
    }
}