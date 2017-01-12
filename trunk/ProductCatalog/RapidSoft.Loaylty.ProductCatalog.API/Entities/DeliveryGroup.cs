namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    public class DeliveryGroup
    {
        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public DeliveryVariant[] DeliveryVariants { get; set; }
    }
}