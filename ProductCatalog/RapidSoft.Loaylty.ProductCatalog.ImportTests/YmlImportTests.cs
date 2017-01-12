using System;
using System.Linq;
using System.Threading;

using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidSoft.Loaylty.ProductCatalog.API.Entities;
using RapidSoft.Loaylty.ProductCatalog.DataSources;
using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
using RapidSoft.Loaylty.ProductCatalog.Import;
using RapidSoft.Loaylty.ProductCatalog.Tests;
using RapidSoft.YML;
using Vtb24.Common.Configuration;

namespace RapidSoft.Loaylty.ProductCatalog.ImportTests
{
    [TestClass]
    public class YmlImportTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            ProductCatalogDB.DropAllProductCatalogs();
        }
        
        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\vtb.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsForOzonWithCheckParamDuplicates()
        {
            const string FILE_PATH = @"TestCatalogXmls\vtb.xml";

            // NOTE: Проверяя дубликаты параметров и пропуская такие продукты
            var task = new ProductImportTask(
                TestDataStore.OzonPartnerId,
                FILE_PATH,
                "import",
                WeightProcessTypes.WeightRequired,
                ParamsProcessTypes.NotAcceptParamDuplicate);
            var repo = new ImportTaskRepository();

            var saved = repo.SaveProductImportTask(task);

            var importer = CreateImporter(saved.Id);

            importer.Execute();

            var processedTask = repo.GetProductImportTask(saved.Id);

            Assert.IsNotNull(processedTask);
            Assert.AreEqual(ImportTaskStatuses.Completed, processedTask.Status);
            Assert.IsNotNull(processedTask.Status);
            Assert.IsNotNull(processedTask.EndDateTime);
            Assert.AreNotEqual(0, processedTask.CountSuccess);
            Assert.AreEqual(26, processedTask.CountFail);

            InitTestPartner(TestDataStore.OzonPartnerId, "7700000000000");
        }

        private static CatalogImporter CreateImporter(int taskId)
        {
            return new CatalogImporter(taskId, false, logEmailSender: new StubLogEmailSender());
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\vtb.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsForOzonWithoutCheckParamDuplicates()
        {
            const string FILE_PATH = @"TestCatalogXmls\vtb.xml";

            // NOTE: НЕ проверяя дубликаты параметров и пропуская такие продукты
            var task = new ProductImportTask(
                TestDataStore.OzonPartnerId,
                FILE_PATH,
                "import",
                WeightProcessTypes.WeightRequired,
                ParamsProcessTypes.AcceptParamDuplicate);
            var repo = new ImportTaskRepository();

            var saved = repo.SaveProductImportTask(task);

            var importer = CreateImporter(saved.Id);

            importer.Execute();

            var processedTask = repo.GetProductImportTask(saved.Id);

            Assert.IsNotNull(processedTask);
            Assert.AreEqual(ImportTaskStatuses.Completed, processedTask.Status);
            Assert.IsNotNull(processedTask.Status);
            Assert.IsNotNull(processedTask.EndDateTime);
            Assert.AreNotEqual(0, processedTask.CountSuccess);
            Assert.AreEqual(1, processedTask.CountFail);

            InitTestPartner(TestDataStore.OzonPartnerId, "7700000000000");
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\div_soft_with_weight.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsForBatchPartner()
        {
            const string FILE_PATH = @"TestCatalogXmls\div_soft_with_weight.xml";

            var task = new ProductImportTask(TestDataStore.QueuedCommitPartnerId, FILE_PATH, "import", WeightProcessTypes.WeightRequired);
            var repo = new ImportTaskRepository();

            var saved = repo.SaveProductImportTask(task);

            var importer = CreateImporter(saved.Id);

            importer.Execute();

            var processedTask = repo.GetProductImportTask(saved.Id);

            Assert.IsNotNull(processedTask);
            Assert.AreEqual(ImportTaskStatuses.Completed, processedTask.Status);
            Assert.IsNotNull(processedTask.Status);
            Assert.IsNotNull(processedTask.EndDateTime);
            Assert.AreEqual(2, processedTask.CountSuccess);
            Assert.AreEqual(1, processedTask.CountFail);
            InitTestPartner(TestDataStore.QueuedCommitPartnerId, "7700000000000");

            var activeCatalog = new ProductCatalogsDataSource().GetActiveCatalog(TestDataStore.QueuedCommitPartnerId);

            var key = activeCatalog.Key;

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                const string PRODUCTS_SQL = @"
SELECT COUNT(*) FROM [prod].[Products_{0}]
WHERE [PartnerProductId] = 'withWeight'
";
                var countProducts = ctx.Database.SqlQuery<int>(string.Format(PRODUCTS_SQL, key)).Single();

                // Assert.AreEqual(1, countErrors);
                Assert.AreEqual(1, countProducts);
            }
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\empty_product.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\div_soft_1_product.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsForPartner3()
        {
            var filePath = @"TestCatalogXmls\empty_product.xml";
            var task = new ProductImportTask(TestDataStore.NoDeliveryRatesPartnerID, filePath, "import", WeightProcessTypes.DefaultWeight);
            var repo = new ImportTaskRepository();

            var saved = repo.SaveProductImportTask(task);

            var importer = CreateImporter(saved.Id);

            importer.Execute();

            var processedTask = repo.GetProductImportTask(saved.Id);

            Assert.IsNotNull(processedTask);
            Assert.AreEqual(ImportTaskStatuses.Completed, processedTask.Status);
            Assert.IsNotNull(processedTask.Status);
            Assert.IsNotNull(processedTask.EndDateTime);
            Assert.AreNotEqual(0, processedTask.CountSuccess);

            Thread.Sleep(1000);

            filePath = @"TestCatalogXmls\div_soft_1_product.xml";

            task = new ProductImportTask(TestDataStore.NoDeliveryRatesPartnerID, filePath, "import", WeightProcessTypes.DefaultWeight);
            repo = new ImportTaskRepository();

            saved = repo.SaveProductImportTask(task);

            importer = CreateImporter(saved.Id);

            importer.Execute();

            processedTask = repo.GetProductImportTask(saved.Id);

            Assert.IsNotNull(processedTask);
            Assert.AreEqual(ImportTaskStatuses.Completed, processedTask.Status);
            Assert.IsNotNull(processedTask.Status);
            Assert.IsNotNull(processedTask.EndDateTime);
            Assert.AreNotEqual(0, processedTask.CountSuccess);

            var productRepo = new ProductsRepository();

            var porudcts = productRepo.GetByPartnerId(TestDataStore.NoDeliveryRatesPartnerID);
            var product = porudcts.First();

            Thread.Sleep(1000);

            task = new ProductImportTask(TestDataStore.NoDeliveryRatesPartnerID, filePath, "import", WeightProcessTypes.DefaultWeight);
            repo = new ImportTaskRepository();

            saved = repo.SaveProductImportTask(task);

            importer = CreateImporter(saved.Id);

            importer.Execute();

            var product2 = productRepo.GetByPartnerId(TestDataStore.NoDeliveryRatesPartnerID).First(x => x.PartnerProductId == product.PartnerProductId);

            Assert.AreEqual(product2.InsertedDate, product.InsertedDate);
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\div_soft_1_product.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsForPartner4()
        {
            const string FILE_PATH = @"TestCatalogXmls\div_soft_1_product.xml";

            var task = new ProductImportTask(TestDataStore.DeactivatedPartnerID, FILE_PATH, "import", WeightProcessTypes.DefaultWeight);
            var repo = new ImportTaskRepository();

            var saved = repo.SaveProductImportTask(task);

            var importer = CreateImporter(saved.Id);

            importer.Execute();

            var processedTask = repo.GetProductImportTask(saved.Id);

            Assert.IsNotNull(processedTask);
            Assert.AreEqual(ImportTaskStatuses.Completed, processedTask.Status);
            Assert.IsNotNull(processedTask.Status);
            Assert.IsNotNull(processedTask.EndDateTime);
            Assert.AreNotEqual(0, processedTask.CountSuccess);
            InitTestPartner(TestDataStore.DeactivatedPartnerID, "4700000200000");
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\div_soft_1_product.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\div_soft_2_product.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsAndCalculateBasePriceRur()
        {
            var site505EnableActionPrice = FeaturesConfiguration.Instance.Site505EnableActionPrice;
            FeaturesConfiguration.Instance.Site505EnableActionPrice = true;

            var partnerId = CreateTestPartner("ShouldImportProductsAndCalculateBasePriceRur",
                                              new[] { Tuple.Create("/Обучающие программы/", 486) });

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\div_soft_1_product.xml");

            var productRepo = new ProductsRepository();

            var porudcts = productRepo.GetByPartnerId(partnerId);
            var productProjection = porudcts.First();
            var product = (new ProductsDataSource()).GetById(productProjection.ProductId, partnerId);

            Assert.IsNull(product.BasePriceRUR);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\div_soft_2_product.xml");


            productRepo = new ProductsRepository();

            porudcts = productRepo.GetByPartnerId(partnerId);
            productProjection = porudcts.First();
            product = (new ProductsDataSource()).GetById(productProjection.ProductId, partnerId);

            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(product.BasePriceRUR, 139);

            FeaturesConfiguration.Instance.Site505EnableActionPrice = site505EnableActionPrice;
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\import_baseprice_products1.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\import_baseprice_products2.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsAndCalculateBasePriceRurWithChangedPrices()
        {
            var site505EnableActionPrice = FeaturesConfiguration.Instance.Site505EnableActionPrice;
            FeaturesConfiguration.Instance.Site505EnableActionPrice = true;

            var partnerId = CreateTestPartner("ShouldImportProductsAndCalculateBasePriceRurWithChangedPrices",
                                              new[] { Tuple.Create("/Обучающие программы/", 486) });

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_baseprice_products1.xml");

            var productRepo = new ProductsRepository();

            var porudcts = productRepo.GetByPartnerId(partnerId);
            var productProjection = porudcts.First();
            var product = (new ProductsDataSource()).GetById(productProjection.ProductId, partnerId);

            Assert.IsNotNull(product.BasePriceRUR);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_baseprice_products2.xml");

            productRepo = new ProductsRepository();

            porudcts = productRepo.GetByPartnerId(partnerId);

            productProjection = porudcts.First();
            product = (new ProductsDataSource()).GetById(productProjection.ProductId, partnerId);
            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(600, product.BasePriceRUR);

            productProjection = porudcts.Skip(1).First();
            product = (new ProductsDataSource()).GetById(productProjection.ProductId, partnerId);
            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(500, product.BasePriceRUR);

            productProjection = porudcts.Last();
            product = (new ProductsDataSource()).GetById(productProjection.ProductId, partnerId);
            Assert.IsNull(product.BasePriceRUR);

            FeaturesConfiguration.Instance.Site505EnableActionPrice = site505EnableActionPrice;
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\div_soft_baseprice_product.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsWithBasePriceRur()
        {
            var site505EnableActionPrice = FeaturesConfiguration.Instance.Site505EnableActionPrice;
            FeaturesConfiguration.Instance.Site505EnableActionPrice = true;

            var partnerId = CreateTestPartner("ShouldImportProductsWithBasePriceRur",
                                              new[] { Tuple.Create("/Обучающие программы/", 486) });

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\div_soft_baseprice_product.xml");

            var productRepo = new ProductsRepository();

            var products = productRepo.GetByPartnerId(partnerId);
            var productProjection = products.First();
            var product = (new ProductsDataSource()).GetById(productProjection.ProductId, partnerId);

            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(product.BasePriceRUR, 130);

            FeaturesConfiguration.Instance.Site505EnableActionPrice = site505EnableActionPrice;
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\div_soft_with_isDeliveredByEmail.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsWithIsDeliveredByEmail()
        {
            var partnerId = CreateTestPartner("ShouldImportProductsWithIsDeliveredByEmail",
                                              new[] { Tuple.Create("/Обучающие программы/", 486) });

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\div_soft_with_isDeliveredByEmail.xml");

            var productRepo = new ProductsRepository();

            var products = productRepo.GetByPartnerId(partnerId);
            var productProjection = products.FirstOrDefault(p => p.PartnerProductId == "withIsDeliveredByEmail");

            Assert.IsNotNull(productProjection);

            var product = (new ProductsDataSource()).GetById(productProjection.ProductId, partnerId);

            Assert.IsNotNull(product);
            Assert.AreEqual(product.IsDeliveredByEmail, true);
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\div_soft_with_isDeliveredByEmail.xml", @"TestCatalogXmls")]
        public void ShouldImportProductsWithoutIsDeliveredByEmail()
        {
            var partnerId = CreateTestPartner("ShouldImportProductsWithoutIsDeliveredByEmail",
                                              new[] { Tuple.Create("/Обучающие программы/", 486) });

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\div_soft_with_isDeliveredByEmail.xml");

            var productRepo = new ProductsRepository();

            var products = productRepo.GetByPartnerId(partnerId);
            var productProjection = products.FirstOrDefault(p => p.PartnerProductId == "withoutIsDeliveredByEmail");

            Assert.IsNotNull(productProjection);

            var product = (new ProductsDataSource()).GetById(productProjection.ProductId, partnerId);

            Assert.IsNotNull(product);
            Assert.AreEqual(product.IsDeliveredByEmail, false);
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\import_nobaseprice_discount1.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\import_nobaseprice_discount2.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\import_nobaseprice_discount3.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\import_nobaseprice_discount4.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\import_nobaseprice_discount5.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\import_nobaseprice_discount6.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\import_nobaseprice_discount7.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\import_nobaseprice_discount8.xml", @"TestCatalogXmls")]
        [DeploymentItem(@"TestCatalogXmls\import_nobaseprice_discount9.xml", @"TestCatalogXmls")]
        public void ShouldImportAndCorrectlySetAutodiscount()
        {
            var site505EnableActionPrice = FeaturesConfiguration.Instance.Site505EnableActionPrice;
            FeaturesConfiguration.Instance.Site505EnableActionPrice = true;

            var partnerId = CreateTestPartner("ShouldImportAndCorrectlySetAutodiscount",
                                              new[] { Tuple.Create("/Обучающие программы/", 486) });

            var productId = string.Format("{0}_{1}", partnerId, "gift1");

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_nobaseprice_discount1.xml");

            var product = (new ProductsDataSource()).GetById(productId, partnerId);
            Assert.IsNull(product.BasePriceRUR);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_nobaseprice_discount2.xml");

            product = (new ProductsDataSource()).GetById(productId, partnerId);
            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(product.BasePriceRUR.Value, 100m);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_nobaseprice_discount3.xml");

            product = (new ProductsDataSource()).GetById(productId, partnerId);
            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(product.BasePriceRUR.Value, 100m);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_nobaseprice_discount4.xml");

            product = (new ProductsDataSource()).GetById(productId, partnerId);
            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(product.BasePriceRUR.Value, 100m);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_nobaseprice_discount5.xml");

            product = (new ProductsDataSource()).GetById(productId, partnerId);
            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(product.BasePriceRUR.Value, 100m);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_nobaseprice_discount6.xml");

            product = (new ProductsDataSource()).GetById(productId, partnerId);
            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(product.BasePriceRUR.Value, 80m);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_nobaseprice_discount7.xml");

            product = (new ProductsDataSource()).GetById(productId, partnerId);
            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(product.BasePriceRUR.Value, 80m);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_nobaseprice_discount8.xml");

            product = (new ProductsDataSource()).GetById(productId, partnerId);
            Assert.IsNotNull(product.BasePriceRUR);
            Assert.AreEqual(product.BasePriceRUR.Value, 80m);

            Thread.Sleep(1000);
            ImportProducts(partnerId, @"TestCatalogXmls\import_nobaseprice_discount9.xml");

            product = (new ProductsDataSource()).GetById(productId, partnerId);
            Assert.IsNull(product.BasePriceRUR);

            FeaturesConfiguration.Instance.Site505EnableActionPrice = site505EnableActionPrice;
        }

        private static int CreateTestPartner(string name, Tuple<string, int>[] categoriesMappings)
        {
            var partnerRepository = new PartnerRepository();
            var partner = partnerRepository.GetByName(name);

            if (partner == null)
            {

                // ReSharper disable UnusedVariable
                var productsDataSource = new ProductsDataSource();
                // ReSharper restore UnusedVariable

                partner = new Partner
                {
                    InsertedUserId = TestDataStore.TestUserId,
                    Name = name,
                    Type = PartnerType.Offline,
                    Status = PartnerStatus.Active,
                    ThrustLevel = PartnerThrustLevel.High,
                    Description = "",
                    InsertedDate = DateTime.Now,
                    CarrierId = null,
                    IsCarrier = false
                };

                partner = partnerRepository.CreateOrUpdate(TestDataStore.TestUserId, partner);

                var dateKey = LoyaltyDBSpecification.GetDateKey(DateTime.Now);
                var importKey = ProductCatalogsDataSource.InsertCatalog(partner.Id, dateKey);
                ProductsDataSource.CreateProductsTable(importKey, partner.Id);
                ProductsDataSource.CreateProductsTableConstraints(importKey, partner.Id);
                ProductCatalogsDataSource.SetActiveCatalog(partner.Id, importKey);
            }

            var partnerProductCategoryLinkRepository = new PartnerProductCateroryLinkRepository();
            foreach (var item in categoriesMappings)
            {
                partnerProductCategoryLinkRepository.SetPartnerProductCateroryLink(new PartnerProductCategoryLink
                {
                    PartnerId = partner.Id,
                    Paths = new[] { new CategoryPath(true, item.Item1) },
                    ProductCategoryId = item.Item2
                }, TestDataStore.TestUserId);
            }

            var categoryPermissionRepository = new CategoryPermissionRepository();
            categoryPermissionRepository.Save(
                TestDataStore.TestUserId,
                categoriesMappings.Select(item => item.Item2)
                                  .Distinct()
                                  .Select(id => new CategoryPermission(partner.Id, id, TestDataStore.TestUserId))
                                  .ToList());

            return partner.Id;
        }

        private static void ImportProducts(int partnerId, string filePath)
        {
            var repo = new ImportTaskRepository();

            var newTask = new ProductImportTask(partnerId, filePath, "import", WeightProcessTypes.DefaultWeight);
            var savedTask = repo.SaveProductImportTask(newTask);

            var importer = CreateImporter(savedTask.Id);
            importer.Execute();

            var processedTask = repo.GetProductImportTask(savedTask.Id);

            Assert.IsNotNull(processedTask);
            Assert.AreEqual(ImportTaskStatuses.Completed, processedTask.Status);
            Assert.IsNotNull(processedTask.Status);
            Assert.IsNotNull(processedTask.EndDateTime);
            Assert.AreNotEqual(0, processedTask.CountSuccess);
        }

        [TestMethod]
        [DeploymentItem(@"TestCatalogXmls\forMappingTests.xml", @"TestCatalogXmls")]
        public void ShouldCorrectMapName()
        {
            // ReSharper disable EmptyGeneralCatchClause
            // NOTE: Чтобы подцепился маппинг определенный в статик конструкторе
            try
            {
                ProductImportSteps.LoadFile(null, null, null);
            }
            catch
            {
            }
            // ReSharper restore EmptyGeneralCatchClause

            var ymlReader = new YmlReader(@"TestCatalogXmls\forMappingTests.xml");
            var offers = ymlReader.Offers.ToList();

            Assert.AreEqual(4, offers.Count(x => x != null));

            var products = offers.Select(Mapper.Map<GenericOffer, Product>).ToList();

            Assert.AreEqual("Name test1", products.First(x => x.PartnerProductId == "test1").Name);
            Assert.AreEqual("Title test2", products.First(x => x.PartnerProductId == "test2").Name);
            Assert.AreEqual("Vendor test3 model test3", products.First(x => x.PartnerProductId == "test3").Name);
            Assert.AreEqual("Vendor test4 model test4", products.First(x => x.PartnerProductId == "test4").Name);
        }
        
        private static void InitTestPartner(int partnerId, string kladr)
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                ctx.Database.ExecuteSqlCommand(Sqls.InsertDeliveryRatesIfNeedFormat, partnerId, kladr);
            }
        }
    }
}
