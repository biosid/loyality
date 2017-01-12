namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Logging.Sql;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;

    public class Logger
    {
        private readonly ILog log = LogManager.GetLogger(typeof(Logger));
        private EtlLogger sqlEtlLogger;
        private IEtlLogger etlLogger;

        public EtlSession EtlSession
        {
            get
            {
                return this.sqlEtlLogger != null ? this.sqlEtlLogger.EtlSession : null;
            }
        }

        public IEtlLogger EtlLogger
        {
            get
            {
                return this.etlLogger;
            }
        }

        public void ETLCreateSession(Guid etlPackageId)
        {
            this.etlLogger = new SqlEtlLogger(DataSourceConfig.ConnectionString, "dbo");
            var etlSessionId = Guid.NewGuid();
            this.sqlEtlLogger = new EtlLogger(etlPackageId, etlSessionId, this.etlLogger);
        }

        /// <summary>
        /// Факт запуска ETL-процесса
        /// </summary>
        /// <param name="message">
        /// Сообщение
        /// </param>
        /// <param name="packageName">Название пакета</param>
        public void ETLStartSession(string message, string packageName)
        {
            this.sqlEtlLogger.StartSession(message, packageName);
            this.sqlEtlLogger.AddMessage(message);
        }

        /// <summary>
        /// Факт ошибки в процессе выполнения ETL-процесса
        /// </summary>
        /// <param name="ex">
        /// Ошибка
        /// </param>
        public void ETLError(Exception ex)
        {
            this.sqlEtlLogger.AddMessageWithException(ex);
            this.log.Error(ex.Message, ex);
        }

        public void ETLError(string errorMessage)
        {
            this.sqlEtlLogger.AddErrorMessage(errorMessage);
            this.log.Error(errorMessage);
        }

        /// <summary>
        /// Факт завершения ETL-процесса
        /// </summary>
        /// <param name="message">
        /// Сообщение
        /// </param>
        /// <param name="status">
        /// Стасус сообщения
        /// </param>
        public void ETLEndSession(string message, EtlStatus status)
        {
            this.sqlEtlLogger.AddMessage(message);
            this.sqlEtlLogger.EndSession(message, status);
        }

        public void ETLMessage(string message)
        {
            this.sqlEtlLogger.AddMessage(message);
        }

        public void LogMessage(string stepId, EtlMessageType type, string message)
        {
            var td = DateTime.Now;
            var etlMessage = new EtlMessage
                             {
                                 EtlPackageId = this.EtlSession.EtlPackageId,
                                 EtlSessionId = this.EtlSession.EtlSessionId,
                                 EtlStepName = stepId,
                                 LogDateTime = td,
                                 LogUtcDateTime = td.ToUniversalTime(),
                                 MessageType = type,
                                 Text = message,
                             };
            etlLogger.LogEtlMessage(etlMessage);
        }

        public void LogCounter(string entityName, string counterName, long counterValue)
        {
            var dt = DateTime.Now;
            EtlCounter counter = new EtlCounter
                                 {
                                     CounterName = counterName,
                                     CounterValue = counterValue,
                                     DateTime = dt,
                                     UtcDateTime = dt.ToUniversalTime(),
                                     EntityName = entityName,
                                     EtlPackageId = this.EtlSession.EtlPackageId,
                                     EtlSessionId = this.EtlSession.EtlSessionId,
                                 };

            etlLogger.LogEtlCounter(counter);
        }

        public void LogStart(string stepId, string message)
        {
            this.LogMessage(stepId, EtlMessageType.StepStart, message);
        }

        public void LogInfo(string stepId, string message)
        {
            this.LogMessage(stepId, EtlMessageType.Information, message);
        }

        public void LogError(string stepId, string message)
        {
            this.LogMessage(stepId, EtlMessageType.Error, message);
        }

        public void LogWarn(string stepId, string message)
        {
            this.LogMessage(stepId, EtlMessageType.Warning, message);
        }

        public void LogEnd(string stepId, string message)
        {
            this.LogMessage(stepId, EtlMessageType.StepEnd, message);
        }

        public void APIMethodStart(string message)
        {
            this.log.Info(string.Format("start {0}", message));
        }

        public void APIMethodEnd(string message)
        {
            this.log.Info(string.Format("end {0}", message));
        }

        public void Info(string message)
        {
            this.log.Info(message);
        }

        public void Error(Exception exception)
        {
            this.log.Error(exception);
        }
    }
}