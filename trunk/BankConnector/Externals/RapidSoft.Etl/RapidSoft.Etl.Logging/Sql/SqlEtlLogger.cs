namespace RapidSoft.Etl.Logging.Sql
{
using System;
    using System.Data;
using System.Data.SqlClient;
using System.Text;

    public sealed class SqlEtlLogger : IEtlLogger
    {
        #region Constants

        private const string DBProviderName = "System.Data.SqlClient";

        #endregion

        #region Constructors

        public SqlEtlLogger(string connectionString, string schemaName)
            : this(DBProviderName, connectionString, schemaName)
        {
        }

        private SqlEtlLogger(string providerName, string connectionString, string schemaName)
        {
            if (providerName == null)
            {
                throw new ArgumentNullException("providerName");
            }

            if (providerName == string.Empty)
            {
                throw new ArgumentException("Parameter \"providerName\" cannot be empty", "providerName");
            }

            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            if (connectionString == string.Empty)
            {
                throw new ArgumentException("Parameter \"connectionString\" cannot be empty", "connectionString");
            }

            _connectionString = connectionString;
            _schemaToken = string.IsNullOrEmpty(schemaName) ? string.Empty : string.Concat("[", schemaName.TrimStart('[').TrimEnd(']'), "].");
        }

        #endregion

        #region Constants

        private const string SecureVariableEscapedValue = "*";

        #endregion

        #region Fields

        public static readonly int EtlSessionStartMessageMaxLength = 1000;
        public static readonly int EtlSessionEndMessageMaxLength = 1000;
        public static readonly int EtlSessionUserNameMaxLength = 50;

        public static readonly int EtlMessageTextMaxLength = 1000;
        public static readonly int EtlMessageErrorObjectIdMaxLength = 255;
        public static readonly int EtlMessageErrorTypeMaxLength = 255;
        public static readonly int EtlMessageErrorTraceMaxLength = 1000;

        public static readonly int EtlStepSourceNameMaxLength = 255;
        public static readonly int EtlStepSourceTypeMaxLength = 255;
        public static readonly int EtlStepSourcePathMaxLength = 2000;

        public static readonly int EtlSessionVariableNameMaxLength = 50;
        public static readonly int EtlSessionVariableValueMaxLength = 1000;

        private readonly string _connectionString;
        private readonly string _schemaToken;

        #endregion

        #region IEtlLogger Members

        public void LogEtlSessionStart(EtlSession session)
		{
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            var insertSessionSql = string.Format(@"
insert into {0}[EtlSessions]
(
    [EtlPackageId],
    [EtlPackageName],
    [EtlSessionId],
    [StartDateTime],
    [StartUtcDateTime],
    [Status],
    [ParentEtlSessionId],
    [UserName]
)
values
(
    @EtlPackageId,
    @EtlPackageName,
    @EtlSessionId,
    @StartDateTime,
    @StartUtcDateTime,
    @Status,
    @ParentEtlSessionId,
    @userName
)
            ", _schemaToken);

            var insertSessionCommand = new SqlCommand(insertSessionSql);
            AddParameter(insertSessionCommand, "@EtlPackageId", session.EtlPackageId);
            AddParameter(insertSessionCommand, "@EtlPackageName", session.EtlPackageName);
            AddParameter(insertSessionCommand, "@EtlSessionId", session.EtlSessionId);
            AddParameter(insertSessionCommand, "@StartDateTime", session.StartDateTime);
            AddParameter(insertSessionCommand, "@StartUtcDateTime", session.StartUtcDateTime);
            AddParameter(insertSessionCommand, "@Status", (int)session.Status);
            AddParameter(insertSessionCommand, "@ParentEtlSessionId", session.ParentEtlSessionId);
            AddParameter(insertSessionCommand, "@UserName", GetSubstring(session.UserName, EtlSessionUserNameMaxLength));

            using (var conn = CreateConnection())
            {
                conn.Open();

                insertSessionCommand.Connection = conn;
                insertSessionCommand.ExecuteNonQuery();
            }
        }

        public void LogEtlSessionContinue(EtlSession session)
        {
            var updateSessionSql = string.Format(@"
delete from {0}[EtlVariables]
where [EtlSessionId] = @EtlSessionId

update {0}[EtlSessions]
set [StartDateTime] = @StartDateTime,
    [StartUtcDateTime] = @StartUtcDateTime,
    [Status] = @Status
where [EtlSessionId] = @EtlSessionId
            ", this._schemaToken);
            var now = DateTime.Now;

            var insertSessionCommand = new SqlCommand(updateSessionSql);
            this.AddParameter(insertSessionCommand, "@EtlSessionId", session.EtlSessionId);
            this.AddParameter(insertSessionCommand, "@StartDateTime", now);
            this.AddParameter(insertSessionCommand, "@StartUtcDateTime", now.ToUniversalTime());
            this.AddParameter(insertSessionCommand, "@Status", (int)EtlStatus.Started);

            using (var conn = this.CreateConnection())
            {
                conn.Open();

                insertSessionCommand.Connection = conn;
                insertSessionCommand.ExecuteNonQuery();
            }
        }
        
        public void LogEtlSessionEnd(EtlSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            var cmdText = string.Format(@"
update {0}[EtlSessions]
set
	[EndDateTime] = getdate(),
	[EndUtcDateTime] = getutcdate(),
	[Status] = @Status
where
	[EtlPackageId] = @EtlPackageId and
	[EtlSessionId] = @EtlSessionId
            ", _schemaToken);

            using (var conn = CreateConnection())
            {
                conn.Open();

                var cmd = new SqlCommand(cmdText, conn);
                AddParameter(cmd, "@EtlPackageId", session.EtlPackageId);
                AddParameter(cmd, "@EtlSessionId", session.EtlSessionId);
                AddParameter(cmd, "@EndDateTime", session.EndDateTime);
                AddParameter(cmd, "@EndUtcDateTime", session.EndUtcDateTime);
                AddParameter(cmd, "@Status", (int)session.Status);

                cmd.ExecuteNonQuery();
            }
        }

        public void LogEtlVariable(EtlVariable variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException("variable");
            }

            var logDateTime = DateTime.Now;

            var sb = new StringBuilder();
            sb.Append("insert into ");
            sb.Append(_schemaToken);
            sb.Append("[EtlVariables]");
            sb.Append("([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [Name], [Modifier], [Value], [IsSecure])");
            sb.Append(" values ");
            sb.Append("(@pid, @sid, @dt, @udt, @name, @mod, @val, @sec)");
            var sql = sb.ToString();

            using (var conn = CreateConnection())
            {
                conn.Open();

                var now = DateTime.Now;

                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                AddParameter(cmd, "@pid", variable.EtlPackageId);
                AddParameter(cmd, "@sid", variable.EtlSessionId);
                AddParameter(cmd, "@dt", variable.DateTime);
                AddParameter(cmd, "@udt", variable.UtcDateTime);
                AddParameter(cmd, "@name", variable.Name);
                AddParameter(cmd, "@mod", (int)variable.Modifier);
                AddParameter(cmd, "@val", variable.Value);
                AddParameter(cmd, "@sec", variable.IsSecure);

                cmd.ExecuteNonQuery();
            }
        }

        public void LogEtlCounter(EtlCounter counter)
        {
            if (counter == null)
            {
                throw new ArgumentNullException("counter");
            }

            var logDateTime = DateTime.Now;
            var sb = new StringBuilder();

            sb.Append("update ");
            sb.Append(_schemaToken);
            sb.Append("[EtlCounters] set ");
            sb.Append("[CounterValue] = @val, ");
            sb.Append("[LogDateTime] = @dt, ");
            sb.Append("[LogUtcDateTime] = @udt ");
            sb.Append("where ");
            sb.Append("([EtlPackageId] = @pid) and ");
            sb.Append("([EtlSessionId] = @sid) and ");
            sb.Append("([EntityName] = @ent) and ");
            sb.Append("([CounterName] = @name) ");

            sb.Append("if (@@ROWCOUNT = 0) ");
            sb.Append("insert into ");
            sb.Append(_schemaToken);
            sb.Append("[EtlCounters]([EtlPackageId], [EtlSessionId], [EntityName], [CounterName], [CounterValue], [LogDateTime], [LogUtcDateTime]) ");
            sb.Append("values (@pid, @sid, @ent, @name, @val, @dt, @udt)");

            var sql = sb.ToString();

            using (var conn = CreateConnection())
            {
                conn.Open();

                var now = DateTime.Now;

                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                AddParameter(cmd, "@pid", counter.EtlPackageId);
                AddParameter(cmd, "@sid", counter.EtlSessionId);
                AddParameter(cmd, "@dt", counter.DateTime);
                AddParameter(cmd, "@udt", counter.UtcDateTime);
                AddParameter(cmd, "@ent", counter.EntityName);
                AddParameter(cmd, "@name", counter.CounterName);
                AddParameter(cmd, "@val", counter.CounterValue);

                cmd.ExecuteNonQuery();
            }
        }

        public void LogEtlMessage(EtlMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var cmdText = string.Format(@"
insert into {0}[EtlMessages]
(
	[EtlPackageId],
	[EtlSessionId],
    [EtlStepName],
	[LogDateTime],
	[LogUtcDateTime],
	[MessageType],
	[Text],
    [Flags],
	[StackTrace]
)
values
(
	@pid,
	@sid,
    @stn,
	@dt,
	@udt,
	@mt,
	@t,
    @f,
	@tr
)

            ", _schemaToken);

            using (var conn = CreateConnection())
            {
                conn.Open();

                var cmd = new SqlCommand(cmdText, conn);

	            AddParameter(cmd, "@pid", message.EtlPackageId);
	            AddParameter(cmd, "@sid", message.EtlSessionId);
                AddParameter(cmd, "@stn", message.EtlStepName);
	            AddParameter(cmd, "@dt", message.LogDateTime);
	            AddParameter(cmd, "@udt", message.LogUtcDateTime);
	            AddParameter(cmd, "@mt", (int)message.MessageType);
	            AddParameter(cmd, "@t", GetSubstring(message.Text, EtlMessageTextMaxLength));
                AddParameter(cmd, "@f", message.Flags);
	            AddParameter(cmd, "@tr", GetSubstring(message.StackTrace, EtlMessageErrorTraceMaxLength));

                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region Methods

        public string EscapeEtlParameterValue(EtlVariable parameter)
        {
            if (parameter.IsSecure)
            {
                return SecureVariableEscapedValue;
            }
            else
            {
                return parameter.Value;
            }
        }

        private string GetSubstring(string str, int length)
        {
            if (str == null)
            {
                return null;
            }
            else
            {
                return str.Substring(0, Math.Min(str.Length, length));
            }
        }

        private void AddParameter(SqlCommand command, string parameterName, object value)
        {
            if (value == null)
            {
                command.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parameterName, value);
            }
        }

        private SqlConnection CreateConnection()
        {
            var conn = new SqlConnection(_connectionString);
            return conn;
        }

        #endregion
    }
}