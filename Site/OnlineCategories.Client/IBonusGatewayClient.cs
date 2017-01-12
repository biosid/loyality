using System;
using Vtb24.OnlineCategories.Client.Models;

namespace Vtb24.OnlineCategories.Client
{
    public interface IBonusGatewayClient
    {
        /// <summary>
        /// реализует взаимеодействие ValidateUser
        /// </summary>
        ClientInfo ResolveClient(string userTicket);

        /// <summary>
        /// создает ссылку для взаимодействия "вызов формы списания баллов"
        /// </summary>
        string CreatePaymentFormUrl(string userTicket, string orderId, decimal maxDiscount);

        /// <summary>
        /// реализует взаимодействие CancelPayment
        /// </summary>
        [Obsolete]
        void CancelPayment(string orderId, decimal? discountRefund);

        /// <summary>
        /// реализует взамодействие NotifyOrder в сценарии "создание заказа"
        /// </summary>
        void CreateOrder(CreateOrderRequest request);

        /// <summary>
        /// реализует взаимодействие NotifyOrder в сценарии "смена статуса заказа"
        /// </summary>
        void NotifyOrderStatus(NotifyOrderStatusRequest request);

        /// <summary>
        /// реализует взаимодействие "GetPaymentStatus" в сценарии "проверка списания баллов за заказ"
        /// </summary>
        bool IsPaid(string orderId);
    }
}
