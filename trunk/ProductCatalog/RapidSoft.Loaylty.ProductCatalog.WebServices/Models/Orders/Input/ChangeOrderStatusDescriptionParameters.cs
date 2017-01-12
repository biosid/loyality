namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeOrderStatusDescriptionParameters
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public string OrderStatusDescription { get; set; }
    }
}
