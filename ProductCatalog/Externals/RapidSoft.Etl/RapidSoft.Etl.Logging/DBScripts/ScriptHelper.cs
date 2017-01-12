using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace RapidSoft.Etl.Logging.DBScripts
{
	public static class ScriptHelper
    {
        #region Methods

        public static void ExecuteScript(string connectionStringName, string script)
        {
            ExecuteScripts(connectionStringName, new[] { script });
        }

        public static void ExecuteScripts(string connectionStringName, string[] scripts)
        {
            if (scripts == null)
            {
                throw new ArgumentNullException("scripts");
            }

            var settings = ConfigurationManager.ConnectionStrings[connectionStringName];
            var factory = DbProviderFactories.GetFactory(settings.ProviderName);

            using (var conn = factory.CreateConnection())
            {
                conn.ConnectionString = settings.ConnectionString;
                conn.Open();

                foreach (var script in scripts)
                {
                    var batches = GetBatches(script);

                    var command = conn.CreateCommand();
                    command.CommandType = CommandType.Text;

                    foreach (var batchSql in batches)
                    {
                        command.CommandText = batchSql;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static string[] GetBatches(string script)
        {
            if (string.IsNullOrEmpty(script))
            {
                return new string[0];
            }

            var batches = script.Split(new []{"\r\nGO"}, StringSplitOptions.RemoveEmptyEntries);
            return batches;
        }

        #endregion
    }
}
