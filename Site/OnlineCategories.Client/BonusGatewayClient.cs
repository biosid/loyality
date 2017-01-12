using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Vtb24.OnlineCategories.Client.Exceptions;
using Vtb24.OnlineCategories.Client.Internal;
using Vtb24.OnlineCategories.Client.Internal.DTO;
using Vtb24.OnlineCategories.Client.Models;

namespace Vtb24.OnlineCategories.Client
{
    public class BonusGatewayClient : IBonusGatewayClient
    {
        private const string VALIDATE_USER_QUERY_TEMPLATE = "?shopid={0}&userticket={1}&signature={2}";

        private const string PAYMENT_FORM_QUERY_TEMPLATE = "?shopid={0}&orderid={1}&maxdiscount={2}&userticket={3}";

        private const string CANCEL_PAYMENT_QUERY_TEMPLATE = "?shopid={0}&orderid={1}&discountrefund={2}&signature={3}";

        private const string GET_PAYMENT_STATUS_TEMPLATE = "?shopid={0}&orderid={1}&signature={2}";

        public ClientInfo ResolveClient(string userTicket)
        {
            var url = CreateValidateUserUrl(userTicket);

            var responseBytes = HttpHelpers.Get(url);

            var response = XmlHelpers.Deserialize<ValidateUserResult>(responseBytes, _encoding);

            ValidateResponse(responseBytes, response.Signature);

            AssertResponseStatus(response);

            var clientInfo = new ClientInfo
            {
                FirstName = CryptoService.DecryptString(response.FirstName, _encoding),
                LastName = CryptoService.DecryptString(response.LastName, _encoding),
                MiddleName = CryptoService.DecryptString(response.MiddleName, _encoding),
                Email = CryptoService.DecryptString(response.Email, _encoding),
                NameLanguageCode = response.NameLang,
                City = response.City,
                Balance = response.BalanceSpecified ? response.Balance : (decimal?)null,
                BonusRate = response.Rate,
                BonusDelta = response.Delta,
                UtcWhen = response.UtcDateTime
            };

            return clientInfo;
        }

        public string CreatePaymentFormUrl(string userTicket, string orderId, decimal maxDiscount)
        {
            var maxDiscountStr = maxDiscount.ToString("N2", CultureInfo.InvariantCulture);

            var url = FormatUrlQuery(Configuration.PaymentFormUrl, PAYMENT_FORM_QUERY_TEMPLATE,
                                     Configuration.ShopId, orderId, maxDiscountStr, userTicket);

            return url;
        }

        [Obsolete]
        public void CancelPayment(string orderId, decimal? discountRefund)
        {
            var url = CreateCancelPaymentUrl(orderId, discountRefund);

            var responseBytes = HttpHelpers.Get(url);

            var response = XmlHelpers.Deserialize<CancelPaymentResult>(responseBytes, _encoding);

            ValidateResponse(responseBytes, response.Signature);

            AssertResponseStatus(response);
        }

        public void CreateOrder(CreateOrderRequest request)
        {
            var when = DateTime.Now;

            var notifyOrder = new NotifyOrder
            {
                UserTicket = request.UserTicket,
                ShopId = Configuration.ShopId,
                OrderId = request.OrderId,
                TotalCost = request.TotalCost,
                OrderStatus = 1,
                InternalStatusCode = request.InternalStatus,
                DateTime = when,
                UtcDateTime = when.ToUniversalTime(),
                Item = new[]
                {
                    new NotifyOrderItem
                    {
                        Id = "1",
                        ArticleId = request.ItemId,
                        ArticleName = request.ItemName,
                        Amount = request.ItemQuantity,
                        Price = request.ItemPrice,
                        BonusPrice = request.ItemBonusPrice,
                        Weight = request.ItemWeight,
                        Comment = request.ItemComment
                    }
                }
            };

            PostNotifyOrder(notifyOrder);
        }

        public void NotifyOrderStatus(NotifyOrderStatusRequest request)
        {
            var when = DateTime.Now;

            var notifyOrder = new NotifyOrder
            {
                UserTicket = request.UserTicket,
                ShopId = Configuration.ShopId,
                OrderId = request.OrderId,
                TotalCost = request.TotalCost,
                OrderStatus = request.Status.Map(),
                Description = request.StatusDescription,
                InternalStatusCode = request.InternalStatus,
                DateTime = when,
                UtcDateTime = when.ToUniversalTime()
            };

            PostNotifyOrder(notifyOrder);
        }

        public bool IsPaid(string orderId)
        {
            var url = CreateGetPaymentStatusUrl(orderId);

            var responseBytes = HttpHelpers.Get(url);

            var response = XmlHelpers.Deserialize<GetPaymentStatusResult>(responseBytes, _encoding);

            ValidateResponse(responseBytes, response.Signature);

            AssertResponseStatus(response);

            return response.Status == 0;
        }

        private readonly Encoding _encoding = new UTF8Encoding(false);

        private CryptoService _cryptoService;

        private CryptoService CryptoService
        {
            get { return _cryptoService ?? (_cryptoService = new CryptoService()); }
        }

        private string CreateValidateUserUrl(string userTicket)
        {
            var shopId = Configuration.ShopId;

            var request = string.Concat(shopId, userTicket);

            var requestBytes = _encoding.GetBytes(request);

            var signature = CryptoService.CresteRequestSignature(requestBytes);

            var url = FormatUrlQuery(Configuration.GatewayEndpoint + "/ValidateUser.ashx", VALIDATE_USER_QUERY_TEMPLATE,
                                     shopId, userTicket, signature);

            return url;
        }

        private string CreateCancelPaymentUrl(string orderId, decimal? discountRefund)
        {
            var shopId = Configuration.ShopId;

            var discountRefundStr = discountRefund.HasValue
                                        ? discountRefund.Value.ToString("N2", CultureInfo.InvariantCulture)
                                        : string.Empty;

            var request = string.Concat(shopId, orderId, discountRefundStr);

            var requestBytes = _encoding.GetBytes(request);

            var signature = CryptoService.CresteRequestSignature(requestBytes);

            var url = FormatUrlQuery(Configuration.GatewayEndpoint + "/CancelPayment.ashx", CANCEL_PAYMENT_QUERY_TEMPLATE,
                                     shopId, orderId, discountRefundStr, signature);

            return url;
        }

        private string CreateGetPaymentStatusUrl(string orderId)
        {
            var shopId = Configuration.ShopId;

            var request = string.Concat(shopId, orderId);

            var requestBytes = _encoding.GetBytes(request);

            var signature = CryptoService.CresteRequestSignature(requestBytes);

            var url = FormatUrlQuery(Configuration.GatewayEndpoint + "/GetPaymentStatus.ashx", GET_PAYMENT_STATUS_TEMPLATE,
                                     shopId, orderId, signature);

            return url;
        }

        private static string CreateNotifyOrderUrl()
        {
            return Configuration.GatewayEndpoint + "/NotifyOrder.ashx";
        }

        private void PostNotifyOrder(NotifyOrder notifyOrder)
        {
            var url = CreateNotifyOrderUrl();

            notifyOrder.Signature = string.Empty;

            var bodyBytesWithoutSignature = XmlHelpers.Serialize(notifyOrder, _encoding);

            var signature = CryptoService.CresteRequestSignature(bodyBytesWithoutSignature);

            notifyOrder.Signature = signature;

            var bodyBytes = XmlHelpers.Serialize(notifyOrder, _encoding);

            var responseBytes = HttpHelpers.Post(url, bodyBytes);

            var response = XmlHelpers.Deserialize<NotifyOrderResult>(responseBytes, _encoding);

            ValidateResponse(responseBytes, response.Signature);

            AssertResponseStatus(response);
        }

        private void ValidateResponse(byte[] responseBytes, string signature)
        {
            var responseXml = _encoding.GetString(responseBytes);

            var responseXMlWithoutSignature = XmlHelpers.RemoveSignature(responseXml);

            var responseBytesWithoutSugnature = _encoding.GetBytes(responseXMlWithoutSignature);

            if (!CryptoService.ValidateResponseSignature(responseBytesWithoutSugnature, signature))
            {
                throw new InvalidResponseSignature();
            }
        }

        private static void AssertResponseStatus(ValidateUserResult response)
        {
            if (response.Status == 1)
            {
                throw new InvalidUserTicket();
            }

            if (response.Status == 41)
            {
                throw new InvalidRequestSignature();
            }

            if (response.Status != 0)
            {
                throw new UnknownResponseStatus(response.Status);
            }
        }

        private static void AssertResponseStatus(CancelPaymentResult response)
        {
            if (response.Status == 1)
            {
                throw new OperationError(response.Error);
            }

            if (response.Status == 10)
            {
                throw new PaymentIsNotFound();
            }

            if (response.Status == 11)
            {
                throw new TooLateToCancel();
            }

            if (response.Status == 41)
            {
                throw new InvalidRequestSignature();
            }

            if (response.Status != 0)
            {
                throw new UnknownResponseStatus(response.Status);
            }
        }

        private static void AssertResponseStatus(NotifyOrderResult response)
        {
            if (response.Status == 1)
            {
                throw new InvalidOrder(response.Error);
            }

            if (response.Status == 2)
            {
                throw new OperationError(response.Error);
            }

            if (response.Status == 41)
            {
                throw new InvalidRequestSignature();
            }

            if (response.Status != 0)
            {
                throw new UnknownResponseStatus(response.Status);
            }
        }

        private static void AssertResponseStatus(GetPaymentStatusResult response)
        {
            if (response.Status == 2)
            {
                throw new InvalidOrder("Неверные данные о заказе");
            }

            if (response.Status == 3)
            {
                throw new OperationError("Произошла неизвестная ошибка");
            }

            if (response.Status == 41)
            {
                throw new InvalidRequestSignature();
            }

            if (response.Status != 0 && response.Status != 1)
            {
                throw new UnknownResponseStatus(response.Status);
            }
        }

        private static string FormatUrlQuery(string url, string queryFormat, params string[] args)
        {
            var encodedArgs = args.Select(arg => (object)HttpUtility.UrlEncode(arg)).ToArray();

            var query = string.Format(queryFormat, encodedArgs);

            return url + query;
        }
    }
}
