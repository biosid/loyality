namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class OrderNotificationItem
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int ProductQuantity { get; set; }
    }
}
