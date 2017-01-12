namespace RapidSoft.Etl.Logging.Dumps
{
    using System;

    public sealed class EtlDumpWriter
    {
        #region Constructors

        public EtlDumpWriter(EtlDumpSettings settings)
        {
            this._settings = settings;
            _dump = new EtlDump();
        }

        #endregion

        #region Fields

        private readonly EtlDump _dump;

        private readonly EtlDumpSettings _settings;

        #endregion

        #region Methods

        public EtlDump GetDump()
        {
            return _dump;
        }

        public void Write(string packageId, string sessionId, IEtlLogParser logParser)
        {
            if (logParser == null)
            {
                throw new ArgumentNullException("logParser");
            }

            InitDump();

            var session = logParser.GetEtlSession(packageId, sessionId);
            if (session != null)
            {
                var sessionSummary = GetEtlSessionSummary(session, logParser);
                _dump.Sessions.Add(sessionSummary);
            }
        }

        public void Write(EtlSessionQuery query, IEtlLogParser logParser)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (logParser == null)
            {
                throw new ArgumentNullException("logParser");
            }

            InitDump();

            var sessions = logParser.GetEtlSessions(query);
            foreach (var session in sessions)
            {
                var sessionSummary = GetEtlSessionSummary(session, logParser);
                _dump.Sessions.Add(sessionSummary);
            }
        }

        private void InitDump()
        {
            if (_dump.DumpDateTime == default(DateTime))
            {
                _dump.DumpDateTime = DateTime.Now;
                _dump.DumpUtcDateTime = _dump.DumpDateTime.ToUniversalTime();
            }
        }

        private EtlSessionSummary GetEtlSessionSummary(EtlSession session, IEtlLogParser logParser)
        {
           var sessionSummary = new EtlSessionSummary
            {
                EtlPackageId = session.EtlPackageId,
                EtlPackageName = session.EtlPackageName,
                EtlSessionId = session.EtlSessionId,
                StartDateTime = session.StartDateTime,
                StartUtcDateTime = session.StartUtcDateTime,
                EndDateTime = session.EndDateTime,
                EndUtcDateTime = session.EndUtcDateTime,
                Status = session.Status,
                UserName = session.UserName,
            };

            var variables = logParser.GetEtlVariables(session.EtlPackageId, session.EtlSessionId);
            sessionSummary.Variables.AddRange(variables);

            var counters = logParser.GetEtlCounters(session.EtlPackageId, session.EtlSessionId);
            sessionSummary.Counters.AddRange(counters);

            if (this._settings.TakeMessageCount == int.MaxValue && this._settings.SkipMessageCount == 0)
            {
                var messages = logParser.GetEtlMessages(session.EtlPackageId, session.EtlSessionId);
                sessionSummary.Messages.AddRange(messages);
                sessionSummary.MessageTotalCount = messages.Length;
                sessionSummary.MessageSkipCount = 0;
                sessionSummary.MessageTakeCount = messages.Length;

                for (var i = messages.Length - 1; i >= 0; i--)
                {
                    if (messages[i].MessageType == EtlMessageType.Error)
                    {
                        sessionSummary.LastErrorMessage = messages[i];
                        break;
                    }
                }
            }
            else
            {
                int totalCount;
                var messages = logParser.GetEtlMessagesPage(
                    session.EtlPackageId, session.EtlSessionId, _settings.SkipMessageCount, _settings.TakeMessageCount, out totalCount);
                var lastErrorMessage = logParser.GetLastEtlErrorMessage(session.EtlPackageId, session.EtlSessionId);

                sessionSummary.Messages.AddRange(messages);
                sessionSummary.MessageTotalCount = totalCount;
                sessionSummary.MessageSkipCount = _settings.SkipMessageCount;
                sessionSummary.MessageTakeCount = _settings.TakeMessageCount;
                sessionSummary.LastErrorMessage = lastErrorMessage;
            }

            return sessionSummary;
        }

        #endregion
    }
}