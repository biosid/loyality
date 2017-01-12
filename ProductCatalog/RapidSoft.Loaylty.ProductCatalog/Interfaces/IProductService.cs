namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public interface IProductService
    {
        void CalculateProductBasePrice(Product oldProduct, Product newProduct, decimal newPrice, decimal? basePrice, bool copyBasePrice);
    }
}
