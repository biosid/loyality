namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetLastDeliveryAddressesParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public int? Take { get; set; }
    }
}
