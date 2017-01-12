namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Output
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [DataContract]
    public class AdminProductsResult : PagedResult<AdminProduct>
    {
        [DataMember]
        public decimal? MaxPrice { get; set; }
    }
}
