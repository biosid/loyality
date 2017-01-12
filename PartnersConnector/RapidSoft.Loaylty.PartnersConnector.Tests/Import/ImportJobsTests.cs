namespace RapidSoft.Loaylty.PartnersConnector.Tests.Import
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Quartz;
    using Quartz.Collection;

    using Interfaces;

    using Interfaces.Entities;

    using QuarzTasks;

    using QuarzTasks.Jobs;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Тесты")]
    [TestClass]
    public class ImportJobsTests
    {
        [TestMethod]
        public void ShouldExecuteImportYmlJob()
        {
            // NOTE: Переменные процесса
            const int PartnerId = TestHelper.TestPartnerID;
            var remoteYmlUrl = Helper.StartHttpListener();
            var localFilePath = Path.GetTempFileName();
            File.Delete(localFilePath);
            const string LocalFileUrl = "web-путь";

            // NOTE: Заглушка 
            var mock = new Mock<ICatalogAdminServiceProvider>();
            var retVal = new ImportProductsFromYmlResult
                             {
                                 ResultCode = 0,
                                 ResultDescription = null,
                                 Success = true,
                                 TaskId = -500
                             };
            mock.Setup(x => x.ImportProductsFromYmlHttp(PartnerId, LocalFileUrl, It.IsAny<string>())).Returns(retVal);

            ImportJobBase.CatalogAdminServiceProviderBuilder = () => mock.Object;

            // NOTE: Ставим job в работу.
            var scheduler = Helper.GetScheduler();

            var job = JobBuilder.Create<ImportYmlJob>()
                .WithIdentity("Импорт каталога тестового партнера", "Импорт каталога")
                .RequestRecovery(false)
                .Build();

            job.JobDataMap.Put(DataKeys.PartnerId, PartnerId);
            job.JobDataMap.Put(DataKeys.RemoteFileUrl, remoteYmlUrl);
            job.JobDataMap.Put(DataKeys.LocalFilePath, localFilePath);
            job.JobDataMap.Put(DataKeys.LocalFileUrl, LocalFileUrl);

            var trigger = TriggerBuilder.Create()
                .WithIdentity("Импорт каталога тестового партнера", "Импорт каталога")
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, new HashSet<ITrigger> { trigger }, true);

            // NOTE: Запускаем scheduler
            scheduler.Start();

            var count = 0;

            // NOTE: Ждем пока не появится файл, либо 10 секунд (5 * 2000 мс)
            while (!File.Exists(localFilePath) || count < 5)
            {
                Thread.Sleep(2000);
                count++;
            }

            Assert.IsTrue(File.Exists(localFilePath));

            mock.Verify(x => x.ImportProductsFromYmlHttp(PartnerId, LocalFileUrl, It.IsAny<string>()), Times.Once());

            Helper.StopHttpListener();
            File.Delete(localFilePath);
        }

        [TestMethod]
        public void ShouldExecuteImportDeliveryRatesOzonJob()
        {
            // NOTE: Переменные процесса
            var remoteYmlUrl = Helper.StartHttpListener();
            var localFilePath = Path.GetTempFileName();
            File.Delete(localFilePath);
            const string LocalFileUrl = "web-путь";

            // NOTE: Заглушка 
            var mock = new Mock<ICatalogAdminServiceProvider>();
            var retVal = new ImportDeliveryRatesResult
                             {
                                 ResultCode = 0,
                                 ResultDescription = null,
                                 Success = true,
                                 JobId = "-500"
                             };
            mock.Setup(x => x.ImportDeliveryRatesHttp(It.IsAny<int>(), LocalFileUrl, It.IsAny<string>())).Returns(retVal);

            ImportJobBase.CatalogAdminServiceProviderBuilder = () => mock.Object;

            // NOTE: Ставим job в работу.
            var scheduler = Helper.GetScheduler();

            var job = JobBuilder.Create<ImportDeliveryRatesOzonJob>()
                .WithIdentity("Импорт тарифов Озона", "Импорт тарифов Озона")
                .RequestRecovery(false)
                .Build();

            job.JobDataMap.Put(DataKeys.RemoteFileUrl, remoteYmlUrl);
            job.JobDataMap.Put(DataKeys.LocalFilePath, localFilePath);
            job.JobDataMap.Put(DataKeys.LocalFileUrl, LocalFileUrl);

            var trigger = TriggerBuilder.Create()
                .WithIdentity("Импорт тарифов Озона", "Импорт тарифов Озона")
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, new HashSet<ITrigger> { trigger }, true);

            // NOTE: Запускаем scheduler
            scheduler.Start();

            var count = 0;

            // NOTE: Ждем пока не появится файл, либо 10 секунд (5 * 2000 мс)
            while (!File.Exists(localFilePath) || count < 5)
            {
                Thread.Sleep(2000);
                count++;
            }

            Assert.IsTrue(File.Exists(localFilePath));

            mock.Verify(x => x.ImportDeliveryRatesHttp(It.IsAny<int>(), LocalFileUrl, It.IsAny<string>()), Times.Once());

            Helper.StopHttpListener();
            File.Delete(localFilePath);
        }
    }
}
