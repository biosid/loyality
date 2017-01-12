using System.Configuration;
using System.Linq;

using RapidSoft.Etl.Runtime.Agents;
using RapidSoft.Etl.Monitor.Configuration;

namespace RapidSoft.Etl.Monitor.Models
{
    public static class SiteConfiguration
    {
        public static int MaxCoulmnsToShow
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxCoulmnsToShow"]))
                {
                    return int.Parse(ConfigurationManager.AppSettings["MaxCoulmnsToShow"]);
                }
                else
                {
                    return 3;
                }
            }
        }

        public static int MaxVariablesToShow
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxVariablesToShow"]))
                {
                    return int.Parse(ConfigurationManager.AppSettings["MaxVariablesToShow"]);
                }
                else
                {
                    return 5;
                }
            }
        }

        public static int RefreshMonitoringEverySeconds
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RefreshMonitoringEverySeconds"]))
                {
                    return int.Parse(ConfigurationManager.AppSettings["RefreshMonitoringEverySeconds"]);
                }
                else
                {
                    return 5;
                }
            }
        }

        public static int CacheExpiresInSeconds
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CacheExpiresInSeconds"]))
                {
                    return int.Parse(ConfigurationManager.AppSettings["CacheExpiresInSeconds"]);
                }
                else
                {
                    return 15;
                }
            }
        }

        static EtlAgentsSection etlAgentsSection = null;
        public static EtlAgentsCollectionElement[] EtlAgents
        {
            get
            {
                if (etlAgentsSection == null)
                {
                    etlAgentsSection = (EtlAgentsSection)System.Configuration.ConfigurationManager.GetSection("etl");
                }

                var retVal = etlAgentsSection == null
                                 ? new EtlAgentsCollectionElement[0]
                                 : etlAgentsSection.EtlAgents.Cast<EtlAgentsCollectionElement>().ToArray();
                return retVal;
            }
        }

        static IEtlAgent etlAgent = null;
        public static IEtlAgent GetEtlAgent()
        {
            if (etlAgent == null)
            {
                if (SiteConfiguration.EtlAgents.Length == 0)
                    return new RapidSoft.Etl.Monitor.Agents.MultiEtlAgent(new IEtlAgent[0]);

                var agents =
                    from a in SiteConfiguration.EtlAgents
                    select RapidSoft.Etl.Runtime.Agents.EtlAgents.CreateAgent(
                        new EtlAgentInfo()
                        {
                            ConnectionString = a.ConnectionString,
                            EtlAgentType = a.EtlAgentType,
                            SchemaName = a.SchemaName,
                        });

                etlAgent = new RapidSoft.Etl.Monitor.Agents.MultiEtlAgent(agents.ToArray());
            }

            return etlAgent;
        }
    }
}