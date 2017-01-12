using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidSoft.Etl.Logging
{
    public sealed class MemoryEtlLogger : IEtlLogger
    {
        #region Fields

        private readonly List<EtlSession> _sessions = new List<EtlSession>();
        private readonly List<EtlVariable> _variables = new List<EtlVariable>();
        private readonly List<EtlCounter> _counters = new List<EtlCounter>();
        private readonly List<EtlMessage> _messages = new List<EtlMessage>();

        #endregion

        #region Properties

        public IList<EtlSession> EtlSessions
        {
            [DebuggerStepThrough]
            get
            {
                return _sessions;
            }
        }

        public IList<EtlVariable> EtlVariables
        {
            [DebuggerStepThrough]
            get
            {
                return _variables;
            }
        }

        public IList<EtlCounter> EtlCounters
        {
            [DebuggerStepThrough]
            get
            {
                return _counters;
            }
        }

        public IList<EtlMessage> EtlMessages
        {
            [DebuggerStepThrough]
            get
            {
                return _messages;
            }
        }

        #endregion

        #region IEtlLogger Members

        public void LogEtlSessionStart(EtlSession session)
		{
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _sessions.Add(session);
        }

        public void LogEtlSessionContinue(EtlSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            foreach (var exSession in _sessions)
            {
                if (exSession.EtlPackageId == session.EtlPackageId && exSession.EtlSessionId == session.EtlSessionId)
                {
                    exSession.StartDateTime = session.EndDateTime;
                    exSession.StartUtcDateTime = session.EndUtcDateTime;
                    exSession.Status = EtlStatus.Started;

                    break;
                }
            }
        }

        public void LogEtlSessionEnd(EtlSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            foreach (var exSession in _sessions)
            {
                if (exSession.EtlPackageId == session.EtlPackageId && exSession.EtlSessionId == session.EtlSessionId)
                {
                    exSession.EndDateTime = session.EndDateTime;
                    exSession.EndUtcDateTime = session.EndUtcDateTime;
                    exSession.Status = session.Status;

                    break;
                }
            }
        }

        public void LogEtlVariable(EtlVariable variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException("variable");
            }

            _variables.Add(variable);
        }

        public void LogEtlCounter(EtlCounter counter)
        {
            if (counter == null)
            {
                throw new ArgumentNullException("counter");
            }

            _counters.Add(counter);
        }

        public void LogEtlMessage(EtlMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            message.SequentialId = _messages.Count + 1;
            _messages.Add(message);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}