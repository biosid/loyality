namespace RapidSoft.Loaylty.IntegrationTests.ProductCatalog
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.IntegrationTests.PromoAction;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogSearcherService;
    using RapidSoft.Loaylty.PromoAction.WsClients.AdminMechanicsService;

    using CatalogResultBase = RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService.ResultBase;
    using ResultBase = RapidSoft.Loaylty.PromoAction.WsClients.AdminMechanicsService.ResultBase;
    using SearchProductsResult = RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogSearcherService.SearchProductsResult;

    [TestClass]
    public class ProductCatalogWSClientTest
    {
        private static readonly List<long> Ids = new List<long>();

        [ClassCleanup]
        public static void ClassCleanup()
        {
            foreach (var id in Ids)
            {
                // NOTE: Убиваем правило
                var innerId = id;
                var deleteRule1 = WebClientCaller.CallService<AdminMechanicsServiceClient, ResultBase>(
                        x => x.DeleteRuleById(innerId, TestHelper.UserId));
                Assert.IsNotNull(deleteRule1);
                Assert.AreEqual(true, deleteRule1.Success, deleteRule1.ResultDescription);
            }
        }

        // NOTE: Тест создан по багу VTBPLK-1149
        [TestMethod]
        public void ShouldApplyRulesWith_p_CategoryId()
        {
            // NOTE: Чистим кэш, иначе получим старые цены
            WebClientCaller.CallService<CatalogAdminServiceClient, CatalogResultBase>(
                x => x.DeleteCache(0, TestHelper.UserId));

            var product = TestUtils.FindProduct();

            // NOTE: Запоминаем 
            var category = product.CategoryId;
            var priceRur = product.PriceRUR;
            var priceBase = product.PriceBase;
            var price = product.Price;

            // NOTE: Находим домен
            var domainResults = WebClientCaller.CallService<AdminMechanicsServiceClient, RuleDomainsResult>(
                    x => x.GetAllRuleDomains(TestHelper.UserId));
            Assert.IsNotNull(domainResults, "Результат загрузки доменов получен");
            var domain = domainResults.RuleDomains.FirstOrDefault(x => x.Name == "Расчёт цены вознаграждения");
            Assert.IsNotNull(domain, "Домен \"Расчёт цены вознаграждения\" найден");

            // NOTE: Создаем правило в домене которое увел. цену на 1, если категория найденого нами товара.
            const int Factor = 1;
            var rule1 = new Rule
                            {
                                Name = "Test",
                                Factor = Factor,
                                Type = RuleTypes.BaseAddition,
                                RuleDomainId = domain.Id,
                                Predicate = @"<?xml version=""1.0"" encoding=""utf-16""?>
<filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <equation operator=""eq"">
    <value type=""attr"">
      <attr object=""p"" name=""CategoryId"" type=""numeric"" />
    </value>
    <value type=""numeric"">" + category + @"</value>
  </equation>
</filter>",
                                Priority = int.MaxValue
                            };
            var saveRule1 = WebClientCaller.CallService<AdminMechanicsServiceClient, RuleResult>(
                    x => x.CreateRule(rule1, TestHelper.UserId));
            Assert.IsTrue(saveRule1.Success, saveRule1.ResultDescription);
            var ruleId = saveRule1.Rule.Id;
            Ids.Add(ruleId);

            // NOTE: Чистим кэш, иначе получим старые цены
            WebClientCaller.CallService<CatalogAdminServiceClient, CatalogResultBase>(
                x => x.DeleteCache(0, TestHelper.UserId));

            // NOTE: Находим товар еще раз
            var ids = new[] { product.ProductId };
            var param2 = new SearchPublicProductsParameters { CountToTake = 20, ClientContext = TestHelper.GetClientContext() };
            var res2 =
                WebClientCaller.CallService<CatalogSearcherClient, SearchProductsResult>(
                    x => x.SearchPublicProducts(param2));
            Assert.IsNotNull(res2, "Продукт найден повторно");
            var product2 = res2.Products.Single(p => p.ProductId == product.ProductId);

            Assert.AreNotEqual(price, product2.Price);
            Assert.AreEqual(price + Factor, product2.Price, "Цена товара в баллах изменилась");
            Assert.AreEqual(priceBase + Factor, product2.PriceBase, "Базовая цена в баллах изменилась");
            Assert.AreEqual(priceRur, product2.PriceRUR, "Цена товара не меняется");

            // NOTE: Убиваем правило
            var deleteRule1 = WebClientCaller.CallService<AdminMechanicsServiceClient, ResultBase>(
                    x => x.DeleteRuleById(ruleId, TestHelper.UserId));
            Assert.IsNotNull(deleteRule1);
            Assert.AreEqual(true, deleteRule1.Success);
        }

        // NOTE: Тест создан по багу VTBPLK-1149
        [TestMethod]
        public void ShouldApplyRulesWith_ClientProfile_Audiences()
        {
            var ctx1 = new Dictionary<string, string> { { "ClientProfile.KLADR", "7700000000000" } };

            // NOTE: Чистим кэш, иначе получим старые цены
            WebClientCaller.CallService<CatalogAdminServiceClient, CatalogResultBase>(
                x => x.DeleteCache(0, TestHelper.UserId));

            // NOTE: Находим товар
            var param = new SearchPublicProductsParameters { CountToTake = 10, ClientContext = ctx1 };
            var res =
                WebClientCaller.CallService<CatalogSearcherClient, SearchProductsResult>(
                    x => x.SearchPublicProducts(param));
            Assert.IsNotNull(res);
            var product = res.Products.Last();

            // NOTE: Запоминаем 
            var priceRur = product.PriceRUR;
            var priceBase = product.PriceBase;
            var price = product.Price;

            // NOTE: Находим домен
            var domainResults = WebClientCaller.CallService<AdminMechanicsServiceClient, RuleDomainsResult>(
                    x => x.GetAllRuleDomains(TestHelper.UserId));
            Assert.IsNotNull(domainResults);
            var domain = domainResults.RuleDomains.FirstOrDefault(x => x.Name == "Расчёт цены вознаграждения");
            Assert.IsNotNull(domain);

            // NOTE: Создаем правило в домене которое увел. цену на 2, если ClientProfile.Audiences=VIP
            const int Factor = 2;
            var rule1 = new Rule
                            {
                                Name = "Test",
                                Factor = Factor,
                                Type = RuleTypes.BaseAddition,
                                RuleDomainId = domain.Id,
                                Predicate = @"<?xml version=""1.0"" encoding=""utf-16""?>
<filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <equation operator=""eq"">
    <value type=""attr"">
      <attr object=""ClientProfile"" name=""Audiences"" type=""string"" />
    </value>
    <value type=""string"">VIP</value>
  </equation>
</filter>",
                                Priority = int.MaxValue - 1
                            };
            var saveRule1 = WebClientCaller.CallService<AdminMechanicsServiceClient, RuleResult>(
                    x => x.CreateRule(rule1, TestHelper.UserId));
            Assert.IsTrue(saveRule1.Success, saveRule1.ResultDescription);
            var ruleId = saveRule1.Rule.Id;
            Ids.Add(ruleId);

            // NOTE: Чистим кэш, иначе получим старые цены
            WebClientCaller.CallService<CatalogAdminServiceClient, CatalogResultBase>(
                x => x.DeleteCache(0, TestHelper.UserId));

            // NOTE: Находим товар еще раз указав контекст клиента { "ClientProfile.Audiences", "VIP" } 
            var ids2 = new[] { product.ProductId };
            var ctx2 = new Dictionary<string, string> { { "ClientProfile.Audiences", "VIP" }, { "ClientProfile.KLADR", "7700000000000" } };
            var param2 = new SearchPublicProductsParameters { CountToTake = 20, ClientContext = ctx2 };
            var res2 =
                WebClientCaller.CallService<CatalogSearcherClient, SearchProductsResult>(
                    x => x.SearchPublicProducts(param2));
            Assert.IsNotNull(res2);
            var product2 = res2.Products.Single(p => p.ProductId == product.ProductId);

            Assert.AreNotEqual(price, product2.Price);
            Assert.AreEqual(price + Factor, product2.Price);
            Assert.AreEqual(priceBase + Factor, product2.PriceBase);
            Assert.AreEqual(priceRur, product2.PriceRUR, "Цена товара не меняется");

            // NOTE: Находим товар еще раз указав контекст клиента { "ClientProfile.Audiences", "NOT_VIP" }
            var ids3 = new[] { product.ProductId };
            var ctx3 = new Dictionary<string, string> { { "ClientProfile.Audiences", "NOT_VIP" }, { "ClientProfile.KLADR", "7700000000000" } };
            var param3 = new SearchPublicProductsParameters { CountToTake = 20, ClientContext = ctx3 };
            var res3 =
                WebClientCaller.CallService<CatalogSearcherClient, SearchProductsResult>(
                    x => x.SearchPublicProducts(param3));
            Assert.IsNotNull(res3);
            var product3 = res3.Products.Single(p => p.ProductId == product.ProductId);

            Assert.AreEqual(price, product3.Price, "Цена не должна измениться");

            // NOTE: Убиваем правило
            var deleteRule1 = WebClientCaller.CallService<AdminMechanicsServiceClient, ResultBase>(
                    x => x.DeleteRuleById(ruleId, TestHelper.UserId));
            Assert.IsNotNull(deleteRule1);
            Assert.AreEqual(true, deleteRule1.Success);
        }

    }
}