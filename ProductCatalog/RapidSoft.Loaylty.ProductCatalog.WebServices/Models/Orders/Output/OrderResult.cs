namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Output
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [DataContract]
    public class OrderResult : ValueResult<Order>
    {
        [DataMember]
        public OrderStatuses[] NextStatuses { get; set; }
    }
}
