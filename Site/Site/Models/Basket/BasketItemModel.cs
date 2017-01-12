using System;
using Vtb24.Site.Models.Catalog.Helpers;
using Vtb24.Site.Services.GiftShop.Model;

namespace Vtb24.Site.Models.Basket
{
    public class BasketItemModel
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public string ProductNotification { get; set; }

        public bool IsAvailable { get; set; }

        public int Quantity { get; set; }

        public decimal BasketItemPrice { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal? PointsDeficit { get; set; }

        public decimal PointsProgress { get; set; }

        public bool HasDiscount { get; set; }

        public bool IsNew { get; set; }

        public bool CanBuy
        {
            get { return IsAvailable && PointsDeficit == null; }
        }

        public static BasketItemModel Map(ReservedProductItem original, decimal balance)
        {
            return new BasketItemModel
            {
                Id = original.Id,
                ProductId = original.Product.Id,
                Title = original.Product.Title,
                Thumbnail = original.Product.Thumbnail,
                IsAvailable = original.ProductStatus == ProductStatus.Available,
                ProductNotification = ProductNotificationMapper.Map(original.ProductStatus),
                Quantity = original.Quantity,
                BasketItemPrice = original.TotalQuantityPrice,
                ProductPrice = original.ItemPrice,
                HasDiscount = original.Product.HasDiscount,
                IsNew = ProductNoveltyCriteria.IsNew(original.Product.AddedToCatalogDate),
                PointsDeficit = balance < original.TotalQuantityPrice
                                    ? original.TotalQuantityPrice - balance
                                    : (decimal?) null,
                PointsProgress = original.TotalQuantityPrice != 0
                                     ? Math.Min(Math.Round(100 * balance / original.TotalQuantityPrice), 100)
                                     : 0
            };
        }
    }
}