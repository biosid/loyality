namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Output
{
    using System.Runtime.Serialization;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [DataContract]
    public class DeliveryVariantsResult : ResultBase
    {
        [DataMember]
        public VariantsLocation Location { get; set; }
        
        [DataMember]
        public DeliveryGroup[] DeliveryGroups { get; set; }
    }
}
