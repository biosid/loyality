namespace RapidSoft.Loaylty.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Loaylty.ProductCatalog.WsClients;
    using Loaylty.ProductCatalog.WsClients.BasketService;
    using Loaylty.ProductCatalog.WsClients.CatalogSearcherService;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Extensions;

    using Product = Loaylty.ProductCatalog.WsClients.CatalogSearcherService.Product;

    public class TestUtils
    {
        private const string clientId = "intUser";

        private static Dictionary<string, string> clientContext = TestHelper.GetClientContext();
        
        public static Product FindProduct(int? partnerId = null)
        {
            // NOTE: Находим товар
            var clientContext = TestHelper.GetClientContext();
            var param = new SearchPublicProductsParameters
                            {
                                    CountToTake = 10, 
                                    ClientContext = clientContext,
                                    PartnerIds = partnerId != null ? partnerId.Value.MakeArray() : null
                            };

            var res =
                WebClientCaller.CallService<CatalogSearcherClient, SearchProductsResult>(
                    x => x.SearchPublicProducts(param));
            
            Assert.IsNotNull(res, "Результат поиска НЕ получен");
            Assert.IsNotNull(res.Products, "Результат НЕ содержит продуктов");
            
            var product = res.Products.Last();
            return product;
        }
    
        public static Partner FindPartner(Func<Partner, bool> selector)
        {
            // NOTE: Находим партнера
            var res =
                WebClientCaller.CallService<CatalogSearcherClient, GetAllPartnersResult>(
                    x => x.GetAllPartners());

            Assert.IsNotNull(res, "Результат поиска НЕ получен  ");
            Assert.IsNotNull(res.Partners, "Результат НЕ содержит партнеров");
            Assert.IsTrue(res.Partners.Length > 0, "Результат НЕ null, но не содержит партнеров");

            var selected = res.Partners.FirstOrDefault(selector);

            return selected;
        }

        public static GetBasketItemResult AddBasketItem(string productId)
        {
            // Создать заказ
            GetBasketItemResult basketItem;

            using (var client = new BasketServiceClient())
            {
                var result = client.SetQuantity(clientId, productId, 1, clientContext);
                Assert.IsTrue(result.Success, result.ResultDescription);

                basketItem = client.GetBasketItem(clientId, productId, clientContext);
                Assert.IsTrue(basketItem.Success);
                Assert.IsNotNull(basketItem.Item);
                Assert.IsNotNull(basketItem.Item.Id);
            }

            return basketItem;
        }
    }
}