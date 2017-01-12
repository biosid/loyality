namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetAllPartnersResult : ResultBase
    {
        public Partner[] Partners { get; set; }
    }
}