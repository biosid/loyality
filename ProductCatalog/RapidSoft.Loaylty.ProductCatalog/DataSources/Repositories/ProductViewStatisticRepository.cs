namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Linq;
    
    using Entities;

    using Extensions;

    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;

    [Obsolete]
    public class ProductViewStatisticRepository : IProductViewStatisticRepository
    {
        public ProductViewStatistic RegisterProductView(string clientId, string productId)
        {
            clientId.ThrowIfNull("clientId");
            productId.ThrowIfNull("productId");

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                 var entity = ctx.ProductViewStatistics.SingleOrDefault(x => x.ProductId == productId && x.ClientId == clientId);

                if (entity == null)
                {
                    entity = new ProductViewStatistic
                    {
                        UpdatedDate = DateTime.Now,
                        ClientId = clientId,
                        ProductId = productId,
                        ViewCount = 1
                    };

                    ctx.ProductViewStatistics.Add(entity);
                }
                else
                {
                    entity.ViewCount++;
                    entity.UpdatedDate = DateTime.Now;
                }

                ctx.SaveChanges();
                return entity;
            }
        }

        public ProductViewStatistic GetProductViewStatistic(string clientId, string productId)
        {
            clientId.ThrowIfNull("clientId");
            productId.ThrowIfNull("productId");

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var existed = ctx.ProductViewStatistics.SingleOrDefault(x => x.ProductId == productId && x.ClientId == clientId);
                return existed;
            }
        }
    }
}