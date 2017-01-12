namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    public class DeliveryVariant
    {
        [DataMember]
        public DeliveryTypes DeliveryType { get; set; }

        [DataMember]
        public string DeliveryVariantName { get; set; }

        [DataMember]
        public string ExternalDeliveryVariantId { get; set; }

        [DataMember]
        public PickupVariant[] PickupPoints { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal DeliveryCost { get; set; }

        [DataMember]
        public decimal BonusDeliveryCost { get; set; }
    }
}