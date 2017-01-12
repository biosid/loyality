namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    internal interface IProductsRepository
    {
        ResultBase MoveProducts(MoveProductsParameters parameters);

        ResultBase ChangeStatuses<T>(T parameters);

        ResultBase ChangeStatusesByPartner(ChangeStatusByPartnerParameters parameters);

        List<ProductSortProjection> GetAll();

        ProductSortProjection GetById(LoyaltyDBEntities context, string productId);

        ProductSortProjection GetById(string productId);

        void DeleteProducts(DeleteProductParameters parameters);

        int GetProductPartnerId(string productId);

        List<ProductSortProjection> GetByPartnerId(int partnerId);

        string[] FilterByTargetAudiences(string[] productIds, string[] targetAudienceIds);

        void RemoveProductTargetAudiences(string userId, string[] productIds);

        void AddProductTargetAudiences(string userId, string[] productIds, string[] targetAudienceIds);
    }
}
