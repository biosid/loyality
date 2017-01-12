using System;
using System.Collections.Generic;
using System.Linq;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.ProductCatalogSearcherService;
using Product = Vtb24.Site.Services.GiftShop.Model.Product;

namespace Vtb24.Site.Services.GiftShop.Catalog
{
    internal static class MappingsFromService
    {
        public static ProductStatus ToProductStatus(ProductAvailabilityStatuses original)
        {
            switch (original)
            {
                case ProductAvailabilityStatuses.Available:
                    return ProductStatus.Available;

                case ProductAvailabilityStatuses.DeliveryRateNotFound:
                    return ProductStatus.DeliveryNotAvailable;

                case ProductAvailabilityStatuses.CategoryIsNotActive:
                case ProductAvailabilityStatuses.CategoryPermissionNotFound:
                case ProductAvailabilityStatuses.ModerationNotApplied:
                case ProductAvailabilityStatuses.PartnerIsNotActive:
                case ProductAvailabilityStatuses.PriceNotFound:
                case ProductAvailabilityStatuses.ProductIsNotActive:
                case ProductAvailabilityStatuses.TargetAudienceNotFound:
                    return ProductStatus.NotInStock;

                default:
                    return ProductStatus.Unknown;
            }
        }

        public static CatalogProduct.Parameter ToCatalogProductParameter(ProductParam original)
        {
            if (original == null)
                return null;

            return new CatalogProduct.Parameter
            {
                Name = original.Name,
                Unit = original.Unit,
                Value = original.Value
            };
        }

        public static ProductPopularityType ToProductPopularityType(PopularProductTypes original)
        {
            switch (original)
            {
                case PopularProductTypes.MostOrdered:
                    return ProductPopularityType.MostOrdered;
                case PopularProductTypes.MostViewed:
                    return ProductPopularityType.MostViewed;
                case PopularProductTypes.MostWished:
                    return ProductPopularityType.MostWished;
                default:
                    return ProductPopularityType.Unknown;
            }
        }

        public static Product ToProduct(ProductCatalogSearcherService.Product original)
        {
            if (original == null)
                return null;

            return new Product
            {
                Id = original.ProductId,
                PartnerId = original.PartnerId,
                CategoryId = original.CategoryId,
                CategoryTitle = original.CategoryName,
                Title = original.Name,
                Thumbnail = original.Pictures == null ? null 
                                    : original.Pictures.FirstOrDefault(),
                Vendor = original.Vendor,
                VendorCode = original.VendorCode,
                HasDiscount = original.IsActionPrice,
                AddedToCatalogDate = original.InsertedDate,
                PriceRur = original.PriceRUR,
                Price = original.Price,
                PriceWithoutDiscount = original.PriceBase
            };
        }

        public static CatalogProduct ToCatalogProduct(ProductCatalogSearcherService.Product original)
        {
            return ToCatalogProduct(original, ProductAvailabilityStatuses.Available, 0);
        }

        public static CatalogProduct ToCatalogProduct(ProductCatalogSearcherService.Product original, ProductAvailabilityStatuses status, int viewsCount)
        {
            if (original == null)
                return null;

            return new CatalogProduct
            {
                Product = ToProduct(original),
                ProductStatus = ToProductStatus(status),
                ViewsCount = viewsCount,
                Description = original.Description,
                Pictures = original.Pictures,
                Parameters = original.Param != null
                                 ? original.Param.Select(ToCatalogProductParameter).ToArray()
                                 : null
            };
        }

        public static CatalogProduct ToCatalogProduct(PopularProduct original)
        {
            if (original == null)
                return null;

            var product = ToCatalogProduct(original.Product);
            product.PopularityRate = original.ProductRate;

            return product;
        }

        public static CategoryType ToCategoryType(ProductCategoryTypes original)
        {
            switch (original)
            {
                case ProductCategoryTypes.Online:
                    return CategoryType.Online;
                case ProductCategoryTypes.Static:
                    return CategoryType.Static;
                default:
                    return CategoryType.Unknown;
            }
        }

        public static CatalogCategory ToCatalogCategory(ProductCategory original)
        {
            if (original == null)
                return null;

            return new CatalogCategory
            {
                Id = original.Id,
                ParentId = original.ParentId,
                ProductsCount = original.ProductsCount,
                SubCategoriesCount = original.SubCategoriesCount,
                Title = original.Name,
                CategoryPath = original.NamePath,
                Depth = original.NestingLevel,
                CategoryType = ToCategoryType(original.Type),
                OnlineCategoryUrl = original.OnlineCategoryUrl,
                OnlineCategoryReturnUrl = original.NotifyOrderStatusUrl,
                OnlineCategoryPartnerId = original.OnlineCategoryPartnerId
            };
        }

        public static CatalogCategoryInfo ToCatalogCategoryInfo(GetCategoryInfoResult original)
        {
            if (original == null)
                return null;

            return new CatalogCategoryInfo
            {
                Category = ToCatalogCategory(original.Category),
                PathToCategory = original.CategoryPath
                                         .Select(ToCatalogCategory)
                                         .ToArray()
            };
        }

        public static IEnumerable<CatalogParameter> ToCatalogParameters(IEnumerable<ProductParamResult> productParams, Dictionary<string, CatalogParameterDefinition> parameterDefinitions)
        {
            using (var enumerator = productParams.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var productParam = enumerator.Current;

                    CatalogParameterDefinition definition;
                    if (!parameterDefinitions.TryGetValue(productParam.Name, out definition))
                        continue;

                    yield return new CatalogParameter
                    {
                        Definition = definition,
                        Values = productParam.Values
                    };
                }
            }
        }

        public const string IS_ORDER_REQUIRES_EMAIL_PARTNER_SETTING_KEY = "IsOrderRequiresEmail";
        public const string ADVANCE_PAYMENT_SUPPORT_PARTNER_SETTING_KEY = "AdvancePaymentSupport";
        public const string ONLINE_DELIVERY_VARIANTS_URL_PARTNER_SETTING_KEY = "GetDeliveryVariants";
        public const string MAX_ADVANCE_FRACTION_PARTNER_SETTING_KEY = "MaxAdvanceFraction";

        private static string ToStringPartnerSetting(IReadOnlyDictionary<string, string> original, string name)
        {
            string value;
            return original.TryGetValue(name, out value) ? value : null;
        }

        private static bool ToBooleanPartnerSetting(IReadOnlyDictionary<string, string> original, string name)
        {
            string stringValue;
            bool value;
            return original.TryGetValue(name, out stringValue) && bool.TryParse(stringValue, out value) && value;
        }

        private static int? ToIntegerPartnerSetting(IReadOnlyDictionary<string, string> original, string name)
        {
            string stringValue;
            int value;
            return original.TryGetValue(name, out stringValue) && int.TryParse(stringValue, out value)
                       ? value
                       : (int?) null;
        }

        private static TEnum ToEnumPartnerSetting<TEnum>(IReadOnlyDictionary<string, string> original, string name, TEnum defaultValue)
            where TEnum : struct
        {
            string stringValue;
            TEnum value;
            return original.TryGetValue(name, out stringValue) && Enum.TryParse(stringValue, out value)
                       ? value
                       : defaultValue;
        }

        public static CatalogPartnerSettings ToCatalogPartnerSettings(Dictionary<string, string> original)
        {
            return new CatalogPartnerSettings
            {
                IsOnlineDeliveryVariansSupported = !string.IsNullOrWhiteSpace(ToStringPartnerSetting(original, ONLINE_DELIVERY_VARIANTS_URL_PARTNER_SETTING_KEY)),
                IsOrderRequiresEmail = ToBooleanPartnerSetting(original, IS_ORDER_REQUIRES_EMAIL_PARTNER_SETTING_KEY),
                AdvancePaymentSupport = ToEnumPartnerSetting(original, ADVANCE_PAYMENT_SUPPORT_PARTNER_SETTING_KEY, AdvancePaymentSupportMode.None),
                MaxAdvanceFraction = ToIntegerPartnerSetting(original, MAX_ADVANCE_FRACTION_PARTNER_SETTING_KEY)
            };
        }

        public static CatalogPartner ToCatalogPartner(Partner original)
        {
            return new CatalogPartner
            {
                Id = original.Id,
                Name = original.Name,
                RawSettings = original.Settings,
                Settings = ToCatalogPartnerSettings(original.Settings)
            };
        }
    }
}
