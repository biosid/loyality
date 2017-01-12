namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using Entities;

    public class GetRecomendedProductsResult : ResultBase
    {
        public Product[]   Products
        {
            get;
            set;
        }
    }
}