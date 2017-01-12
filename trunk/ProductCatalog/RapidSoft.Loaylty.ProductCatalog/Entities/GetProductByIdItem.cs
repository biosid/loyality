namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetProductByIdItem
    {
        public Product Product { get; set; }

        public ProductAvailabilityStatuses AvailabilityStatus { get; set; }
    }
}
