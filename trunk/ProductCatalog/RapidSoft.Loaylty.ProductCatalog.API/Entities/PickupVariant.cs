namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    public class PickupVariant
    {
        [DataMember]
        public PickupPoint PickupPoint { get; set; }
        
        [DataMember]
        public decimal DeliveryCost { get; set; }

        [DataMember]
        public decimal BonusDeliveryCost { get; set; }
    }
}