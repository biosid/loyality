namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    internal class ProductsFixBasePriceDatesRepository : BaseRepository, IProductsFixBasePriceDatesRepository
    {
        private const string CLEANUPSQLCOMMANDTEMPLATE =
            @"delete from [prod].[ProductsFixedPrices] where not ProductId in (select productId from [prod].[Products])
              update [prod].[Products] set BasePriceRUR = null where ProductId in (select ProductId from [prod].[ProductsFixedPrices] where FixedPriceDate < '{0}')
              delete from [prod].[ProductsFixedPrices] where FixedPriceDate < '{0}'";

        public ProductsFixBasePriceDatesRepository()
            : base(DataSourceConfig.ConnectionString)
        {
        }

        public ProductsFixBasePriceDatesRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void Set(string productId)
        {
            using (var ctx = DbNewContext())
            {
                var now = DateTime.Now;

                var existing = ctx.ProductsFixBasePriceDates.SingleOrDefault(d => d.ProductId == productId);

                if (existing != null)
                {
                    existing.FixedPriceDate = now;
                }
                else
                {
                    ctx.ProductsFixBasePriceDates.Add(new ProductFixBasePriceDate
                    {
                        ProductId = productId,
                        FixedPriceDate = now
                    });
                }

                ctx.SaveChanges();
            }
        }

        public void Reset(string productId)
        {
            using (var ctx = DbNewContext())
            {
                var existing = ctx.ProductsFixBasePriceDates.SingleOrDefault(d => d.ProductId == productId);

                if (existing != null)
                {
                    ctx.ProductsFixBasePriceDates.Remove(existing);
                    ctx.SaveChanges();
                }
            }
        }

        public void Cleanup(DateTime until)
        {
            using (var ctx = DbNewContext())
            {
                var cmd = string.Format(CLEANUPSQLCOMMANDTEMPLATE, until.ToString("yyyy-MM-ddTHH:mm:ss"));
                ctx.Database.ExecuteSqlCommand(cmd);
            }
        }

        public string[] GetByProductIds(string[] productIds)
        {
            if (productIds == null || productIds.Length == 0)
            {
                return new string[0];
            }

            using (var ctx = DbNewContext())
            {
                var result = ctx.ProductsFixBasePriceDates
                                .Where(d => productIds.Contains(d.ProductId))
                                .Select(d => d.ProductId)
                                .ToArray();

                return result;
            }
        }
    }
}
