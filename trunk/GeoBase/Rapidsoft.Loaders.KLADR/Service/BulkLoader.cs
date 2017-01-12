using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace RapidSoft.Loaders.KLADR.Service
{
    public class BulkLoader : IBulkLoader
    {
        public BulkLoader(string dbfFolderName, string dbfTableName, string sqlConnectionString, string sqlTableName)
        {
            DbfFolderName = dbfFolderName;
            DbfTableName = dbfTableName;
            SqlConnectionString = sqlConnectionString;
            SqlTableName = sqlTableName;

            DbfConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=DBASE IV;Persist Security Info=False;", DbfFolderName);
        }

        public string DbfFolderName { get; private set; }

        public string DbfTableName { get; private set; }

        public string SqlTableName { get; private set; }

        public string SqlSchemaName { get; private set; }

        public string SqlConnectionString { get; private set; }

        public string DbfConnectionString { get; private set; }

        public void Processing()
        {
            var transferTable = new DataTable();

            using (var dbfConnection = new OleDbConnection(DbfConnectionString))
            {
                dbfConnection.Open();

                var dbfCommand = new OleDbCommand(string.Format("SELECT * FROM {0}", DbfTableName), dbfConnection);
                transferTable.Load(dbfCommand.ExecuteReader());
            }

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null))
                {
                    bulkCopy.DestinationTableName = SqlTableName;
                    bulkCopy.WriteToServer(transferTable);
                }
            }

            transferTable.Clear();
        }
    }
}
