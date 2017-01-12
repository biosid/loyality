namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using API;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.ImportTests;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.ProductCatalog.Settings;
    using RapidSoft.Loaylty.ProductCatalog.Tests.DataSources;
    using RapidSoft.VTB24.ArmSecurity;
    using Vtb24.Common.Configuration;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class CatalogAdminServiceTests
    {
        private const string CatNamePath1 = "/Игровые приставки/";
        private const string CatNamePath2 = "/Акции/Акция!!Горящие цены!/";
        private const int ReturnTaskId = -1;

        private const string UserId = TestDataStore.TestUserId;

        private const int TestParnterId = TestDataStore.OzonPartnerId;

        private static readonly ProductCategoryRepository CategoryRepository =
            new ProductCategoryRepository(DataSourceConfig.ConnectionString);

        private static readonly List<string> JobTest = new List<string>();

        private readonly ICatalogAdminService catalogAdminService = MockFactory.GetCatalogAdminService();

        private readonly ProductCategoriesDataSource categoriedDataSource = new ProductCategoriesDataSource();

        private readonly ProductsRepository productsRepository = new ProductsRepository();

        private ProductCategory someParentCatalog;
        private ProductCategory someParentCatalog2;
        private int partnerId = TestDataStore.OzonPartnerId;

        #region Additional test attributes

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var mock = MockFactory.GetUserService();
            var service = mock.Object;
            ArmSecurity.UserServiceCreator = () => service;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ArmSecurity.UserServiceCreator = null;
        }

        [TestInitialize]
        public void Initialize()
        {
            var categoriesResult = this.catalogAdminService.GetAllSubCategories(
                new GetAllSubCategoriesParameters
                {
                    Type = ProductCategoryTypes.Static,
                    UserId = TestDataStore.TestUserId
                });

            if (categoriesResult == null || categoriesResult.Categories == null)
            {
                var code = categoriesResult == null
                    ? "unknow"
                    : categoriesResult.ResultCode.ToString(CultureInfo.InvariantCulture);
                var desc = categoriesResult == null ? "unknow" : categoriesResult.ResultDescription;
                throw new Exception(
                    string.Format("Нет ни одной статической категории в каталоге code:{0} description:{1}", code, desc));
            }

            this.someParentCatalog = categoriesResult.Categories.FirstOrDefault();
            this.someParentCatalog2 = categoriesResult.Categories.Skip(1).FirstOrDefault();

            this.DeleteTestJob(ReturnTaskId.ToString(CultureInfo.InvariantCulture));
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.DeleteTestJob(ReturnTaskId.ToString(CultureInfo.InvariantCulture));
            foreach (var job in JobTest)
            {
                this.DeleteTestJob(job);
            }
        }

        #endregion
        
        [TestMethod]
        public void ShouldSetProductsTargetAudiences()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var productIds = ctx.ProductSortProjections.Take(4).Select(p => p.ProductId).ToArray();

                var guids = new[]
                            {
                                Guid.NewGuid().ToString(), 
                                Guid.NewGuid().ToString(), 
                                Guid.NewGuid().ToString(),
                                Guid.NewGuid().ToString()
                            };

                var parameters = new SetProductsTargetAudiencesParameters
                                 {
                                     UserId = UserId,
                                     ProductIds = productIds,
                                     TargetAudienceIds = guids
                                 };
                var result = this.catalogAdminService.SetProductsTargetAudiences(parameters);

                Assert.IsTrue(result.Success);

                var audiencies =
                    ctx.ProductTargetAudiences.Where(
                        a => productIds.Contains(a.ProductId) && guids.Contains(a.TargetAudienceId)).ToList();

                Assert.IsNotNull(audiencies);
                Assert.AreEqual(16, audiencies.Count());

                var param = new SetProductsTargetAudiencesParameters
                            {
                                UserId = UserId,
                                ProductIds = new[]
                                             {
                                                 productIds[0]
                                             },
                                TargetAudienceIds = new[]
                                                    {
                                                        guids[0]
                                                    }
                            };
                var result2 = this.catalogAdminService.SetProductsTargetAudiences(param);

                Assert.IsTrue(result2.Success);

                var audiencies2 = ctx.ProductTargetAudiences.Where(a => param.ProductIds.Contains(a.ProductId)).ToList();

                Assert.IsNotNull(audiencies2);
                Assert.AreEqual(1, audiencies2.Count());
                Assert.IsTrue(audiencies2.All(a => a.TargetAudienceId == guids[0].ToUpperInvariant()));

                // NOTE: Подчищаем за некоторыми
                using (var ctx1 = new LoyaltyDBEntities())
                {
                    foreach (var guid in guids)
                    {
                        var sql =
                        string.Format(
                            "DELETE FROM [prod].[ProductTargetAudiences] WHERE [TargetAudienceId] = '{0}'", guid);
                        ctx1.Database.ExecuteSqlCommand(sql);
                    }
                }
            }
        }

        [TestMethod]
        public void ShouldSetProductsTargetAudiencesWithVeryLongProductId()
        {
            var veryLongProductId = new string('1', 255);
            var productIds = new[]
                                {
                                    veryLongProductId
                                };

            var guids = new[]
                        {
                            Guid.NewGuid().ToString()
                        };

            var parameters = new SetProductsTargetAudiencesParameters
                                 {
                                     UserId = UserId,
                                     ProductIds = productIds,
                                     TargetAudienceIds = guids
                                 };
            var result = this.catalogAdminService.SetProductsTargetAudiences(parameters);

            Assert.IsTrue(result.Success, result.ResultDescription);

            // NOTE: Подчищаем за собой
            using (var ctx1 = new LoyaltyDBEntities())
            {
                foreach (var guid in guids)
                {
                    var sql =
                    string.Format(
                        "DELETE FROM [prod].[ProductTargetAudiences] WHERE [TargetAudienceId] = '{0}'", guid);
                    ctx1.Database.ExecuteSqlCommand(sql);
                }
            }
        }

        [TestMethod]
        public void ShouldSetProductsTargetAudiencesIgnoreCaseSensetive()
        {
            var productId = Guid.NewGuid().ToString();

            var targetAudienceIds = new[] { "TEST", "test" };

            var parameters = new SetProductsTargetAudiencesParameters
                                 {
                                     UserId = UserId,
                                     ProductIds = productId.MakeArray(),
                                     TargetAudienceIds = targetAudienceIds
                                 };
            var result = this.catalogAdminService.SetProductsTargetAudiences(parameters);

            Assert.IsTrue(result.Success, result.ResultDescription);

            using (var ctx1 = new LoyaltyDBEntities())
            {
                var c = ctx1.ProductTargetAudiences.Count(x => x.ProductId == productId);

                Assert.AreEqual(1, c, "При привязки TargetAudienceId должны привести в в верхний регистр и выполнить distinct");
            }

            // NOTE: Подчищаем за собой
            using (var ctx1 = new LoyaltyDBEntities())
            {
                var sql = string.Format(
                        "DELETE FROM [prod].[ProductTargetAudiences] WHERE [ProductId] = '{0}'", productId);
                ctx1.Database.ExecuteSqlCommand(sql);
            }
        }

        [TestMethod]
        public void ShouldNotSetProductsTargetAudiences()
        {
            var result = this.catalogAdminService.SetProductsTargetAudiences(
                new SetProductsTargetAudiencesParameters
                {
                    UserId = UserId
                });

             Assert.IsFalse(result.Success);
             Assert.AreEqual(ResultCodes.INVALID_PARAMETER_VALUE, result.ResultCode);
         }

        [TestMethod]
        public void ShouldReturnCategoriesPermissions()
        {
            var mock = MockFactory.GetCategoryPermissionRepository();

            var service = new CatalogAdminService(categoryPermissionRepository: mock.Object);

            var result = service.GetCategoriesPermissions(TestDataStore.TestUserId, TestParnterId);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);

            Assert.AreEqual(50, result.CategoryIds.First());

            mock.Verify(x => x.GetByPartner(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ShouldAddCategoryPermission()
        {
            var mock = MockFactory.GetCategoryPermissionRepository();

            ProductCategory category;
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                category = ctx.ProductCategories.First();
            }

            var ids = new[] { category.Id };

            var service = new CatalogAdminService(categoryPermissionRepository: mock.Object);

            var param = new SetCategoriesPermissionsParameters
                            {
                                PartnerId = TestParnterId,
                                AddCategoriesId = ids,
                                RemoveCategoriesId = null,
                                UserId = UserId
                            };

            var result = service.SetCategoriesPermissions(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);

            mock.Verify(x => x.Save(UserId, It.IsAny<IList<CategoryPermission>>()), Times.Once());
            mock.Verify(x => x.Delete(UserId, It.IsAny<int>(), It.IsAny<IList<int>>()), Times.Never());
        }

        [TestMethod]
        public void ShouldNotAddCategoryPermissionWithBadPartnerId()
        {
            var mock = MockFactory.GetCategoryPermissionRepository();

            ProductCategory category;
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                category = ctx.ProductCategories.First();
            }

            var ids = new[] { category.Id };

            var service = new CatalogAdminService(categoryPermissionRepository: mock.Object);

            const int BadPartnerId = -555;

            var param = new SetCategoriesPermissionsParameters
                        {
                            PartnerId = BadPartnerId,
                            AddCategoriesId = ids,
                            RemoveCategoriesId = null,
                            UserId = UserId
                        };

            var result = service.SetCategoriesPermissions(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(ResultCodes.PARTNER_NOT_FOUND, result.ResultCode);

            mock.Verify(x => x.Save(UserId, It.IsAny<IList<CategoryPermission>>()), Times.Never());
            mock.Verify(x => x.Delete(UserId, It.IsAny<int>(), It.IsAny<IList<int>>()), Times.Never());
        }

        [TestMethod]
        public void ShouldNotAddCategoryPermissionWithBadCategoryId()
        {
            var mock = MockFactory.GetCategoryPermissionRepository();

            ProductCategory category;
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                category = ctx.ProductCategories.First();
            }

            const int BadCategoryId = -333;

            var ids = new[] { category.Id, BadCategoryId };

            var service = new CatalogAdminService(categoryPermissionRepository: mock.Object);

            var param = new SetCategoriesPermissionsParameters
                        {
                            PartnerId = TestParnterId,
                            AddCategoriesId = ids,
                            RemoveCategoriesId = null,
                            UserId = UserId
                        };

            var result = service.SetCategoriesPermissions(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(ResultCodes.CATEGORY_NOT_FOUND, result.ResultCode);

            mock.Verify(x => x.Save(UserId, It.IsAny<IList<CategoryPermission>>()), Times.Never());
            mock.Verify(x => x.Delete(UserId, It.IsAny<int>(), It.IsAny<IList<int>>()), Times.Never());
        }

        [TestMethod]
        public void ShouldRemoveCategoryPermission()
        {
            var mock = MockFactory.GetCategoryPermissionRepository();

            ProductCategory category;
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                category = ctx.ProductCategories.First();
            }

            var ids = new[] { category.Id };

            var service = new CatalogAdminService(categoryPermissionRepository: mock.Object);

            var param = new SetCategoriesPermissionsParameters
                            {
                                PartnerId = TestParnterId,
                                AddCategoriesId = null,
                                RemoveCategoriesId = ids,
                                UserId = UserId
                            };

            var result = service.SetCategoriesPermissions(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success, result.ResultDescription);

            mock.Verify(x => x.Save(UserId, It.IsAny<IList<CategoryPermission>>()), Times.Never());
            mock.Verify(x => x.Delete(UserId, It.IsAny<int>(), It.IsAny<IList<int>>()), Times.Once());
        }

        [TestMethod]
        public void ShouldNotMoveProducts()
        {
            var parameters = new MoveProductsParameters
                             {
                                 ProductIds = new[]
                                              {
                                                  "3232"
                                              },
                                 UserId = TestDataStore.TestUserId,
                                 TargetCategoryId = 999999
                             };
            var result = this.catalogAdminService.MoveProducts(parameters);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.NOT_FOUND, result.ResultCode);
        }

        [TestMethod]
        public void ShouldNotRemoveCategoryPermissionWithBadCategoryId()
        {
            var mock = MockFactory.GetCategoryPermissionRepository();

            ProductCategory category;
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                category = ctx.ProductCategories.First();
            }

            const int BadCategoryId = -333;

            var ids = new[] { category.Id, BadCategoryId };

            var service = new CatalogAdminService(categoryPermissionRepository: mock.Object);

            var param = new SetCategoriesPermissionsParameters
            {
                PartnerId = TestParnterId,
                AddCategoriesId = null,
                RemoveCategoriesId = ids,
                UserId = UserId
            };

            var result = service.SetCategoriesPermissions(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(ResultCodes.CATEGORY_NOT_FOUND, result.ResultCode);

            mock.Verify(x => x.Save(UserId, It.IsAny<IList<CategoryPermission>>()), Times.Never());
            mock.Verify(x => x.Delete(UserId, It.IsAny<int>(), It.IsAny<IList<int>>()), Times.Never());
        }

        [TestMethod]
        public void ShouldReturnProductImportTasks()
        {
            var mock = MockFactory.GetImportTaskRepository(ReturnTaskId);

            var service = new CatalogAdminService(importTaskRepository: mock.Object);

            var param = new GetImportTasksHistoryParameters
                        {
                            UserId = TestDataStore.TestUserId
                        };

            var result = service.GetProductCatalogImportTasksHistory(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
            Assert.IsNotNull(result.Tasks);
            Assert.AreEqual(1, result.Tasks.Length);
        }

        [TestMethod]
        public void ShouldReturnDeliveryRateImportTasks()
        {
            var mock = MockFactory.GetImportTaskRepository(ReturnTaskId);

            var service = new CatalogAdminService(importTaskRepository: mock.Object);

            var param = new GetImportTasksHistoryParameters
                        {
                            UserId = TestDataStore.TestUserId
                        };

            var result = service.GetDeliveryRateImportTasksHistory(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
            Assert.IsNotNull(result.Tasks);
            Assert.AreEqual(1, result.Tasks.Length);
        }

        [TestMethod]
        [DeploymentItem(@"quartz.config")]
        [DeploymentItem(@"log4net.config")]
        public void ShouldCreateImportYmlJob()
        {
            const int PartnerId = TestDataStore.PartnerId;
            const string FilePath = "Путь к файлу, для теста не важно";

            var mock = MockFactory.GetImportTaskRepository(ReturnTaskId);

            var service = new CatalogAdminService(importTaskRepository: mock.Object);

            var param = new ImportProductsFromYmlHttpParameters
            {
                PartnerId = PartnerId,
                FullFilePath = FilePath,
                UserId = TestDataStore.TestUserId
            };

            var result = service.ImportProductsFromYmlHttp(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
            Assert.IsNotNull(result.TaskId);
            Assert.AreEqual(ReturnTaskId, result.TaskId);

            var jobGuid = result.TaskId.Value.ToString(CultureInfo.InvariantCulture);

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                const string TriggerSql = "SELECT COUNT(*) FROM [dbo].[QRTZ_TRIGGERS] WHERE [TRIGGER_NAME] = '{0}'";
                var triggerCount = ctx.Database.SqlQuery<int>(string.Format(TriggerSql, jobGuid)).SingleOrDefault();
                Assert.AreEqual(1, triggerCount);

                const string JobSql = "SELECT COUNT(*) FROM [dbo].[QRTZ_JOB_DETAILS] WHERE [JOB_NAME] = '{0}'";
                var jobCount = ctx.Database.SqlQuery<int>(string.Format(JobSql, jobGuid)).SingleOrDefault();
                Assert.AreEqual(1, jobCount);
            }
        }

        [TestMethod]
        public void ShouldNotCreateImportYmlJob()
        {
            const int PartnerId = 100500;
            const string FilePath = "Путь к файлу, для теста не важно";

            var mock = MockFactory.GetImportTaskRepository(ReturnTaskId);

            var partnerMock = new Mock<IPartnerRepository>();
            partnerMock.Setup(x => x.GetById(PartnerId)).Returns(new Partner { Type = PartnerType.Online });

            var service = new CatalogAdminService(importTaskRepository: mock.Object, partnerRepository: partnerMock.Object);

            var param = new ImportProductsFromYmlHttpParameters
                            {
                                PartnerId = PartnerId,
                                FullFilePath = FilePath,
                                UserId = UserId
                            };

            var result = service.ImportProductsFromYmlHttp(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(ResultCodes.PARTNER_CANT_IMPORT_CATALOG, result.ResultCode);
        }

        [TestMethod]
        [DeploymentItem(@"quartz.config")]
        [DeploymentItem(@"log4net.config")]
        public void ShouldCreateImportDeliveryRatesJob()
        {
            const int PartnerId = TestDataStore.PartnerId;
            const string FilePath = "http://localhost/Test/deliveryRates.csv";

            int countWaitingEtlSessionsBefore;
            const string SessionSql = "SELECT COUNT(*) FROM [dbo].[EtlSessions] WHERE [Status] = -1";

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                countWaitingEtlSessionsBefore = ctx.Database.SqlQuery<int>(SessionSql).SingleOrDefault();
            }

            var result = catalogAdminService.ImportDeliveryRatesFromHttp(PartnerId, FilePath, TestDataStore.TestUserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
            Assert.IsNotNull(result.JobId);

            var jobGuid = result.JobId;

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                const string TriggerSql = "SELECT COUNT(*) FROM [dbo].[QRTZ_TRIGGERS] WHERE [TRIGGER_NAME] = '{0}'";
                var triggerCount = ctx.Database.SqlQuery<int>(string.Format(TriggerSql, jobGuid)).SingleOrDefault();
                Assert.AreEqual(1, triggerCount);

                const string JobSql = "SELECT COUNT(*) FROM [dbo].[QRTZ_JOB_DETAILS] WHERE [JOB_NAME] = '{0}'";
                var jobCount = ctx.Database.SqlQuery<int>(string.Format(JobSql, jobGuid)).SingleOrDefault();
                Assert.AreEqual(1, jobCount);

                var countWaitingEtlSessionsAfter = ctx.Database.SqlQuery<int>(SessionSql).SingleOrDefault();
                Assert.IsTrue(countWaitingEtlSessionsAfter > countWaitingEtlSessionsBefore);
            }

            // NOTE: Я решил оставить job, пусть отработывает на TeamCity, и на почту придет письмо с ошибок что файл не доступен.
            // Тем не менее наличие письма с ошибкой указывает на работу сервера кварц.
            // JobTest.Add(jobGuid);
        }

        [TestMethod]
        public void ShouldGetOnlyActiveCategoriesTest()
        {
            var createCatRes = this.catalogAdminService.CreateCategory(
                new CreateCategoryParameters
                {
                    Type = ProductCategoryTypes.Static,
                    UserId = TestDataStore.TestUserId,
                    Name = TestHelper.CategoryName + Guid.NewGuid(),
                    Status = ProductCategoryStatuses.NotActive,
                });

            Assert.AreEqual(true, createCatRes.Success, createCatRes.ResultDescription);

            var result = this.catalogAdminService.GetAllSubCategories(
                new GetAllSubCategoriesParameters
                {
                    Status = ProductCategoryStatuses.Active,
                    UserId = TestDataStore.TestUserId
                });

            Assert.IsTrue(result.Categories.Length > 0);

            var wrongCat = result.Categories.FirstOrDefault(c => c.Status == ProductCategoryStatuses.NotActive);

            Assert.IsNull(wrongCat, "Присутствует не активная категория, а запрашивались только активные");

            ProductCatalogDB.DeleteCategory(createCatRes.Category.Id);
        }

        [TestMethod]
        public void ShouldGetOnlyNotActiveCategoriesTest()
        {
            var createCatRes = this.catalogAdminService.CreateCategory(new CreateCategoryParameters
            {
                Type = ProductCategoryTypes.Static,
                UserId = TestDataStore.TestUserId,
                Name = TestHelper.CategoryName + Guid.NewGuid(),
                Status = ProductCategoryStatuses.NotActive,
            });

            var result = this.catalogAdminService.GetAllSubCategories(
                new GetAllSubCategoriesParameters
                {
                    Status = ProductCategoryStatuses.NotActive,
                    UserId = TestDataStore.TestUserId
                });

            Assert.IsTrue(result.Categories.Length > 0);

            var wrongCat = result.Categories.FirstOrDefault(c => c.Status == ProductCategoryStatuses.Active);

            Assert.IsNull(wrongCat, "Присутствует активная категория, а запрашивались только не активные");
            
            ProductCatalogDB.DeleteCategory(createCatRes.Category.Id);
        }

        [TestMethod]
        public void ShouldCreateRootCategory()
        {
            var categoryName = TestHelper.CategoryName + Guid.NewGuid();
            const string OnlineCategoryUrl = "url";

            var createCategoryParameters = new CreateCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                Name = categoryName,
                Status = ProductCategoryStatuses.Active,
                Type = ProductCategoryTypes.Online,
                OnlineCategoryUrl = OnlineCategoryUrl
            };

            var result = this.catalogAdminService.CreateCategory(createCategoryParameters);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Category);

            var category = this.GetCategoryById(result.Category.Id);

            Assert.IsNotNull(category);
            Assert.AreEqual(TestDataStore.TestUserId, category.InsertedUserId);
            Assert.AreEqual(categoryName, category.Name);
            Assert.AreEqual(ProductCategoryStatuses.Active, category.Status);
            Assert.AreEqual(ProductCategoryTypes.Online, category.Type);
            Assert.AreEqual(OnlineCategoryUrl, OnlineCategoryUrl);
            Assert.IsTrue(category.InsertedDate > DateTime.MinValue);
            Assert.AreNotEqual(0, result.Category.CatOrder);

            ProductCatalogDB.DeleteCategory(category.Id);
        }

        [TestMethod]
        public void ShouldCreateNotRootCategory()
        {
            var categoryName = TestHelper.CategoryName + Guid.NewGuid();
            var parent = this.someParentCatalog;

            var createCategoryParameters = new CreateCategoryParameters
            {
                UserId = UserId,
                Name = categoryName,
                Status = ProductCategoryStatuses.Active,
                ParentCategoryId = parent.Id
            };

            var result = this.catalogAdminService.CreateCategory(createCategoryParameters);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Category);

            var category = result.Category;

            Assert.IsNotNull(category);
            Assert.AreEqual(parent.Id, category.ParentId);
            Assert.AreEqual(TestDataStore.TestUserId, category.InsertedUserId);
            Assert.AreEqual(categoryName, category.Name);
            Assert.AreEqual(ProductCategoryStatuses.Active, category.Status);
            Assert.AreEqual(ProductCategoryTypes.Static, category.Type);
            Assert.IsTrue(category.InsertedDate > DateTime.MinValue);

            ProductCatalogDB.DeleteCategory(category.Id);
        }

        [TestMethod]
        public void ShouldNotCreateCategory()
        {
            var categoryName = TestHelper.CategoryName + Guid.NewGuid();

            var parameters = new CreateCategoryParameters
            {
                UserId = UserId,
                Name = categoryName,
                Status = ProductCategoryStatuses.Active,
            };

            var tempResult = this.catalogAdminService.CreateCategory(parameters);

            Assert.IsTrue(tempResult.Success);

            var exists = this.catalogAdminService.CreateCategory(parameters);

            Assert.AreEqual(false, exists.Success, exists.ResultDescription);
            Assert.AreEqual(ResultCodes.CATEGORY_WITH_NAME_EXISTS, exists.ResultCode, exists.ResultDescription);

            var parameters2 = new CreateCategoryParameters
            {
                UserId = UserId,
                Name = categoryName + Guid.NewGuid(),
                Status = ProductCategoryStatuses.Active,
                ParentCategoryId = -500
            };

            var invalidParent = this.catalogAdminService.CreateCategory(parameters2);

            Assert.AreEqual(invalidParent.Success, false);
            Assert.AreEqual(invalidParent.ResultCode, ResultCodes.PARENT_CATEGORY_NOT_FOUND);

            ProductCatalogDB.DeleteCategory(tempResult.Category.Id);
        }

        [TestMethod]
        public void ShouldUpdateCategory()
        {
            var categoryName = TestHelper.CategoryName + Guid.NewGuid();

            var parameters = new CreateCategoryParameters
            {
                UserId = UserId,
                Name = categoryName,
                Status = ProductCategoryStatuses.Active,
            };

            var tempResult = this.catalogAdminService.CreateCategory(parameters);

            var createdCategory = this.GetCategoryById(tempResult.Category.Id);

            var newName = TestDataStore.TestCategoryName + "Newname" + Guid.NewGuid();
            var updateCategoryParameters = new UpdateCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                CategoryId = createdCategory.Id,
                NewName = newName,
                NewStatus = ProductCategoryStatuses.NotActive,
                NewOnlineCategoryUrl = "NewUrl" + Guid.NewGuid()
            };

            var result = this.catalogAdminService.UpdateCategory(updateCategoryParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Success, true, result.ResultDescription);

            var category = result.Category;

            Assert.IsNotNull(category);
            Assert.IsNotNull(category.NamePath);
            Assert.AreEqual(TestDataStore.TestUserId, category.InsertedUserId);
            Assert.AreEqual(TestDataStore.TestUserId, category.UpdatedUserId);
            Assert.AreEqual(newName, category.Name);
            Assert.AreEqual(ProductCategoryStatuses.NotActive, category.Status);
            Assert.AreEqual(ProductCategoryTypes.Static, category.Type);
            Assert.IsTrue(category.InsertedDate > DateTime.MinValue);
            Assert.IsTrue(category.UpdatedDate > DateTime.MinValue);

            var newName2 = TestDataStore.TestCategoryName + "Newname2" + Guid.NewGuid();
            var updateCategoryParameters2 = new UpdateCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                CategoryId = createdCategory.Id,
                NewName = newName2
            };

            var result2 = this.catalogAdminService.UpdateCategory(updateCategoryParameters2);

            Assert.IsNotNull(result2);
            Assert.AreEqual(result2.Success, true, result2.ResultDescription);

            var category2 = result2.Category;

            Assert.IsNotNull(category2);
            Assert.IsNotNull(category2.NamePath);
            Assert.AreEqual(TestDataStore.TestUserId, category2.InsertedUserId);
            Assert.AreEqual(TestDataStore.TestUserId, category2.UpdatedUserId);
            Assert.AreEqual(newName2, category2.Name);
            Assert.IsTrue(category2.InsertedDate > DateTime.MinValue);
            Assert.IsTrue(category2.UpdatedDate > DateTime.MinValue);
        }

        [TestMethod]
        public void ShouldUpdateCategoryAndNamePathsForChildren()
        {
            var parentCat = TestHelper.NewCategory();
            var child1 = TestHelper.NewCategory(parentCat.Id);
            var child2 = TestHelper.NewCategory(parentCat.Id);
            var grandChild1 = TestHelper.NewCategory(child2.Id);
            var grandChild2 = TestHelper.NewCategory(child2.Id);

            var newName = TestDataStore.TestCategoryName + "new Name Goes Here" + Guid.NewGuid();
            var updateCategoryParameters = new UpdateCategoryParameters
                                           {
                                               CategoryId = parentCat.Id,
                                               UserId = UserId,
                                               NewName = newName
                                           };
            var updResult = this.catalogAdminService.UpdateCategory(updateCategoryParameters);

            parentCat = CategoryRepository.GetById(parentCat.Id);
            child1 = CategoryRepository.GetById(child1.Id);
            child2 = CategoryRepository.GetById(child2.Id);
            grandChild1 = CategoryRepository.GetById(grandChild1.Id);
            grandChild2 = CategoryRepository.GetById(grandChild2.Id);

            const string Form1 = "/{0}/";
            const string Form2 = "/{0}/{1}/";
            const string Form3 = "/{0}/{1}/{2}/";

            Assert.IsTrue(updResult.Success);
            Assert.AreEqual(string.Format(Form1, parentCat.Name), parentCat.NamePath);
            Assert.AreEqual(string.Format(Form2, parentCat.Name, child1.Name), child1.NamePath);
            Assert.AreEqual(string.Format(Form3, parentCat.Name, child2.Name, grandChild1.Name), grandChild1.NamePath);
            Assert.AreEqual(string.Format(Form3, parentCat.Name, child2.Name, grandChild2.Name), grandChild2.NamePath);
        }

        [TestMethod]
        public void ShouldNotUpdateIfNamePathExistCategory()
        {
            var newName = TestHelper.CategoryName + Guid.NewGuid();

            var newCatParam = new CreateCategoryParameters
                              {
                                  UserId = TestDataStore.TestUserId,
                                  Name = newName,
                                  Status = ProductCategoryStatuses.Active,
                              };

            var newCat = this.catalogAdminService.CreateCategory(newCatParam);

            Assert.IsNotNull(newCat);
            Assert.IsNotNull(newCat.Category);

            var updCat = this.catalogAdminService.UpdateCategory(
                new UpdateCategoryParameters
                {
                    CategoryId = newCat.Category.Id,
                    UserId = TestDataStore.TestUserId,
                    NewName = newName
                });

            Assert.IsTrue(updCat.Success);

            var paramsNew = new CreateCategoryParameters
                            {
                                UserId = TestDataStore.TestUserId,
                                Name = TestHelper.CategoryName + "_paramsNew" + Guid.NewGuid()
                            };

            var newCat2 = this.catalogAdminService.CreateCategory(paramsNew);

            Assert.IsTrue(newCat2.Success);

            var result = this.catalogAdminService.UpdateCategory(
                new UpdateCategoryParameters
                {
                    CategoryId = newCat2.Category.Id,
                    UserId = TestDataStore.TestUserId,
                    NewName = newName
                });

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.CATEGORY_WITH_NAME_EXISTS, result.ResultCode);
        }

        [TestMethod]
        public void ShouldNotCreateSubInOnlineCatTest()
        {
            var parameters = new CreateCategoryParameters
            {
                UserId = UserId,
                Name = TestHelper.CategoryName + Guid.NewGuid(),
                Status = ProductCategoryStatuses.Active,
                Type = ProductCategoryTypes.Online,
                OnlineCategoryUrl = "url"
            };

            var createCategoryResult = this.catalogAdminService.CreateCategory(parameters);

            Assert.IsNotNull(createCategoryResult);
            Assert.IsNotNull(createCategoryResult.Category);

            var parameters2 = new CreateCategoryParameters
            {
                UserId = UserId,
                Name = TestHelper.CategoryName + Guid.NewGuid(),
                Status = ProductCategoryStatuses.Active,
                Type = ProductCategoryTypes.Static,
                ParentCategoryId = createCategoryResult.Category.Id
            };

            var createParentCategoryResult = this.catalogAdminService.CreateCategory(parameters2);

            Assert.AreEqual(createParentCategoryResult.Success, false);
            Assert.AreEqual(
                createParentCategoryResult.ResultCode,
                ResultCodes.PARENT_CATEGORY_IS_DYNAMIC,
                createParentCategoryResult.ResultDescription);
        }

        [TestMethod]
        public void ShouldChangeCategoriesStatus()
        {
            var parameters = new CreateCategoryParameters
            {
                UserId = UserId,
                Name = TestHelper.CategoryName + Guid.NewGuid(),
                Status = ProductCategoryStatuses.Active,
                Type = ProductCategoryTypes.Online,
                OnlineCategoryUrl = "url"
            };

            var parameters2 = new CreateCategoryParameters
            {
                UserId = UserId,
                Name = TestHelper.CategoryName + Guid.NewGuid(),
                Status = ProductCategoryStatuses.Active,
                Type = ProductCategoryTypes.Online,
                OnlineCategoryUrl = "url"
            };

            var createCategoryResult = this.catalogAdminService.CreateCategory(parameters);
            var createCategoryResult2 = this.catalogAdminService.CreateCategory(parameters2);

            Assert.IsNotNull(createCategoryResult);
            Assert.IsNotNull(createCategoryResult.Category);

            Assert.IsNotNull(createCategoryResult2);
            Assert.IsNotNull(createCategoryResult2.Category);

            Assert.IsTrue(createCategoryResult.Category.Status == ProductCategoryStatuses.Active &&
                createCategoryResult2.Category.Status == ProductCategoryStatuses.Active);

            var ids = new[]
            {
                createCategoryResult.Category.Id, createCategoryResult2.Category.Id
            };

            var prodRepo = new ProductCategoryRepository(DataSourceConfig.ConnectionString);

            var res = prodRepo.ChangeCategoriesStatus(UserId, ids, ProductCategoryStatuses.NotActive);

            Assert.IsTrue(res.Success);

            var changedCategory = prodRepo.GetById(ids[0]);

            Assert.AreEqual(ProductCategoryStatuses.NotActive, changedCategory.Status);
        }

        [TestMethod]
        public void ShouldChangeCategoryStatusCheckRequiredParameters()
        {
            var categoryIds = new[] { 1, 2, 3 };
            var result = catalogAdminService.ChangeCategoriesStatus(null, categoryIds, ProductCategoryStatuses.NotActive);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.INVALID_PARAMETER_VALUE, result.ResultCode);

            var result2 = catalogAdminService.ChangeCategoriesStatus(
                TestDataStore.TestUserId, null, ProductCategoryStatuses.NotActive);

            Assert.IsFalse(result2.Success);
            Assert.AreEqual(ResultCodes.INVALID_PARAMETER_VALUE, result2.ResultCode);
        }

        [TestMethod]
        public void ShouldDeleteCategory()
        {
            var prodRepo = CategoryRepository;

            var result = prodRepo.DeleteCategory(TestDataStore.TestUserId, 361498888);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.NOT_FOUND, result.ResultCode);

            const string User = UserId;

            var guid = Guid.NewGuid().ToString();

            var catName1 = TestDataStore.TestCategoryName + "TestProductCategory1" + guid;
            var catName2 = TestDataStore.TestCategoryName + "TestProductCategory2" + guid;
            var catName3 = TestDataStore.TestCategoryName + "TestProductCategory3" + guid;
            var catName4 = TestDataStore.TestCategoryName + "TestProductCategory4" + guid;

            var category1 = new ProductCategory
                            {
                                Name = catName1,
                                ParentId = null,
                                NamePath = catName1,
                                Status = ProductCategoryStatuses.NotActive,
                                InsertedDate = DateTime.Now,
                                InsertedUserId = User
                            };
            prodRepo.Add(category1);
            
            var category2 = new ProductCategory
            {
                Name = catName2,
                ParentId = category1.Id,
                NamePath = category1.Name + "/TestProductCategory2",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category2);

            var category3 = new ProductCategory
            {
                Name = catName3,
                ParentId = category1.Id,
                NamePath = category1.Name + "/TestProductCategory3",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category3);

            var category4 = new ProductCategory
            {
                Name = catName4,
                ParentId = category3.Id,
                NamePath = category3.Name + "/productCatetory3/TestProductCategory4",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category4);

            var permissions = new List<CategoryPermission>
                              {
                                  new CategoryPermission(1, category1.Id, category1.InsertedUserId),
                                  new CategoryPermission(1, category2.Id, category2.InsertedUserId),
                                  new CategoryPermission(1, category3.Id, category3.InsertedUserId),
                                  new CategoryPermission(1, category4.Id, category4.InsertedUserId),
                              };
            
            new CategoryPermissionRepository(DataSourceConfig.ConnectionString).Save(UserId, permissions);

            var deleteResult = prodRepo.DeleteCategory(TestDataStore.TestUserId, category1.Id);

            Assert.IsTrue(deleteResult.Success);
            Assert.AreEqual(ResultCodes.SUCCESS, deleteResult.ResultCode);

            var listResult = new ProductCategoriesDataSource().GetPublicCategories(
                TestDataStore.GetSqlPrice(), 
                TestDataStore.KladrCode, 
                includeParent: true);

            Assert.IsNotNull(listResult);
            Assert.IsNotNull(listResult.Categories);
            Assert.IsTrue(listResult.Categories.All(x => x.Id != category1.Id));
        }

        [TestMethod]
        public void ShouldDeleteProducts()
        {
            List<PartnerProductCatalog> catalogs;

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                catalogs = ctx.PartnerProductCatalogs.Where(c => c.IsActive).Take(2).ToList();
            }

            Assert.AreEqual(2, catalogs.Count);

            var category = TestHelper.NewCategory();
            var createProductParams = this.NewCreateProductParameters(category.Id);
            var createProductParams2 = this.NewCreateProductParameters(category.Id);

            createProductParams.PartnerId = catalogs[0].PartnerId;
            createProductParams2.PartnerId = catalogs[1].PartnerId;

            var permissionRepo = new CategoryPermissionRepository();

            permissionRepo.Save(UserId, new[]
            {
                new CategoryPermission(createProductParams.PartnerId, category.Id, TestDataStore.TestUserId),
                new CategoryPermission(createProductParams2.PartnerId, category.Id, TestDataStore.TestUserId)
            });

            var result = this.catalogAdminService.CreateProduct(createProductParams);
            var result2 = this.catalogAdminService.CreateProduct(createProductParams2);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Product);
            Assert.IsNotNull(result2.Product);
            Assert.AreNotEqual(result.Product.PartnerId, result2.Product.PartnerId);

            permissionRepo.Delete(UserId, result.Product.PartnerId, new List<int> { category.Id });
            permissionRepo.Delete(UserId, result2.Product.PartnerId, new List<int> { category.Id });

            var id1 = result.Product.ProductId;
            var id2 = result2.Product.ProductId;

            this.catalogAdminService.DeleteProducts(
                new DeleteProductParameters
                {
                    UserId = TestDataStore.SecondTestUserId,
                    ProductIds = new[]
                                 {
                                     id1, id2
                                 }
                });

            var res = this.productsRepository.GetById(id1);
            var res2 = this.productsRepository.GetById(id2);

            Assert.IsNull(res);
            Assert.IsNull(res2);
        }

        [TestMethod]
        public void ShouldCreateProductArticulSpaces()
        {
            var category = TestHelper.NewCategory();
            var createProductParams = this.NewCreateProductParameters(category.Id);

            var partnerProductId = Guid.NewGuid() + "articulspaces   ";
            createProductParams.PartnerProductId = partnerProductId;

            var permissionRepo = new CategoryPermissionRepository();

            var permParam = new CategoryPermission(createProductParams.PartnerId, category.Id, TestDataStore.TestUserId);
            permissionRepo.Save(UserId, permParam.MakeArray());

            var result = this.catalogAdminService.CreateProduct(createProductParams);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(createProductParams.PartnerProductId, result.Product.PartnerProductId);

            new ProductsDataSource().UpdateProductsFromAllPartners();

            var parameters = new AdminSearchProductsParameters
                                 {
                                     UserId = TestDataStore.TestUserId,
                                     ProductIds = result.Product.ProductId.MakeArray()
                                 };

            var productRes = this.catalogAdminService.SearchProducts(parameters);

            Assert.IsTrue(productRes.Success, productRes.ResultDescription);
            Assert.IsTrue(productRes.Products.Any(), "Товар с пробелами в идентификторе не найден");
            Assert.AreEqual(result.Product.ProductId, productRes.Products[0].ProductId);
            Assert.AreNotEqual(partnerProductId, productRes.Products[0].PartnerProductId);
            Assert.AreEqual(partnerProductId.Trim(), productRes.Products[0].PartnerProductId);
        }

        [TestMethod]
        public void ShouldMoveCategoryAndUpdateChildrenNames()
        {
            var firstCat = TestHelper.NewCategory();

            var secondCat = TestHelper.NewCategory();
            var secondChild = TestHelper.NewCategory(secondCat.Id);
            var secondGrandChild = TestHelper.NewCategory(secondChild.Id);
            var secondGrandGrandChild = TestHelper.NewCategory(secondGrandChild.Id);

            var moveCategoryParameters = new MoveCategoryParameters
                                         {
                                             UserId = TestDataStore.TestUserId,
                                             ReferenceCategoryId = firstCat.Id,
                                             CategoryId = secondCat.Id,
                                             PositionType = CategoryPositionTypes.Append
                                         };
            this.catalogAdminService.MoveCategory(moveCategoryParameters);

            var actualFirst = CategoryRepository.GetById(firstCat.Id);

            var actualSecond = CategoryRepository.GetById(secondCat.Id);
            var actualChild = CategoryRepository.GetById(secondChild.Id);
            var actualGrandChild = CategoryRepository.GetById(secondGrandChild.Id);
            var actualGrandGrandChild = CategoryRepository.GetById(secondGrandGrandChild.Id);

            const string SecondNode = "/{0}/{1}/";
            const string ThirdNode = "/{0}/{1}/{2}/";
            const string ForthNode = "/{0}/{1}/{2}/{3}/";
            const string FifthNode = "/{0}/{1}/{2}/{3}/{4}/";

            Assert.AreEqual(null, actualFirst.ParentId);
            Assert.AreEqual(firstCat.Id, actualSecond.ParentId);

            Assert.AreEqual(string.Format(SecondNode, actualFirst.Name, actualSecond.Name), actualSecond.NamePath);
            Assert.AreEqual(string.Format(ThirdNode, actualFirst.Name, actualSecond.Name, actualChild.Name), actualChild.NamePath);
            Assert.AreEqual(string.Format(ForthNode, actualFirst.Name, actualSecond.Name, actualChild.Name, actualGrandChild.Name), actualGrandChild.NamePath);
            Assert.AreEqual(string.Format(FifthNode, actualFirst.Name, actualSecond.Name, actualChild.Name, actualGrandChild.Name, actualGrandGrandChild.Name), actualGrandGrandChild.NamePath);
        }

        [TestMethod]
        public void ShouldMoveCategoryIntoRootAndUpdateNames()
        {
            var c1 = TestHelper.NewCategory();
            var c2 = TestHelper.NewCategory(c1.Id);
            var c3 = TestHelper.NewCategory();

            // move c1 into c3 by append
            var categoryParameters = new MoveCategoryParameters
                                     {
                                         UserId = TestDataStore.TestUserId,
                                         CategoryId = c1.Id,
                                         ReferenceCategoryId = c3.Id,
                                         PositionType = CategoryPositionTypes.Append
                                     };
            this.catalogAdminService.MoveCategory(categoryParameters);

            var ac1 = CategoryRepository.GetById(c1.Id);
            var ac2 = CategoryRepository.GetById(c2.Id);

            const string FirstNode = "/{0}/";
            const string SecondNode = "/{0}/{1}/";
            const string ThirdNode = "/{0}/{1}/{2}/";

            Assert.AreEqual(string.Format(SecondNode, c3.Name, c1.Name), ac1.NamePath);
            Assert.AreEqual(string.Format(ThirdNode, c3.Name, c1.Name, c2.Name), ac2.NamePath);

            // move c1 into root
            var moveCategoryParameters = new MoveCategoryParameters
                                         {
                                             UserId = TestDataStore.TestUserId,
                                             CategoryId = c1.Id,
                                             ReferenceCategoryId = null,
                                             PositionType = CategoryPositionTypes.Append
                                         };
            this.catalogAdminService.MoveCategory(moveCategoryParameters);

            ac1 = CategoryRepository.GetById(c1.Id);
            ac2 = CategoryRepository.GetById(c2.Id);

            Assert.AreEqual(string.Format(FirstNode, c1.Name), ac1.NamePath);
            Assert.AreEqual(string.Format(SecondNode, c1.Name, c2.Name), ac2.NamePath);
        }

        [TestMethod]
        public void ShouldMoveCategoryBefore()
        {
            var firstCat = TestHelper.NewCategory();
            var secondCat = TestHelper.NewCategory();

            var moveCategoryParameters = new MoveCategoryParameters
                                         {
                                             UserId = TestDataStore.TestUserId,
                                             ReferenceCategoryId = firstCat.Id,
                                             CategoryId = secondCat.Id,
                                             PositionType = CategoryPositionTypes.Before
                                         };
            var res = this.catalogAdminService.MoveCategory(moveCategoryParameters);

            AssertResult(res);

            var actualFirst = CategoryRepository.GetById(firstCat.Id);
            var actualSecond = CategoryRepository.GetById(secondCat.Id);

            Assert.IsTrue(actualSecond.CatOrder < actualFirst.CatOrder);

            var firstCat1 = TestHelper.NewCategory(firstCat.Id);
            var firstCat2 = TestHelper.NewCategory(firstCat.Id);
            var firstCat3 = TestHelper.NewCategory(firstCat.Id);
            var firstCat4 = TestHelper.NewCategory(firstCat.Id);

            var secondCat1 = TestHelper.NewCategory(secondCat.Id);
            var secondCat2 = TestHelper.NewCategory(secondCat.Id);
            var secondCat3 = TestHelper.NewCategory(secondCat.Id);
            var secondCat4 = TestHelper.NewCategory(secondCat.Id);

            // move firstCat1 before firstCat3
            var moveCategoryParameters1 = new MoveCategoryParameters
                                          {
                                              UserId = TestDataStore.TestUserId,
                                              ReferenceCategoryId = firstCat3.Id,
                                              CategoryId = firstCat1.Id,
                                              PositionType = CategoryPositionTypes.Before
                                          };
            this.catalogAdminService.MoveCategory(moveCategoryParameters1);

            var sortedList = new SortedList<int, ProductCategory>();

            var afirstCat1 = CategoryRepository.GetById(firstCat1.Id);
            var afirstCat2 = CategoryRepository.GetById(firstCat2.Id);
            var afirstCat3 = CategoryRepository.GetById(firstCat3.Id);
            var afirstCat4 = CategoryRepository.GetById(firstCat4.Id);

            sortedList.Add(afirstCat1.CatOrder, afirstCat1);
            sortedList.Add(afirstCat2.CatOrder, afirstCat2);
            sortedList.Add(afirstCat3.CatOrder, afirstCat3);
            sortedList.Add(afirstCat4.CatOrder, afirstCat4);

            Assert.AreEqual(0, sortedList.IndexOfValue(afirstCat2));
            Assert.AreEqual(1, sortedList.IndexOfValue(afirstCat1));
            Assert.AreEqual(2, sortedList.IndexOfValue(afirstCat3));
            Assert.AreEqual(3, sortedList.IndexOfValue(afirstCat4));

            // move afirstCat3 after afirstCat2 
            var parameters = new MoveCategoryParameters
                             {
                                 UserId = TestDataStore.TestUserId,
                                 ReferenceCategoryId = afirstCat2.Id,
                                 CategoryId = afirstCat3.Id,
                                 PositionType = CategoryPositionTypes.After
                             };
            this.catalogAdminService.MoveCategory(parameters);

            sortedList = new SortedList<int, ProductCategory>();

            afirstCat1 = CategoryRepository.GetById(firstCat1.Id);
            afirstCat2 = CategoryRepository.GetById(firstCat2.Id);
            afirstCat3 = CategoryRepository.GetById(firstCat3.Id);
            afirstCat4 = CategoryRepository.GetById(firstCat4.Id);

            sortedList.Add(afirstCat1.CatOrder, afirstCat1);
            sortedList.Add(afirstCat2.CatOrder, afirstCat2);
            sortedList.Add(afirstCat3.CatOrder, afirstCat3);
            sortedList.Add(afirstCat4.CatOrder, afirstCat4);

            Assert.AreEqual(0, sortedList.IndexOfValue(afirstCat2));
            Assert.AreEqual(1, sortedList.IndexOfValue(afirstCat3));
            Assert.AreEqual(2, sortedList.IndexOfValue(afirstCat1));
            Assert.AreEqual(3, sortedList.IndexOfValue(afirstCat4));

            // move afirstCat2 before secondCat2
            var categoryParameters = new MoveCategoryParameters
                                     {
                                         UserId = TestDataStore.TestUserId,
                                         ReferenceCategoryId = secondCat2.Id,
                                         CategoryId = afirstCat2.Id,
                                         PositionType = CategoryPositionTypes.Before
                                     };
            this.catalogAdminService.MoveCategory(categoryParameters);

            sortedList = new SortedList<int, ProductCategory>();

            var asecondCat1 = CategoryRepository.GetById(secondCat1.Id);
            var asecondCat2 = CategoryRepository.GetById(secondCat2.Id);
            var asecondCat3 = CategoryRepository.GetById(secondCat3.Id);
            var asecondCat4 = CategoryRepository.GetById(secondCat4.Id);
            afirstCat2 = CategoryRepository.GetById(firstCat2.Id);

            sortedList.Add(asecondCat1.CatOrder, asecondCat1);
            sortedList.Add(asecondCat2.CatOrder, asecondCat2);
            sortedList.Add(asecondCat3.CatOrder, asecondCat3);
            sortedList.Add(asecondCat4.CatOrder, asecondCat4);
            sortedList.Add(afirstCat2.CatOrder, afirstCat2);

            Assert.AreEqual(0, sortedList.IndexOfValue(asecondCat1));
            Assert.AreEqual(1, sortedList.IndexOfValue(afirstCat2));
            Assert.AreEqual(2, sortedList.IndexOfValue(asecondCat2));
            Assert.AreEqual(3, sortedList.IndexOfValue(asecondCat3));
            Assert.AreEqual(4, sortedList.IndexOfValue(asecondCat4));

            // move afirstCat2 after afirstCat1
            var categoryParameters1 = new MoveCategoryParameters
                                      {
                                          UserId = TestDataStore.TestUserId,
                                          ReferenceCategoryId = afirstCat1.Id,
                                          CategoryId = afirstCat2.Id,
                                          PositionType = CategoryPositionTypes.After
                                      };
            this.catalogAdminService.MoveCategory(categoryParameters1);

            sortedList = new SortedList<int, ProductCategory>();

            afirstCat1 = CategoryRepository.GetById(firstCat1.Id);
            afirstCat2 = CategoryRepository.GetById(firstCat2.Id);
            afirstCat3 = CategoryRepository.GetById(firstCat3.Id);
            afirstCat4 = CategoryRepository.GetById(firstCat4.Id);

            sortedList.Add(afirstCat1.CatOrder, afirstCat1);
            sortedList.Add(afirstCat2.CatOrder, afirstCat2);
            sortedList.Add(afirstCat3.CatOrder, afirstCat3);
            sortedList.Add(afirstCat4.CatOrder, afirstCat4);

            Assert.AreEqual(0, sortedList.IndexOfValue(afirstCat3));
            Assert.AreEqual(1, sortedList.IndexOfValue(afirstCat1));
            Assert.AreEqual(2, sortedList.IndexOfValue(afirstCat2));
            Assert.AreEqual(3, sortedList.IndexOfValue(afirstCat4));
        }

        [TestMethod]
        public void ShouldMoveCategoryAfter()
        {
            var firstCat = TestHelper.NewCategory();
            var secondCat = TestHelper.NewCategory();

            var moveCategoryParameters = new MoveCategoryParameters
                                         {
                                             UserId = TestDataStore.TestUserId,
                                             ReferenceCategoryId = secondCat.Id,
                                             CategoryId = firstCat.Id,
                                             PositionType = CategoryPositionTypes.After
                                         };

            var res = this.catalogAdminService.MoveCategory(moveCategoryParameters);

            AssertResult(res);

            var actualFirst = CategoryRepository.GetById(firstCat.Id);
            var actualSecond = CategoryRepository.GetById(secondCat.Id);

            Assert.IsTrue(actualSecond.CatOrder < actualFirst.CatOrder);
        }

        [TestMethod]
        public void ShouldMoveCategoryToNewParentBefore()
        {
            var firstCatParent = TestHelper.NewCategory();
            var firstCat = TestHelper.NewCategory(firstCatParent.Id);
            var secondCat = TestHelper.NewCategory();

            var moveCategoryParameters = new MoveCategoryParameters
                                         {
                                             UserId = TestDataStore.TestUserId,
                                             ReferenceCategoryId = firstCat.Id,
                                             CategoryId = secondCat.Id,
                                             PositionType = CategoryPositionTypes.Before
                                         };
            var res = this.catalogAdminService.MoveCategory(moveCategoryParameters);

            AssertResult(res);

            var actualFirst = CategoryRepository.GetById(firstCat.Id);
            var actualSecond = CategoryRepository.GetById(secondCat.Id);

            Assert.IsTrue(actualSecond.CatOrder < actualFirst.CatOrder);
            Assert.IsTrue(actualSecond.ParentId == actualFirst.ParentId);
            Assert.AreNotEqual(secondCat.NamePath, actualSecond.NamePath);
        }

        // VTBPLK-1731
        [TestMethod]
        public void ShouldMoveCategoryToParentOfItsParent()
        {
            var first = TestHelper.NewCategory();
            var second = TestHelper.NewCategory(first.Id);
            var third = TestHelper.NewCategory(second.Id);

            var moveCategoryParameters = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = first.Id,
                CategoryId = third.Id,
                PositionType = CategoryPositionTypes.Append
            };
            
            var res = this.catalogAdminService.MoveCategory(moveCategoryParameters);

            AssertResult(res);

            var actualFirst = CategoryRepository.GetById(first.Id);
            var actualSecond = CategoryRepository.GetById(second.Id);
            var actualThird = CategoryRepository.GetById(third.Id);

            Assert.IsTrue(actualThird.CatOrder > actualSecond.CatOrder);
            Assert.IsTrue(actualThird.ParentId == actualFirst.Id);
        }

        [TestMethod]
        public void ShouldMoveCategoryToNewParentAfter()
        {
            var firstCat = TestHelper.NewCategory();
            var secondCatParent = TestHelper.NewCategory();
            var secondCat = TestHelper.NewCategory(secondCatParent.Id);

            var moveCategoryParameters = new MoveCategoryParameters
                                         {
                                             UserId = TestDataStore.TestUserId,
                                             ReferenceCategoryId = secondCat.Id,
                                             CategoryId = firstCat.Id,
                                             PositionType = CategoryPositionTypes.After
                                         };
            this.catalogAdminService.MoveCategory(moveCategoryParameters);

            var actualFirst = CategoryRepository.GetById(firstCat.Id);
            var actualSecond = CategoryRepository.GetById(secondCat.Id);

            Assert.IsTrue(actualSecond.CatOrder < actualFirst.CatOrder);
            Assert.IsTrue(actualSecond.ParentId == actualFirst.ParentId);
            Assert.AreNotEqual(firstCat.NamePath, actualFirst.NamePath);
        }

        [TestMethod]
        public void ShouldMoveCategoryToNewEmptyParentPrepend()
        {
            var firstCat = TestHelper.NewCategory();
            var secondCat = TestHelper.NewCategory();

            this.catalogAdminService.MoveCategory(
                new MoveCategoryParameters
                {
                    UserId = TestDataStore.TestUserId,
                    ReferenceCategoryId = firstCat.Id,
                    CategoryId = secondCat.Id,
                    PositionType = CategoryPositionTypes.Prepend
                });

            var actualFirst = CategoryRepository.GetById(firstCat.Id);
            var actualSecond = CategoryRepository.GetById(secondCat.Id);

            Assert.IsTrue(actualSecond.ParentId == actualFirst.Id);
            Assert.AreNotEqual(secondCat.NamePath, actualSecond.NamePath);
        }

        [TestMethod]
        public void ShouldMoveCategoryToNewFilledParentPrepend()
        {
            var firstCatParent = TestHelper.NewCategory();
            var firstCat = TestHelper.NewCategory(firstCatParent.Id);
            var secondCat = TestHelper.NewCategory();

            var moveCategoryParameters = new MoveCategoryParameters
                                         {
                                             UserId = TestDataStore.TestUserId,
                                             ReferenceCategoryId = firstCatParent.Id,
                                             CategoryId = secondCat.Id,
                                             PositionType = CategoryPositionTypes.Prepend
                                         };
            this.catalogAdminService.MoveCategory(moveCategoryParameters);

            var actualFirst = CategoryRepository.GetById(firstCat.Id);
            var actualSecond = CategoryRepository.GetById(secondCat.Id);

            Assert.IsTrue(actualSecond.CatOrder < actualFirst.CatOrder);
            Assert.IsTrue(actualSecond.ParentId == actualFirst.ParentId);
            Assert.AreNotEqual(secondCat.NamePath, actualSecond.NamePath);
        }

        [TestMethod]
        public void ShouldMoveCategoryToNewFilledParentAppend()
        {
            var firstCatParent = TestHelper.NewCategory();
            var firstCat = TestHelper.NewCategory(firstCatParent.Id);
            var secondCat = TestHelper.NewCategory();

            this.catalogAdminService.MoveCategory(
                new MoveCategoryParameters
                {
                    UserId = TestDataStore.TestUserId,
                    ReferenceCategoryId = firstCatParent.Id,
                    CategoryId = secondCat.Id,
                    PositionType = CategoryPositionTypes.Append
                });

            var actualFirst = CategoryRepository.GetById(firstCat.Id);
            var actualSecond = CategoryRepository.GetById(secondCat.Id);

            Assert.IsTrue(actualSecond.CatOrder > actualFirst.CatOrder);
            Assert.IsTrue(actualSecond.ParentId == actualFirst.ParentId);
            Assert.AreNotEqual(secondCat.NamePath, actualSecond.NamePath);
        }

        [TestMethod]
        public void ShouldNotMoveParentCategoryToChild()
        {
            var prodRepo = CategoryRepository;

            const string User = UserId;

            var guid = Guid.NewGuid().ToString();

            var category1 = new ProductCategory
            {
                Name = "TestProductCategory1" + guid,
                ParentId = null,
                NamePath = "TestProductCategory1" + guid,
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category1);

            var category2 = new ProductCategory
            {
                Name = "TestProductCategory2",
                ParentId = category1.Id,
                NamePath = "TestProductCategory1" + guid + "/TestProductCategory2",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category2);

            var param = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = category2.Id,
                CategoryId = category1.Id,
                PositionType = CategoryPositionTypes.Prepend
            };

            var result = this.catalogAdminService.MoveCategory(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void ShouldNotMoveParentCategoryToChildWhenChildHasChild()
        {
            var prodRepo = CategoryRepository;

            const string User = UserId;

            var guid = Guid.NewGuid().ToString();

            var category1 = new ProductCategory
            {
                Name = "TestProductCategory1" + guid,
                ParentId = null,
                NamePath = "TestProductCategory1" + guid,
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category1);

            var category2 = new ProductCategory
            {
                Name = "TestProductCategory2",
                ParentId = category1.Id,
                NamePath = "TestProductCategory1" + guid + "/TestProductCategory2",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category2);

            var category3 = new ProductCategory
            {
                Name = "TestProductCategory3",
                ParentId = category2.Id,
                NamePath = "TestProductCategory1" + guid + "/TestProductCategory2/TestProductCategory3",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category3);

            var param = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = category2.Id,
                CategoryId = category1.Id,
                PositionType = CategoryPositionTypes.Prepend
            };

            var result = this.catalogAdminService.MoveCategory(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void ShouldNotMoveParentCategoryToChildOfChild()
        {
            var prodRepo = CategoryRepository;

            const string User = UserId;

            var guid = Guid.NewGuid().ToString();

            var category1 = new ProductCategory
            {
                Name = "TestProductCategory1" + guid,
                ParentId = null,
                NamePath = "TestProductCategory1" + guid,
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category1);

            var category2 = new ProductCategory
            {
                Name = "TestProductCategory2",
                ParentId = category1.Id,
                NamePath = "TestProductCategory1" + guid + "/TestProductCategory2",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category2);

            var category3 = new ProductCategory
            {
                Name = "TestProductCategory3",
                ParentId = category2.Id,
                NamePath = "TestProductCategory1" + guid + "/TestProductCategory2/TestProductCategory3",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category3);

            var param = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = category3.Id,
                CategoryId = category1.Id,
                PositionType = CategoryPositionTypes.Prepend
            };

            var result = this.catalogAdminService.MoveCategory(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void ShouldNotInsertParentBeforeChildOfChild()
        {
            var prodRepo = CategoryRepository;

            const string User = UserId;

            var guid = Guid.NewGuid().ToString();

            var category1 = new ProductCategory
            {
                Name = "TestProductCategory1" + guid,
                ParentId = null,
                NamePath = "TestProductCategory1" + guid,
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category1);

            var category2 = new ProductCategory
            {
                Name = "TestProductCategory2",
                ParentId = category1.Id,
                NamePath = "TestProductCategory1" + guid + "/TestProductCategory2",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category2);

            var category3 = new ProductCategory
            {
                Name = "TestProductCategory3",
                ParentId = category2.Id,
                NamePath = "TestProductCategory1" + guid + "/TestProductCategory2/TestProductCategory3",
                Status = ProductCategoryStatuses.NotActive,
                InsertedDate = DateTime.Now,
                InsertedUserId = User
            };
            prodRepo.Add(category3);

            var param = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = category3.Id,
                CategoryId = category1.Id,
                PositionType = CategoryPositionTypes.Before
            };

            var result = this.catalogAdminService.MoveCategory(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void ShouldNotMoveCategory2()
        {
            var cat1 = TestHelper.NewCategory();
            var cat2 = TestHelper.NewCategory();
            var cat3 = TestHelper.NewCategory();

            var moveCategory2Parameters = new MoveCategoryParameters
                                             {
                                                 UserId = TestDataStore.TestUserId,
                                                 ReferenceCategoryId = cat1.Id,
                                                 CategoryId = cat2.Id
                                             };
            var res2Move = this.catalogAdminService.MoveCategory(moveCategory2Parameters);
            Assert.IsTrue(res2Move.Success);

            cat1 = CategoryRepository.GetById(cat1.Id);
            cat2 = CategoryRepository.GetById(cat2.Id);
            Assert.AreEqual(cat1.Id, cat2.ParentId);

            var moveCategory3Parameters = new MoveCategoryParameters
                                              {
                                                  UserId = TestDataStore.TestUserId,
                                                  ReferenceCategoryId = cat1.Id,
                                                  CategoryId = cat3.Id
                                              };
            var res3Move = this.catalogAdminService.MoveCategory(moveCategory3Parameters);
            Assert.IsTrue(res3Move.Success);

            cat3 = CategoryRepository.GetById(cat3.Id);
            Assert.AreEqual(cat1.Id, cat3.ParentId);

            var updateCategoryParameters = new UpdateCategoryParameters
                                           {
                                               CategoryId = cat2.Id,
                                               UserId = TestDataStore.TestUserId,
                                               NewName = cat3.Name
                                           };
            var result = this.catalogAdminService.UpdateCategory(updateCategoryParameters);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.CATEGORY_WITH_NAME_EXISTS, result.ResultCode);
        }

        [TestMethod]
        public void ShouldNotDeleteCategory()
        {
            var result = this.catalogAdminService.DeleteCategory(null, 4);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.INVALID_PARAMETER_VALUE, result.ResultCode);
        }

        [TestMethod]
        public void ShouldSearchAllProducts()
        {
            var parameters = new AdminSearchProductsParameters
                             {
                                 UserId = TestDataStore.TestUserId
                             };
            var result = catalogAdminService.SearchProducts(parameters);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Products);
            Assert.IsTrue(result.Products.Any());
            Assert.IsFalse(string.IsNullOrEmpty(result.Products[0].CategoryName));
            Assert.IsFalse(string.IsNullOrEmpty(result.Products[0].CategoryNamePath));
        }

        [TestMethod]
        public void ShouldSetPartnerProductCateroryLink()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.PartnerProductCategoryLink.Where(
                    p =>
                        p.PartnerId == partnerId &&
                            (p.NamePath == CatNamePath1 || p.NamePath == CatNamePath2)).ToList().ForEach(p => ctx.Entry(p).State = EntityState.Deleted);

                ctx.SaveChanges();
            }
            
            var link = new PartnerProductCategoryLink
            {
                PartnerId = partnerId,
                ProductCategoryId = this.someParentCatalog.Id,
                Paths = new[]
                {
                    new CategoryPath
                    {
                        IncludeSubcategories = true, 
                        NamePath = CatNamePath1
                    },
                    new CategoryPath
                    {
                        IncludeSubcategories = false, 
                        NamePath = CatNamePath2
                    }
                }
            };

            var setLinkRes =
                this.catalogAdminService.SetPartnerProductCategoryLink(new CreatePartnerProductCateroryLinkParameters
                {
                    UserId = TestDataStore.TestUserId,
                    Link = link
                });

            Assert.IsNotNull(setLinkRes);
            Assert.IsTrue(setLinkRes.Success, setLinkRes.ResultDescription);

            Assert.IsNotNull(setLinkRes.Link);
            Assert.IsNotNull(setLinkRes.Link.Paths);

            Assert.AreEqual(partnerId, setLinkRes.Link.PartnerId);
            Assert.AreEqual(this.someParentCatalog.Id, setLinkRes.Link.ProductCategoryId);

            Assert.AreEqual(2, setLinkRes.Link.Paths.Length);

            var path1 = setLinkRes.Link.Paths.FirstOrDefault(l => l.NamePath == CatNamePath1);
            Assert.IsNotNull(path1);
            Assert.AreEqual(true, path1.IncludeSubcategories);

            var path2 = setLinkRes.Link.Paths.FirstOrDefault(l => l.NamePath == CatNamePath2);
            Assert.IsNotNull(path2);
            Assert.AreEqual(false, path2.IncludeSubcategories);

            link.ProductCategoryId = someParentCatalog2.Id;

            setLinkRes =
               this.catalogAdminService.SetPartnerProductCategoryLink(new CreatePartnerProductCateroryLinkParameters
               {
                   UserId = TestDataStore.TestUserId,
                   Link = link 
               });

            Assert.IsNotNull(setLinkRes);
            Assert.IsFalse(setLinkRes.Success);
            Assert.AreEqual(ResultCodes.ALLREADY_EXISTS, setLinkRes.ResultCode);

            link.ProductCategoryId = someParentCatalog.Id;
            var paths = link.Paths.ToList();
            paths.Add(link.Paths[0]);

            link.Paths = paths.ToArray();

            setLinkRes =
               this.catalogAdminService.SetPartnerProductCategoryLink(new CreatePartnerProductCateroryLinkParameters
               {
                   UserId = TestDataStore.TestUserId,
                   Link = link
               });

            Assert.IsNotNull(setLinkRes);
            Assert.IsFalse(setLinkRes.Success);
            Assert.AreEqual(ResultCodes.ALLREADY_EXISTS, setLinkRes.ResultCode);
        }

        [TestMethod]
        public void ShouldReturnPartnerProductCateroryLinks()
        {
            const int PartnerId = 1;
            const int Cat1 = 50;
            const int Cat2 = 51;

            var mock = MockFactory.GetPartnerProductCateroryLinkRepository(PartnerId, Cat1, Cat2);

            var service = new CatalogAdminService(partnerProductCateroryLinkRepository: mock.Object);

            var result = service.GetPartnerProductCategoryLinks(TestDataStore.TestUserId, PartnerId, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success, result.ResultDescription);
            Assert.IsNotNull(result.Links);
            Assert.AreEqual(2, result.Links.Count());

            var l1 = result.Links.FirstOrDefault(x => x.PartnerId == PartnerId && x.ProductCategoryId == Cat1);
            Assert.IsNotNull(l1);
            Assert.IsNotNull(l1.Paths);
            Assert.AreEqual(2, l1.Paths.Count());
            Assert.IsTrue(l1.Paths.All(x => x.NamePath.EndsWith("категория 1")));

            var l2 = result.Links.FirstOrDefault(x => x.PartnerId == PartnerId && x.ProductCategoryId == Cat2);
            Assert.IsNotNull(l2);
            Assert.IsNotNull(l2.Paths);
            Assert.AreEqual(1, l2.Paths.Count());
            Assert.IsTrue(l2.Paths.All(x => x.NamePath.EndsWith("категория 2")));
        }

        [TestMethod]
        public void ShouldMoveCategoryAsFirstRootWhenBefore()
        {
            var parentCat = TestHelper.NewCategory();
            var childCat = TestHelper.NewCategory(parentCat.Id);

            var beforeMoveCatOrder = CategoryRepository.GetById(childCat.Id).CatOrder;

            var param = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = null,
                CategoryId = childCat.Id,
                PositionType = CategoryPositionTypes.Before
            };

            this.catalogAdminService.MoveCategory(param);

            var afterMoveCat = CategoryRepository.GetById(childCat.Id);

            Assert.IsNull(afterMoveCat.ParentId);
            Assert.AreNotEqual(beforeMoveCatOrder, afterMoveCat.CatOrder);
        }

        [TestMethod]
        public void ShouldMoveCategoryAsFirstRootWhenPrepend()
        {
            var parentCat = TestHelper.NewCategory();
            var childCat = TestHelper.NewCategory(parentCat.Id);

            var beforeMoveCatOrder = CategoryRepository.GetById(childCat.Id).CatOrder;

            var param = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = null,
                CategoryId = childCat.Id,
                PositionType = CategoryPositionTypes.Prepend
            };

            this.catalogAdminService.MoveCategory(param);

            var afterMoveCat = CategoryRepository.GetById(childCat.Id);

            Assert.IsNull(afterMoveCat.ParentId);
            Assert.AreNotEqual(beforeMoveCatOrder, afterMoveCat.CatOrder);
        }

        [TestMethod]
        public void ShouldMoveCategoryAsLastRootWhenAfter()
        {
            var catresult = this.catalogAdminService.GetAllSubCategories(
                new GetAllSubCategoriesParameters
                {
                    IncludeParent = true,
                    Status = ProductCategoryStatuses.Active,
                    UserId = TestDataStore.TestUserId
                });
            var parentCat = catresult.Categories.First(x => x.ParentId == null);
            var childCat = TestHelper.NewCategory(parentCat.Id);

            var beforeMoveCatOrder = CategoryRepository.GetById(childCat.Id).CatOrder;

            var param = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = null,
                CategoryId = childCat.Id,
                PositionType = CategoryPositionTypes.After
            };

            this.catalogAdminService.MoveCategory(param);

            var afterMoveCat = CategoryRepository.GetById(childCat.Id);

            Assert.IsNull(afterMoveCat.ParentId);
            Assert.AreNotEqual(beforeMoveCatOrder, afterMoveCat.CatOrder);
        }

        [TestMethod]
        public void ShouldMoveCategoryAsLastRootWhenAppend()
        {
            var catresult = this.catalogAdminService.GetAllSubCategories(
                new GetAllSubCategoriesParameters
                {
                    IncludeParent = true,
                    Status = ProductCategoryStatuses.Active,
                    UserId = TestDataStore.TestUserId
                });
            var parentCat = catresult.Categories.First(x => x.ParentId == null);
            var childCat = TestHelper.NewCategory(parentCat.Id);

            var beforeMoveCatOrder = CategoryRepository.GetById(childCat.Id).CatOrder;

            var param = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = null,
                CategoryId = childCat.Id,
                PositionType = CategoryPositionTypes.Append
            };

            this.catalogAdminService.MoveCategory(param);

            var afterMoveCat = CategoryRepository.GetById(childCat.Id);

            Assert.IsNull(afterMoveCat.ParentId);
            Assert.AreNotEqual(beforeMoveCatOrder, afterMoveCat.CatOrder);
        }

        [TestMethod]
        public void ShouldNotMoveCategory()
        {
            const int ID = 5;

            var param = new MoveCategoryParameters
            {
                UserId = TestDataStore.TestUserId,
                ReferenceCategoryId = ID,
                CategoryId = ID,
                PositionType = CategoryPositionTypes.Append
            };

            var result = this.catalogAdminService.MoveCategory(param);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.INVALID_PARAMETER_VALUE, result.ResultCode);
        }

        [TestMethod]
        public void ShouldCreateProduct()
        {
            var category = TestHelper.NewCategory();
            var createProductParams = this.NewCreateProductParameters(category.Id);
            var permissionRepo = new CategoryPermissionRepository();

            permissionRepo.Save(UserId, new[]
            {
                new CategoryPermission(createProductParams.PartnerId, category.Id, TestDataStore.TestUserId)
            });

            var result = this.catalogAdminService.CreateProduct(createProductParams);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Product);

            permissionRepo.Delete(UserId, result.Product.PartnerId, new List<int> { category.Id });
        }

        [TestMethod]
        public void ShouldNotCreateProduct()
        {
            var category = TestHelper.NewCategory();
            var createProductParams = this.NewCreateProductParameters(category.Id);

            createProductParams.PartnerProductId = string.Join(", ", Enumerable.Range(0, 300));
            var result = this.catalogAdminService.CreateProduct(createProductParams);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.ARGUMENT_OUT_OF_RANGE, result.ResultCode);
        }

        [TestMethod]
        public void ShouldUpdateProduct()
        {
            var category = TestHelper.NewCategory();
            var createProductParams = this.NewCreateProductParameters(category.Id);

            var permissionRepo = new CategoryPermissionRepository();

            permissionRepo.Save(UserId, new[]
            {
                new CategoryPermission(createProductParams.PartnerId, category.Id, TestDataStore.TestUserId)
            });

            try
            {
                var result = this.catalogAdminService.CreateProduct(createProductParams);

                Assert.IsTrue(result.Success);
                Assert.IsNotNull(result.Product);

                result.Product.Name = result.Product.Name + "_Updated";

                const int Weight = 0;

                var param = new UpdateProductParameters
                            {
                                UserId = TestDataStore.TestUserId,
                                ProductId = result.Product.ProductId,
                                CategoryId = result.Product.CategoryId,
                                CurrencyId = "RUR",
                                Name = result.Product.Name,
                                Param = result.Product.Param,
                                PartnerId = result.Product.PartnerId,
                                Pictures = result.Product.Pictures,
                                Description = result.ResultDescription,
                                PriceRUR = 10,
                                Weight = Weight
                            };

                var updateResult = this.catalogAdminService.UpdateProduct(param);

                Assert.IsTrue(updateResult.Success);

                new ProductsDataSource().UpdateProductsFromAllPartners();

                var updatedProduct = this.productsRepository.GetById(result.Product.ProductId);

                Assert.AreEqual(result.Product.Name, updatedProduct.Name);
                Assert.AreEqual(0, updatedProduct.Weight);
            }
            finally
            {
                permissionRepo.Delete(UserId, createProductParams.PartnerId, new List<int> { category.Id });
            }
        }

        [TestMethod]
        public void ShouldUpdateAndClearProductBasePrice()
        {
            var site505EnableActionPrice = FeaturesConfiguration.Instance.Site505EnableActionPrice;
            FeaturesConfiguration.Instance.Site505EnableActionPrice = true;

            var category = TestHelper.NewCategory();
            var createProductParams = this.NewCreateProductParameters(category.Id);
            createProductParams.PriceRUR = 20;

            var permissionRepo = new CategoryPermissionRepository();

            permissionRepo.Save(UserId, new[]
            {
                new CategoryPermission(createProductParams.PartnerId, category.Id, TestDataStore.TestUserId)
            });

            try
            {
                var result = this.catalogAdminService.CreateProduct(createProductParams);

                Assert.IsTrue(result.Success);
                Assert.IsNotNull(result.Product);

                var param = new UpdateProductParameters
                {
                    UserId = TestDataStore.TestUserId,
                    ProductId = result.Product.ProductId,
                    CategoryId = result.Product.CategoryId,
                    CurrencyId = "RUR",
                    Name = result.Product.Name,
                    Param = result.Product.Param,
                    PartnerId = result.Product.PartnerId,
                    Pictures = result.Product.Pictures,
                    Description = result.ResultDescription,
                    PriceRUR = 10,
                };

                var updateResult = this.catalogAdminService.UpdateProduct(param);

                Assert.IsTrue(updateResult.Success);

                var updatedProduct = new ProductsDataSource().GetById(result.Product.ProductId, result.Product.PartnerId);

                Assert.AreEqual(20, updatedProduct.BasePriceRUR);

                param.PriceRUR = 20;
                updateResult = this.catalogAdminService.UpdateProduct(param);

                Assert.IsTrue(updateResult.Success);

                updatedProduct = new ProductsDataSource().GetById(result.Product.ProductId, result.Product.PartnerId);

                Assert.IsNull(updatedProduct.BasePriceRUR);

                param.PriceRUR = 10;
                this.catalogAdminService.UpdateProduct(param);

                new ProductService().CleanupProductBasePriceDate();

                updatedProduct = new ProductsDataSource().GetById(result.Product.ProductId, result.Product.PartnerId);

                Assert.AreEqual(20, updatedProduct.BasePriceRUR);

                using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
                {
                    ctx.Database.ExecuteSqlCommand(string.Format("UPDATE [prod].[ProductsFixedPrices] SET [FixedPriceDate] = GETDATE() - 40 WHERE [ProductId] = '{0}'", result.Product.ProductId));

                    var fixedDatesCount = ctx.Database.SqlQuery<int>(String.Format("SELECT COUNT(*) FROM [prod].[ProductsFixedPrices] WHERE [ProductId] = '{0}'", result.Product.ProductId)).SingleOrDefault();
                    Assert.AreEqual(1, fixedDatesCount);
                }

                new ProductService().CleanupProductBasePriceDate();

                updatedProduct = new ProductsDataSource().GetById(result.Product.ProductId, result.Product.PartnerId);

                Assert.IsNull(updatedProduct.BasePriceRUR);

                using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
                {
                    var fixedDatesCount = ctx.Database.SqlQuery<int>(String.Format("SELECT COUNT(*) FROM [prod].[ProductsFixedPrices] WHERE [ProductId] = '{0}'", result.Product.ProductId)).SingleOrDefault();
                    Assert.AreEqual(0, fixedDatesCount);
                }

                param.BasePriceRUR = 30;
                this.catalogAdminService.UpdateProduct(param);
                updatedProduct = new ProductsDataSource().GetById(result.Product.ProductId, result.Product.PartnerId);

                Assert.AreEqual(30, updatedProduct.BasePriceRUR);

                using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
                {
                    var fixedDatesCount = ctx.Database.SqlQuery<int>(String.Format("SELECT COUNT(*) FROM [prod].[ProductsFixedPrices] WHERE [ProductId] = '{0}'", result.Product.ProductId)).SingleOrDefault();
                    Assert.AreEqual(0, fixedDatesCount);
                }

                param.BasePriceRUR = 0;
                this.catalogAdminService.UpdateProduct(param);
                updatedProduct = new ProductsDataSource().GetById(result.Product.ProductId, result.Product.PartnerId);

                Assert.IsNull(updatedProduct.BasePriceRUR);
            }
            finally
            {
                permissionRepo.Delete(UserId, createProductParams.PartnerId, new List<int> { category.Id });
                FeaturesConfiguration.Instance.Site505EnableActionPrice = site505EnableActionPrice;
            }
        }

        [TestMethod]
        public void ShouldCreateProductWithBasePrice()
        {
            var site505EnableActionPrice = FeaturesConfiguration.Instance.Site505EnableActionPrice;
            FeaturesConfiguration.Instance.Site505EnableActionPrice = true;

            var category = TestHelper.NewCategory();
            var createProductParams = this.NewCreateProductParameters(category.Id);
            createProductParams.PriceRUR = 20;
            createProductParams.BasePriceRUR = 30;

            var permissionRepo = new CategoryPermissionRepository();

            permissionRepo.Save(UserId, new[]
            {
                new CategoryPermission(createProductParams.PartnerId, category.Id, TestDataStore.TestUserId)
            });

            try
            {
                var result = this.catalogAdminService.CreateProduct(createProductParams);

                Assert.IsTrue(result.Success);
                Assert.IsNotNull(result.Product);

                var insertedProduct = new ProductsDataSource().GetById(result.Product.ProductId, result.Product.PartnerId);

                Assert.AreEqual(30, insertedProduct.BasePriceRUR);

                using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
                {
                    var fixedDatesCount = ctx.Database.SqlQuery<int>(String.Format("SELECT COUNT(*) FROM [prod].[ProductsFixedPrices] WHERE [ProductId] = '{0}'", result.Product.ProductId)).SingleOrDefault();
                    Assert.AreEqual(0, fixedDatesCount);
                }
            }
            finally
            {
                permissionRepo.Delete(UserId, createProductParams.PartnerId, new List<int> { category.Id });
                FeaturesConfiguration.Instance.Site505EnableActionPrice = site505EnableActionPrice;
            }
        }

        [TestMethod]
        public void ShouldNotUpdateProduct()
        {
            var category = TestHelper.NewCategory();

            var createProductParams = this.NewCreateProductParameters(category.Id);

            var permissionRepo = new CategoryPermissionRepository();

            permissionRepo.Save(UserId, new[]
            {
                new CategoryPermission(createProductParams.PartnerId, category.Id, TestDataStore.TestUserId)
            });

            try
            {
                var result = this.catalogAdminService.CreateProduct(createProductParams);

                Assert.IsTrue(result.Success);
                Assert.IsNotNull(result.Product);

                result.Product.Name = result.Product.Name + "_Updated";
            }
            finally
            {
                permissionRepo.Delete(UserId, createProductParams.PartnerId, new List<int> { category.Id });
            }
        }

        [TestMethod]
        public void ShouldNotCreateProductInvalidParams()
        {
            var createProductParams = new CreateProductParameters
                                      {
                                          UserId = "lll",
                                          Description = "ll2"
                                      };

            var result = this.catalogAdminService.CreateProduct(createProductParams);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.INVALID_PARAMETER_VALUE, result.ResultCode);
        }

        [TestMethod]
        public void ShouldGetProductById()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var product = ctx.ProductSortProjections.First();

                var parameters = new ArmGetProductByIdParameters
                                 {
                                     ProductId = product.ProductId,
                                     UserId = TestDataStore.TestUserId
                                 };
                var result = this.catalogAdminService.GetProductById(parameters);

                Assert.IsTrue(result.Success);
                Assert.AreEqual(product.ProductId, result.Product.ProductId);
                Assert.AreEqual(product.PartnerId, result.Product.PartnerId);
            }
        }

        [TestMethod]
        public void ShouldNotGetProductById()
        {
            var parameters = new ArmGetProductByIdParameters
                             {
                                 ProductId = "999",
                                 UserId = TestDataStore.TestUserId
                             };
            var result = this.catalogAdminService.GetProductById(parameters);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.PARTNER_CATALOG_NOT_FOUND, result.ResultCode);
        }

        [TestMethod]
        public void ShouldCreatePartner()
        {
            var param = new CreatePartnerParameters
                        {
                            Name = "TestPartner" + Guid.NewGuid(),
                            UserId = TestDataStore.TestUserId,
                            IsCarrier = true,
                            Settings = new Dictionary<string, string>() { {PartnerSettingsExtension.MultiPositionOrdersKey, "true"} }
                        };
            var result = this.catalogAdminService.CreatePartner(param);

            Assert.IsTrue(result.Success, result.ResultDescription);
            Assert.IsNotNull(result.Partner);
            Assert.IsTrue(result.Partner.IsCarrier);
            Assert.AreEqual(result.Partner.Settings[PartnerSettingsExtension.MultiPositionOrdersKey], "true");
        }

        [TestMethod]
        public void ShouldFillPopularProducts()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.Database.ExecuteSqlCommand(
                    "[prod].[FillPopularProducts] @viewsDate",
                    new SqlParameter("@viewsDate", DateTime.Now.Date));
            }
        }

        [TestMethod]
        public void ShouldChangeOrderStatusDescription()
        {
            var order = TestDataStore.GetOrder();
            var ds = new OrdersDataSource();
            var orderId = ds.Insert(order);
            Console.WriteLine(orderId);

            var orderResult = catalogAdminService.GetOrderById(ApiSettings.ClientSiteUserName, orderId);
            Assert.IsNotNull(orderResult);

            catalogAdminService.ChangeOrdersStatusDescription(ApiSettings.ClientSiteUserName, orderId, "Description1");
            var orderResult1 = catalogAdminService.GetOrderById(ApiSettings.ClientSiteUserName, orderId);
            Assert.IsNotNull(orderResult1);
            Assert.AreEqual("Description1", orderResult1.Order.OrderStatusDescription);
            Assert.AreEqual(orderResult.Order.Status, orderResult1.Order.Status);

            var nextOrderStatus = orderResult1.NextOrderStatuses.First();
            var ordersStatus = new OrdersStatus
                               {
                                   OrderId = orderId,
                                   OrderStatus = nextOrderStatus,
                                   OrderStatusDescription = "Description2"
                               };
            var ordersStatuses = new[] { ordersStatus };

            catalogAdminService.ChangeOrdersStatuses(ApiSettings.ClientSiteUserName, ordersStatuses);

            var orderResult2 = catalogAdminService.GetOrderById(ApiSettings.ClientSiteUserName, orderId);
            Assert.IsNotNull(orderResult2);
            Assert.AreEqual("Description2", orderResult2.Order.OrderStatusDescription);
            Assert.AreEqual(nextOrderStatus, orderResult2.Order.Status);

            catalogAdminService.ChangeOrdersStatusDescription(ApiSettings.ClientSiteUserName, orderId, "Description3");

            var orderResult3 = catalogAdminService.GetOrderById(ApiSettings.ClientSiteUserName, orderId);
            Assert.IsNotNull(orderResult3);
            Assert.AreEqual("Description3", orderResult3.Order.OrderStatusDescription);
            Assert.AreEqual(nextOrderStatus, orderResult3.Order.Status);

            var getOrderStatusesHistoryParameters = new GetOrderStatusesHistoryParameters
                                                    {
                                                        CountToSkip = 0,
                                                        CountToTake = 5000,
                                                        UserId = ApiSettings.ClientSiteUserName,
                                                        OrderId = orderId
                                                    };
            var history = catalogAdminService.GetOrderStatusesHistory(getOrderStatusesHistoryParameters);

            Assert.IsNotNull(history);
            Assert.IsNotNull(history.OrderHistory);
            Assert.AreEqual(4, history.OrderHistory.Length);
        }

        [TestMethod]
        public void ShouldUpdatePartner()
        {
            var createPartnerParameters = new CreatePartnerParameters
                           {
                               CarrierId = 2,
                               Description = "Test " + Guid.NewGuid(),
                               UserId = ApiSettings.ClientSiteUserName,
                               Name = "Test " + Guid.NewGuid(),
                               Type = PartnerType.Direct,
                               ThrustLevel = PartnerThrustLevel.High,
                               Status = PartnerStatus.Active,
                               Settings = new Dictionary<string, string>()
                           };
            var createPartnerResult = catalogAdminService.CreatePartner(createPartnerParameters);
            Assert.IsNotNull(createPartnerResult);
            Assert.AreEqual(true, createPartnerResult.Success);

            var partnerId = createPartnerResult.Partner.Id;

            var partnerByIdResult = catalogAdminService.GetPartnerById(partnerId, ApiSettings.ClientSiteUserName);
            Assert.IsNotNull(partnerByIdResult);
            Assert.AreEqual(true, partnerByIdResult.Success);
            var partner = partnerByIdResult.Partner;

            var settings = new Dictionary<string, string>
                           {
                               {
                                   "BatchConfirmation",
                                   "https://rphqbuild1.rapidsoft.local:643/Actions/BatchConfirmOrder"
                               },
                               {
                                   "CertificateThumbprint",
                                   "d1 6c e5 79 ad 19 cb 9a 99 37 67 cc 66 ec ad 37 ee ac 98 c2"
                               },
                               {
                                   "Check", "https://rphqbuild1.rapidsoft.local:643/Actions/CheckOrder"
                               },
                               {
                                   "Confirmation", "https://rphqbuild1.rapidsoft.local:643/Actions/ConfirmOrder"
                               },
                               {
                                   "FixBasketItemPrice",
                                   "https://rphqbuild1.rapidsoft.local:643/Actions/FixBasketItemPrice"
                               },
                               {
                                   "UnitellerShopId", "444444444"
                               },
                               {
                                   "UseBatch", "true"
                               }
                           };

            var param = new UpdatePartnerParameters
                        {
                            Id = partner.Id,
                            UpdatedUserId = ApiSettings.ClientSiteUserName,
                            NewDescription = partner.Description,
                            NewName = partner.Name,
                            NewType = partner.Type,
                            NewStatus = partner.Status,
                            NewThrustLevel = partner.ThrustLevel,
                            NewCarrierId = null,
                            NewSettings = settings
                        };
            var result = catalogAdminService.UpdatePartner(param);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success, result.ResultDescription);

            param = new UpdatePartnerParameters
                    {
                        Id = partner.Id,
                        UpdatedUserId = ApiSettings.ClientSiteUserName,
                        NewDescription = partner.Description,
                        NewName = partner.Name,
                        NewType = partner.Type,
                        NewStatus = partner.Status,
                        NewThrustLevel = partner.ThrustLevel,
                        NewCarrierId = partner.CarrierId,
                        NewSettings = settings
                    };
            result = catalogAdminService.UpdatePartner(param);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success, result.ResultDescription);

            DeletePartner(partnerId);
        }

        [TestMethod]
        public void ShouldChangeCarrier()
        {
            var parntersResult = catalogAdminService.GetPartners(null, ApiSettings.ClientSiteUserName);
            Assert.IsNotNull(parntersResult);
            Assert.IsNotNull(parntersResult.Partners);
            var partners = parntersResult.Partners.Reverse().Take(2).ToArray();

            var createPartnerParameters = new CreatePartnerParameters
                                              {
                                                  CarrierId = partners.First().Id,
                                                  Description = "Test " + Guid.NewGuid(),
                                                  UserId = ApiSettings.ClientSiteUserName,
                                                  Name = "Test " + Guid.NewGuid(),
                                                  Type = PartnerType.Direct,
                                                  ThrustLevel = PartnerThrustLevel.High,
                                                  Status = PartnerStatus.Active,
                                                  Settings = new Dictionary<string, string>()
                                              };
            var createPartnerResult = catalogAdminService.CreatePartner(createPartnerParameters);
            Assert.IsNotNull(createPartnerResult);
            Assert.AreEqual(true, createPartnerResult.Success);

            var partner = createPartnerResult.Partner;

            var param = new UpdatePartnerParameters
                            {
                                Id = partner.Id,
                                UpdatedUserId = ApiSettings.ClientSiteUserName,
                                NewDescription = partner.Description,
                                NewName = partner.Name,
                                NewType = partner.Type,
                                NewStatus = partner.Status,
                                NewThrustLevel = partner.ThrustLevel,
                                NewCarrierId = partners.Skip(1).First().Id,
                                NewSettings = new Dictionary<string, string>()
                            };
            var result = catalogAdminService.UpdatePartner(param);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void ShouldUpdateKladrInDeliveryLocation()
        {
            // NOTE: Вероятность того что такой КЛАДР уже используется -> 0%, вероятность того что такой КЛАДР есть в геобазе -> 100%.
            const string TestKladr = "6300000102400";
            var testRepo = new TestDeliveryRatesRepository();
            testRepo.DeleteByPartnerIdAndKladrCodeLike(TestParnterId, TestKladr);

            // NOTE: Создаем не правильную
            testRepo.Create(TestParnterId, 0, 1000, 50, "6300000102500", "135 км	ж/д_казарм");

            var parameters = new GetDeliveryLocationsParameters { PartnerId = TestParnterId, };
            var locs = catalogAdminService.GetDeliveryLocations(parameters, ApiSettings.ClientSiteUserName);

            Assert.IsNotNull(locs);
            Assert.AreEqual(true, locs.Success);
            Assert.IsTrue(locs.DeliveryLocations.Any(), "У партнера должна быть хотя бы одна локация");

            var first = locs.DeliveryLocations.First();
            var oldKladr = first.Kladr;

            var historyParameters = new GetDeliveryLocationHistoryParameters
                                                           {
                                                               PartnerId = TestParnterId,
                                                               LocationId = first.Id
                                                           };
            var history = catalogAdminService.GetDeliveryLocationHistory(historyParameters, ApiSettings.ClientSiteUserName);
            Assert.IsNotNull(history);
            Assert.AreEqual(true, history.Success);
            var oldHistoryCount = history.DeliveryLocationHistory.Count();

            var result = catalogAdminService.SetDeliveryLocationKladr(first.Id, TestKladr, ApiSettings.ClientSiteUserName);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);

            result = catalogAdminService.ResetDeliveryLocation(first.Id, ApiSettings.ClientSiteUserName);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);

            result = catalogAdminService.SetDeliveryLocationKladr(first.Id, oldKladr, ApiSettings.ClientSiteUserName);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);

            history = catalogAdminService.GetDeliveryLocationHistory(historyParameters, ApiSettings.ClientSiteUserName);
            Assert.IsNotNull(history);
            Assert.AreEqual(true, history.Success);
            Assert.AreEqual(oldHistoryCount + 3, history.DeliveryLocationHistory.Count());
        }

        [TestMethod]
        public void ShouldUpdateKladrInDeliveryLocationWhenExistsOtherInvalidDeliveryLocation()
        {
            var testRepo = new TestDeliveryRatesRepository();
            testRepo.DeleteByPartnerIdAndKladrCodeLike(TestParnterId, "6300000800100");

            // NOTE: Создаем две локации на один КЛАДР - 6300000800100 (село Кашпир)
            var del1 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "КашпирАТОР", DeliveryLocationStatus.KladrDuplication);
            var del2 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "КашпирА", DeliveryLocationStatus.KladrDuplication);
            var del3 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "Кашпир", DeliveryLocationStatus.KladrDuplication);

            // NOTE: Успешно сохраняем для второго
            var result = catalogAdminService.SetDeliveryLocationKladr(del2.LocationId, "6300000800100", ApiSettings.ClientSiteUserName);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);

            var resultNext = catalogAdminService.SetDeliveryLocationKladr(del3.LocationId, "6300000800100", ApiSettings.ClientSiteUserName);

            Assert.IsNotNull(resultNext);
            Assert.AreNotEqual(true, resultNext.Success, "Когда есть уже одна локация со статусом DeliveryLocationStatus.CorrectBinded нельзя создать/установить другую.");

            // NOTE: Чистим
            testRepo.Delete(del1.LocationId);
            testRepo.Delete(del2.LocationId);
            testRepo.Delete(del3.LocationId);
        }

        [TestMethod]
        public void ShouldChangeDeliveryLocationStatusWhenResetOneOfDublicates()
        {
            var testRepo = new TestDeliveryRatesRepository();
            testRepo.DeleteByPartnerIdAndKladrCodeLike(TestParnterId, "6300000800100");

            // NOTE: Создаем две локации на один КЛАДР - 6300000800100 (село Кашпир)
            var del1 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "КашпирАТОР", DeliveryLocationStatus.KladrDuplication);
            var del2 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "КашпирА", DeliveryLocationStatus.KladrDuplication);

            // NOTE: Сбрасываем для второго
            catalogAdminService.ResetDeliveryLocation(del2.LocationId, ApiSettings.ClientSiteUserName);

            // NOTE: Проверяем что для первого статус изменился в ОК.
            var @params = new GetDeliveryLocationsParameters { PartnerId = TestParnterId, SearchTerm = "кашпиратор" };
            var locationsResult = catalogAdminService.GetDeliveryLocations(@params, ApiSettings.ClientSiteUserName);

            Assert.IsNotNull(locationsResult);
            Assert.IsNotNull(locationsResult.DeliveryLocations);

            var location = locationsResult.DeliveryLocations.FirstOrDefault(x => x.Kladr == "6300000800100");
            Assert.IsNotNull(location);

            Assert.AreEqual(DeliveryLocationStatus.CorrectBinded, location.Status);
            
            // NOTE: Чистим
            testRepo.Delete(del1.LocationId);
            testRepo.Delete(del2.LocationId);
        }

        [TestMethod]
        public void ShouldNotChangeDeliveryLocationStatusWhenResetOneButExistMoreThenOneOthers()
        {
            var testRepo = new TestDeliveryRatesRepository();
            testRepo.DeleteByPartnerIdAndKladrCodeLike(TestParnterId, "6300000800100");

            // NOTE: Создаем две локации на один КЛАДР - 6300000800100 (село Кашпир)
            var del1 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "КашпирАТОР", DeliveryLocationStatus.KladrDuplication);
            var del2 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "КашпирАТОР", DeliveryLocationStatus.KladrDuplication);
            var del3 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "КашпирА", DeliveryLocationStatus.KladrDuplication);

            // NOTE: Сбрасываем для второго
            catalogAdminService.ResetDeliveryLocation(del2.Id, ApiSettings.ClientSiteUserName);

            // NOTE: Проверяем что для первого статус НЕ изменился в ОК.
            var @params = new GetDeliveryLocationsParameters { PartnerId = TestParnterId, SearchTerm = "кашпиратор" };
            var locationsResult = catalogAdminService.GetDeliveryLocations(@params, ApiSettings.ClientSiteUserName);

            Assert.IsNotNull(locationsResult);
            Assert.IsNotNull(locationsResult.DeliveryLocations);

            var location = locationsResult.DeliveryLocations.FirstOrDefault(x => x.Kladr == "6300000800100");
            Assert.IsNotNull(location);

            Assert.AreNotEqual(DeliveryLocationStatus.CorrectBinded, location.Status);

            // NOTE: Чистим
            testRepo.Delete(del1.LocationId);
            testRepo.Delete(del2.LocationId);
            testRepo.Delete(del3.LocationId);
        }

        [TestMethod]
        public void ShouldChangeDeliveryLocationStatusWhenOneOfDublicatesGetOthersKladr()
        {
            var testRepo = new TestDeliveryRatesRepository();
            testRepo.DeleteByPartnerIdAndKladrCodeLike(TestParnterId, "6300000800100");
            testRepo.DeleteByPartnerIdAndKladrCodeLike(TestParnterId, "6300300000200");
            testRepo.DeleteByPartnerIdAndKladrCodeLike(TestParnterId, "6300000102400");

            /* NOTE: Создаем три локации на один КЛАДР - 6300000800100 (село Кашпир), но разные нп 
             * Настоящий код 6300300000200 - 1004 км; 6300000102400 - 135 км */
            var del1 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "1004 км", DeliveryLocationStatus.KladrDuplication);
            var del2 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "135 км", DeliveryLocationStatus.KladrDuplication);
            var del3 = testRepo.Create(TestParnterId, 0, 1000, 50, "6300000800100", "Кашпир", DeliveryLocationStatus.KladrDuplication);

            // NOTE: Устанавливаем для второго правильный КЛАДР.
            SetDeliveryLocationKladr(del2.LocationId, "6300000102400");

            // NOTE: Проверяем что для первого и третьего статус НЕ изменился в ОК.
            var @params = new GetDeliveryLocationsParameters { PartnerId = TestParnterId, SearchTerm = "6300000800100" };
            var locationsResult = catalogAdminService.GetDeliveryLocations(@params, ApiSettings.ClientSiteUserName);

            Assert.IsNotNull(locationsResult);
            Assert.IsNotNull(locationsResult.DeliveryLocations);
            Assert.IsTrue(
                locationsResult.DeliveryLocations.All(x => x.Status == DeliveryLocationStatus.KladrDuplication),
                "Все c 6300000800100 должны остаться DeliveryLocationStatus.KladrDuplication");

            // NOTE: Устанавливаем для первого правильный КЛАДР.
            catalogAdminService.SetDeliveryLocationKladr(del1.LocationId, "6300300000200", ApiSettings.ClientSiteUserName);

            // NOTE: Проверяем что для третьего статус изменился в ОК.
            var nextLocationsResult = catalogAdminService.GetDeliveryLocations(@params, ApiSettings.ClientSiteUserName);

            Assert.IsNotNull(nextLocationsResult);
            Assert.IsNotNull(nextLocationsResult.DeliveryLocations);

            var loc = nextLocationsResult.DeliveryLocations.First();
            Assert.IsNotNull(loc);
            Assert.AreEqual(
                DeliveryLocationStatus.CorrectBinded,
                loc.Status,
                "Статус последней локации, в группе по КЛАДРу, должен измениться с DeliveryLocationStatus.KladrDuplication на DeliveryLocationStatus.CorrectBinded");

            // NOTE: Чистим
            testRepo.Delete(del1.LocationId);
            testRepo.Delete(del2.LocationId);
            testRepo.Delete(del3.LocationId);
        }

        private void SetDeliveryLocationKladr(int locationId, string kladr)
        {
            var res = catalogAdminService.SetDeliveryLocationKladr(locationId, kladr, ApiSettings.ClientSiteUserName);
            
            Assert.IsTrue(res.Success, res.ResultDescription);
        }

        [TestMethod]
        public void ShouldNotUpdateKladrWhenKladrUsedByAnother()
        {
            var testRepo = new TestDeliveryRatesRepository();
            testRepo.DeleteByPartnerIdAndKladrCodeLike(TestParnterId, "6300000800100");
            testRepo.DeleteByPartnerIdAndKladrCodeLike(TestParnterId, "6300300000200");

            var parameters = new GetDeliveryLocationsParameters { PartnerId = TestParnterId, };
            var locs = catalogAdminService.GetDeliveryLocations(parameters, ApiSettings.ClientSiteUserName);

            Assert.IsNotNull(locs);
            Assert.AreEqual(true, locs.Success);

            DeliveryRate t1 = null, t2 = null;
            TestDeliveryRatesRepository testDs = null;
            if (locs.DeliveryLocations.Count() < 2)
            {
                // NOTE: Создадим тестовые
                testDs = new TestDeliveryRatesRepository();

                t1 = testDs.Create(TestParnterId, 0, 1000, 50, "6300000800100");
                t2 = testDs.Create(TestParnterId, 0, 1000, 50, "6300300000200");
            }

            locs = catalogAdminService.GetDeliveryLocations(parameters, ApiSettings.ClientSiteUserName);

            var first = locs.DeliveryLocations[0];
            var usedKladr = locs.DeliveryLocations[1].Kladr;

            var result = catalogAdminService.SetDeliveryLocationKladr(first.Id, usedKladr, ApiSettings.ClientSiteUserName);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result.Success);

            // NOTE: Подчищаем
            if (testDs != null)
            {
                testDs.Delete(t1.Id);
                testDs.Delete(t2.Id);
            }
        }

        private static void DeletePartner(int partnerId)
        {
            // NOTE: Удаляем каталог
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                var cmdText = string.Format("DELETE FROM [prod].[PartnerProductCatalogs] WHERE [PartnerId] = @PartnerId");

                using (var sqlCmd = new SqlCommand(cmdText, conn))
                {
                    sqlCmd.Parameters.AddWithValue("PartnerId", partnerId);
                    sqlCmd.ExecuteNonQuery();
                }
            }

            new PartnerRepository().Delete(partnerId);
        }

        private static void AssertResult(ResultBase res)
        {
            Assert.IsNotNull(res);
            Assert.IsTrue(
                res.Success, string.Format("ResultCode:{0}ResultDescription:{1}", res.ResultCode, res.ResultDescription));
        }

        private void DeleteTestJob(string jobName)
        {
            const string SQL = @"DELETE FROM [dbo].[QRTZ_TRIGGERS] WHERE [TRIGGER_NAME] = '{0}'
DELETE FROM [dbo].[QRTZ_JOB_DETAILS] WHERE [JOB_NAME] = '{0}'";

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.Database.ExecuteSqlCommand(string.Format(SQL, jobName));
            }
        }

        private CreateProductParameters NewCreateProductParameters(int categoryId)
        {
            return new CreateProductParameters
                   {
                       UserId = TestDataStore.TestUserId,
                       PartnerId = TestDataStore.OzonPartnerId,
                       PartnerProductId = "test" + Guid.NewGuid(),
                       CategoryId = categoryId,
                       Name = "TestProduct",
                       PriceRUR = 1,
                       Description = "TestProduct descr",
                       Vendor = "vendor here",
                       Weight = 1,
                       Pictures = new[]
                                  {
                                      "picture 1", "picture 2"
                                  },
                       Param = new[]
                               {
                                   new ProductParam
                                   {
                                       Name = "nameHere",
                                       Unit = "unitHere",
                                       Value = "valueHelee"
                                   }
                               },
                       CurrencyId = "RUR"
                   };
        }

        private ProductCategory GetCategoryById(decimal categoryId)
        {
            var productCategory = this.categoriedDataSource.GetProductCategoryById((int)categoryId);

            Assert.IsNotNull(productCategory);

            return productCategory;
        }
    }
}
