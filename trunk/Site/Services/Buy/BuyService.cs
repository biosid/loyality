using System;
using System.Globalization;
using System.Linq;
using Vtb24.Site.Security;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Security.OneTimePasswordService.Models;
using Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions;
using Vtb24.Site.Security.OneTimePasswordService.Models.Inputs;
using Vtb24.Site.Services.AdvancePayment.Models.Outputs;
using Vtb24.Site.Services.BonusPayments;
using Vtb24.Site.Services.BonusPayments.Models;
using Vtb24.Site.Services.BonusPayments.Models.Inputs;
using Vtb24.Site.Services.Buy.Models.Exceptions;
using Vtb24.Site.Services.Buy.Models.Inputs;
using Vtb24.Site.Services.Buy.Models.Outputs;
using Vtb24.Site.Services.GiftShop;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.Infrastructure;
using ConfirmOrderParams = Vtb24.Site.Services.Buy.Models.Inputs.ConfirmOrderParams;

namespace Vtb24.Site.Services.Buy
{
    public class BuyService : IBuy, IDisposable
    {
        public const string ORDER_OTP_TYPE = "order_confirm";

        public static readonly int BankProductsPartnerId = AppSettingsHelper.Int("bank_orders_partner_id", -1);

        public BuyService(ClientPrincipal principal,
                          IClientService client,
                          IOneTimePasswordService otp,
                          IBonusPayments payments,
                          ICardRegistration card,
                          IGiftShopOrders orders,
                          IAdvancePaymentService advancePayment,
                          IBankProductsService bankProducts)
        {
            _principal = principal;
            _client = client;
            _otp = otp;
            _payments = payments;
            _card = card;
            _orders = orders;
            _advancePayment = advancePayment;
            _bankProducts = bankProducts;
        }

        private readonly ClientPrincipal _principal;
        private readonly IClientService _client;
        private readonly IOneTimePasswordService _otp;
        private readonly IBonusPayments _payments;
        private readonly IGiftShopOrders _orders;
        private readonly ICardRegistration _card;
        private readonly IAdvancePaymentService _advancePayment;
        private readonly IBankProductsService _bankProducts;

        #region API

        public DeliveryVariants GetDeliveryVariants(Guid[] basketItemIds, DeliveryLocationInfo locationInfo)
        {
            AssertUser("Невозможно получить варианты доставки");

            return _orders.GetDeliveryVariants(
                _principal.ClientId, 
                _client.GetMechanicsContext(), 
                basketItemIds,
                locationInfo
            );
        }

        public int BeginOrderConfirmation(BeginOrderConfirmationParams parameters)
        {
            AssertUser("Невозможно создать заказ");

            var order = _orders.CreateOrderDraft(_principal.ClientId,
                                                 _client.GetMechanicsContext(),
                                                 parameters.BasketItemIds,
                                                 parameters.Delivery,
                                                 parameters.TotalAdvance);

            // проверка изменения цены
            if (order.TotalPrice != parameters.TotalPrice)
            {
                throw new OrderPriceChangedException(order.TotalPrice);
            }

            // проверка правильности суммы доплаты рублями
            if (order.TotalAdvance != parameters.TotalAdvance)
            {
                throw new Exception("Сумма доплаты рублями не совпадает");
            }

            AssertBalance(order.TotalPrice);

            return order.Id;
        }

        public BeginConfirmationOtp SendOrderConfirmationOtp(int orderId)
        {
            AssertUser("Невозможно отправить код подтверждения заказа");

            var order = _orders.GetOrder(_principal.ClientId, orderId);

            AssertBalance(order.TotalPrice);

            return SendOrderConfirmationOtp(order);
        }

        public BeginConfirmationOtp SendOnlineOrderConfirmationOtp(BeginOnlineOrderConfirmationParams parameters)
        {
            AssertUser("Невозможно отправить код подтверждения заказа онлайн партнёра");

            var order = _orders.GetOrderByExternalId(_principal.ClientId, parameters.PartnerId, parameters.ExternalOrderId);

            AssertBalance(order.TotalPrice);

            return SendOrderConfirmationOtp(order);
        }

        public bool IsAdvancePaymentRequired(int orderId)
        {
            AssertUser("Невозможно проверить заказ");
            AssertCard("Невозможно проверить заказ");

            // получение заказа
            var order = _orders.GetOrder(_principal.ClientId, orderId);

            return order.TotalAdvance > 0;
        }

        public PaymentFormParameters GetAdvancePaymentFormParameters(int orderId, string otpToken, string returnUrlSuccess, string returnUrlFail)
        {
            AssertUser("Невозможно подтвердить заказ");
            AssertCard("Невозможно подтвердить заказ");

            // получение заказа
            var order = _orders.GetOrder(_principal.ClientId, orderId);

            // проверка статуса заказа и OTP
            AssertOrderConfirmable(order, otpToken);

            return _advancePayment.GetPaymentFormParameters(_principal.ClientId, orderId, order.TotalAdvance, returnUrlSuccess, returnUrlFail);
        }

        public GiftShopOrder ConfirmOrder(ConfirmOrderParams options)
        {
            AssertUser("Невозможно подтвердить заказ");
            AssertCard("Невозможно подтвердить заказ");

            // получение заказа
            var order = _orders.GetOrder(_principal.ClientId, options.OrderId);

            // проверка статуса заказа и OTP
            AssertOrderConfirmable(order, options.OtpToken);

            // проверка платежа картой
            AssertAdvancePayment(order);

            // списание баллов
            Charge(order);

            // подтверждение заказа
            var confirmed = _orders.ConfirmOrder(_principal.ClientId, options.OrderId);

            return confirmed;
        }

        public int BeginBankProductOrderConfirmation(string bankProductId)
        {
            AssertUser("Невозможно создать заказ");

            if (string.IsNullOrWhiteSpace(bankProductId))
            {
                throw new InvalidOperationException("Не указан Id продукта банка");
            }

            if (BankProductsPartnerId <= 0)
            {
                throw new InvalidOperationException("Не настроен PartnerId для продуктов банка");
            }

            var bankProduct = _client.GetBankProduct(bankProductId);
            if (bankProduct == null)
            {
                throw new InvalidOperationException("Для клиента не найден продукт банка. Id = " + bankProductId);
            }

            var order = _orders.CreateCustomOrder(_principal.ClientId,
                                                  BankProductsPartnerId,
                                                  bankProduct.Id,
                                                  bankProduct.Id,
                                                  bankProduct.Description,
                                                  bankProduct.Cost,
                                                  0);

            AssertBalance(order.TotalPrice);

            return order.Id;
        }

        public GiftShopOrder ConfirmBankProductOrder(ConfirmOrderParams options)
        {
            AssertUser("Невозможно подтвердить заказ");
            AssertCard("Невозможно подтвердить заказ");

            // получение заказа
            var order = _orders.GetOrder(_principal.ClientId, options.OrderId);

            // проверка статуса заказа и OTP
            AssertOrderConfirmable(order, options.OtpToken);

            // деактивация продукта банка
            _bankProducts.DeactivateProduct(order.ExternalId);

            // списание баллов
            Charge(order);

            // подтверждение заказа
            var confirmed = _orders.ConfirmOrder(_principal.ClientId, options.OrderId);

            return confirmed;
        }

        #endregion

        private void Charge(GiftShopOrder order)
        {
            var chargeParams = new ChargeParameters
            {
                ClientId = _principal.ClientId,
                ChequeNumber = order.Id.ToString(CultureInfo.InvariantCulture),
                ChequeTime = DateTime.Now,
                Sum = order.TotalPrice,
                Items = order.Items
                             .Select(item =>
                                     new ChequeItem
                                     {
                                         Article = item.ProductId ?? item.Article, // Для продуктов каталога - Id, для онлайн партнёров - артикул
                                         Title = item.Title,
                                         Price = item.QuantityBonusPrice,
                                         Quantity = item.Quantity
                                     })
                             .ToArray()
            };
            _payments.Charge(chargeParams);
        }

        private BeginConfirmationOtp SendOrderConfirmationOtp(GiftShopOrder order)
        {
            // отправка otp
            var template = AppSettingsHelper.String(
                "order_confirm_otp_notification",
                "Код подтверждения заказа на сумму {0:N0} {0:pluralize|бонус|бонуса|бонусов}: {1}. Спасибо за Ваш заказ в Программе Коллекция."
            );
            var message = string.Format(new PluralizerFormatter(), template, order.TotalPrice, "{0}");
            var otpParams = new SendOtpParameters
            {
                To = _principal.PhoneNumber,
                OtpType = ORDER_OTP_TYPE,
                ExternalId = order.Id.ToString(CultureInfo.InvariantCulture),
                MessageTemplate = message,
                DeliveryMeans = OtpDeliveryMeans.Sms
            };
            var otp = _otp.Send(otpParams);

            return new BeginConfirmationOtp
            {
                OrderId = order.Id,
                OtpToken = otp.OtpToken,
                CreationTimeUtc = otp.CreationTimeUtc,
                ExpirationTimeUtc = otp.ExpirationTimeUtc
            };
        }

        private void AssertOrderConfirmable(GiftShopOrder order, string otpToken)
        {
            // проверка статуса
            if (order.Status == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    string.Format("Заказ отменен партнером (id: {0}).", order.Id));
            }

            if (order.Status != OrderStatus.Registration)
            {
                throw new InvalidOperationException(
                    string.Format("Заказ уже подтверждён (id: {0}). Невозможно подтвердить повторно", order.Id));
            }

            // проверка OTP
            var otpParams = new IsConfirmedOtpParameters
            {
                OtpType = ORDER_OTP_TYPE,
                ExternalId = order.Id.ToString(CultureInfo.InvariantCulture),
                OtpToken = otpToken
            };

            if (!_otp.IsConfirmed(otpParams))
            {
                throw new OneTimePasswordServiceException(
                    string.Format("Неверный одноразовый пароль. Невозможно подтвердить заказ (id: {0})", order.Id)
                );
            }
        }

        private void AssertAdvancePayment(GiftShopOrder order)
        {
            if (order.DeliveryAdvance == 0)
            {
                return;
            }

            if (!_advancePayment.IsPaymentAuthorized(order.Id))
            {
                throw new PaymentNotAuthorizedException();
            }
        }

        private void AssertUser(string message)
        {
            if (!_principal.IsAuthenticated)
            {
                throw new InvalidOperationException("Пользователь не аутентифицирован. " + message);
            }
        }
 
        private void AssertBalance(decimal price)
        {
            var balance = _client.GetBalance();
            if (balance < price)
            {
                throw new NotEnoughPointsException(balance, price);
            }
        }

        private void AssertCard(string message)
        {
            if (!_card.IsCardRegistered())
            {
                throw new InvalidOperationException("Карта не зарегистрирована. " + message);
            }
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}