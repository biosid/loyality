using Vtb24.Site.Services.GiftShop.Catalog.Models;

namespace Vtb24.Site.Models.Main
{
    public class CustomProductModel
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string Thumbnail { get; set; }

        public string PriceText { get; set; }

        public static CustomProductModel Map(CustomProduct original)
        {
            return new CustomProductModel
            {
                Title = original.Name,
                Url = original.Url,
                Thumbnail = original.ImageUrl,
                PriceText = original.PriceText
            };
        }
    }
}
