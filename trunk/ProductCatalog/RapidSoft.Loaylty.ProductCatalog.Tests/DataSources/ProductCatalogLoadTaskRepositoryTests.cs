using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ProductCatalogLoadTaskRepositoryTests
    {
        private int partnerId = TestDataStore.OzonPartnerId;

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ShouldCreateProductImportTask()
        {
            var fileUrl = @"C:\Test\Test.xml";
            var userId = TestDataStore.TestUserId;

            var task = new ProductImportTask(partnerId, fileUrl, userId);

            var repo = new ImportTaskRepository();

            var saved = repo.SaveProductImportTask(task);

            Assert.IsNotNull(saved);
            Assert.AreNotEqual(0, saved.Id);
        }

        [TestMethod]
        public void ShouldReturnProductImportTaskById()
        {
            var fileUrl = @"C:\Test\Test.xml";
            var userId = TestDataStore.TestUserId;

            var task = new ProductImportTask(partnerId, fileUrl, userId);

            var repo = new ImportTaskRepository();

            var saved = repo.SaveProductImportTask(task);

            var getted = repo.GetProductImportTask(saved.Id);

            Assert.IsNotNull(getted);
        }

        [TestMethod]
        public void ShouldUpdateProductImportTask()
        {
            var fileUrl = @"C:\Test\Test.xml";
            var userId = TestDataStore.TestUserId;

            var task = new ProductImportTask(partnerId, fileUrl, userId);

            task.Status = ImportTaskStatuses.Waiting;

            var repo = new ImportTaskRepository();

            var saved = repo.SaveProductImportTask(task);

            var taskForUpdate = new ProductImportTask(partnerId, fileUrl, userId);

            taskForUpdate.Id = saved.Id;
            taskForUpdate.Status = ImportTaskStatuses.Completed;
            taskForUpdate.CountSuccess = 5;
            taskForUpdate.CountFail = 10;
            taskForUpdate.StartDateTime = DateTime.Now.AddDays(-5);
            taskForUpdate.EndDateTime = DateTime.Now.AddDays(-1);

            repo.SaveProductImportTask(taskForUpdate);

            var updated = repo.GetProductImportTask(saved.Id);

            Assert.IsNotNull(updated);
            Assert.AreEqual(updated.PartnerId, taskForUpdate.PartnerId);
            Assert.AreEqual(updated.Status, taskForUpdate.Status);
            Assert.AreEqual(updated.CountSuccess, taskForUpdate.CountSuccess);
            Assert.AreEqual(updated.CountFail, taskForUpdate.CountFail);
            Assert.AreEqual(updated.StartDateTime.Value.ToString("dd.MM.yyyy HH:mm"), taskForUpdate.StartDateTime.Value.ToString("dd.MM.yyyy HH:mm"));
            Assert.AreEqual(updated.EndDateTime.Value.ToString("dd.MM.yyyy HH:mm"), taskForUpdate.EndDateTime.Value.ToString("dd.MM.yyyy HH:mm"));
            Assert.AreEqual(updated.FileUrl, taskForUpdate.FileUrl);
        }

        [TestMethod]
        public void ShouldReturnPageOfProductImportTasks()
        {
            var repo = new ImportTaskRepository();

            var fileUrl = @"C:\Test\Test.xml";
            var userId = TestDataStore.TestUserId;

            var task = new ProductImportTask(partnerId, fileUrl, userId);
            task.StartDateTime = DateTime.Now.AddDays(-5);
            repo.SaveProductImportTask(task);

            task = new ProductImportTask(partnerId, fileUrl, userId);
            task.StartDateTime = DateTime.Now;
            repo.SaveProductImportTask(task);

            task = new ProductImportTask(partnerId, fileUrl, userId);
            task.InsertedDate = DateTime.Now.AddHours(-1);
            repo.SaveProductImportTask(task);

            task = new ProductImportTask(partnerId, fileUrl, userId);
            repo.SaveProductImportTask(task);

            task = new ProductImportTask(partnerId, fileUrl, userId);
            task.StartDateTime = DateTime.Now.AddDays(-500);
            repo.SaveProductImportTask(task);

            task = new ProductImportTask(TestDataStore.QueuedCommitPartnerId, fileUrl, userId);
            task.StartDateTime = DateTime.Now.AddDays(-500);
            repo.SaveProductImportTask(task);

            var page = repo.GetPageProductImportTask(partnerId, 0, 4, true);

            Assert.IsNotNull(page);
            Assert.AreEqual(4, page.Count);
            var first = page[0];
            var second = page[1];
            Assert.IsTrue(first.InsertedDate >= second.InsertedDate);
            Assert.IsNotNull(page.TotalCount);
            Assert.IsTrue(page.All(x => x.PartnerId == partnerId));

            page = repo.GetPageProductImportTask(null, null, null, false);

            Assert.IsNotNull(page);
            Assert.IsNull(page.TotalCount);

            var notStarted = page.SkipWhile(x => !x.StartDateTime.HasValue).Take(2).ToArray();

            first = notStarted[0];
            second = notStarted[1];

            Assert.IsTrue(first.InsertedDate >= second.InsertedDate, "Задачи которые уже выполняются/выполнились должны быть отсортированы");

            Assert.IsTrue(page.Any(x => x.PartnerId == partnerId));
            Assert.IsTrue(page.Any(x => x.PartnerId == TestDataStore.QueuedCommitPartnerId), "Должен быть минимум один таск для второго тестового партнера");
        }

        [TestMethod]
        public void ShouldReturnPageOfDeliveryRateImportTasks()
        {
            var repo = new ImportTaskRepository();

            var tasks = repo.GetPageDeliveryRateImportTask(null, 0, 1000, true);

            Assert.IsNotNull(tasks);
            Assert.IsNotNull(tasks.TotalCount);

            var f = tasks.FirstOrDefault();
            if (f == null)
            {
                return;
            }

            var tasksByPartners = repo.GetPageDeliveryRateImportTask(f.PartnerId, 0, 1000, null);

            Assert.IsNotNull(tasksByPartners);
            Assert.IsNull(tasksByPartners.TotalCount);
            Assert.IsTrue(tasksByPartners.All(x => x.PartnerId == f.PartnerId));
        }
    }
}
