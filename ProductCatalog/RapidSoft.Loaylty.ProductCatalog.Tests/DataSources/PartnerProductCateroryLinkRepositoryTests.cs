using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Extensions;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class PartnerProductCateroryLinkRepositoryTests
    {
        private const int PartnerId = 1;

        private const int CategoryId1 = 5;

        private const int CategoryId2 = 6;

        [TestCleanup]
        public void MyTestCleanup()
        {
            const string SQL =
                "DELETE FROM [prod].[PartnerProductCategoryLinks] WHERE [PartnerId] = 1 AND [ProductCategoryId] = {0} AND [NamePath] LIKE 'Тестовый путь %'";
            string.Format(SQL, CategoryId1).ExecuteNonQuery(DataSourceConfig.ConnectionString);
            string.Format(SQL, CategoryId2).ExecuteNonQuery(DataSourceConfig.ConnectionString);
        }

        [TestMethod]
        public void ShouldSetPartnerProductCateroryLink()
        {
            var repo = new PartnerProductCateroryLinkRepository();

            var paths = new[] { new CategoryPath(true, "Тестовый путь 1"), new CategoryPath(false, "Тестовый путь 2") };

            var link = new PartnerProductCategoryLink
                           {
                               PartnerId = PartnerId,
                               ProductCategoryId = CategoryId1,
                               Paths = paths
                           };

            var saved = repo.SetPartnerProductCateroryLink(link, TestDataStore.TestUserId);

            Assert.IsNotNull(saved);
            Assert.AreEqual(2, saved.Count);
        }

        [TestMethod]
        public void ShouldSetPartnerProductCateroryLinkWithNormalize()
        {
            var repo = new PartnerProductCateroryLinkRepository();

            var paths = new[] { new CategoryPath(true, "Тестовый//путь/1"), new CategoryPath(false, "Тестовый\\путь\\2") , new CategoryPath(false, "\\Тестовый путь\\3\\"), };

            var link = new PartnerProductCategoryLink
                       {
                           PartnerId = PartnerId,
                           ProductCategoryId = CategoryId1,
                           Paths = paths
                       };

            var saved = repo.SetPartnerProductCateroryLink(link, TestDataStore.TestUserId);

            Assert.IsNotNull(saved);
            Assert.AreEqual(3, saved.Count);

            var first = saved.First(x => x.NamePath.Contains("1"));

            Assert.IsTrue(first.NamePath.StartsWith("/"));
            Assert.IsTrue(first.NamePath.EndsWith("/"));
            Assert.IsTrue(!first.NamePath.Contains('\\'));

            var second = saved.First(x => x.NamePath.Contains("2"));

            Assert.IsTrue(second.NamePath.StartsWith("/"));
            Assert.IsTrue(second.NamePath.EndsWith("/"));
            Assert.IsTrue(!second.NamePath.Contains('\\'));

			var third = saved.First(x => x.NamePath.Contains("3"));

			Assert.AreEqual(third.NamePath, "/Тестовый путь/3/");
		}

        [TestMethod]
        public void ShouldReturnAllPartnerProductCateroryLink()
        {
            CreateTestPartnerProductCateroryLink(PartnerId, CategoryId1);
            CreateTestPartnerProductCateroryLink(PartnerId, CategoryId2);

            var repo = new PartnerProductCateroryLinkRepository();

            var list = repo.GetPartnerProductCateroryLinks(PartnerId);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any(x => x.PartnerId == PartnerId && x.ProductCategoryId == CategoryId1), "Должен быть созданный link");
            Assert.IsTrue(list.Any(x => x.PartnerId == PartnerId && x.ProductCategoryId == CategoryId2), "Должен быть созданный link");
        }

        [TestMethod]
        public void ShouldReturnPartnerProductCateroryLinkByCategoryId1()
        {
            CreateTestPartnerProductCateroryLink(PartnerId, CategoryId1);

            var repo = new PartnerProductCateroryLinkRepository();

            var list = repo.GetPartnerProductCateroryLinks(PartnerId, new[] { CategoryId1 });

            Assert.IsNotNull(list);
            Assert.IsTrue(list.All(x => x.PartnerId == PartnerId && x.ProductCategoryId == CategoryId1), "Все должны быть с категорией " + CategoryId1);
        }

        private void CreateTestPartnerProductCateroryLink(int partnerId, int categoryId)
        {
            var repo = new PartnerProductCateroryLinkRepository();

            var paths = new[] { new CategoryPath(true, "Тестовый путь 1"), new CategoryPath(false, "Тестовый путь 2"), };

            var link = new PartnerProductCategoryLink
                           {
                               PartnerId = partnerId,
                               ProductCategoryId = categoryId,
                               Paths = paths
                           };

            repo.SetPartnerProductCateroryLink(link, TestDataStore.TestUserId);
        }
    }
}
