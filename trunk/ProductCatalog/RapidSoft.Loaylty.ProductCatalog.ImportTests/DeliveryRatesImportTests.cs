namespace RapidSoft.Loaylty.ProductCatalog.ImportTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Etl.Logging;
    using Etl.Runtime.Agents;
    using Etl.Runtime.Agents.Sql;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Import;
    using RapidSoft.Loaylty.ProductCatalog.Tests.DataSources;

    using Services;

    using Settings;

    using Tests;

    [TestClass]
    public class DeliveryRatesImportTests
    {
        private readonly string sql = @"
SELECT COUNT(*) AS [Count],[Status]
  FROM [prod].[BUFFER_DeliveryRates]
GROUP BY [Status]";

        private int partnerIdNotOzon;
        private int partnerIdOzon;

        private int partnerIdOzonAsSimple;
        private string userId = TestDataStore.TestUserId;

        #region Additional test attributes
        [TestInitialize]
        public void MyTestInitialize()
        {
            var partnerRepo = new PartnerRepository();

            var partnerNotOzon = new Partner
                          {
                              Name = "Тестовый " + Guid.NewGuid(),
                              InsertedDate = DateTime.Now,
                              InsertedUserId = "FSY"
                          };

            var createdNotOzon = partnerRepo.CreateOrUpdate(userId, partnerNotOzon, settings: null);
            this.partnerIdNotOzon = createdNotOzon.Id;

            var partnerOzon = new Partner
                              {
                                  Name = "Тестовый " + Guid.NewGuid(),
                                  InsertedDate = DateTime.Now,
                                  InsertedUserId = "FSY"
                              };

            var createdOzon = partnerRepo.CreateOrUpdate(userId, partnerOzon, settings: null);
            partnerRepo.SetSetting(partnerOzon.Id, PartnerSettingsExtension.ImportDeliveryRatesEtlPackageId, "777ff1a8-3fbf-4127-96d3-70a0fa7fd05c");
            this.partnerIdOzon = createdOzon.Id;

            var partnerOzonAsSimple = new Partner
                                      {
                                          Name = "Тестовый " + Guid.NewGuid(),
                                          InsertedDate = DateTime.Now,
                                          InsertedUserId = "FSY"
                                      };

            var createdOzonAsSimple = partnerRepo.CreateOrUpdate(userId, partnerOzonAsSimple, settings: null);
            this.partnerIdOzonAsSimple = createdOzonAsSimple.Id;
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            var deliveryRateRepo = new TestDeliveryRatesRepository();
            deliveryRateRepo.DeleteByPartnerId(this.partnerIdNotOzon);
            deliveryRateRepo.DeleteByPartnerId(this.partnerIdOzon);
            deliveryRateRepo.DeleteByPartnerId(this.partnerIdOzonAsSimple);

            var partnerRepo = new PartnerRepository();

            partnerRepo.Delete(this.partnerIdNotOzon);
            partnerRepo.Delete(this.partnerIdOzon);
            partnerRepo.Delete(this.partnerIdOzonAsSimple);
        }
        #endregion

        [TestMethod]
        [DeploymentItem(@"TestDeliveryRatesCsv\deliveryRates.csv", "TestDeliveryRatesCsv")]
        public void ShouldImportDeliveryRates()
        {
            //// NOTE: Заполняем тариф бредом, тестируемый процесс должен затереть бред
            var repo = new TestDeliveryRatesRepository();
            for (var i = 0; i < 2; i++)
            {
                var dr = new DeliveryRate
                             {
                                 Kladr = "770000000000" + i,
                                 MaxWeightGram = 0,
                                 MinWeightGram = 0,
                                 PartnerId = this.partnerIdNotOzon,
                                 PriceRUR = 0m
                             };
                repo.Create(dr);
            }

            // NOTE: Создаем существующую локацию
            var exist = new DeliveryRate
            {
                LocationName = "Существующая локация",
                Kladr = "7900500007900",
                MaxWeightGram = 0,
                MinWeightGram = 0,
                PartnerId = this.partnerIdNotOzon,
                PriceRUR = 0m
            };

            repo.Create(exist);

            List<ImportStatus> before, after;

            // NOTE: Запоминает кол-во записей в буферной таблице
            using (var ctx = new LoyaltyDBEntities())
            {
                before = ctx.Database.SqlQuery<ImportStatus>(sql).ToList();
            }

            // NOTE: Запускаем сервер, что отдаст файл
            // var remoteFileUrl = "http://localhost/Test/" + "testOzonDelivery_2013_07_15-2.csv";
            var remoteFileUrl = Helper.StartHttpListener(@".\TestDeliveryRatesCsv", "deliveryRates.csv");

            var session = CreateWaitSession();

            var deliveryRatesImporter = CreteImporter(remoteFileUrl, session.EtlSessionId);

            var sessionId = deliveryRatesImporter.Execute();

            var rates = repo.GetByPartnerId(this.partnerIdNotOzon);
            Console.WriteLine(rates.Count);
            Assert.IsNotNull(rates);

            Assert.IsTrue(rates.Count > 0, "Должны загрузить хоть что-то");

            var expRatesCount = 16;
            Assert.AreEqual(expRatesCount, rates.Count, string.Format("Для партнера тарифы доставки должны перезаписаться и остаться только {0} записей", expRatesCount));

            using (var ctx = new LoyaltyDBEntities())
            {
                after = ctx.Database.SqlQuery<ImportStatus>(sql).ToList();
            }

            var diff = Diff(after, before);

            Assert.IsTrue(diff.Single(x => x.Status == -3).Count == 1, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 1 запись со статусом -3");
            Assert.IsTrue(diff.Single(x => x.Status == -1).Count == 1, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 1 запись со статусом -1");
            Assert.IsTrue(diff.Single(x => x.Status == -2).Count == 3, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 3 записи со статусом -2");

            // NOTE: 3 не корректный формат КЛАДР и 3 не существует в геобазе
            Assert.IsTrue(diff.Single(x => x.Status == -4).Count == 3, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 3 записи со статусом -4");

            // NOTE: 3 не корректный формат КЛАДР и 3 не существует в геобазе
            Assert.IsTrue(diff.Single(x => x.Status == -9).Count == 3, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 3 записи со статусом -9");
            
            // NOTE: 2 населеный пункт имет разные КЛАДР
            Assert.IsTrue(diff.Single(x => x.Status == -5).Count == 2, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 2 записи со статусом -5");

            // NOTE: теперь это не ошибка
            // NOTE: 2 КЛАДР имеет разные населенные пункты
            // Assert.IsTrue(diff.Single(x => x.Status == -6).Count == 2, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 2 записи со статусом -6");

            // NOTE: 2 населеный пункт имеет разные идентификаторы города
            Assert.IsTrue(diff.Single(x => x.Status == -7).Count == 2, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 2 записи со статусом -7");

            // NOTE: 2 идентификатор города имеет разные населенные пункты
            Assert.IsTrue(diff.Single(x => x.Status == -8).Count == 2, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 2 записи со статусом -8");

            // NOTE: теперь это не ошибка
            // NOTE: 1 идентификатор города уже имеет другой КЛАДР код в локациях
            // Assert.IsTrue(diff.Single(x => x.Status == -10).Count == 1, "В таблице [prod].[BUFFER_DeliveryRates] должно появится минимум 1 записи со статусом -10");

            var expectedOkRates = 10;
            Assert.AreEqual(expectedOkRates, diff.Single(x => x.Status == 1).Count, string.Format("В таблице [prod].[BUFFER_DeliveryRates] должно появится {0} записи со статусом 1", expectedOkRates));

            Helper.StopHttpListener();
        }

        private DeliveryRatesImporter CreteImporter(string remoteFileUrl, string etlSessionId)
        {
            return new DeliveryRatesImporter(this.partnerIdNotOzon, remoteFileUrl, "FSY", etlSessionId, logEmailSender: new StubLogEmailSender());
        }

        [TestMethod]
        [DeploymentItem(@"TestDeliveryRatesCsv\deliveryRatesWrongHeader.csv", "TestDeliveryRatesCsv")]
        public void ShouldFailWhenWrongHeaderTest()
        {
            // NOTE: Запускаем сервер, что отдаст файл
            // var remoteFileUrl = "http://localhost/Test/" + "testOzonDelivery_2013_07_15-2.csv";
            var remoteFileUrl = Helper.StartHttpListener(@".\TestDeliveryRatesCsv", "deliveryRatesWrongHeader.csv");

            var session = CreateWaitSession();

            var deliveryRatesImporter = CreteImporter(remoteFileUrl, session.EtlSessionId);

            var sessionId = deliveryRatesImporter.Execute();

            var agentInfo = new EtlAgentInfo
            {
                ConnectionString = DataSourceConfig.ConnectionString,
                SchemaName = ApiSettings.EtlSchemaName,
            };

            var agent = new SqlEtlAgent(agentInfo);

            session = agent.GetEtlLogParser().GetEtlSession(PartnerSettingsExtension.DefaultImportDeliveryRatesEtlPackageId.ToString(), sessionId);

            Assert.AreEqual(EtlStatus.Failed, session.Status);
        }

        private EtlSession CreateWaitSession()
        {
            var agentInfo = new EtlAgentInfo
            {
                ConnectionString = DataSourceConfig.ConnectionString,
                SchemaName = ApiSettings.EtlSchemaName,
            };

            var agent = new SqlEtlAgent(agentInfo);

            return agent.CreateWaitingEtlSession(PartnerSettingsExtension.DefaultImportDeliveryRatesEtlPackageId.ToString(), "FSY");
        }

        private static ImportStatus[] Diff(List<ImportStatus> after, List<ImportStatus> before)
        {
            var diff = after.GroupJoin(
                before,
                c => c.Status,
                p => p.Status,
                (afterItem, bItems) =>
                {
                    var bItemsArr = (bItems ?? new ImportStatus[0]).ToArray();

                    return new ImportStatus
                           {
                               Status = afterItem.Status,
                               Count = afterItem.Count - bItemsArr.Sum(x => x.Count)
                           };
                }).ToArray();
            return diff;
        }

        public class ImportStatus
        {
            public int Count { get; set; }

            public int Status { get; set; }
        }
    }
}
