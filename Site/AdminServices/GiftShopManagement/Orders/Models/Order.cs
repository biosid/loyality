using System;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public int SupplierId { get; set; }

        public int? CarrierId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public OrderStatus Status { get; set; }

        public string StatusDescription { get; set; }

        public OrderStatus[] NextStatuses { get; set; }

        public OrderPaymentStatus ProductPaymentStatus { get; set; }

        public OrderPaymentStatus DeliveryPaymentStatus { get; set; }

        public OrderItem[] Items { get; set; }

        public decimal ItemsPrice { get; set; }

        public decimal ItemsBonusPrice { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal DeliveryBonusPrice { get; set; }

        public decimal DeliveryAdvance { get; set; }

        public decimal TotalAdvance { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalBonusPrice { get; set; }

        public OrderDelivery Delivery { get; set; }
    }
}
