namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System;

    using RapidSoft.Etl.Logging;

    public class EtlLogger
    {
        public EtlLogger(Guid etlPackageId, Guid etlSessionId, IEtlLogger etlLogger)
        {
            this.EtlPackageId = etlPackageId;
            this.EtlSessionId = etlSessionId;
            this.Logger = etlLogger;
        }

        public Guid EtlPackageId { get; private set; }

        public Guid EtlSessionId { get; private set; }

        public IEtlLogger Logger { get; private set; }

        public EtlSession EtlSession { get; private set; }

        public void AddMessage(string message)
        {
            var etlMessage = new EtlMessage
            {
                EtlPackageId = this.EtlPackageId.ToString(),
                EtlSessionId = this.EtlSessionId.ToString(),
                LogDateTime = DateTime.Now,
                LogUtcDateTime = DateTime.UtcNow,
                MessageType = EtlMessageType.Information,
                Text = message
            };
            this.Logger.LogEtlMessage(etlMessage);
        }

        public void AddMessageWithException(Exception ex)
        {
            var message = new EtlMessage
            {
                EtlPackageId = this.EtlPackageId.ToString(),
                EtlSessionId = this.EtlSessionId.ToString(),
                LogDateTime = DateTime.Now,
                LogUtcDateTime = DateTime.UtcNow,
                MessageType = EtlMessageType.Error,
                StackTrace = ex.StackTrace,
                Text = ex.Message,
            };
            this.Logger.LogEtlMessage(message);
        }

        public void AddErrorMessage(string errorMessage)
        {
            var message = new EtlMessage
            {
                EtlPackageId = this.EtlPackageId.ToString(),
                EtlSessionId = this.EtlSessionId.ToString(),
                LogDateTime = DateTime.Now,
                LogUtcDateTime = DateTime.UtcNow,
                MessageType = EtlMessageType.Error,
                StackTrace = null,
                Text = errorMessage,
            };
            this.Logger.LogEtlMessage(message);
        }

        public void StartSession(string message, string packageName, string user = null)
        {
            var session = new EtlSession
            {
                EtlPackageId = this.EtlPackageId.ToString(),
                EtlPackageName = packageName,
                EtlSessionId = this.EtlSessionId.ToString(),
                StartDateTime = DateTime.Now,
                StartUtcDateTime = DateTime.UtcNow,
                Status = EtlStatus.Started,
                UserName = user
            };
            this.Logger.LogEtlSessionStart(session);
            this.EtlSession = session;
        }

        public void EndSession(string message, EtlStatus status)
        {
            if (this.EtlSession == null)
            {
                throw new ApplicationException("EtlSession is not initialized!");
            }

            this.EtlSession.EndDateTime = DateTime.Now;

            this.EtlSession.EndUtcDateTime = DateTime.UtcNow;
            this.EtlSession.Status = status;
            this.Logger.LogEtlSessionEnd(this.EtlSession);
        }
    }
}
