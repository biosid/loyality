namespace RapidSoft.Etl.Runtime.Agents
{
using RapidSoft.Etl.Logging;

    public interface IEtlAgent
    {
        EtlAgentInfo GetEtlAgentInfo();

        void DeployEtlPackage(EtlPackage package, EtlPackageDeploymentOptions options);

        EtlPackage[] GetEtlPackages();

        EtlPackage GetEtlPackage(string etlPackageId);

        EtlSession InvokeEtlPackage(
            string etlPackageId,
            EtlVariableAssignment[] parameters,
            string parentSessionId,
            string user = null,
            string waitingEtlSessionId = null);

        IEtlLogParser GetEtlLogParser();

        EtlSession CreateWaitingEtlSession(string etlPackageId, string user = null);
    }
}