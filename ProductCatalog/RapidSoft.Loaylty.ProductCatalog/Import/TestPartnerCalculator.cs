namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class TestPartnerCalculator : IModerationStatusCalculator
    {
        public ProductModerationStatuses CalcModerationStatus(Product newProduct)
        {
            return ProductModerationStatuses.Applied;
        }
    }
}