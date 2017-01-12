using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using RapidSoft.Etl.Logging.Sql;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Agents.Sql
{
    using System.Threading;

    public sealed class SqlEtlAgent : ILocalEtlAgent
    {
        #region Constructors

        public SqlEtlAgent(EtlAgentInfo agentInfo)
		{
            if (agentInfo == null)
            {
                throw new ArgumentNullException("agentInfo");
            }

            if (string.IsNullOrEmpty(agentInfo.ConnectionString))
            {
                throw new ArgumentException("Parameter \"connectionString\" cannot be empty", "connectionString");
            }

            _connectionString = agentInfo.ConnectionString;
            _schemaName = agentInfo.SchemaName;

            if (!string.IsNullOrEmpty(_schemaName))
            {
                _schemaToken = string.Format("[{0}].", _schemaName.TrimStart('[').TrimEnd(']').Replace("]", @"\]"));
            }
            else
            {
                _schemaToken = "";
            }
        }

        #endregion

        #region Constants

        private const int PRIMARY_KEY_VIOLATION_NUMBER = 2627;

        #endregion

        #region Fields

        private readonly string _connectionString;
        private readonly string _schemaName;
        private readonly string _schemaToken;

        private readonly List<IEtlLogger> _attachedLoggers = new List<IEtlLogger>();

        #endregion

        #region IEtlAgent Members

        public EtlAgentInfo GetEtlAgentInfo()
        {
            return new EtlAgentInfo
            {
                EtlAgentType = this.GetType().AssemblyQualifiedName,
                ConnectionString = _connectionString,
                SchemaName = _schemaName,
            };
        }

        public void DeployEtlPackage(EtlPackage package, EtlPackageDeploymentOptions options)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            if (options == null)
            {
                options = new EtlPackageDeploymentOptions();
            }

            if (options.Overwrite)
            {
                InsertOrUpdateEtlPackage(package);
            }
            else
            {
                InsertPackage(package);
            }
        }

        public EtlPackage[] GetEtlPackages()
        {
            var packages = SelectEtlPackages();
            return packages.ToArray();
        }

        public EtlPackage GetEtlPackage(string etlPackageId)
        {
            var packageInfo = SelectEtlPackage(etlPackageId);
            return packageInfo;
        }

        public EtlSession InvokeEtlPackage(
            string etlPackageId,
            EtlVariableAssignment[] parameters,
            string parentSessionId,
            string user = null,
            string waitingEtlSessionId = null)
        {
            var package = SelectEtlPackage(etlPackageId);
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            EtlSession waitingEtlSession = null;
            if (!string.IsNullOrEmpty(waitingEtlSessionId))
            {
                var parser = this.GetEtlLogParser();
                waitingEtlSession = parser.GetEtlSession(etlPackageId, waitingEtlSessionId);
            }

            using (var defaultLogger = CreateDefaultLogger())
            {
                var loggers = new IEtlLogger[_attachedLoggers.Count + 1];
                loggers[0] = defaultLogger;
                for (var i = 0; i < _attachedLoggers.Count; i++)
                {
                    loggers[i + 1] = _attachedLoggers[i];
                }

                var session = package.Invoke(loggers, parameters, parentSessionId, user, waitingEtlSession);
                return session;
            }
        }

        public IEtlLogParser GetEtlLogParser()
        {
            var logParser = new SqlEtlLogParser(_connectionString, _schemaName);
            return logParser;
        }

        public EtlSession CreateWaitingEtlSession(string etlPackageId, string user = null)
        {
            var package = this.GetEtlPackage(etlPackageId);
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            var session = new EtlSession
                              {
                                  EtlSessionId = Guid.NewGuid().ToString(),
                                  EtlPackageId = package.Id,
                                  EtlPackageName = package.Name,
                                  ParentEtlSessionId = null,
                                  Status = EtlStatus.Waiting,
                                  UserName = user ?? GetCurrentUserName()
                              };

            CreateDefaultLogger().LogEtlSessionStart(session);

            return session;
        }

        #endregion

        #region ILocalEtlAgent Members

        public void AttachLogger(IEtlLogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (!_attachedLoggers.Contains(logger))
            {
                _attachedLoggers.Add(logger);
            }
        }

        #endregion

        #region Security
// TODO: Это точно надо???
        private static string GetCurrentUserName()
        {
            if
            (
                Thread.CurrentPrincipal != null &&
                Thread.CurrentPrincipal.Identity != null &&
                Thread.CurrentPrincipal.Identity.IsAuthenticated
            )
            {
                return
                    string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name)
                    ? null
                    : Thread.CurrentPrincipal.Identity.Name;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Methods

        public void InsertPackage(EtlPackage package)
        {
            var sql = @"
insert into {0}[EtlPackages]([Id], [Name], [Enabled], [RunIntervalSeconds], [Text])
values (@id, @name, @enabled, @ris, @text)
";

            var packageText = EtlPackageXmlSerializer.Serialize(package);

            using (var conn = CreateConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(sql, _schemaToken);

                cmd.Parameters.AddWithValue("id", package.Id);
                cmd.Parameters.AddWithValue("name", package.Name);
                cmd.Parameters.AddWithValue("enabled", package.Enabled);
                cmd.Parameters.AddWithValue("ris", package.RunIntervalSeconds);
                cmd.Parameters.AddWithValue("text", packageText);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException exc)
                {
                    if (exc.Number == PRIMARY_KEY_VIOLATION_NUMBER)
                    {
                        throw new EtlPackageAlreadyExistsException(package.Id, null, exc);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void InsertOrUpdateEtlPackage(EtlPackage package)
        {
            var sql = @"
if exists(select 1 from {0}[EtlPackages] p with (nolock) where p.[Id] = @id)
update {0}[EtlPackages] set [Name] = @name, [Enabled] = @enabled, [RunIntervalSeconds] = @ris, [Text] = @text 
where [Id] = @id
else
insert into {0}[EtlPackages]([Id], [Name], [Enabled], [RunIntervalSeconds], [Text])
values (@id, @name, @enabled, @ris, @text)
";

            var packageText = EtlPackageXmlSerializer.Serialize(package);

            using (var conn = CreateConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(sql, _schemaToken);

                cmd.Parameters.AddWithValue("id", package.Id);
                cmd.Parameters.AddWithValue("name", package.Name);
                cmd.Parameters.AddWithValue("enabled", package.Enabled);
                cmd.Parameters.AddWithValue("ris", package.RunIntervalSeconds);
                cmd.Parameters.AddWithValue("text", packageText);

                cmd.ExecuteNonQuery();
            }
        }

        private List<EtlPackage> SelectEtlPackages()
        {
            var sql = @"
select 
    p.[Id],
    p.[Name],
    p.[RunIntervalSeconds],
    p.[Enabled]
from 
    {0}[EtlPackages] p with (nolock)
order by 
    p.[Name]
";

            var packages = new List<EtlPackage>();

            using (var conn = CreateConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(sql, _schemaToken);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        packages.Add
                        (
                            new EtlPackage
                            {
                                Id = EtlValueConverter.ToString(reader["Id"]),
                                Name = EtlValueConverter.ToString(reader["Name"]),
                                Enabled = EtlValueConverter.ParseBoolean(reader["Enabled"]),
                                RunIntervalSeconds = Convert.ToInt32(reader["RunIntervalSeconds"]),
                            }
                        );
                    }
                }
            }

            return packages;
        }

        private EtlPackage SelectEtlPackage(string etlPackageId)
        {
            var sql = @"
select top 1
    p.[Id],
    p.[Name],
    p.[RunIntervalSeconds],
    p.[Text],
    p.[Enabled]
from 
    {0}[EtlPackages] p with (nolock)
where
    p.[Id] = @id
";

            EtlPackage package = null;

            using (var conn = CreateConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(sql, _schemaToken);
                cmd.Parameters.AddWithValue("id", etlPackageId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var id = EtlValueConverter.ToString(reader["Id"]);
                        var name = EtlValueConverter.ToString(reader["Name"]);
                        var enabled = EtlValueConverter.ParseBoolean(reader["Enabled"]);
                        var runIntervalSeconds = Convert.ToInt32(reader["RunIntervalSeconds"]);
                        var text = EtlValueConverter.ToString(reader["Text"]);

                        if (!string.IsNullOrEmpty(text))
                        {
                            package = EtlPackageXmlSerializer.Deserialize(text);
                        }
                        else
                        {
                            package = new EtlPackage();
                        }

                        package.Id = id;
                        package.Name = name;
                        package.Enabled = enabled;
                        package.RunIntervalSeconds = runIntervalSeconds;
                    }
                }
            }

            return package;
        }

        private SqlConnection CreateConnection()
        {
            var conn = new SqlConnection(_connectionString);
            return conn;
        }

        public IEtlLogger CreateDefaultLogger()
        {
            var logger = new SqlEtlLogger(_connectionString, _schemaName);
            return logger;
        }

        #endregion
    }
}