namespace RapidSoft.VTB24.BankConnector.EtlExecutionWrapper
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Etl.Runtime;
    using RapidSoft.Etl.Runtime.Agents;
    using RapidSoft.Etl.Runtime.Agents.Sql;
    using RapidSoft.Etl.Runtime.Steps;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.VTB24.BankConnector.Configuration;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    public class WrapperBase
    {
        protected readonly EtlVariableAssignment[] Assignments = new EtlVariableAssignment[]
                                                                 {
                                                                 };

        private readonly ILog logger = LogManager.GetLogger(typeof(WrapperBase));

        public WrapperBase(JobExecutionParameters parameters)
        {
            this.Assignments = parameters.Assignments;
            this.JobName = parameters.JobName;
            this.PackageId = parameters.PackageId;
        }

        public string SessionId
        {
            get;
            private set;
        }

        public string PackageId
        {
            get;
            set;
        }

        protected string JobName
        {
            get;
            set;
        }

        public string Execute()
        {
            var abortReason = this.GetEtlExecutionAbortReason();

            if (abortReason != null)
            {
                this.logger.Info(string.Format("Job:{0} Abort reason:{1}", GetJobName(), abortReason.Message));
                return null;
            }

            var etlVariables = this.GetConfigSectionVariableAssignment();

            etlVariables = UnionWithOverride(etlVariables, Assignments);

            var agentInfo = new EtlAgentInfo
                            {
                                ConnectionString = ConfigHelper.ConnectionString, 
                                SchemaName = ConfigHelper.SchemaName, 
                            };

            var agent = new SqlEtlAgent(agentInfo);

            return this.ExecuteInternal(etlVariables, agent);
        }

        protected virtual EtlExecutionAbortReason GetEtlExecutionAbortReason()
        {
            return new EtlExecutionHandler.EtlExecutionHandler().CheckAbortReason(Assignments);
        }

        protected EtlVariableAssignment[] GetConfigSectionVariableAssignment()
        {
            var etlConfigSection = EtlConfigSection.Current;
            if (etlConfigSection == null)
            {
                return new EtlVariableAssignment[]
                       {
                       };
            }

            var retVal = etlConfigSection.Variables.Select(
                v => new EtlVariableAssignment
                     {
                         Name = v.Name, 
                         Value = v.Value
                     }).ToArray();
            return retVal;
        }

        protected string ExecuteInternal(EtlVariableAssignment[] etlVariables, SqlEtlAgent agent)
        {
            var etlSession = agent.InvokeEtlPackage(this.PackageId, etlVariables, null);

            this.SessionId = etlSession.EtlSessionId;

            var logParser = agent.GetEtlLogParser();
            var etlVars = logParser.GetEtlVariables(this.PackageId, this.SessionId);
            var passedMailsCount = etlVars.FirstOrDefault(v => v.Name == EtlEmailReceiveImapStep.PassedMailsCountVarName);

            if (passedMailsCount == null)
            {
                return this.SessionId;
            }

            if (int.Parse(passedMailsCount.Value) > 1)
            {
                this.ExecuteInternal(etlVariables, agent);
            }

            return this.SessionId;
        }

        protected virtual string GetJobName()
        {
            return JobName ?? PackageId;
        }

        private EtlVariableAssignment[] UnionWithOverride(EtlVariableAssignment[] etlVariables, EtlVariableAssignment[] assignments)
        {
            var res = new List<EtlVariableAssignment>();

            res.AddRange(etlVariables);

            foreach (var assignment in assignments)
            {
                var existedVar = res.FirstOrDefault(v => v.Name == assignment.Name);

                if (existedVar == null)
                {
                    res.Add(assignment);
                }
                else
                {
                    existedVar.Value = assignment.Value;
                }
            }

            return res.ToArray();
        }
    }
}