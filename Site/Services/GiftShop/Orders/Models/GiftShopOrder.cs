using System;

namespace Vtb24.Site.Services.GiftShop.Orders.Models
{
    public class GiftShopOrder
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public int PartnerId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public OrderStatus Status { get; set; }

        public string StatusDescription { get; set; }

        public GiftShopOrderItem[] Items { get; set; }

        public decimal ItemsPrice { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalPriceRur { get; set; }

        public decimal ItemsAdvance { get; set; }

        public decimal DeliveryAdvance { get; set; }

        public decimal TotalAdvance { get; set; }

        public OrderDelivery Delivery { get; set; }

        public string DeliveryInstructions { get; set; }
    }
}