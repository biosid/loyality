namespace RapidSoft.Etl.Runtime.DataSources.DB
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class FileCounterDataSource
    {
        private const string ProviderName = "System.Data.SqlClient";

        private const string SaveSql = @"
UPDATE {0}.[EtlFileCounter]
SET [FileCount] = @FileCount,
[EtlSessionId] = @EtlSessionId
WHERE 
FileTemplate = @FileTemplate
AND 
CounterDate = @CounterDate
if (@@ROWCOUNT = 0)
INSERT INTO {0}.[EtlFileCounter]
           ([EtlPackageId]
           ,[EtlSessionId]
           ,[FileTemplate]
           ,[FileCount]
           ,[CounterDate])
     VALUES
           (@EtlPackageId
           ,@EtlSessionId
           ,@FileTemplate
           ,@FileCount
           ,@CounterDate);";

        private const string SelectSql = @"
SELECT FileCount
FROM {0}.[EtlFileCounter]
WHERE 
FileTemplate = @FileTemplate
AND 
CounterDate = @CounterDate";

        private const int InitialIndex = 1;

        private readonly string dbSchemaName;

        private readonly string connectionString;

        private readonly string etlPackageId;

	    private readonly string etlSessionId;

	    private readonly string fileTemplate;

	    private readonly DateTime counterDate;

		public FileCounterDataSource(string connectionString, string etlPackageId, string etlSessionId, string fileTemplate, string dbSchemaName = "dbo", DateTime? counterDate = null)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            if (connectionString.Trim() == string.Empty)
            {
                throw new ArgumentException(string.Format("Parameter \"{0}\" cannot be empty", "connectionString"));
            }

            if (dbSchemaName == null)
            {
                throw new ArgumentNullException("dbSchemaName");
            }

            if (string.IsNullOrWhiteSpace(dbSchemaName))
            {
                throw new ArgumentException(string.Format("Parameter \"{0}\" cannot be empty", "dbSchemaName"));
            }

			if (etlPackageId == null)
			{
				throw new ArgumentNullException("etlPackageId");
			}

			if (etlSessionId == null)
			{
				throw new ArgumentNullException("etlSessionId");
			}

			if (fileTemplate == null)
			{
				throw new ArgumentNullException("fileTemplate");
			}

			this.counterDate = counterDate ?? DateTime.Now.Date;
		    this.connectionString = connectionString;
		    this.etlPackageId = etlPackageId;
			this.etlSessionId = etlSessionId;
			this.fileTemplate = fileTemplate;

			this.dbSchemaName = dbSchemaName;

            /*
             * if (ProviderName != "System.Data.SqlClient")
             * {
             *     throw new NotSupportedException(string.Format("Provider \"{0}\" not supported. Use System.Data.SqlClient"));
             * }
             */
        }

        public int GetNextFileNumber()
        {
            using (var connection = CreateConnection())
            {
                var cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(SelectSql, this.dbSchemaName);

                cmd.Parameters.Add(new SqlParameter("@EtlSessionId", etlSessionId));
                cmd.Parameters.Add(new SqlParameter("@FileTemplate", fileTemplate));
                cmd.Parameters.Add(new SqlParameter("@CounterDate", counterDate.ToString("yyyy-MM-dd")));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0) + 1;
                    }
                }
            }

            return InitialIndex;
        }

		public int SaveFileNumber(int fileNumber)
		{
            using(var connection = this.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = string.Format(SaveSql, this.dbSchemaName);

                command.Parameters.Add(new SqlParameter("@EtlPackageId", this.etlPackageId));
                command.Parameters.Add(new SqlParameter("@EtlSessionId", this.etlSessionId));
                command.Parameters.Add(new SqlParameter("@FileTemplate", this.fileTemplate));
                command.Parameters.Add(new SqlParameter("@CounterDate", this.counterDate));
                command.Parameters.Add(new SqlParameter("@FileCount", fileNumber));
                command.ExecuteNonQuery();

                return InitialIndex;   
            }
		}

        private DbConnection CreateConnection()
        {
            var factory = DbProviderFactories.GetFactory(ProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
        }
    }
}