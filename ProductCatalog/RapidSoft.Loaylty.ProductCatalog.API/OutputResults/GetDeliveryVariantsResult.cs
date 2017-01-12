namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class GetDeliveryVariantsResult : ResultBase
    {
        [DataMember]
        public VariantsLocation Location
        {
            get;
            set;
        }
        
        [DataMember]
        public DeliveryGroup[] DeliveryGroups
        {
            get;
            set;
        }
    }
}