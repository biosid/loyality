namespace RapidSoft.VTB24.BankConnector.EtlLogger
{
    using System;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime;

    [Obsolete("Only use this class for logging inside ETL.")]
    public class EtlLogger
    {
        private readonly string etlPackageId;

        private readonly string etlSessionId;

        private readonly IEtlLogger etlUtilityLogger;

        public EtlLogger(IEtlLogger etlUtilityLogger, string etlPackageId, string etlSessionId)
        {
            this.etlUtilityLogger = etlUtilityLogger;
            this.etlPackageId = etlPackageId;
            this.etlSessionId = etlSessionId;
            EtlStatus = EtlStatus.Succeeded;
        }

        public EtlLogger(IEtlLogger etlUtilityLogger, EtlContext context)
        {
            this.etlUtilityLogger = etlUtilityLogger;
            this.etlPackageId = context.EtlPackageId;
            this.etlSessionId = context.EtlSessionId;
            EtlStatus = EtlStatus.Succeeded;
        }

        public IEtlLogger EtlUtilityLogger
        {
            get
            {
                return this.etlUtilityLogger;
            }
        }

        public string EtlPackageId
        {
            get
            {
                return this.etlPackageId;
            }
        }

        public string EtlSessionId
        {
            get
            {
                return this.etlSessionId;
            }
        }

        public EtlStatus EtlStatus
        {
            get;
            set;
        }

        public void Counter(string entityName, string name, long value)
        {
            var now = DateTime.Now;

            this.EtlUtilityLogger.LogEtlCounter(new EtlCounter
            {
                EntityName = entityName,
                CounterName = name,
                CounterValue = value,
                EtlPackageId = EtlPackageId,
                EtlSessionId = EtlSessionId,
                DateTime = now,
                UtcDateTime = now.ToUniversalTime()
            });
        }

        public void Info(string message)
        {
            this.Message(message, EtlMessageType.Information);
        }

        public void InfoFormat(string format, params object[] args)
        {
            this.Info(string.Format(format, args));
        }

        public void Error(string message, Exception exception = null)
        {
            if (EtlStatus != EtlStatus.Failed)
            {
                EtlStatus = EtlStatus.FinishedWithErrors;
            }

            var stackTrace = exception != null ? exception.ToString() : null;
            this.Message(message, EtlMessageType.Error, stackTrace);           
        }

        public void Warn(string message, Exception exception = null)
        {
            if (EtlStatus != EtlStatus.Failed &&
                EtlStatus != EtlStatus.FinishedWithErrors)
            {
                EtlStatus = EtlStatus.FinishedWithWarnings;
            }
         
            var stackTrace = exception != null ? exception.ToString() : null;
            this.Message(message, EtlMessageType.Warning, stackTrace);
        }

        public void WarnFormat(string format, params object[] args)
        {
            this.WarnFormat(null, format, args);
        }

        public void WarnFormat(Exception exception, string format, params object[] args)
        {
            this.Warn(string.Format(format, args), exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this.ErrorFormat(null, format, args);
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            this.Error(string.Format(format, args), exception);
        }

        private void Message(string message, EtlMessageType messageType, string stackTrace = null)
        {           
            var etlMessage = new EtlMessage
                             {
                                 EtlPackageId = this.EtlPackageId, 
                                 EtlSessionId = this.EtlSessionId, 
                                 LogDateTime = DateTime.Now, 
                                 LogUtcDateTime = DateTime.UtcNow, 
                                 MessageType = messageType, 
                                 Text = message, 
                                 StackTrace = stackTrace
                             };
            this.EtlUtilityLogger.LogEtlMessage(etlMessage);
        }
    }
}