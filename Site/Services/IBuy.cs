using System;
using Vtb24.Site.Services.AdvancePayment.Models.Outputs;
using Vtb24.Site.Services.Buy.Models.Inputs;
using Vtb24.Site.Services.Buy.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Site.Services
{
    public interface IBuy
    {
        DeliveryVariants GetDeliveryVariants(Guid[] basketItemId, DeliveryLocationInfo locationInfo);

        int BeginOrderConfirmation(BeginOrderConfirmationParams parameters);

        BeginConfirmationOtp SendOrderConfirmationOtp(int orderId);

        BeginConfirmationOtp SendOnlineOrderConfirmationOtp(BeginOnlineOrderConfirmationParams parameters);

        bool IsAdvancePaymentRequired(int orderId);

        PaymentFormParameters GetAdvancePaymentFormParameters(int orderId, string otpToken, string returnUrlSuccess, string returnUrlFail);

        GiftShopOrder ConfirmOrder(ConfirmOrderParams parameters);

        int BeginBankProductOrderConfirmation(string bankProductId);

        GiftShopOrder ConfirmBankProductOrder(ConfirmOrderParams options);
    }
}