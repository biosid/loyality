namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    [DataContract]
    public class OrderStatusChange
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public int? OrderId { get; set; }

        [DataMember]
        public OrderStatuses? OrderStatus { get; set; }

        [DataMember]
        public string OrderStatusDescription { get; set; }
    }
}
