namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    public class DeliveryVariant
    {
        [DataMember]
        public string DeliveryVariantName { get; set; }

        [DataMember]
        public string ExternalDeliveryVariantId { get; set; }

        [DataMember]
        public PickupPoint[] PickupPoints { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal ItemsCost { get; set; }

        [DataMember]
        public decimal DeliveryCost { get; set; }

        [DataMember]
        public decimal TotalCost { get; set; }
    }
}