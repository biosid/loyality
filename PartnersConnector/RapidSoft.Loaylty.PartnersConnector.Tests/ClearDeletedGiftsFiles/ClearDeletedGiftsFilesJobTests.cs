using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quartz;
using Quartz.Impl;
using RapidSoft.Loaylty.PartnersConnector.Common;
using RapidSoft.Loaylty.PartnersConnector.Common.Interfaces;
using RapidSoft.Loaylty.PartnersConnector.Interfaces;
using RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs;
using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.ClearDeletedGiftsFiles
{
    [TestClass]
    public class ClearDeletedGiftsFilesJobTests
    {
        public Mock<ICatalogAdminServiceProvider> CatalogAdminServiceProviderMock;
        public Mock<IFileSystem> FileSystemMock;
        public string BasePath = ConfigurationManager.AppSettings["GiftFilesPath"];

        [TestInitialize]
        public void Initialize()
        {
            var testProducts = new[]
            {
                new Product
                {
                    ProductId = "1"
                },
                new Product
                {
                    ProductId = "2"
                },
                new Product
                {
                    ProductId = "4"
                },
                new Product
                {
                    ProductId = "5"
                },
                new Product
                {
                    ProductId = "6"
                },
                new Product
                {
                    ProductId = "7"
                },
                new Product
                {
                    ProductId = "8"
                },
                new Product
                {
                    ProductId = "9"
                },
                new Product
                {
                    ProductId = "10"
                },
                new Product
                {
                    ProductId = "11"
                },
                new Product
                {
                    ProductId = "12"
                }
            };

            var testDirectories = new List<string>
            {
                "1",
                "3",
                "4",
                "8",
                "10",
                "11",
                "12"
            };

            CatalogAdminServiceProviderMock = new Mock<ICatalogAdminServiceProvider>();
            CatalogAdminServiceProviderMock
                    .Setup(x => x.SearchAllProducts(It.IsAny<string>(), It.IsAny<string[]>()))
                    .Returns(new SearchProductsResult
                    {
                        Success = true,
                        ResultCode = 0,
                        ResultDescription = null,
                        Products = testProducts
                    });

            FileSystemMock = new Mock<IFileSystem>();
            FileSystemMock
                .Setup(x => x.EnumerateDirectories(It.IsAny<string>()))
                .Returns(testDirectories);
            FileSystemMock
                .Setup(x => x.DeleteDirectory(It.IsAny<string>(), true))
                .Callback<string, bool>((path, isrecursive) =>
                {
                    var dirName = path.Replace(BasePath, "");
                    testDirectories.Remove(dirName);
                });

            JobBase.CatalogAdminServiceProviderBuilder = () => CatalogAdminServiceProviderMock.Object;
            JobBase.FileSystemBuilder = () => FileSystemMock.Object;
        }

        [TestMethod]
        public void ShouldClearDeletedGiftsFiles()
        {
            var scheduler = InitAndStartJob();

            var count = 0;
            while (FileSystemMock.Object.EnumerateDirectories(BasePath).Contains("3") || count < 5)
            {
                Thread.Sleep(2000);
                count++;
            }

            Assert.IsFalse(FileSystemMock.Object.EnumerateDirectories(BasePath).Contains("3"));
            Assert.IsTrue(FileSystemMock.Object.EnumerateDirectories(BasePath).Contains("1"));
            CatalogAdminServiceProviderMock.Verify(x => x.SearchAllProducts(It.IsAny<string>(), It.IsAny<string[]>()), Times.Exactly(2));

            scheduler.Clear();
        }

        private static IScheduler InitAndStartJob()
        {
            var scheduler = GetScheduler();

            var job = JobBuilder.Create<ClearDeletedGiftsFilesJob>()
                .WithIdentity("Удаление файлов товаров, отсутствующих в каталоге", "Удаление файлов товаров, отсутствующих в каталоге")
                .RequestRecovery(false)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("Удаление файлов товаров, отсутствующих в каталоге", "Удаление файлов товаров, отсутствующих в каталоге")
                .Build();

            scheduler.ScheduleJob(job, new Quartz.Collection.HashSet<ITrigger> { trigger }, true);

            scheduler.Start();

            return scheduler;
        }

        private static IScheduler GetScheduler()
        {
            var shedulerFactory = new StdSchedulerFactory();
            return shedulerFactory.GetScheduler();
        }
    }
}
