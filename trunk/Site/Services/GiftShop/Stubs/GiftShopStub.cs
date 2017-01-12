using System;
using System.Collections.Generic;
using System.Linq;
using Vtb24.Site.Services.GiftShop.Basket.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Wishlist.Models.Outputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GiftShop.Stubs
{
    public class GiftShopStub : IGiftShop
    {
        private int _lastBasketId;
        private int _lastOrderId;

        public CategoriesResult GetCategories(int? parentCategoryId, int? depth, PagingSettings paging)
        {
            IEnumerable<CatalogCategory> categories;

            int? level;
            int immediate;
            if (parentCategoryId == null)
            {
                level = depth;
                immediate = GiftShopStubData.CategoriesRoots.Count;
                categories = GiftShopStubData.Categories.ToArray();
            }
            else
            {
                var parent = GiftShopStubData.Categories.FirstOrDefault(c => c.Id == parentCategoryId);
                if (parent == null)
                {
                    return null;
                }

                level = depth.HasValue ? parent.Depth + depth : null;
                immediate = parent.SubCategoriesCount;
                categories = GiftShopStubData.Categories
                    .SkipWhile(c => c != parent).Skip(1)
                    .TakeWhile(c => c.Depth > parent.Depth)
                    .ToArray();
            }

            var total = categories.Count();

            //  вложенность
            if (level.HasValue)
            {
                categories = categories.Where(c => c.Depth <= level);
            }

            // пейджинг
            categories = categories.MaybeSkip(paging.Skip).MaybeTake(paging.Take);

            var result = categories.ToArray();

            return new CategoriesResult(result, immediate, total, paging);
        }

        public CatalogCategoryInfo GetCategoryInfo(int categoryId)
        {
            var category = GiftShopStubData.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return null;
            }

            var path = new List<CatalogCategory>();
            var current = category;
            while (current.ParentId != null)
            {
                path.Add(current);
                current = GiftShopStubData.Categories.First(c => c.Id == current.ParentId);
            }
            path.Add(current);

            path.Reverse();

            return new CatalogCategoryInfo
            {
                Category = category,
                PathToCategory = path.ToArray()
            };
        }

        public CatalogProduct GetProduct(string productId, bool countAsView = false)
        {
            return GiftShopStubData.Products.FirstOrDefault(p => p.Product.Id == productId);
        }

        public SearchResult Search(SearchCriteria criteria, PagingSettings paging)
        {
            // TODO: реализовать поиск по нескольким категориям и по CatalogPartner'ам

            var allproducts =
                GetCategories(
                    criteria.Categories == null ? (int?)null : criteria.Categories.FirstOrDefault(),
                    criteria.ExcludeSubCategories ? 1 : (int?)null,
                    paging
                )
                .SelectMany(c => c.Products)
                .Where(
                    p => p.ProductStatus == ProductStatus.Available
                    && (string.IsNullOrWhiteSpace(criteria.SearchTerm) || p.Product.Title.Contains(criteria.SearchTerm))
                )
                .ToArray();

            var products = allproducts.Where(p => (criteria.MinPrice == null || p.Product.Price >= criteria.MinPrice)
                                               && (criteria.MaxPrice == null || p.Product.Price <= criteria.MaxPrice))
                                            .ToArray();

            var totalCount = products.Count();

            var maxPrice = allproducts.Max(p => p.Product.Price);

            var result = products.MaybeSkip(paging.Skip).MaybeTake(paging.Take);

            return new SearchResult(result, totalCount, maxPrice, paging);
        }

        public SubCatalogInfo GetFilterMetaData(int? parentCategoryId = null)
        {
            return new SubCatalogInfo
            {
                Vendors = new[] { "aaa", "bbb" }
            };
        }

        public CatalogParameter[] GetCategoryParameters(int categoryId)
        {
            return new CatalogParameter[0];
        }

        public IEnumerable<CatalogProduct> GetPopularProducts(ProductPopularityType type, int? countToTake)
        {
            return GiftShopStubData.Products.MaybeTake(countToTake);
        }

        public IEnumerable<CustomProduct> GetCustomPopularProducts(decimal balance)
        {
            return new CustomProduct[0];
        }

        public IEnumerable<CatalogProduct> GetRecommendedProducts(int? countToTake)
        {
            return GiftShopStubData.Products.MaybeTake(countToTake);
        }

        public IEnumerable<CatalogProduct> GetRecommendedProductsForPriceRange(decimal minPrice, decimal maxPrice, int? countToTake)
        {
            return GiftShopStubData.Products.MaybeTake(countToTake);
        }

        public IEnumerable<CatalogProduct> GetRecommendedProductsForCategory(int categoryId, int? countToTake)
        {
            return GiftShopStubData.Products.MaybeTake(countToTake);
        }

        public IEnumerable<CustomProduct> GetCustomRecommendedProducts(decimal balance)
        {
            return new CustomProduct[0];
        }

        public CatalogPartner[] GetPartners()
        {
            return new[]
            {
                new CatalogPartner
                {
                    Id = 1,
                    Name = "Stub",
                    RawSettings = new Dictionary<string, string>()
                }
            };
        }

        public WishListResult GetWishList(PagingSettings paging)
        {
            var items = GiftShopStubData.WishList.MaybeSkip(paging.Skip).MaybeTake(paging.Take);
            var result = new WishListResult(items, GiftShopStubData.WishList.Count, paging);

            return result;
        }

        public ReservedProductItem GetWishedProduct(string productId)
        {
            var item = GiftShopStubData.WishList.FirstOrDefault(p => p.Product.Id == productId);
            return item;
        }

        public void AddToWishList(string productId, int quantity = 1)
        {
            var item = GiftShopStubData.WishList.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                item.Quantity += quantity;
                item.TotalQuantityPrice = item.Product.Price * item.Quantity;
                return;
            }

            var product = GetProduct(productId);

            if (product == null)
            {
                return;
            }

            item = new ReservedProductItem
            {
                Product = product.Product,
                TotalQuantityPrice = product.Product.Price * quantity,
                AddedDate = DateTime.Now,
                Quantity = quantity
            };

            GiftShopStubData.WishList.Add(item);
        }

        public void RemoveFromWishList(string productId)
        {
            var item = GiftShopStubData.WishList.FirstOrDefault(w => w.Product.Id == productId);

            if (item == null)
            {
                return;
            }

            GiftShopStubData.WishList.Remove(item);
        }

        public void UpdateWishListItemQuantity(string productId, int quantity)
        {
            var item = GiftShopStubData.WishList.FirstOrDefault(w => w.Product.Id == productId);

            if (item == null)
            {
                return;
            }

            item.Quantity++;
            item.TotalQuantityPrice = item.Product.Price * item.Quantity;
        }

        public BasketResult GetBasket(PagingSettings paging)
        {
            var items = GiftShopStubData.Basket.MaybeSkip(paging.Skip).MaybeTake(paging.Take).MaybeToArray();
            var result = new BasketResult(items, GiftShopStubData.Basket.Count, paging)
            {
                TotalPrice = GiftShopStubData.Basket.Sum(i => i.TotalQuantityPrice)
            };

            return result;
        }

        public ReservedProductItem GetBasketItem(string productId, string locationKladr = null)
        {
            var item = GiftShopStubData.Basket.FirstOrDefault(i => i.Product.Id == productId);
            return item;
        }

        public ReservedProductItem[] GetBasketItems(string[] productIds, string locationKladr = null)
        {
            var items = GiftShopStubData.Basket.Where(i => productIds.Contains(i.Product.Id)).ToArray();
            return items;
        }

        public void AddToBasket(string productId, int quantity = 1)
        {
            var item = GiftShopStubData.Basket.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                item.Quantity += quantity;
                item.TotalQuantityPrice = item.Product.Price * item.Quantity;
                return;
            }

            var product = GetProduct(productId);

            if (product == null)
            {
                return;
            }

            item = new ReservedProductItem
            {
                Id = (_lastBasketId++).ToString(),
                Product = product.Product,
                Quantity = quantity,
                TotalQuantityPrice = product.Product.Price * quantity,
                AddedDate = DateTime.Now
            };

            GiftShopStubData.Basket.Add(item);
        }

        public void RemoveFromBasket(string productId)
        {
            var item = GiftShopStubData.Basket.FirstOrDefault(i => i.Product.Id == productId);

            if (item == null)
            {
                return;
            }

            GiftShopStubData.Basket.Remove(item);
        }

        public void UpdateBasketItemQuantity(string productId, int quantity)
        {
            var item = GiftShopStubData.Basket.FirstOrDefault(i => i.Product.Id == productId);

            if (item == null)
            {
                return;
            }

            item.Quantity++;
            item.TotalQuantityPrice = item.Product.Price * item.Quantity;
        }

        public GiftShopOrder GetOrder(int orderId)
        {
            return GiftShopStubData.Orders.FirstOrDefault(o=>o.Id == orderId);
        }

        public GiftShopOrder GetOrderByExternalId(int partnerId, string externalOrderId)
        {
            return new GiftShopOrder();
        }

        public DeliveryAddressesResult GetDeliveryAddresses(bool excludeAddressesWithoutKladr)
        {
            // TODO: поправить стаб
            //return new DeliveryAddressesResult(
            //    GiftShopStubData.Addresses, 
            //    GiftShopStubData.Addresses.Count, 
            //    PagingSettings.ByOffset(0, 20)
            //);
            throw new NotImplementedException();
        }

        public GiftShopOrdersResult GetOrdersHistory(OrderStatus[] statuses, DateTimeRange range, PagingSettings paging)
        {
            //throw new NotImplementedException(); // TODO: нужно проверить стаб
            IList<GiftShopOrder> orders = new List<GiftShopOrder>();
            var basket = GiftShopStubData.Basket.First();
            var order = new GiftShopOrder
            {
                Id = _lastOrderId++,
                ExternalId = null,
                CreateDate = DateTime.Now,
                Items = new[]{ 
                    new GiftShopOrderItem
                    {
                        BasketId = basket.Id,
                        ProductId = basket.Product.Id,
                        Title = basket.Product.Title,
                        Quantity = basket.Quantity,
                        Price = basket.Product.Price
                    } 
                },
                DeliveryPrice = 0,
                ItemsPrice = basket.TotalQuantityPrice,
                TotalPrice = basket.TotalQuantityPrice,
                Status = OrderStatus.Processing
            };
            if(statuses.Contains(order.Status) && order.CreateDate > range.Start && order.CreateDate < range.End) orders.Add(order);

            return new GiftShopOrdersResult(orders, GiftShopStubData.Addresses.Count, PagingSettings.ByOffset(0, 20));
        }
    }
}
