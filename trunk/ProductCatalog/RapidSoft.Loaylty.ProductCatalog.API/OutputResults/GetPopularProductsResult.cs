namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public class GetPopularProductsResult : ResultBase
    {
        public PopularProduct[] PopularProducts
        {
            get;
            set;
        }
    }
}