namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ArmGetProductByIdParameters
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string ProductId { get; set; }
    }
}