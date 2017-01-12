using System;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models
{
    public class OrderDelivery
    {
        public OrderDeliveryType Type { get; set; }

        public string ExternalDeliveryVariantId { get; set; }

        public string DeliveryVariantName { get; set; }

        public OrderDeliveryAddress Address { get; set; }

        public OrderDeliveryPickupPoint PickupPoint { get; set; }

        public OrderDeliveryContact Contact { get; set; }

        public string Comment { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public TimeSpan? DeliveryTimeFrom { get; set; }

        public TimeSpan? DeliveryTimeTo { get; set; }

        public string AdditionalText { get; set; }
    }
}
