using System;
using System.Collections.Generic;

namespace RapidSoft.Etl.Logging
{
    public sealed class MultiEtlLogger : IEtlLogger
    {
        #region Constructors

        public MultiEtlLogger(IEtlLogger logger)
            : this(new[] { logger })
        {
        }

        public MultiEtlLogger(IEtlLogger logger0, IEtlLogger logger1)
            : this(new[]{logger0, logger1})
        {
        }

        public MultiEtlLogger(IEtlLogger logger0, IEtlLogger logger1, IEtlLogger logger2)
            : this(new[] { logger0, logger1, logger2 })
        {
        }

        public MultiEtlLogger(IEtlLogger logger0, IEtlLogger logger1, IEtlLogger logger2, IEtlLogger logger3)
            : this(new[] { logger0, logger1, logger2, logger3 })
        {
        }

        public MultiEtlLogger(IEtlLogger[] loggers)
        {
            _loggers = loggers ?? new IEtlLogger[0];
        }

        #endregion

        #region Fields

        private readonly IEtlLogger[] _loggers;

        #endregion

        #region IEtlLogger Members

        public void LogEtlSessionStart(EtlSession session)
		{
            foreach (var logger in _loggers)
            {
                logger.LogEtlSessionStart(session);
            }
        }

        public void LogEtlSessionContinue(EtlSession session)
        {
            foreach (var logger in _loggers)
            {
                logger.LogEtlSessionContinue(session);
            }
        }

        public void LogEtlSessionEnd(EtlSession session)
        {
            foreach (var logger in _loggers)
            {
                logger.LogEtlSessionEnd(session);
            }
        }

        public void LogEtlVariable(EtlVariable variable)
        {
            foreach (var logger in _loggers)
            {
                logger.LogEtlVariable(variable);
            }
        }

        public void LogEtlCounter(EtlCounter counter)
        {
            foreach (var logger in _loggers)
            {
                logger.LogEtlCounter(counter);
            }
        }

        public void LogEtlMessage(EtlMessage message)
        {
            foreach (var logger in _loggers)
            {
                logger.LogEtlMessage(message);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            foreach (var logger in _loggers)
            {
                logger.Dispose();
            }
        }

        #endregion
    }
}