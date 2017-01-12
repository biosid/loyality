namespace RapidSoft.Etl.Runtime.DataSources.DB
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class MailDataSource : IDisposable
    {
        private const string ProviderName = "System.Data.SqlClient";

        private const string InsertSql = @"
INSERT INTO {0}.[EtlIncomingMails]
([EtlPackageId], [EtlSessionId],[MessageUid],[MessageRaw])
     VALUES
(@EtlPackageId,@EtlSessionId,@MessageUid,@MessageRaw)";

        private const string InsertAttachSql = @"
INSERT INTO {0}.[EtlIncomingMailAttachments]
([EtlPackageId], [EtlSessionId], [FileName])
     VALUES
(@EtlPackageId,@EtlSessionId,@FileName)";

        private const string SelectSql = @"
SELECT [MessageUid]
  FROM {0}.[EtlIncomingMails]
WHERE [EtlSessionId] = @EtlSessionId AND [IsDeleted] = 0";

        private const string SelectMaxUID = @"
SELECT ISNULL(MAX([MessageUid]), 0) FROM {0}.[EtlIncomingMails]
WHERE [IsDeleted] = 1 AND [EtlPackageId] = @EtlPackageId";

        private const string UpdateAsDeleted = @"
UPDATE {0}.[EtlIncomingMails]
   SET [IsDeleted] = 1
 WHERE [EtlSessionId] = @EtlSessionId
   AND [MessageUid] = @MessageUid";

        private const string InsertOutcomingMails = @"INSERT INTO {0}.[EtlOutcomingMails]
([EtlPackageId],[EtlSessionId],[Subject],[From],[To])
     VALUES
(@EtlPackageId,@EtlSessionId,@Subject,@From,@To)

SELECT SCOPE_IDENTITY()";

        private const string InsertOutcommintAttachmet = @"INSERT INTO {0}.[EtlOutcomingMailAttachments]
([EtlOutcomingMailId],[EtlPackageId],[EtlSessionId],[FileName])
     VALUES
(@EtlOutcomingMailId,@EtlPackageId,@EtlSessionId,@FileName)";

        private readonly string dbSchemaName;

        private readonly DbConnection connection;

        public MailDataSource(string connectionString, string dbSchemaName = "dbo")
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

            this.dbSchemaName = dbSchemaName;

            /*
             * if (ProviderName != "System.Data.SqlClient")
             * {
             *     throw new NotSupportedException(string.Format("Provider \"{0}\" not supported. Use System.Data.SqlClient"));
             * }
             */

            var factory = DbProviderFactories.GetFactory(ProviderName);
            this.connection = factory.CreateConnection();
            this.connection.ConnectionString = connectionString;
            this.connection.Open();
        }

        public void SaveIncomingMessage(string etlPackageId, string etlSessionId, uint messageUid, string messageRaw)
        {
            string innerMessageRaw;
            innerMessageRaw = messageRaw;

            var cmd = this.connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format(InsertSql, this.dbSchemaName);

            cmd.Parameters.Add(new SqlParameter("@EtlPackageId", etlPackageId));
            cmd.Parameters.Add(new SqlParameter("@EtlSessionId", etlSessionId));
            cmd.Parameters.Add(new SqlParameter("@MessageUid", (long)messageUid));
            cmd.Parameters.Add(new SqlParameter("@MessageRaw", innerMessageRaw));

            cmd.ExecuteNonQuery();
        }

        public void SaveOutcomingMessageWithAttachments(string etlPackageId, string etlSessionId, string subject, string from, string[] to, string[] attachments)
        {
            var id = this.SaveOutcomingMessage(etlPackageId, etlSessionId, subject, @from, to);

            this.SaveOutcomingAttachments(etlPackageId, etlSessionId, attachments, id);
        }

        public void SaveIncomingMessageAttachment(string etlPackageId, string etlSessionId, string attachFileName)
        {
            var cmd = this.connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format(InsertAttachSql, this.dbSchemaName);

            cmd.Parameters.Add(new SqlParameter("@EtlPackageId", etlPackageId));
            cmd.Parameters.Add(new SqlParameter("@EtlSessionId", etlSessionId));
            cmd.Parameters.Add(new SqlParameter("@FileName", attachFileName));

            cmd.ExecuteNonQuery();
        }

        public IList<uint> GetIncomingMessagesUid(string etlSessionId)
        {
            var cmd = this.connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format(SelectSql, this.dbSchemaName);
            cmd.Parameters.Add(new SqlParameter("@EtlSessionId", etlSessionId));

            var uids = new List<uint>();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var uid = reader.GetInt64(0);

                    uids.Add((uint)uid);
                }
            }

            return uids;
        }

        public uint GetMaxDeletedIncomingMessageUid(string etlPackageId)
        {
            var cmd = this.connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format(SelectMaxUID, this.dbSchemaName);
            cmd.Parameters.Add(new SqlParameter("@EtlPackageId", etlPackageId));

            var retVal = Convert.ToInt64(cmd.ExecuteScalar());

            return (uint)retVal;
        }

        public void MarkIncomingMessageAsDeleted(string etlSessionId, uint messageUid)
        {
            var cmd = this.connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format(UpdateAsDeleted, this.dbSchemaName);

            cmd.Parameters.Add(new SqlParameter("@EtlSessionId", etlSessionId));
            cmd.Parameters.Add(new SqlParameter("@MessageUid", (long)messageUid));

            cmd.ExecuteNonQuery();
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.connection.Dispose();
        }

        #endregion

        private void SaveOutcomingAttachments(string etlPackageId, string etlSessionId, IEnumerable<string> attachments, long messageId)
        {
            using (var cmd = this.connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(InsertOutcommintAttachmet, this.dbSchemaName);

                cmd.Parameters.Add(new SqlParameter("@EtlOutcomingMailId", messageId));
                cmd.Parameters.Add(new SqlParameter("@EtlPackageId", etlPackageId));
                cmd.Parameters.Add(new SqlParameter("@EtlSessionId", etlSessionId));

                foreach (var attachment in attachments)
                {
                    if (cmd.Parameters.Contains("@FileName"))
                    {
                        cmd.Parameters["@FileName"].Value = attachment;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@FileName", attachment));
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private long SaveOutcomingMessage(string etlPackageId, string etlSessionId, string subject, string @from, string[] to)
        {
            using (var cmd = this.connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(InsertOutcomingMails, this.dbSchemaName);

                cmd.Parameters.Add(new SqlParameter("@EtlPackageId", etlPackageId));
                cmd.Parameters.Add(new SqlParameter("@EtlSessionId", etlSessionId));
                cmd.Parameters.Add(new SqlParameter("@Subject", subject));
                cmd.Parameters.Add(new SqlParameter("@From", @from));
                cmd.Parameters.Add(new SqlParameter("@To", string.Join("; ", to)));

                var dirtyId = cmd.ExecuteScalar();

                return Convert.ToInt64(dirtyId);
            }
        }
    }
}