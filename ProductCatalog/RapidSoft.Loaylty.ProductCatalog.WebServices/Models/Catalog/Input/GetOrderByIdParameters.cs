namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetOrderByIdParameters : CatalogAdminParameters
    {
        [DataMember]
        public int OrderId { get; set; }
    }
}
