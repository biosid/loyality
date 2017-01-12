namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using System.Collections.Generic;

    using API.InputParameters;
    using API.OutputResults;

    using DataSources;

    using RapidSoft.Loaylty.ProductCatalog.Entities;

    public interface IProductsSearcher
    {
        GetProductByIdItem GetProductById(string productId, Dictionary<string, string> clientContext);

        GetProductByIdItem[] GetProductsByIds(string[] productIds, Dictionary<string, string> clientContext);

        SearchProductsResult AdminSearchProducts(AdminSearchProductsParameters parameters);

        SearchProductsResult SearchPublicProducts(SearchProductsParameters parameters);

        string[] GetListOfAttributeValues(ProductAttributes attribute, int? categoryId);

        PopularProduct[] GetPopularProducts(GetPopularProductParameters parameters);
    }
}
