using Vtb24.Site.Content.Advertisements.Models.Output;
using Vtb24.Site.Content.Models;

namespace Vtb24.Site.Content.Advertisements
{
    public interface IAdvertisementsManagement
    {
		GetAllAdvertisementsResult GetAdvertisements(string clientId, PagingSettings paging);

        void IncreaseShowCounter(long advertisementId, string clientId);
    }
}
