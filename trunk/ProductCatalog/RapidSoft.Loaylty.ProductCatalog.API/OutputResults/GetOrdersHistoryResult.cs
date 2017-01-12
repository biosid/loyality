namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetOrdersHistoryResult : ResultBase
    {
        public Order[] Orders { get; set; }

        public int? TotalCount { get; set; }
    }
}