namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using System;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class OrderNotification
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public string ExternalOrderId { get; set; }

        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public decimal TotalCost { get; set; }

        [DataMember]
        public string DeliveryInfo { get; set; }

        public DeliveryInfo DeliveryInfoObject { get; set; }

        [DataMember]
        public OrderNotificationItem[] Items { get; set; }
    }
}
