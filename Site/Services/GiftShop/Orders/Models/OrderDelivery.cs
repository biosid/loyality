using System;

namespace Vtb24.Site.Services.GiftShop.Orders.Models
{
    public class OrderDelivery
    {
        public DeliveryType Type { get; set; }

        public string ExternalDeliveryVariantId { get; set; }

        public string DeliveryVariantName { get; set; }

        public string DeliveryVariantDescription { get; set; }

        public DeliveryVariantsLocation DeliveryVariantsLocation { get; set; }

        public DeliveryAddress Address { get; set; }

        public DeliveryPickupPoint PickupPoint { get; set; }

        public DeliveryContact Contact { get; set; }

        public string Comment { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public TimeSpan? DeliveryTimeFrom { get; set; }

        public TimeSpan? DeliveryTimeTo { get; set; }

        public string AdditionalText { get; set; }
    }
}
