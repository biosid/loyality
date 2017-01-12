namespace RapidSoft.Etl.Logging.Sql
{
using System;
using System.Collections.Generic;
	using System.Data;
using System.Data.SqlClient;
using System.Linq;

    public sealed class SqlEtlLogParser : IEtlLogParser
	{
		#region Fields

		private readonly string connectionString;
		private readonly string schemaName;
		private readonly string schemaToken;

		#endregion

		#region Constructors

		public SqlEtlLogParser(string connectionString, string schemaName)
		{
			if (connectionString == null)
			{
				throw new ArgumentNullException("connectionString");
			}

			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentException("Parameter \"connectionString\" cannot be empty", "connectionString");
			}

			this.connectionString = connectionString;
			this.schemaName = schemaName;
			this.schemaToken = !string.IsNullOrEmpty(this.schemaName)
								   ? string.Format(
									   "[{0}].", this.schemaName.TrimStart('[').TrimEnd(']').Replace("]", @"\]"))
								   : string.Empty;
		}

		#endregion

		#region Methods

		public EtlSession GetEtlSession(string packageId, string sessionId)
		{
			if (!(IsGuid(packageId) && IsGuid(sessionId)))
			{
				return null;
			}

			var session = SelectEtlSession(packageId, sessionId);
			return session;
		}

		public EtlSession[] GetEtlSessions(EtlSessionQuery query)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}

			if (!IsGuids(query.EtlPackageIds))
			{
				return new EtlSession[0];
			}

			var sessions = SelectEtlSessions(query);
			return sessions.ToArray();
		}

		public EtlSession[] GetEtlSessions(EtlSessionPagedQuery query, out int? totalCount)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}

			if (!IsGuids(query.EtlPackageIds))
			{
				totalCount = query.CalcCount ? (int?)0 : null;
				return new EtlSession[0];
			}

			var sessions = SelectEtlSessions(query, out totalCount);
			return sessions.ToArray();
		}

		public EtlSession[] GetLatestEtlSessions(string[] etlPackageIds)
		{
			if (etlPackageIds == null)
			{
				throw new ArgumentNullException("etlPackageIds");
			}

			if (etlPackageIds.Length == 0 || !IsGuids(etlPackageIds))
			{
				return new EtlSession[0];
			}

			var sessions = SelectLatestEtlSessions(etlPackageIds);
			return sessions.ToArray();
		}

		public EtlVariable[] GetEtlVariables(string etlPackageId, string etlSessionId)
		{
			var variables = SelectEtlVariables(etlPackageId, etlSessionId);
			return variables.ToArray();
		}

		public EtlVariable[] GetEtlVariables(string[] etlPackageIds, string[] etlSessionIds)
		{
			var variables = SelectEtlVariables(etlPackageIds, etlSessionIds);
			return variables.ToArray();
		}

		public EtlCounter[] GetEtlCounters(string etlPackageId, string etlSessionId)
		{
			var counters = SelectEtlCounters(etlPackageId, etlSessionId);
			return counters.ToArray();
		}

		public EtlCounter[] GetEtlCounters(string[] etlPackageIds, string[] etlSessionIds)
		{
			var counters = SelectEtlCounters(etlPackageIds, etlSessionIds);
			return counters.ToArray();
		}

		public EtlMessage[] GetEtlMessages(string etlPackageId, string etlSessionId)
		{
			var messages = SelectEtlMessages(etlPackageId, etlSessionId);
			return messages.ToArray();
		}

		public EtlMessage[] GetEtlMessagesPage(
			string etlPackageId, string etlSessionId, int skipMessageCount, int takeMessageCount, out int totalCount)
		{
			var messages = SelectEtlMessages(etlPackageId, etlSessionId, skipMessageCount, takeMessageCount, out totalCount);
			return messages.ToArray();
		}

		public EtlMessage GetLastEtlErrorMessage(string etlPackageId, string etlSessionId)
		{
		    var message = SelectLastEtlMessage(etlPackageId, etlSessionId, EtlMessageType.Error);
		    return message;
		}

	    private EtlSession SelectEtlSession(string packageId, string sessionId)
		{
			var cmd = CreateSelectEtlSessionCommand(packageId, sessionId);
			using (var conn = CreateConnection())
			{
				conn.Open();
				cmd.Connection = conn;

				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.Read())
					{
						return null;
					}

					var session = this.ReadEtlSession(reader);
					return session;
				}
			}
		}

		private List<EtlSession> SelectEtlSessions(EtlSessionPagedQuery query, out int? totalCount)
		{
			var sessions = new List<EtlSession>();
			const string TotalCountSql = @"SELECT	COUNT(*)
FROM	{0}[EtlSessions] s
WHERE {1}";
			const string SQL = @"SELECT TOP (@take) *
FROM    (	SELECT	ROW_NUMBER() OVER ( ORDER BY [InsertDateTime] DESC ) AS RowNum, *
			FROM	{0}[EtlSessions] s
			WHERE	{1}
		) AS RowConstrainedResult
WHERE   RowNum > @skip
ORDER BY RowNum";

			using (var conn = CreateConnection())
			{
				conn.Open();

				var cmd = conn.CreateCommand();
				var cmdTotalCount = conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmdTotalCount.CommandType = CommandType.Text;

				var packageIdsToken = string.Empty;
				if (query.EtlPackageIds.Count > 0)
				{
					for (var i = 0; i < query.EtlPackageIds.Count; i++)
					{
						var paramName = string.Concat("@pid", i);

						packageIdsToken = i == 0
							? string.Concat("s.[EtlPackageId] in (", paramName)
							: string.Concat(packageIdsToken, ", ", paramName);

						cmd.Parameters.AddWithValue(paramName, query.EtlPackageIds[i]);
						cmdTotalCount.Parameters.AddWithValue(paramName, query.EtlPackageIds[i]);
					}

					packageIdsToken = string.Concat(packageIdsToken, ") ");
				}

				var variablesToken = string.Empty;
				for (var i = 0; i < query.Variables.Count; i++)
				{
					var paramName = string.Concat("@", query.Variables[i].Name);

					if (i != 0)
					{
						variablesToken = string.Concat(variablesToken, " AND ");
					}

				    variablesToken = string.Concat(variablesToken, @" EXISTS (	SELECT 1 FROM {0}[EtlVariables] v 
								WHERE v.[Name] = '", query.Variables[i].Name, @"' 
								  AND v.Value = ", paramName, @"
								  AND v.EtlSessionId = s.EtlSessionId 
								  AND v.EtlPackageId = s.EtlPackageId) ");
					cmd.Parameters.AddWithValue(paramName, query.Variables[i].Value);
					cmdTotalCount.Parameters.AddWithValue(paramName, query.Variables[i].Value);
				}

				var where = packageIdsToken;

				if (where.Length > 0 && variablesToken.Length > 0)
				{
					where = string.Concat(where, " AND ", variablesToken);
				}

				if (where.Length == 0)
				{
					where = "1=1";
				}

				where = string.Format(where, this.schemaToken);

				cmd.CommandText = string.Format(SQL, this.schemaToken, where);
				cmdTotalCount.CommandText = string.Format(TotalCountSql, this.schemaToken, where);

				cmd.Parameters.AddWithValue("@take", query.CountToTake);
				cmd.Parameters.AddWithValue("@skip", query.CountToSkip);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var session = ReadEtlSession(reader);
						sessions.Add(session);
					}
				}

				if (query.CalcCount)
				{
					totalCount = Convert.ToInt32(cmdTotalCount.ExecuteScalar());
				}
				else
				{
					totalCount = null;
				}
			}

			return sessions;
		}

		private List<EtlSession> SelectEtlSessions(EtlSessionQuery query)
		{
			var cmd = CreateSelectEtlSessionsCommand(query);
			var sessions = new List<EtlSession>();

			using (var conn = CreateConnection())
			{
				conn.Open();
				cmd.Connection = conn;

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var session = ReadEtlSession(reader);
						sessions.Add(session);
					}
				}
			}

			return sessions;
		}

		private List<EtlSession> SelectLatestEtlSessions(string[] etlPackageIds)
		{
			var cmd = CreateSelectLatestEtlSessionsCommand(etlPackageIds);
			var sessions = new List<EtlSession>();

			using (var conn = CreateConnection())
			{
				conn.Open();
				cmd.Connection = conn;

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var session = ReadEtlSession(reader);
						sessions.Add(session);
					}
				}
			}

			return sessions;
		}

		private SqlCommand CreateSelectEtlSessionCommand(string packageId, string sessionId)
		{
			var sql = @"
select  
	s.[EtlPackageId],
	s.[EtlPackageName],
	s.[EtlSessionId],
	s.[StartDateTime],
	s.[StartUtcDateTime],
	s.[EndDateTime],
	s.[EndUtcDateTime],
	s.[Status],
	s.[UserName],
	s.[InsertDateTime],
	s.[InsertUtcDateTime]
from 
	{0}[EtlSessions] s with (nolock)
where 
	s.[EtlPackageId] = @EtlPackageId and
	s.[EtlSessionId] = @EtlSessionId
			";

		    var cmd = new SqlCommand { CommandText = string.Format(sql, this.schemaToken) };

		    cmd.Parameters.AddWithValue("@EtlPackageId", packageId);
			cmd.Parameters.AddWithValue("@EtlSessionId", sessionId);

			return cmd;
		}

		private SqlCommand CreateSelectEtlSessionsCommand(EtlSessionQuery query)
		{
			var sql = @"
select {1}
	s.[EtlPackageId],
	s.[EtlPackageName],
	s.[EtlSessionId],
	s.[StartDateTime],
	s.[StartUtcDateTime],
	s.[EndDateTime],
	s.[EndUtcDateTime],
	s.[Status],
	s.[UserName],
	s.[InsertDateTime],
	s.[InsertUtcDateTime]
from 
	{0}[EtlSessions] s with (nolock)
where 
	{2} {3} {4} s.[StartDateTime] between @DateFrom and @DateTo
order by
	s.[InsertDateTime] desc
			";

			var cmd = new SqlCommand();

			var topToken = string.Empty;
			if (query.MaxSessionCount.HasValue)
			{
				topToken = string.Concat("top ", query.MaxSessionCount);
			}

			var packageIdsToken = string.Empty;
			if (query.EtlPackageIds.Count > 0)
			{
				for (var i = 0; i < query.EtlPackageIds.Count; i++)
				{
					var paramName = string.Concat("@pid", i);

					if (i == 0)
					{
						packageIdsToken = string.Concat("s.[EtlPackageId] in (", paramName);
					}
					else
					{
						packageIdsToken = string.Concat(packageIdsToken, ", ", paramName);
					}

					cmd.Parameters.AddWithValue(paramName, query.EtlPackageIds[i]);
				}

				packageIdsToken = string.Concat(packageIdsToken, ") and ");
			}

			var variablesToken = string.Empty;
			if (query.Variables.Count > 0)
			{
				for (var i = 0; i < query.Variables.Count; i++)
				{
					var nameParam = string.Concat("@vn", i);
					var valueParam = string.Concat("@vv", i);

					if (i > 0)
					{
						variablesToken = string.Concat(variablesToken, " or ");
					}

					variablesToken = string.Concat
					(
						variablesToken, 
						"(sv.[Name] = ", 
						nameParam, 
						" and sv.[Value] = ", 
						valueParam,
						")"
					);

					cmd.Parameters.AddWithValue(nameParam, query.Variables[i].Name);
					cmd.Parameters.AddWithValue(valueParam, query.Variables[i].Value);
				}

				variablesToken = string.Concat
				(
					String.Format("exists (select 1 from {0}[EtlVariables] sv with (nolock) where (sv.[EtlPackageId] = s.[EtlPackageId]) and (sv.[EtlSessionId] = s.[EtlSessionId]) and ", this.schemaToken),
					variablesToken, 
					") and "
				);
			}
			
			var etlStatusesToken = string.Empty;
			if (query.EtlStatuses.Count > 0)
			{
				for (var i = 0; i < query.EtlStatuses.Count; i++)
				{
					var paramName = string.Concat("@etlstatus", i);

					if (i == 0)
					{
						etlStatusesToken = string.Concat("s.[Status] in (", paramName);
					}
					else
					{
						etlStatusesToken = string.Concat(etlStatusesToken, ", ", paramName);
					}

					cmd.Parameters.AddWithValue(paramName, query.EtlStatuses[i]);
				}

				etlStatusesToken = string.Concat(etlStatusesToken, ") and ");
			}

			cmd.CommandText = string.Format
			(
				sql, 
				this.schemaToken,
				topToken,
				packageIdsToken,
				variablesToken,
				etlStatusesToken
			);

			cmd.Parameters.AddWithValue("@DateFrom", query.FromDateTime);
			cmd.Parameters.AddWithValue("@DateTo", query.ToDateTime);

			return cmd;
		}

		private SqlCommand CreateSelectLatestEtlSessionsCommand(string[] etlPackageIds)
		{
			var sql = @"
select
	s.[EtlPackageId],
	p.[Name] as [EtlPackageName],
	p.[Text],
	p.[RunIntervalSeconds],
	s.[EtlSessionId],
	s.[StartDateTime],
	s.[StartUtcDateTime],
	s.[EndDateTime],
	s.[EndUtcDateTime],
	s.[Status],
	s.[UserName],
	s.[InsertDateTime],
	s.[InsertUtcDateTime]
from 
	{0}[EtlSessions] s with (nolock)
	inner join 
	(
		select	[EtlPackageId],
				MAX([StartDateTime]) as [StartDateTime]
		from	{0}[EtlSessions] s with (nolock)
		where {1}
		group by [EtlPackageId]
	) as d on s.EtlPackageId = d.EtlPackageId and s.StartDateTime = d.StartDateTime
	inner join
	{0}[EtlPackages] p on p.Id = s.EtlPackageId
order by
	s.[InsertDateTime] desc
			";

			var cmd = new SqlCommand();

			var packageIdsToken = string.Empty;
			if (etlPackageIds != null && etlPackageIds.Length > 0)
			{
				for (var i = 0; i < etlPackageIds.Length; i++)
				{
					var paramName = string.Concat("@pid", i);

					if (i == 0)
					{
						packageIdsToken = string.Concat("s.[EtlPackageId] in (", paramName);
					}
					else
					{
						packageIdsToken = string.Concat(packageIdsToken, ", ", paramName);
					}

					cmd.Parameters.AddWithValue(paramName, etlPackageIds[i]);
				}

				packageIdsToken = string.Concat(packageIdsToken, ")");
			}

			cmd.CommandText = string.Format
			(
				sql,
				this.schemaToken,
				packageIdsToken
			);

			return cmd;
		}

		private EtlSession ReadEtlSession(SqlDataReader reader)
		{
			var session = new EtlSession
			{
				EtlPackageId = EtlValueConverter.ToString(reader["EtlPackageId"]),
				EtlPackageName = EtlValueConverter.ToString(reader["EtlPackageName"]),
				EtlSessionId = EtlValueConverter.ToString(reader["EtlSessionId"]),
				StartDateTime = EtlValueConverter.ParseDateTimeOrNull(reader["StartDateTime"]),
				StartUtcDateTime = EtlValueConverter.ParseDateTimeOrNull(reader["StartUtcDateTime"]),
				EndDateTime = EtlValueConverter.ParseDateTimeOrNull(reader["EndDateTime"]),
				EndUtcDateTime = EtlValueConverter.ParseDateTimeOrNull(reader["EndUtcDateTime"]),
				Status = (EtlStatus)EtlValueConverter.ParseInt32(reader["Status"]),
				UserName = EtlValueConverter.ToString(reader["UserName"]),
				InsertDateTime = EtlValueConverter.ParseDateTime(reader["InsertDateTime"]),
				InsertUtcDateTime = EtlValueConverter.ParseDateTime(reader["InsertUtcDateTime"])
			};

			return session;
		}

		private List<EtlVariable> SelectEtlVariables(string[] etlPackageIds, string[] etlSessionIds)
		{
			const string sql =
@"select 
	sv.[EtlPackageId],
	sv.[EtlSessionId],
	sv.[Name],
	sv.[Modifier],
	sv.[Value],
	sv.[LogDateTime], 
	sv.[LogUtcDateTime], 
	sv.[IsSecure]
from
	{0}[EtlVariables] sv with (nolock)
where 
	{1} 
";
			var variables = new List<EtlVariable>();
			using (var conn = CreateConnection())
			{
				conn.Open();

				var cmd = conn.CreateCommand();
				cmd.CommandType = CommandType.Text;

				var where = string.Empty;
				if (etlPackageIds != null && etlPackageIds.Length > 0)
				{
					for (var i = 0; i < etlPackageIds.Length; i++)
					{
						var paramName = string.Concat("@pid", i);

						where = i == 0
							? string.Concat(where, "sv.[EtlPackageId] in (", paramName)
							: string.Concat(where, ", ", paramName);

						cmd.Parameters.AddWithValue(paramName, etlPackageIds[i]);
					}

					where = string.Concat(where, ")");
				}

				if (etlSessionIds != null && etlSessionIds.Length > 0)
				{
					if (where.Length != 0)
					{
						where = string.Concat(where, " and ");
					}

					for (var i = 0; i < etlSessionIds.Length; i++)
					{
						var paramName = string.Concat("@sid", i);

						where = i == 0
							? string.Concat(where, "sv.[EtlSessionId] in (", paramName)
							: string.Concat(where, ", ", paramName);

						cmd.Parameters.AddWithValue(paramName, etlSessionIds[i]);
					}

					where = string.Concat(where, ")");
				}

				cmd.CommandText = string.Format(sql, this.schemaToken, where);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var es = new EtlVariable
						{
							EtlPackageId = EtlValueConverter.ToString(reader["EtlPackageId"]),
							EtlSessionId = EtlValueConverter.ToString(reader["EtlSessionId"]),
							Name = EtlValueConverter.ToString(reader["Name"]),
							Modifier = (EtlVariableModifier)EtlValueConverter.ParseInt32(reader["Modifier"]),
							Value = EtlValueConverter.ToString(reader["Value"]),
							IsSecure = EtlValueConverter.ParseBoolean(reader["IsSecure"]),
							DateTime = EtlValueConverter.ParseDateTime(reader["LogDateTime"]),
							UtcDateTime = EtlValueConverter.ParseDateTime(reader["LogUtcDateTime"]),
						};

						variables.Add(es);
					}
				}
			}

			return variables;
		}

		private List<EtlVariable> SelectEtlVariables(string etlPackageId, string etlSessionId)
		{
			const string sql =
@"select 
	sv.[Name],
	sv.[Modifier],
	sv.[Value],
	sv.[LogDateTime], 
	sv.[LogUtcDateTime], 
	sv.[IsSecure]
from
	{0}[EtlVariables] sv with (nolock)
where 
	sv.[EtlPackageId] = @EtlPackageId and
	sv.[EtlSessionId] = @EtlSessionId
";
			var variables = new List<EtlVariable>();
			using (var conn = CreateConnection())
			{
				conn.Open();

				var cmd = conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = string.Format(sql, this.schemaToken);
				cmd.Parameters.AddWithValue("@EtlPackageId", etlPackageId);
				cmd.Parameters.AddWithValue("@EtlSessionId", etlSessionId);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var es = new EtlVariable
						{
							EtlPackageId = etlPackageId,
							EtlSessionId = etlSessionId,
							Name = EtlValueConverter.ToString(reader["Name"]),
							Modifier = (EtlVariableModifier)EtlValueConverter.ParseInt32(reader["Modifier"]),
							Value = EtlValueConverter.ToString(reader["Value"]),
							IsSecure = EtlValueConverter.ParseBoolean(reader["IsSecure"]),
							DateTime = EtlValueConverter.ParseDateTime(reader["LogDateTime"]),
							UtcDateTime = EtlValueConverter.ParseDateTime(reader["LogUtcDateTime"]),
						};

						variables.Add(es);
					}
				}
			}

			return variables;
		}

		private List<EtlCounter> SelectEtlCounters(string[] etlPackageIds, string[] etlSessionIds)
		{
			const string SQL =
@"select 
	sv.[EtlPackageId],
	sv.[EtlSessionId],
	sv.[EntityName],
	sv.[CounterName],
	sv.[CounterValue],
	sv.[LogDateTime], 
	sv.[LogUtcDateTime]
from
	{0}[EtlCounters] sv with (nolock)
where 
	{1} 
";
			var counters = new List<EtlCounter>();
			using (var conn = CreateConnection())
			{
				conn.Open();

				var cmd = conn.CreateCommand();
				cmd.CommandType = CommandType.Text;

				var where = string.Empty;
				if (etlPackageIds != null && etlPackageIds.Length > 0)
				{
					for (var i = 0; i < etlPackageIds.Length; i++)
					{
						var paramName = string.Concat("@pid", i);

						where = i == 0
							? string.Concat(where, "sv.[EtlPackageId] in (", paramName)
							: string.Concat(where, ", ", paramName);

						cmd.Parameters.AddWithValue(paramName, etlPackageIds[i]);
					}

					where = string.Concat(where, ")");
				}

				if (etlSessionIds != null && etlSessionIds.Length > 0)
				{
					if (where.Length != 0)
					{
						where = string.Concat(where, " and ");
					}

					for (var i = 0; i < etlSessionIds.Length; i++)
					{
						var paramName = string.Concat("@sid", i);

						where = i == 0
							? string.Concat(where, "sv.[EtlSessionId] in (", paramName)
							: string.Concat(where, ", ", paramName);

						cmd.Parameters.AddWithValue(paramName, etlSessionIds[i]);
					}

					where = string.Concat(where, ")");
				}

				cmd.CommandText = string.Format(SQL, this.schemaToken, where);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var es = new EtlCounter
						{
							EtlPackageId = EtlValueConverter.ToString(reader["EtlPackageId"]),
							EtlSessionId = EtlValueConverter.ToString(reader["EtlSessionId"]),
							EntityName = EtlValueConverter.ToString(reader["EntityName"]),
							CounterName = EtlValueConverter.ToString(reader["CounterName"]),
							CounterValue = EtlValueConverter.ParseInt64(reader["CounterValue"]),
							DateTime = EtlValueConverter.ParseDateTime(reader["LogDateTime"]),
							UtcDateTime = EtlValueConverter.ParseDateTime(reader["LogUtcDateTime"]),
						};

						counters.Add(es);
					}
				}
			}

			return counters;
		}

		private List<EtlCounter> SelectEtlCounters(string etlPackageId, string etlSessionId)
		{
			const string sql =
@"select 
	sv.[EntityName],
	sv.[CounterName],
	sv.[CounterValue],
	sv.[LogDateTime], 
	sv.[LogUtcDateTime]
from
	{0}[EtlCounters] sv with (nolock)
where 
	sv.[EtlPackageId] = @EtlPackageId and
	sv.[EtlSessionId] = @EtlSessionId
";
			var counters = new List<EtlCounter>();
			using (var conn = CreateConnection())
			{
				conn.Open();

				var cmd = conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = string.Format(sql, this.schemaToken);
				cmd.Parameters.AddWithValue("@EtlPackageId", etlPackageId);
				cmd.Parameters.AddWithValue("@EtlSessionId", etlSessionId);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var es = new EtlCounter
						{
							EtlPackageId = etlPackageId,
							EtlSessionId = etlSessionId,
							EntityName = EtlValueConverter.ToString(reader["EntityName"]),
							CounterName = EtlValueConverter.ToString(reader["CounterName"]),
							CounterValue = EtlValueConverter.ParseInt64(reader["CounterValue"]),
							DateTime = EtlValueConverter.ParseDateTime(reader["LogDateTime"]),
							UtcDateTime = EtlValueConverter.ParseDateTime(reader["LogUtcDateTime"]),
						};

						counters.Add(es);
					}
				}
			}

			return counters;
		}

		private List<EtlMessage> ReadMessages(SqlDataReader reader)
		{
			var messages = new List<EtlMessage>();
			while (reader.Read())
			{
				var msg = new EtlMessage
							  {
								  SequentialId = EtlValueConverter.ParseInt64(reader["SequentialId"]),
								  EtlPackageId = EtlValueConverter.ToString(reader["EtlPackageId"]),
								  EtlSessionId = EtlValueConverter.ToString(reader["EtlSessionId"]),
								  EtlStepName = EtlValueConverter.ToString(reader["EtlStepName"]),
								  LogDateTime = EtlValueConverter.ParseDateTime(reader["LogDateTime"]),
								  LogUtcDateTime = EtlValueConverter.ParseDateTime(reader["LogUtcDateTime"]),
								  MessageType =
									  ConvertToEtlMessageType(EtlValueConverter.ParseInt32(reader["MessageType"])),
								  Text = EtlValueConverter.ToString(reader["Text"]),
								  Flags = EtlValueConverter.ParseInt64OrNull(reader["Flags"]),
								  StackTrace = EtlValueConverter.ToString(reader["StackTrace"]),
							  };

				messages.Add(msg);
			}

			return messages;
		}
		
		private List<EtlMessage> SelectEtlMessages(string etlPackageId, string etlSessionId, int skip, int take, out int totalCount)
		{
			const string SQLTotalCount =
	 @"select COUNT(*)
from
	{0}[EtlMessages] m with (nolock)
where 
	m.[EtlPackageId] = @EtlPackageId and
	m.[EtlSessionId] = @EtlSessionId
";
			const string SQL =
 @"SELECT TOP (@take) rowed.[SequentialId],
	rowed.[EtlPackageId],
	rowed.[EtlSessionId],
	rowed.[EtlStepName],
	rowed.[LogDateTime],
	rowed.[LogUtcDateTime],
	rowed.[MessageType],
	rowed.[Text],
	rowed.[Flags],
	rowed.[StackTrace]
FROM	(
		select 
			m.*,
			ROW_NUMBER() OVER (order by m.[SequentialId]) AS [RowNum]
		from
			{0}[EtlMessages] m with (nolock)
		where 
			m.[EtlPackageId] = @EtlPackageId and
			m.[EtlSessionId] = @EtlSessionId
		) rowed
WHERE rowed.[RowNum] > @skip
order by 
	rowed.[SequentialId]
";
			
			using (var conn = CreateConnection())
			{
				conn.Open();

				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = string.Format(SQLTotalCount, this.schemaToken);
					cmd.Parameters.AddWithValue("@EtlPackageId", etlPackageId);
					cmd.Parameters.AddWithValue("@EtlSessionId", etlSessionId);

					var retVal = cmd.ExecuteScalar();
					totalCount = Convert.ToInt32(retVal);
				}

				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = string.Format(SQL, this.schemaToken);
					cmd.Parameters.AddWithValue("@EtlPackageId", etlPackageId);
					cmd.Parameters.AddWithValue("@EtlSessionId", etlSessionId);
					cmd.Parameters.AddWithValue("@take", take);
					cmd.Parameters.AddWithValue("@skip", skip);

					using (var reader = cmd.ExecuteReader())
					{
						var messages = ReadMessages(reader);
						return messages;
					}
				}
			}
		}

		private List<EtlMessage> SelectEtlMessages(string etlPackageId, string etlSessionId)
		{
			const string sql =
 @"select 
	m.[SequentialId],
	m.[EtlPackageId],
	m.[EtlSessionId],
	m.[EtlStepName],
	m.[LogDateTime],
	m.[LogUtcDateTime],
	m.[MessageType],
	m.[Text],
	m.[Flags],
	m.[StackTrace]
from
	{0}[EtlMessages] m with (nolock)
where 
	m.[EtlPackageId] = @EtlPackageId and
	m.[EtlSessionId] = @EtlSessionId
order by 
	m.[SequentialId]
";
			using (var conn = CreateConnection())
			{
				conn.Open();

				var cmd = conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = string.Format(sql, this.schemaToken);
				cmd.Parameters.AddWithValue("@EtlPackageId", etlPackageId);
				cmd.Parameters.AddWithValue("@EtlSessionId", etlSessionId);

				using (var reader = cmd.ExecuteReader())
				{
					var messages = ReadMessages(reader);
					return messages;
				}
			}
		}

        private EtlMessage SelectLastEtlMessage(string etlPackageId, string etlSessionId, EtlMessageType type)
        {
            const string sql =
 @"select TOP (1) 
	m.[SequentialId],
	m.[EtlPackageId],
	m.[EtlSessionId],
	m.[EtlStepName],
	m.[LogDateTime],
	m.[LogUtcDateTime],
	m.[MessageType],
	m.[Text],
	m.[Flags],
	m.[StackTrace]
		from
			{0}[EtlMessages] m with (nolock)
		where 
			m.[EtlPackageId] = @EtlPackageId and
			m.[EtlSessionId] = @EtlSessionId and
			m.MessageType = @Type
ORDER BY m.SequentialId DESC
";
            using (var conn = CreateConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(sql, this.schemaToken);
                cmd.Parameters.AddWithValue("@EtlPackageId", etlPackageId);
                cmd.Parameters.AddWithValue("@EtlSessionId", etlSessionId);
                cmd.Parameters.AddWithValue("@Type", type);

                using (var reader = cmd.ExecuteReader())
                {
                    var messages = ReadMessages(reader);
                    return messages.FirstOrDefault();
                }
            }
        }

		private static EtlMessageType ConvertToEtlMessageType(int value)
		{
            return (EtlMessageType)EtlValueConverter.ParseInt32(value);
		}

		private SqlConnection CreateConnection()
		{
			var conn = new SqlConnection(this.connectionString);
			return conn;
		}
		
		private static bool IsGuid(string str)
		{
			try
			{
				var g = new Guid(str);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		private static bool IsGuids(IEnumerable<string> strings)
		{
			try
			{
				foreach (var str in strings)
				{
					var g = new Guid(str);
				}
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		#endregion
	}
}