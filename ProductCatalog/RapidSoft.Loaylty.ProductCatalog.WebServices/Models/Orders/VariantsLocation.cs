namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    public class VariantsLocation
    {
        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public string KladrCode { get; set; }

        [DataMember]
        public string PostCode { get; set; }
    }
}
