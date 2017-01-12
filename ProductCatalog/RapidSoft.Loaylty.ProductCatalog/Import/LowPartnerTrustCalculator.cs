namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;

    /// <summary>
    /// Стратегия вычисления статуса модерации для партнера с низким уровнем доверия.
    /// </summary>
    internal class LowPartnerTrustCalculator : ModerationStatusCalculator
    {
        private readonly AdoMapComparer<Product> comparer =
            new AdoMapComparer<Product>(ProductsDataSource.ProductColumns, "PriceRUR", "CategoryId");

        public LowPartnerTrustCalculator(IList<Product> existsProduct)
            : base(existsProduct)
        {
        }

        public override ProductModerationStatuses CalcModerationStatus(Product newProduct)
        {
            var exist = this.GetExistAs(newProduct);

            if (exist == null)
            {
                return ProductModerationStatuses.InModeration;
            }

            var isEquals = comparer.Equals(exist, newProduct);
            if (isEquals)
            {
                return exist.ModerationStatus;
            }
            else
            {
                return ProductModerationStatuses.InModeration;
            }
        }
    }
}