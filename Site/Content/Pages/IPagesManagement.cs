using System;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Pages.Models.Inputs;
using Vtb24.Site.Content.Pages.Models.Outputs;

namespace Vtb24.Site.Content.Pages
{
    public interface IPagesManagement
    {
        GetAllPagesResult GetAllPlainPages(GetPlainPagesOptions options, PagingSettings paging);

        Page GetPlainPageById(Guid id, GetPlainPagesOptions options = new GetPlainPagesOptions());

        bool HasPageWithUrl(string url, GetPlainPagesOptions options = new GetPlainPagesOptions());

        Page GetPageByUrl(string url, GetPlainPagesOptions options = new GetPlainPagesOptions());

        PageData GetPageVersionById(Guid id);

        void ReloadBuiltinPagesFromConfiguration();

        Page GetOfferPageByPartnerId(int partnerId, bool loadFullHistory);

        bool IsPartnerGotOfferPage(int partnerId);
    }
}
