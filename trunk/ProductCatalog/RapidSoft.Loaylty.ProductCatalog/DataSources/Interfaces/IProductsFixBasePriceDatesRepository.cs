namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System;

    internal interface IProductsFixBasePriceDatesRepository
    {
        void Set(string productId);

        void Reset(string productId);

        void Cleanup(DateTime until);

        string[] GetByProductIds(string[] productIds);
    }
}
