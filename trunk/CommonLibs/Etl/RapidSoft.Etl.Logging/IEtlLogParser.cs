namespace RapidSoft.Etl.Logging
{
	public interface IEtlLogParser
	{
		EtlSession[] GetEtlSessions(EtlSessionQuery query);

		EtlSession[] GetEtlSessions(EtlSessionPagedQuery query, out int? totalCount);

		EtlSession[] GetLatestEtlSessions(string[] etlPackageIds);

		EtlSession GetEtlSession(string etlPackageId, string etlSessionId);

		EtlVariable[] GetEtlVariables(string etlPackageId, string etlSessionId);

		EtlVariable[] GetEtlVariables(string[] etlPackageIds, string[] etlSessionIds);

		EtlCounter[] GetEtlCounters(string etlPackageId, string etlSessionId);

		EtlCounter[] GetEtlCounters(string[] etlPackageIds, string[] etlSessionIds);

		EtlMessage[] GetEtlMessages(string etlPackageId, string etlSessionId);

		EtlMessage[] GetEtlMessagesPage(
			string etlPackageId, string etlSessionId, int skipMessageCount, int takeMessageCount, out int totalCount);

		EtlMessage GetLastEtlErrorMessage(string etlPackageId, string etlSessionId);
	}
}