namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
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
