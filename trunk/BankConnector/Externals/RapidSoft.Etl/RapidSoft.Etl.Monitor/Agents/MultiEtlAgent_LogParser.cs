namespace RapidSoft.Etl.Monitor.Agents
{
using System;
using System.Collections.Generic;
using System.Linq;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.Agents;

    partial class MultiEtlAgent
    {
        private class _LogParser : IEtlLogParser
        {
            #region Constructors

            public _LogParser(IEtlAgent[] etlAgents)
            {
                this.etlAgents = etlAgents ?? new IEtlAgent[0];
            }

            #endregion

            #region Fields

            private readonly IEtlAgent[] etlAgents;

            #endregion

            #region Public methods

            public EtlSession[] GetEtlSessions(EtlSessionQuery query)
            {
                var result = new List<EtlSession>();

                foreach (var agent in this.etlAgents)
                {
                    var logParser = agent.GetEtlLogParser();
                    result.AddRange(logParser.GetEtlSessions(query));
                }

                return result.ToArray();
            }

            public EtlSession[] GetEtlSessions(EtlSessionPagedQuery query, out int? totalCount)
            {
                throw new NotSupportedException();
            }

            public EtlSession[] GetLatestEtlSessions(string[] etlPackageIds)
            {
                var result = new List<EtlSession>();

                foreach (var agent in this.etlAgents)
                {
                    var logParser = agent.GetEtlLogParser();
                    result.AddRange(logParser.GetLatestEtlSessions(etlPackageIds));
                }

                return result.ToArray();
            }

            public EtlSession GetEtlSession(string etlPackageId, string etlSessionId)
            {
                EtlSession result = null;

                foreach (var agent in this.etlAgents)
                {
                    var logParser = agent.GetEtlLogParser();
                    result = logParser.GetEtlSession(etlPackageId, etlSessionId);
                    if (result != null)
                    {
                        break;
                	}
                }

                return result;
            }

            public EtlVariable[] GetEtlVariables(string etlPackageId, string etlSessionId)
            {
                var result = new List<EtlVariable>();

                foreach (var agent in this.etlAgents)
                {
                    var logParser = agent.GetEtlLogParser();
                    result.AddRange(logParser.GetEtlVariables(etlPackageId, etlSessionId));
                }

                return result.ToArray();
            }

            public EtlVariable[] GetEtlVariables(string[] etlPackageId, string[] etlSessionIds)
            {
                throw new NotSupportedException();
            }

            public EtlCounter[] GetEtlCounters(string etlPackageId, string etlSessionId)
            {
                var result = new List<EtlCounter>();

                foreach (var agent in this.etlAgents)
                {
                    var logParser = agent.GetEtlLogParser();
                    result.AddRange(logParser.GetEtlCounters(etlPackageId, etlSessionId));
                }

                return result.ToArray();
            }

            public EtlCounter[] GetEtlCounters(string[] etlPackageId, string[] etlSessionIds)
            {
                throw new NotSupportedException();
            }

            public EtlMessage[] GetEtlMessages(string etlPackageId, string etlSessionId)
            {
                var result = new List<EtlMessage>();

                foreach (var agent in this.etlAgents)
                {
                    var logParser = agent.GetEtlLogParser();
                    result.AddRange(logParser.GetEtlMessages(etlPackageId, etlSessionId));
                }

                return result.ToArray();
            }

            public EtlMessage[] GetEtlMessagesPage(
                string etlPackageId, string etlSessionId, int skipMessageCount, int takeMessageCount, out int totalCount)
            {
                foreach (var logParser in this.etlAgents.Select(etlAgent => etlAgent.GetEtlLogParser()))
                {
                    var retVal = logParser.GetEtlMessagesPage(
                        etlPackageId, etlSessionId, skipMessageCount, takeMessageCount, out totalCount);
                    if (totalCount > 0)
                    {
                        return retVal;
                    }
                }

                totalCount = 0;
                return new EtlMessage[0];
            }

            public EtlMessage GetLastEtlErrorMessage(string etlPackageId, string etlSessionId)
            {
                return
                    this.etlAgents.Select(etlAgent => etlAgent.GetEtlLogParser())
                        .Select(logParser => logParser.GetLastEtlErrorMessage(etlPackageId, etlSessionId))
                        .FirstOrDefault(retVal => retVal != null);
            }

            #endregion
        }
    }
}
