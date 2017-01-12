using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.Agents;

using RapidSoft.Etl.Monitor.Models;
using RapidSoft.Etl.Monitor.Agents;

namespace RapidSoft.Etl.Monitor
{
    [ServiceContract]
    public interface IPing
    {
        [OperationContract]
        void PingService();
    }

    public class Ping : IPing
    {
        private static IEtlAgent GetEtlAgent()
        {
            if (SiteConfiguration.EtlAgents.Length == 0)
                throw new Exception("At least one EtlAgent has to be configured");

            var agents =
                from a in SiteConfiguration.EtlAgents
                select EtlAgents.CreateAgent(
                    new EtlAgentInfo()
                    {
                        ConnectionString = a.ConnectionString,
                        EtlAgentType = a.EtlAgentType,
                        SchemaName = a.SchemaName,
                    });

            var multiEtlAgent = new MultiEtlAgent(agents.ToArray());

            return multiEtlAgent;
        }

        public void PingService()
        {
            try
            {
                var agent = GetEtlAgent();
                var packages = agent.GetEtlPackages();

                var logParser = agent.GetEtlLogParser();
                var latestSessions = logParser.GetLatestEtlSessions(packages.Select(p => p.Id).ToArray());

                foreach (var session in latestSessions.Where(x => x.StartDateTime.HasValue))
                {
                    var package = packages.Single(p => String.Equals(p.Id, session.EtlPackageId, StringComparison.InvariantCultureIgnoreCase));

                    if (package.RunIntervalSeconds <= 0)
                        continue;

                    if ((session.Status != EtlStatus.Succeeded && session.Status != EtlStatus.Started) || (DateTime.Now.Subtract(session.StartDateTime.Value).TotalSeconds > package.RunIntervalSeconds))
                        throw new Exception();
                }
            }
            catch
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
        }
    }
}
