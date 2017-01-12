namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.LogSender;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.Etl.Runtime.Agents;
    using RapidSoft.Etl.Runtime.Agents.Sql;
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Interaction;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    public class DeliveryRatesImporter : IDeliveryRatesImporter
    {
        private const string SubjectFormat = "Отчет по импорту тарифов доставки партнера \"{0}\" из файла.";

        private readonly int partnerId;
        private readonly string fileUrl;
        private readonly string userId;
        private readonly string etlSesionId;

        private readonly IPartnerRepository partnerRepository;

        private readonly ILog log = LogManager.GetLogger(typeof(DeliveryRatesImporter));
        private readonly ILogEmailSender logEmailSender;

        public DeliveryRatesImporter(
            int partnerId, 
            string fileUrl, 
            string userId, 
            string etlSesionId, 
            IPartnerRepository partnerRepository = null,
            ILogEmailSender logEmailSender = null)
        {
            fileUrl.ThrowIfNull("fileUrl");
            userId.ThrowIfNull("userId");

            this.partnerId = partnerId;
            this.fileUrl = fileUrl;
            this.userId = userId;
            this.etlSesionId = etlSesionId;
            this.logEmailSender = logEmailSender ?? new LogEmailSender();

            this.partnerRepository = partnerRepository ?? new PartnerRepository();
        }

        public string Execute()
        {
            var partner = partnerRepository.GetById(this.partnerId);

            if (partner == null)
            {
                throw new ApplicationException(string.Format("Партнер с иденфитикатором {0} не найден => импорт тарифов доставки не возможен", this.partnerId));
            }
            
            var packageId = partner.Settings.GetImportDeliveryRatesEtlPackageId();

            log.Info("Creating ETL agent...");
            var agentInfo = new EtlAgentInfo
                            {
                                ConnectionString = DataSourceConfig.ConnectionString,
                                SchemaName = ApiSettings.EtlSchemaName,
                            };

            var agent = new SqlEtlAgent(agentInfo);

            var tempDir = Path.GetTempPath();
            var partnerIdStr = partnerId.ToString(CultureInfo.InvariantCulture);

            var parameters = new[]
                                 {
                                     new EtlVariableAssignment("DB", DataSourceConfig.ConnectionString),
                                     new EtlVariableAssignment("FileUrl", fileUrl),
                                     new EtlVariableAssignment("PartnerId", partnerIdStr),
                                     new EtlVariableAssignment("TempDir", tempDir)
                                 };

            log.Info("ETL agent created");

            EtlSession etlSession;

            var logEntry = StartInteraction.With("Partner").For("ImportDeliveryRates");
            logEntry.Info["PartnerId"] = partnerId;
            logEntry.Info["RemoteUrl"] = fileUrl;

            try
            {
                log.Info("Invoking package...");
                etlSession = agent.InvokeEtlPackage(packageId, parameters, null, this.userId, this.etlSesionId);
                log.Info(string.Format("Package has been executed with result {0}", etlSession.Status));

                if (etlSession.Status == EtlStatus.Failed)
                {
                    logEntry.Failed("ETL сессия завершилась со статусом Failed", null);
                }
                else
                {
                    logEntry.Succeeded();
                }
            }
            catch (Exception e)
            {
                logEntry.Failed("ошибка взаимодействия", e);
                throw;
            }
            finally
            {
                logEntry.FinishAndWrite(log);
            }

            var partnerRecipients = partner.Settings.GetReportRecipients(string.Empty);
            var internalRecipients = ConfigurationManager.AppSettings["reportRecipients"] ?? string.Empty;

            log.InfoFormat("Отправка отчета, получатели партнера: {0}, внутрение получатели: {1}", partnerRecipients, internalRecipients);

            var subject = string.Format(SubjectFormat, partner.Name);

            var separator = new[] { ';' };

            var recipients =
                partnerRecipients.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                                 .Union(internalRecipients.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                                 .Where(x => x != null)
                                 .Select(x => x.Trim())
                                 .Where(x => x.Length > 0)
                                 .ToArray();

            if (recipients.Length > 0)
            {
                agent.SendSessionLog(etlSession, subject, recipients, logEmailSender: logEmailSender);
            }

            return etlSession.EtlSessionId;
        }
    }
}