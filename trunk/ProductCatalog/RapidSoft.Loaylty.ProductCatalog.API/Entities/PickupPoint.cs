namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PickupPoint
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ExternalPickupPointId { get; set; }

        [DataMember]
        public string ExternalDeliveryVariantId { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string[] Phones { get; set; }
        
        [DataMember]
        public string[] OperatingHours { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}