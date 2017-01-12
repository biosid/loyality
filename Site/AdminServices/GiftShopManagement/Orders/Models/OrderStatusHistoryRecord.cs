using System;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models
{
    public class OrderStatusHistoryRecord
    {
        public DateTime When { get; set; }

        public string Who { get; set; }

        public OrderStatusHistoryField<OrderStatus?> Status { get; set; }

        public OrderStatusHistoryField<string> StatusDescription { get; set; }

        public OrderStatusHistoryField<OrderPaymentStatus?> ProductPaymentStatus { get; set; }

        public OrderStatusHistoryField<OrderPaymentStatus?> DeliveryPaymentStatus { get; set; }
    }
}
