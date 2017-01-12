namespace RapidSoft.Etl.Logging
{
using System;

    public interface IEtlLogger : IDisposable
    {
        void LogEtlSessionStart(EtlSession session);

        void LogEtlSessionContinue(EtlSession session);

        void LogEtlSessionEnd(EtlSession session);

        void LogEtlVariable(EtlVariable variables);

        void LogEtlCounter(EtlCounter counters);
        
        void LogEtlMessage(EtlMessage message);
    }
}