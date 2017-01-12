namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.LogSender;
    using RapidSoft.Etl.Runtime.Agents;
    using RapidSoft.Etl.Runtime.Agents.Sql;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Interaction;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    public class CatalogImporter
    {
        private const string PackageId = "77388aa7-9c66-4771-87b9-1d3c2b96a8d5";
        private const string SubjectFormat = "Отчет по импорту каталога товаров партнера \"{0}\" из файла.";

        private readonly int taskId;
        private readonly bool loadRemoteFile;

        private readonly Logger log = new Logger();

        private readonly IImportTaskRepository importTaskRepository;

        private readonly IPartnerRepository partnerRepository;
        private readonly ILogEmailSender logEmailSender;

        public CatalogImporter(
            int taskId, 
            bool loadRemoteFile = true, 
            IImportTaskRepository importTaskRepository = null, 
            IPartnerRepository partnerRepository = null,
            ILogEmailSender logEmailSender = null)
        {           
            this.taskId = taskId;
            this.loadRemoteFile = loadRemoteFile;

            this.importTaskRepository = importTaskRepository ?? new ImportTaskRepository();
            this.partnerRepository = partnerRepository ?? new PartnerRepository();
            this.logEmailSender = logEmailSender ?? new LogEmailSender();
        }

        public void Execute()
        {
            this.log.APIMethodStart(string.Format("ImportProductsFromYML(taskId = {0})", this.taskId));

            this.log.ETLCreateSession(new Guid(PackageId));
            this.log.ETLStartSession("ImportProductsFromYML started", "Импорт каталога");

            var task = this.importTaskRepository.GetProductImportTask(this.taskId);

            if (task == null || task.Status != ImportTaskStatuses.Waiting)
            {
                var message = string.Format(
                    "Задача с идентификатором {0} не найдена или имеет не правильный статус", this.taskId);

                this.log.ETLError(message);
                this.log.ETLEndSession("ImportProductsFromYML end", EtlStatus.Failed);
                this.log.APIMethodEnd(message);
                return;
            }

            var etlSession = this.log.EtlSession;
            if (!etlSession.StartDateTime.HasValue)
            {
                etlSession.StartDateTime = DateTime.Now;
            }

            var startDateTime = etlSession.StartDateTime.Value;

            task = MarkTaskAsStarted(task, startDateTime);

            var dateKey = LoyaltyDBSpecification.GetDateKey(etlSession.StartDateTime.Value);

            Partner partner;
            string ymlFilePath;

            var logEntry = StartInteraction.With("Partner").For("ImportYml");

            try
            {
                partner = this.partnerRepository.GetById(task.PartnerId);

                if (partner == null)
                {
                    throw new Exception("Не найден партнер");
                }

                logEntry.Info["PartnerId"] = partner.Id;
                logEntry.Info["RemoteUrl"] = task.FileUrl;

                if (partner.Type == PartnerType.Online)
                {
                    throw new Exception("Импорт каталога для online партнера не возможен");
                }

                if (this.loadRemoteFile)
                {
                    ymlFilePath = Path.GetTempFileName().Replace(".tmp", ".xml");
                    ProductImportSteps.LoadFile(this.log, task.FileUrl, ymlFilePath);
                }
                else
                {
                    ymlFilePath = task.FileUrl;
                }

                ProductImportSteps.InitTables(this.log, task.PartnerId, dateKey);

                ProductImportSteps.ImportPartnerCategories(this.log, task.PartnerId, dateKey, ymlFilePath);

                ProductImportSteps.ImportPartnerProducts(this.log, partner, dateKey, ymlFilePath, task);

                logEntry.Info["TotalProducts"] = task.CountSuccess + task.CountFail;
                logEntry.Info["ImportedProducts"] = task.CountSuccess;

                ProductImportSteps.CreateConstraints(this.log, task.PartnerId, dateKey);

                ProductImportSteps.SwitchToNewCatalog(this.log, task.PartnerId, dateKey);

                ProductImportSteps.UpdateProductsFromAllPartners(this.log, task.PartnerId, dateKey);

                logEntry.Succeeded();

                task = this.MarkTaskAsCompleted(task, ImportTaskStatuses.Completed);
            }
            catch (Exception ex)
            {
                logEntry.Failed("ошибка взаимодействия", ex);

                this.MarkTaskAsCompleted(task, ImportTaskStatuses.Error);

                this.log.ETLError(ex);
                this.log.ETLEndSession("ImportProductsFromYML end", EtlStatus.Failed);
                this.log.APIMethodEnd(string.Format("ImportProductsFromYML(taskId = {0}) with error {1}", this.taskId, ex));
                return;
            }
            finally
            {
                logEntry.FinishAndWrite(LogManager.GetLogger(typeof(CatalogImporter)));
            }

            this.log.ETLEndSession("ImportProductsFromYML end", EtlStatus.Succeeded);

            SendReport(partner, ymlFilePath, this.log.EtlSession);

            this.log.APIMethodEnd(string.Format("ImportProductsFromYML(taskId = {0})", this.taskId));
        }

        private void SendReport(Partner partner, string fileUrl, EtlSession etlSession)
        {
            var partnerRecipients = partner.Settings.GetReportRecipients(string.Empty);
            var internalRecipients = ConfigurationManager.AppSettings["reportRecipients"] ?? string.Empty;

            this.log.Info(string.Format("Отправка отчета, получатели партнера: {0}, внутрение получатели: {1}", partnerRecipients, internalRecipients));

            var subject = string.Format(SubjectFormat, partner.Name);

            var separator = new[] { ';' };

            var recipients =
                partnerRecipients.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                                 .Union(internalRecipients.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                                 .Where(x => x != null)
                                 .Select(x => x.Trim())
                                 .Where(x => x.Length > 0)
                                 .ToArray();

            var agentInfo = new EtlAgentInfo
            {
                ConnectionString = DataSourceConfig.ConnectionString,
                SchemaName = ApiSettings.EtlSchemaName,
            };

            var agent = new SqlEtlAgent(agentInfo);

            if (recipients.Length > 0)
            {
                agent.SendSessionLog(etlSession, subject, recipients, logEmailSender);
            }
        }

        private ProductImportTask MarkTaskAsCompleted(ProductImportTask task, ImportTaskStatuses status)
        {
            task.EndDateTime = DateTime.Now;
            task.Status = status;
            task = this.importTaskRepository.SaveProductImportTask(task);
            return task;
        }

        private ProductImportTask MarkTaskAsStarted(ProductImportTask task, DateTime startDateTime)
        {
            task.StartDateTime = startDateTime;
            task.Status = ImportTaskStatuses.Loading;
            task = this.importTaskRepository.SaveProductImportTask(task);
            return task;
        }
    }
}