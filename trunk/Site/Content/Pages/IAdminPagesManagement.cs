using System;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Pages.Models.Inputs;
using Vtb24.Site.Content.Pages.Models.Outputs;

namespace Vtb24.Site.Content.Pages
{
    public interface IAdminPagesManagement : IPagesManagement
    {
        Guid CreatePlainPage(CreatePlainPageOptions options);

        void UpdatePlainPage(UpdatePlainPageOptions options);

        void RemovePlainPage(Guid id);

        void ChangePlainPagesStatus(Guid[] ids, PageStatus status);

        GetAllPagesResult GetAllOfferPages(PagingSettings paging);

        int CreateOfferPage(CreateOfferPageOptions options);

        void UpdateOfferPage(UpdateOfferPageOptions options);

        void RemoveOfferPage(int partnerId);

        void ChangeOfferPagesStatus(int[] partnerIds, PageStatus status);
    }
}
