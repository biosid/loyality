namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SetDeliveryLocationKladrParameters : CatalogAdminParameters
    {
        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string Kladr { get; set; }
    }
}
