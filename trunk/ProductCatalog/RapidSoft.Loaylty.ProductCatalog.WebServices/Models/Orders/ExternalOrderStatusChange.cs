namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ExternalOrderStatusChange : OrderStatusChange
    {
        [DataMember]
        public int? PartnerId { get; set; }

        [DataMember]
        public string ExternalOrderId { get; set; }

        [DataMember]
        public DateTime? ExternalOrderStatusDateTime { get; set; }

        [DataMember]
        public string ExternalOrderStatusCode { get; set; }
    }
}
