namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class OrdersNotificationsEmail
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string EtlSessionId { get; set; }

        [DataMember]
        public string Recipients { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public OrdersNotificationsEmailStatus Status { get; set; }
    }
}
