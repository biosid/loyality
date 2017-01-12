namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CreateProductParameters : ProductParameters
    {
        [DataMember]
        public string PartnerProductId { get; set; }
    }
}
