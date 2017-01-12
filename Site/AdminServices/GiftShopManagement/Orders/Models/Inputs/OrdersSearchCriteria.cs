using System;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs
{
    public class OrdersSearchCriteria
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int[] SupplierIds { get; set; }

        public int[] CarrierIds { get; set; }

        public int? OrderId { get; set; }

        public OrderStatus[] Statuses { get; set; }

        public OrderStatus[] SkipStatuses { get; set; }

        public OrderPaymentStatus[] ProductPaymentStatuses { get; set; }

        public OrderPaymentStatus[] DeliveryPaymentStatuses { get; set; }
    }
}