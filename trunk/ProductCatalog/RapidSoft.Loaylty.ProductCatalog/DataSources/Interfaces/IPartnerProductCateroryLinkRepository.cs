namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System.Collections.Generic;

    using Entities;

    public interface IPartnerProductCateroryLinkRepository
    {
        IList<PartnerProductCategoryLink> SetPartnerProductCateroryLink(API.Entities.PartnerProductCategoryLink link, string userId);

        IList<PartnerProductCategoryLink> GetPartnerProductCateroryLinks(int partnerId, int[] categoryIds = null);

        bool IsUniqueLinks(int partnerId, int categoryId, string partnerCategoryName);
    }
}