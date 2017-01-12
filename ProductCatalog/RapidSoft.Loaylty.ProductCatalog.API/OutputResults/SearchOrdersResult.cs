namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using Entities;

    public class SearchOrdersResult : ResultBase
    {
        public Order[] Orders { get; set; }

        public int? TotalCount { get; set; }
    }
}