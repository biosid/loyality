namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System;

    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    [Obsolete]
    public interface IProductViewStatisticRepository
    {
        ProductViewStatistic RegisterProductView(string clientId, string productId);

        ProductViewStatistic GetProductViewStatistic(string userId, string productId);
    }
}