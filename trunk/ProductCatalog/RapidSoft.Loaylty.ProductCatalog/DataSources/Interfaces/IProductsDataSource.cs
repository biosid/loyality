namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using API.InputParameters;
    using API.OutputResults;

    using PromoAction.WsClients.MechanicsService;

    public interface IProductsDataSource
    {
        SearchProductsResult AdminSearchProducts(AdminSearchProductsParameters parameters);

        SearchProductsResult SearchPublicProducts(SearchProductsParameters parameters, GenerateResult mechanicsSql);

        CalculatedProductPrices[] CalculateProductsPrices(string[] productsIds, GenerateResult mechanicsSql);
    }
}