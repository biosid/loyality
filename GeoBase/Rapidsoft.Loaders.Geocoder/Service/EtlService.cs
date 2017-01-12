using System;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Loaders.Geocoder.Service
{
    public class EtlService : IEtlService
    {
        public EtlService(Guid etlPackageId, Guid etlSessionId, IEtlLogger etlLogger)
        {
            EtlPackageId = etlPackageId;
            EtlSessionId = etlSessionId;
            EtlLogger = etlLogger;
        }

        public Guid EtlPackageId { get; private set; }

        public Guid EtlSessionId { get; private set; }

        public IEtlLogger EtlLogger { get; private set; }

        public EtlSession EtlSession { get; private set; }

        public void AddMessage(string message)
        {
            var etlMessage = new EtlMessage
            {
                EtlPackageId = EtlPackageId.ToString(),
                EtlSessionId = EtlSessionId.ToString(),
                LogDateTime = DateTime.Now,
                LogUtcDateTime = DateTime.UtcNow,
                MessageType = EtlMessageType.Information,
                Text = message
            };
            EtlLogger.LogEtlMessage(etlMessage);
        }

        public void AddMessageWithException(Exception ex)
        {
            var message = new EtlMessage
            {
                EtlPackageId = EtlPackageId.ToString(),
                EtlSessionId = EtlSessionId.ToString(),
                LogDateTime = DateTime.Now,
                LogUtcDateTime = DateTime.UtcNow,
                MessageType = EtlMessageType.CriticalError,
                StackTrace = ex.StackTrace,
                Text = ex.Message,
            };
            EtlLogger.LogEtlMessage(message);
        }

        public void StartSession(string message)
        {
            var session = new EtlSession
            {
                EtlPackageId = EtlPackageId.ToString(),
                EtlSessionId = EtlSessionId.ToString(),
                StartDateTime = DateTime.Now,
                StartUtcDateTime = DateTime.UtcNow,
                StartMessage = message,
                Status = EtlStatus.Started,
            };
            EtlLogger.LogEtlSessionStart(session);
            EtlSession = session;
        }

        public void EndSession(string message, EtlStatus status)
        {
            if (EtlSession==null)
            {
                throw new ApplicationException("EtlSession is not initialized!");
            }

            EtlSession.EndDateTime = DateTime.Now;
            EtlSession.EndMessage = message;
            EtlSession.EndUtcDateTime = DateTime.UtcNow;
            EtlSession.Status = status;
            EtlLogger.LogEtlSessionEnd(EtlSession);
        }
    }
}
