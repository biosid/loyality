namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// Стратегия вычисления статуса модерации для партнера с низким уровнем доверия.
    /// </summary>
    internal class HighPartnerTrustCalculator : ModerationStatusCalculator
    {
        public HighPartnerTrustCalculator(IList<Product> existsProduct)
            : base(existsProduct)
        {
        }

        public override ProductModerationStatuses CalcModerationStatus(Product newProduct)
        {
            var exist = this.GetExistAs(newProduct);

            if (exist == null)
            {
                return ProductModerationStatuses.Applied;
            }
            else
            {
                return exist.ModerationStatus;
            }
        }
    }
}