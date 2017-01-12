namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeExternalOrdersStatusesParameters
    {
        [DataMember]
        public ExternalOrderStatusChange[] Changes { get; set; }
    }
}
