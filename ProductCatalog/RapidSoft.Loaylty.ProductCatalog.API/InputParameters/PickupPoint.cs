namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PickupPoint
    {
        [DataMember]
        public string ExternalPickupPointId { get; set; }
    }
}