namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    internal class OrderStatusChangeFact
    {
        public int OrderId { get; set; }

        public OrderStatuses OriginalStatus { get; set; }

        public OrderStatuses NewStatus { get; set; }
    }
}
