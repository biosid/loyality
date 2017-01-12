namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public interface ICategoryPermissionRepository
    {
        IList<CategoryPermission> GetByPartner(int partnerId);

        string[] GetProductIdsHavingPermissions(string[] productIds);

        void Save(string userId, IList<CategoryPermission> categoryPermissions);

        void Delete(string userId, int partnerId, IList<int> categoryIds);
    }
}