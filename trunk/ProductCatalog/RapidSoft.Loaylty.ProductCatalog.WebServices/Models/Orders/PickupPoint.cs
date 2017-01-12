namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    public class PickupPoint
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ExternalPickupPointId { get; set; }

        [DataMember]
        public string DeliveryVariantName { get; set; }

        [DataMember]
        public string ExternalDeliveryVariantId { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Phones { get; set; }
        
        [DataMember]
        public string OperatingHours { get; set; }

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