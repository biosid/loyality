namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class RecommendProductsParameters : CatalogAdminParameters
    {
        [DataMember]
        public string[] ProductIds { get; set; }

        [DataMember]
        public bool IsRecommended { get; set; }
    }
}
