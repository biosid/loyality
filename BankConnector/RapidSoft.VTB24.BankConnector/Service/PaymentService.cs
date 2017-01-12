namespace RapidSoft.VTB24.BankConnector.Service
{
    using System;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller;
    using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Inputs;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.API.Exceptions;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Extension;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    public class PaymentService : IPaymentService
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(PaymentService));

        private readonly ICatalogAdminService catalogAdmin;
        private readonly IUnitellerAcquiring uniteller;
        private readonly IUnitOfWork uow;
 
        public PaymentService(ICatalogAdminService catalogAdmin, IUnitellerAcquiring uniteller, IUnitOfWork uow)
        {
            this.catalogAdmin = catalogAdmin;
            this.uniteller = uniteller;
            this.uow = uow;
        }

        public GenericBankConnectorResponse<PaymentFormParameters> GetPaymentFormParameters(PaymentFormRequest request)
        {
            try
            {
                return new GenericBankConnectorResponse<PaymentFormParameters>(GetUnitellerPaymentFormParameters(request));
            }
            catch (Exception e)
            {
                var message = "Ошибка получения параметров формы оплаты: " + e.Message;
                logger.Error(message, e);
                return new GenericBankConnectorResponse<PaymentFormParameters>((int)ExceptionType.GeneralException, false, message);
            }
        }

        public GenericBankConnectorResponse<bool> IsPaymentAuthorized(int orderId)
        {
            try
            {
                var isAuthorized = IsUnitellerPaymentAuthorized(orderId);

                return isAuthorized.HasValue
                           ? new GenericBankConnectorResponse<bool>(isAuthorized.Value)
                           : new GenericBankConnectorResponse<bool>((int)ExceptionType.PaymentNotFound, false, "Ошибка проверки статуса оплаты: платеж не найден");
            }
            catch (Exception e)
            {
                var message = "Ошибка проверки статуса оплаты: " + e.Message;
                logger.Error(message, e);
                return new GenericBankConnectorResponse<bool>((int)ExceptionType.GeneralException, false, message);
            }
        }

        public SimpleBankConnectorResponse ConfirmPayment(int orderId)
        {
            try
            {
                return ConfirmUnitellerPayment(orderId)
                           ? new SimpleBankConnectorResponse()
                           : new SimpleBankConnectorResponse((int)ExceptionType.PaymentNotFound, false, "Ошибка подтверждения оплаты: платеж не найден");
            }
            catch (Exception e)
            {
                var message = "Ошибка поддтверждения оплаты: " + e.Message;
                logger.Error(message, e);
                return new SimpleBankConnectorResponse((int)ExceptionType.GeneralException, false, message);
            }
        }

        public SimpleBankConnectorResponse CancelPayment(int orderId)
        {
            try
            {
                return CancelUnitellerPayment(orderId)
                           ? new SimpleBankConnectorResponse()
                           : new SimpleBankConnectorResponse((int)ExceptionType.PaymentNotFound, false, "Ошибка отмены оплаты: платеж не найден");
            }
            catch (Exception e)
            {
                var message = "Ошибка отмены оплаты: " + e.Message;
                logger.Error(message, e);
                return new SimpleBankConnectorResponse((int)ExceptionType.GeneralException, false, message);
            }
        }

        public GenericBankConnectorResponse<PaymentInfo> GetPaymentByOrderId(int orderId)
        {
            try
            {
                var paymentInfo = new PaymentInfo
                {
                    OrderId = orderId
                };

                FillUnitellerPaymentInfo(paymentInfo);

                return new GenericBankConnectorResponse<PaymentInfo>(paymentInfo);
            }
            catch (Exception e)
            {
                var message = "Ошибка при получении данных о платеже по заказу " + orderId.ToString("D") + ": " + e.Message;
                logger.Error(message, e);
                return new GenericBankConnectorResponse<PaymentInfo>((int)ExceptionType.GeneralException, false, message);
            }
        }

        private Order GetOrder(int orderId)
        {
            var response = catalogAdmin.GetOrderById(ConfigHelper.VtbSystemUser, orderId);

            if (!response.Success)
            {
                var message = string.Format("CatalogAdminService.GetOrderById вернул ошибку: {0} - {1}", response.ResultCode, response.ResultDescription);
                throw new Exception(message);
            }

            if (response.Order == null)
            {
                throw new Exception("CatalogAdminService.GetOrderById вернул null");
            }

            return response.Order;
        }

        private Partner GetPartner(int partnerId)
        {
            var response = catalogAdmin.GetPartnerById(partnerId, ConfigHelper.VtbSystemUser);

            if (!response.Success)
            {
                var message = string.Format("CatalogAdminService.GetPartnerById вернул ошибку: {0} - {1}", response.ResultCode, response.ResultDescription);
                throw new Exception(message);
            }

            if (response.Partner == null)
            {
                throw new Exception("CatalogAdminService.GetPartnerById вернул null");
            }

            return response.Partner;
        }

        private string GetShopId(Partner partner)
        {
            if (!partner.IsAdvancePaymentsSupported())
            {
                throw new Exception("партнер не поддерживает платежи картой");
            }

            var shopId = partner.UnitellerAdvancePaymentShopId();

            if (string.IsNullOrWhiteSpace(shopId))
            {
                throw new Exception("неверные настройки партнера");
            }

            return shopId;
        }

        private string GetShopId(int orderId)
        {
            var order = GetOrder(orderId);

            var partner = GetPartner(order.PartnerId);

            var shopId = GetShopId(partner);

            return shopId;
        }

        private PaymentFormParameters GetUnitellerPaymentFormParameters(PaymentFormRequest request)
        {
            var shopId = GetShopId(request.OrderId);

            uow.UnitellerPaymentsRepository.SaveRequest(request.OrderId, shopId);
            uow.Save();

            var parameters = new UnitellerPayParameters
            {
                ShopId = shopId,
                OrderId = request.OrderId.ToString("D"),
                Subtotal = request.Amount,
                ReturnUrlSuccess = request.ReturnUrlSuccess,
                ReturnUrlFail = request.ReturnUrlFail,
                CustomerId = request.ClientId + "_advance",
                Lifetime = ConfigHelper.PaymentUnitellerLifetime,
                Preauth = ConfigHelper.PaymentUnitellerUsePreauth
            };

            return new PaymentFormParameters
            {
                Url = uniteller.PaymentFormUrl,
                Method = "POST",
                Parameters = uniteller.GetPaymentFormParameters(parameters)
            };
        }

        private bool? IsUnitellerPaymentAuthorized(int orderId)
        {
            var unitellerPayment = uow.UnitellerPaymentsRepository.GetByOrderId(orderId);
            if (unitellerPayment == null)
            {
                return null;
            }

            var paymentInfo = uniteller.GetPaymentInfo(unitellerPayment.ShopId, orderId.ToString("D"));
            if (paymentInfo == null)
            {
                return null;
            }

            unitellerPayment.BillNumber = paymentInfo.BillNumber;
            uow.Save();

            return paymentInfo.IsAuthorized;
        }

        private bool ConfirmUnitellerPayment(int orderId)
        {
            var unitellerPayment = uow.UnitellerPaymentsRepository.GetByOrderId(orderId);
            if (unitellerPayment == null)
            {
                return false;
            }

            if (unitellerPayment.BillNumber == null)
            {
                var paymentInfo = uniteller.GetPaymentInfo(unitellerPayment.ShopId, orderId.ToString("D"));
                if (paymentInfo == null)
                {
                    return false;
                }

                unitellerPayment.BillNumber = paymentInfo.BillNumber;
                uow.Save();
            }

            // подтверждаем, только если используется преавторизация
            if (ConfigHelper.PaymentUnitellerUsePreauth)
            {
                uniteller.ConfirmPayment(unitellerPayment.ShopId, unitellerPayment.BillNumber);
            }

            try
            {
                unitellerPayment.ConfirmDate = DateTime.Now;
                uow.Save();
            }
            catch (Exception e)
            {
                logger.Error("ошибка при сохранении факта подтверждения платежа в uniteller по заказу " + orderId.ToString("D"), e);
            }

            return true;
        }

        private bool CancelUnitellerPayment(int orderId)
        {
            var unitellerPayment = uow.UnitellerPaymentsRepository.GetByOrderId(orderId);
            if (unitellerPayment == null)
            {
                return false;
            }

            if (unitellerPayment.BillNumber == null)
            {
                var paymentInfo = uniteller.GetPaymentInfo(unitellerPayment.ShopId, orderId.ToString("D"));
                if (paymentInfo == null)
                {
                    return false;
                }

                unitellerPayment.BillNumber = paymentInfo.BillNumber;
                uow.Save();
            }

            uniteller.CancelPayment(unitellerPayment.ShopId, unitellerPayment.BillNumber);

            try
            {
                unitellerPayment.CancelDate = DateTime.Now;
                uow.Save();
            }
            catch (Exception e)
            {
                logger.Error("ошибка при сохранении факта отмены платежа в uniteller по заказу " + orderId.ToString("D"), e);
            }

            return true;
        }

        private void FillUnitellerPaymentInfo(PaymentInfo paymentInfo)
        {
            var unitellerPayment = uow.UnitellerPaymentsRepository.GetByOrderId(paymentInfo.OrderId);
            if (unitellerPayment == null)
            {
                return;
            }

            paymentInfo.UnitellerShopId = unitellerPayment.ShopId;
            paymentInfo.UnitellerBillNumber = unitellerPayment.BillNumber;
        }
    }
}
