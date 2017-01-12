namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Basket.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class RemoveParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string ProductId { get; set; }
    }
}
