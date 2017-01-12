using System;
using Vtb24.Site.Helpers;
using Vtb24.Site.Models.Catalog.Helpers;
using Vtb24.Site.Models.Shared;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Models.Catalog
{
    public class ProductModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public BreadCrumbModel[] BreadCrumbs { get; set; }

        public RecomendedProductModel[] RecomendedProducts { get; set; }

        public string Vendor { get; set; }

        public string[] Pictures { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithoutDiscount { get; set; }

        public decimal? PointsDeficit { get; set; }

        public decimal PointsProgress { get; set; }

        public string ProductNotification { get; set; }

        public bool IsAvailible { get; set; }

        public bool CanRedeem { get; set; }

        public ProductParamModel[] Params { get; set; }

        public bool HasDiscount { get; set; }

        public bool IsNew { get; set; }

        public int? DiscountPercent { get; set; }

        public int ViewsCount { get; set; }

        public string[] Keywords { get; set; }

        public static ProductModel Map(CatalogProduct original, decimal? balance)
        {
            var product = new ProductModel
            {
                Id = original.Product.Id,
                Title = original.Product.Title,
                Description = original.Description,
                Vendor = original.Product.Vendor,
                Pictures = original.Pictures,
                Price = original.Product.Price,
                PriceWithoutDiscount = original.Product.PriceWithoutDiscount,
                IsAvailible = original.ProductStatus == ProductStatus.Available,
                ProductNotification = ProductNotificationMapper.Map(original.ProductStatus),
                HasDiscount = original.Product.HasDiscount,
                IsNew = ProductNoveltyCriteria.IsNew(original.Product.AddedToCatalogDate),
                Params = original.Parameters.MaybeSelect(ProductParamModel.Map).MaybeToArray(),
                PointsProgress = 100,
                ViewsCount = original.ViewsCount
            };

            if (balance.HasValue && balance < product.Price)
            {
                product.PointsDeficit = product.Price - balance;
            }

            if (balance.HasValue && product.Price != 0m)
            {
                var progress = Math.Round((balance.Value / product.Price) * 100);
                product.PointsProgress = Math.Min(progress, 100);
            }

            product.CanRedeem = product.IsAvailible &&
                                (!balance.HasValue || ProductHelpers.CanRedeem(product.Price, balance.Value));

            if (original.Product.HasDiscount)
            {
                var discount = (int)Math.Round(100 - (original.Product.Price / original.Product.PriceWithoutDiscount) * 100);

                if (discount == 100)
                {
                    discount = 99;
                }

                product.DiscountPercent = discount > 0 ? (int?)discount : null;
            }

            return product;
        }
    }
}