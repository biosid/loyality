using Vtb24.Arms.Catalog.Models.Gifts.Helpers;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;
using Vtb24.Arms.AdminServices.Infrastructure;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class GiftModel
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string Name { get; set; }

        public string CategoryPath { get; set; }

        public string PictureUrl { get; set; }

        public Statuses Status { get; set; }

        public string ModerationStatus { get; set; }

        public bool IsRecommended { get; set; }

        public string[] Segments { get; set; }

        public decimal Price { get; set; }

        public static GiftModel Map(Product product)
        {
            return new GiftModel
            {
                Id = product.Id,
                ProductId = product.SupplierProductId,
                Name = product.Name,
                CategoryPath = product.CategoryPath,
                PictureUrl = product.Pictures.MaybeFirstOrDefault(),
                Status = product.Status.Map(),
                ModerationStatus = ProductModerationStatusMapper.Map(product.ModerationStatus),
                IsRecommended = product.IsRecommended,
                Segments = product.Segments,
                Price = product.PriceRUR
            };
        }
    }
}
