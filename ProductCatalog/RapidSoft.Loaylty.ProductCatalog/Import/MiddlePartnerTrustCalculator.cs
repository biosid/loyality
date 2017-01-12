namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// Стратегия вычисления статуса модерации для партнера со средним уровнем доверия.
    /// </summary>
    internal class MiddlePartnerTrustCalculator : ModerationStatusCalculator
    {
        public MiddlePartnerTrustCalculator(IList<Product> existsProduct)
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
            else
            {
                return exist.ModerationStatus;
            }
        }
    }
}