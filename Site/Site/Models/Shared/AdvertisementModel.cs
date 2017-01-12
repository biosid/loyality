using Vtb24.Site.Content.Advertisements.Models;

namespace Vtb24.Site.Models.Shared
{
    public class AdvertisementModel
    {
        public string Url { get; set; }

        public string PictureUrl { get; set; }

        public string CssClass { get; set; }

        public static AdvertisementModel Map(ClientAdvertisement original)
        {
            return new AdvertisementModel
            {
                PictureUrl = original.Advertisement.PictureUrl,
                Url = original.Advertisement.Url,
                CssClass = original.Advertisement.CssClass ?? string.Empty
            };
        }
    }
}