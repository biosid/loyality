namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeOrdersStatusesParameters
    {
        [DataMember]
        public OrderStatusChange[] Changes { get; set; }
    }
}
