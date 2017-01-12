using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using ProductCatalog.Services;

    using PromoAction.WsClients.MechanicsService;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class CategoryPermissionRepositoryTests
    {
        private const int TestParnterId = 1;

        private const string TestUser = "TestUser";

        private int categoryIdForCreate;

        private int categoryIdForDelete;
        private string userId = TestDataStore.TestUserId;

        #region Additional test attributes

        [TestInitialize]
        public void MyTestInitialize()
        {
            var ds = new ProductCategoriesDataSource();

            var result = ds.GetPublicCategories(
                TestDataStore.GetSqlPrice(), 
                TestDataStore.KladrCode, 
                includeParent: true);

            Assert.IsTrue(result.Categories.Count() >= 2, "Для тестов надо как минимум 2 категории");

            this.categoryIdForCreate = result.Categories[0].Id;
            this.categoryIdForDelete = result.Categories[1].Id;
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            var repo = new CategoryPermissionRepository();

            repo.Delete(TestDataStore.TestUserId, TestParnterId, new[] { this.categoryIdForCreate, this.categoryIdForDelete });
        }

        #endregion

        [TestMethod]
        public void ShouldReturnListByPartnerId()
        {
            var repo = new CategoryPermissionRepository();

            var list = repo.GetByPartner(TestParnterId);

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void ShouldCreate()
        {
            var repo = new CategoryPermissionRepository();

            var permission = new CategoryPermission { CategoryId = this.categoryIdForCreate, PartnerId = TestParnterId, InsertedUserId = TestUser };

            var permissions = new[] { permission };

            // NOTE: Можно два и более раз создать разрешение.
            repo.Save(userId, permissions);
            repo.Save(userId, permissions);

            var list = repo.GetByPartner(TestParnterId);
            Assert.IsTrue(list.Any(x => x.CategoryId == this.categoryIdForCreate));
        }

        [TestMethod]
        public void ShouldDelete()
        {
            var repo = new CategoryPermissionRepository();
            var permission = new CategoryPermission { CategoryId = this.categoryIdForDelete, PartnerId = TestParnterId, InsertedUserId = TestUser };
            var permissions = new[] { permission };
            repo.Save(userId, permissions);

            var categoryIds = new[] { this.categoryIdForDelete };

            // NOTE: Можно два и более раз удалить разрешение.
            repo.Delete(userId, TestParnterId, categoryIds);
            repo.Delete(userId, TestParnterId, categoryIds);

            var list = repo.GetByPartner(TestParnterId);
            Assert.IsFalse(list.Any(x => x.CategoryId == this.categoryIdForDelete));
        }
    }
}
