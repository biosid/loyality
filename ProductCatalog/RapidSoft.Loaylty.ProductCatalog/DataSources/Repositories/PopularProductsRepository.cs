namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;

    public class PopularProductsRepository : IPopularProductsRepository
    {
        public int GetRate(PopularProductTypes popularType, string productId)
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var rate = ctx.PopularProducts.FirstOrDefault(pp => pp.PopularType == popularType && pp.ProductId == productId);

                return rate != null ? rate.ProductRate : 0;
            }
        }
    }
}