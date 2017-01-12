using RapidSoft.Loaylty.Logging;

namespace RapidSoft.Loaylty.ProductCatalog.ImportTests
{
    using Etl.Logging;

    internal class EtlLogger : IEtlLogger
    {
        private readonly ILog log = LogManager.GetLogger(typeof(EtlLogger));

        public void Dispose()
        {
        }

        public void LogEtlSessionStart(EtlSession session)
        {
            log.Debug("LogEtlSessionStart: " + session);
        }

        public void LogEtlSessionContinue(EtlSession session)
        {
            log.Debug("LogEtlSessionContinue: " + session);
        }

        public void LogEtlSessionEnd(EtlSession session)
        {
            log.Debug("EtlSessionEnd: " + session);
        }

        public void LogEtlVariable(EtlVariable variables)
        {
            log.Debug("EtlVariable: " + variables);
        }

        public void LogEtlCounter(EtlCounter counters)
        {
            log.Debug("EtlCounter: " + counters);
        }

        public void LogEtlMessage(EtlMessage message)
        {
            log.Debug("EtlMessage: " + message);
        }
    }
}