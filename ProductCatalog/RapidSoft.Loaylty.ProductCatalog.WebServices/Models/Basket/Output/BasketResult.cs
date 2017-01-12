namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Basket.Output
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [DataContract]
    public class BasketResult : PagedResult<ClientItem>
    {
        [DataMember]
        public decimal TotalPrice { get; set; }
    }
}
