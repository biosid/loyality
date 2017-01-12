using System;
using Vtb24.Site.Helpers;
using Vtb24.Site.Models.Catalog.Helpers;
using Vtb24.Site.Services.GiftShop.Catalog.Models;

namespace Vtb24.Site.Models.Catalog
{
    public class ListProductModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public decimal? Price { get; set; }

        public bool IsInAction { get; set; }

        public bool IsNew { get; set; }

        public decimal? UserBalance { get; set; }

        public bool CanRedeem { get; set; }

        public int? DiscountPercent { get; set; }

        public static ListProductModel Map(CatalogProduct original, decimal? userBalance)
        {
            var model = new ListProductModel
            {
                Id = original.Product.Id,
                Title = original.Product.Title,
                Thumbnail = original.Product.Thumbnail,
                Price = original.Product.Price,
                IsInAction = original.Product.HasDiscount,
                IsNew = ProductNoveltyCriteria.IsNew(original.Product.AddedToCatalogDate),
                UserBalance = userBalance,
                CanRedeem = !userBalance.HasValue || ProductHelpers.CanRedeem(original.Product.Price, userBalance.Value)
            };

            if (original.Product.HasDiscount)
            {
                var discount = (int)Math.Round(100 - (original.Product.Price / original.Product.PriceWithoutDiscount) * 100);

                if (discount == 100)
                {
                    discount = 99;
                }

                model.DiscountPercent = discount > 0 ? (int?) discount : null;
            }

            return model;
        }
    }
}
