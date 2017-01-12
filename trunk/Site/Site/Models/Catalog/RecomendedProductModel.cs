using Vtb24.Site.Services.GiftShop.Catalog.Models;

namespace Vtb24.Site.Models.Catalog
{
    public class RecomendedProductModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public decimal? Price { get; set; }

        public static RecomendedProductModel Map(CatalogProduct original)
        {
            return new RecomendedProductModel
            {
                Id = original.Product.Id,
                Title = original.Product.Title,
                Thumbnail = original.Product.Thumbnail,
                Price = original.Product.Price
            };
        }
    }
}
