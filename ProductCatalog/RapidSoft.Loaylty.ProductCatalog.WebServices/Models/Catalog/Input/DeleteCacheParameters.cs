namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DeleteCacheParameters : CatalogAdminParameters
    {
        [DataMember]
        public int Seconds { get; set; }
    }
}
