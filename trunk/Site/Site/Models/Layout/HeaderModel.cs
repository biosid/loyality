using Vtb24.Site.Models.Shared.Location;

namespace Vtb24.Site.Models.Layout
{
    public class HeaderModel
    {
        public bool IsAuthenticated { get; set; }

        public bool ShowMyInfoLink { get; set; }

        public string UserFullName { get; set; }

        public string UserLocationName { get; set; }

        public string UserLocationKladr { get; set; }

        public int UnreadMessagesCount { get; set; }

        public decimal Balance { get; set; }

        public long BasketCount { get; set; }

        public long WishListCount { get; set; }

        public string ActiveMenu { get; set; }

        public bool EnableHeaderDropdown { get; set; }

        public bool HideRegionSelector { get; set; }

        public bool SearchSiteInitial { get; set; }

        public LocationItem[] Regions { get; set; }
    }
}
