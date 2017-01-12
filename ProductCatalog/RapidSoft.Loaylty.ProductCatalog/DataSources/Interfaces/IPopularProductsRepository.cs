namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public interface IPopularProductsRepository
    {
        int GetRate(PopularProductTypes popularType, string productId);
    }
}
