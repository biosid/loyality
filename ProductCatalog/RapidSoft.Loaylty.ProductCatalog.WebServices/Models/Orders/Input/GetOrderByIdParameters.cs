namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetOrderByIdParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public int OrderId { get; set; }
    }
}