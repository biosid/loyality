namespace RapidSoft.Loaylty.ProductCatalog.QuarzJobs
{
    using System;
    using System.Globalization;

    using Quartz;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime.Agents;
    using RapidSoft.Etl.Runtime.Agents.Sql;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Import;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    using Services;

    [GroupDisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class ImportDeliveryRatesJob : IJob
    {
        private const string JobGroupName = "ImportDeliveryRates";

        private static readonly ILog Log = LogManager.GetLogger(typeof(ImportDeliveryRatesJob));

        public static string AddJob(int partnerId, string fileUrl, string userId, IPartnerRepository partnerRepository = null)
        {
            var scheduler = Helper.GetScheduler();

            var guid = Guid.NewGuid().ToString();

            var sessionId = CreateEtlSession(partnerId, fileUrl, userId, partnerRepository);

            var job = JobBuilder.Create<ImportDeliveryRatesJob>()
                                .WithIdentity(guid, JobGroupName)
                                .RequestRecovery(false)
                                .Build();

            job.JobDataMap.Put(DataKeys.PartnerId, partnerId);
            job.JobDataMap.Put(DataKeys.FileUrl, fileUrl);
            job.JobDataMap.Put(DataKeys.UserId, userId);
            job.JobDataMap.Put(DataKeys.EtlSessionId, sessionId);

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity(guid, JobGroupName)
                                        .StartNow()
                                        .Build();

            scheduler.ScheduleJob(job, trigger);

            return guid;
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Log.Info("Начало импорта списка тарифов доставки");

                var dataMap = context.JobDetail.JobDataMap;

                int partnerId = dataMap.GetIntValue(DataKeys.PartnerId);
                string fileUrl = dataMap.GetAsString(DataKeys.FileUrl);
                string userId = dataMap.GetAsString(DataKeys.UserId);
                string etlSessionId = dataMap.GetAsString(DataKeys.EtlSessionId);

                var importer = this.GetImporter(partnerId, fileUrl, userId, etlSessionId);

                importer.Execute();
            }
            catch (Exception ex)
            {
                Log.Error("Ошибка импортирования списка тарифов доставки", ex);
                throw;
            }
        }

        public IDeliveryRatesImporter GetImporter(int partnerId, string fileUrl, string userId, string etlSessionId)
        {
            return new DeliveryRatesImporter(partnerId, fileUrl, userId, etlSessionId);
        }

        private static string CreateEtlSession(int partnerId, string fileUrl, string userId, IPartnerRepository partnerRepository = null)
        {
            var partner = (partnerRepository ?? new PartnerRepository()).GetById(partnerId);

            if (partner == null)
            {
                throw new ApplicationException(string.Format("Партнер с иденфитикатором {0} не найден => импорт тарифов доставки не возможен", partnerId));
            }

            var packageId = partner.Settings.GetImportDeliveryRatesEtlPackageId();

            Log.Info("Creating ETL agent...");
            var agentInfo = new EtlAgentInfo
            {
                ConnectionString = DataSourceConfig.ConnectionString,
                SchemaName = ApiSettings.EtlSchemaName,
            };

            var agent = new SqlEtlAgent(agentInfo);

            var session = agent.CreateWaitingEtlSession(packageId, userId);

            var logger = agent.CreateDefaultLogger();

            var now = DateTime.Now;

            var etlVariable = new EtlVariable
                                  {
                                      DateTime = now,
                                      UtcDateTime = now.ToUniversalTime(),
                                      EtlPackageId = session.EtlPackageId,
                                      EtlSessionId = session.EtlSessionId,
                                      Name = "PartnerId",
                                      Value = partnerId.ToString(CultureInfo.InvariantCulture)
                                  };
            logger.LogEtlVariable(etlVariable);
            etlVariable = new EtlVariable
                              {
                                  DateTime = now,
                                  UtcDateTime = now.ToUniversalTime(),
                                  EtlPackageId = session.EtlPackageId,
                                  EtlSessionId = session.EtlSessionId,
                                  Name = "FileUrl",
                                  Value = fileUrl
                              };
            logger.LogEtlVariable(etlVariable);

            return session.EtlSessionId;
        }
    }
}