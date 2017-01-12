namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    internal interface IModerationStatusCalculator
    {
        ProductModerationStatuses CalcModerationStatus(Product newProduct);
    }
}
