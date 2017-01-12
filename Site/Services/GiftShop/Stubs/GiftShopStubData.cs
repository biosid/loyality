using System.Collections.Generic;
using System.Linq;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Site.Services.GiftShop.Stubs
{
    internal static class GiftShopStubData
    {        
        static GiftShopStubData()
        {
            Basket = new List<ReservedProductItem>();
            WishList = new List<ReservedProductItem>();

            Categories = CategoriesGenerator.Generate();
            CategoriesRoots = Categories.Where(cat => !cat.ParentId.HasValue).ToList();
            
            Products = ProductGenerator.Generate(30, Categories);

            Orders = new List<GiftShopOrder>();

            Addresses = new List<DeliveryAddress>(); // TODO: сгенерячить чего-нибудь
        }

        public static List<ReservedProductItem> WishList { get; private set; }

        public static List<ReservedProductItem> Basket { get; private set; } 

        public static List<CatalogCategory> Categories { get; private set; }

        public static List<CatalogCategory> CategoriesRoots { get; private set; }

        public static List<CatalogProduct> Products { get; private set; }

        public static List<GiftShopOrder> Orders { get; private set; }

        public static List<DeliveryAddress> Addresses { get; private set; } 
    }
}