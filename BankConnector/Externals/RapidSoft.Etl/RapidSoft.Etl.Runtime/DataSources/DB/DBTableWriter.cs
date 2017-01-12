using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace RapidSoft.Etl.Runtime.DataSources.DB
{
    internal sealed class DBTableWriter
    {
        #region Constructors

        public DBTableWriter(string connectionString, string providerName, string tableName)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            if (connectionString.Trim() == "")
            {
                throw new ArgumentException(string.Format("Parameter \"{0}\" cannot be empty", "connectionString"));
            }

            if (tableName == null)
            {
                throw new ArgumentNullException("tableName");
            }

            if (tableName.Trim() == "")
            {
                throw new ArgumentException(string.Format("Parameter \"{0}\" cannot be empty", "tableName"));
            }

            _tableName = tableName;

            var factory = DbProviderFactories.GetFactory(providerName);
            _connection = factory.CreateConnection();
            _connection.ConnectionString = connectionString;
        }

        public DBTableWriter(DbConnection connection, string tableName)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (tableName == null)
            {
                throw new ArgumentNullException("tableName");
            }

            if (tableName.Trim() == "")
            {
                throw new ArgumentException(string.Format("Parameter \"{0}\" cannot be empty", "tableName"));
            }

            _tableName = tableName;
            _connection = connection;
        }

        #endregion

        #region Fields

        private readonly DbConnection _connection;
        private readonly string _tableName;

        #endregion

        #region Events

        public event EventHandler<DBTableWriterErrorEventArgs> ErrorOccured;

        #endregion

        #region Methods

        public long Write(IDataReader reader, int? timeoutMilliseconds, int batchSize)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            var columnNames = GetDataReaderColumnNames(reader);
            var statement = GenerateStatement(_tableName, columnNames);
            var timeout = timeoutMilliseconds.HasValue ? timeoutMilliseconds.Value : 0;

            var recordIndex = 0L;
            var recordCount = 0L;

            using (var conn = _connection)
            {
                conn.Open();

                var read = false;
                DbTransaction tran = null;

                try
                {
                    read = reader.Read();
                }
                catch (Exception exc)
                {
                    var e = new DBTableWriterErrorEventArgs(DBTableWriterErrorFlags.ReadError, recordIndex, exc);
                    OnError(e);

                    if (!e.TrySkipError)
                    {
                        return recordCount;
                    }
                }

                while (read)
                {
                    var batchRecordIndex = 0;

                    if (batchSize > 1)
                    {
                        tran = conn.BeginTransaction();
                    }

                    var command = _connection.CreateCommand();
                    command.Transaction = tran;
                    command.CommandType = CommandType.Text;
                    command.CommandText = statement;
                    command.CommandTimeout = timeout;

                    while (read)
                    {
                        try
                        {
                            recordIndex++;

                            AddParameters(command, reader);
                            command.ExecuteNonQuery();

                            recordCount++;
                            batchRecordIndex++;
                        }
                        catch (Exception exc)
                        {
                            var e = new DBTableWriterErrorEventArgs(DBTableWriterErrorFlags.WriteError, recordIndex, exc);
                            OnError(e);

                            if (!e.TrySkipError)
                            {
                                if (tran != null)
                                {
                                    tran.Rollback();
                                    tran.Dispose();
                                }

                                return recordCount;
                            }
                        }

                        try
                        {
                            read = reader.Read();
                        }
                        catch (Exception exc)
                        {
                            var e = new DBTableWriterErrorEventArgs(DBTableWriterErrorFlags.ReadError, recordIndex, exc);
                            OnError(e);

                            if (!e.TrySkipError)
                            {
                                if (tran != null)
                                {
                                    tran.Rollback();
                                    tran.Dispose();
                                }

                                return recordCount;
                            }
                        }

                        if (batchRecordIndex >= batchSize)
                        {
                            batchRecordIndex = 0;
                            break;
                        }
                    }

                    if (batchSize > 1)
                    {
                        tran.Commit();
                    }
                }
            }

            return recordCount;
        }

        private void OnError(DBTableWriterErrorEventArgs e)
        {
            if (this.ErrorOccured != null)
            {
                this.ErrorOccured(this, e);
            }
        }

        private static string GenerateStatement(string tableName, string[] columnNames)
        {
            var sb = new StringBuilder();

            sb.Append("insert into ");
            sb.Append(tableName);
            sb.Append("(");

            for (var i = 0; i < columnNames.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }

                var columnToken = DecorateColumnName(columnNames[i]);
                sb.Append(columnToken);
            }

            sb.Append(") values (");

            for (var i = 0; i < columnNames.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(DecorateParameterName(i.ToString()));
            }

            sb.Append(")");

            return sb.ToString();
        }

        private static void AddParameters(DbCommand command, IDataReader reader)
        {
            command.Parameters.Clear();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var p = command.CreateParameter();
                p.ParameterName = DecorateParameterName(i.ToString());
                p.Value = ConvertToDbValue(reader.GetValue(i));

                command.Parameters.Add(p);
            }
        }

        private static string DecorateColumnName(string name)
        {
            return string.Concat("[", name, "]");
        }

        private static string DecorateParameterName(string name)
        {
            return string.Concat("@", name);
        }

        private static object ConvertToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        private static string[] GetDataReaderColumnNames(IDataReader reader)
        {
            var names = new string[reader.FieldCount];

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                names[i] = name;
            }

            return names;
        }

        #endregion
    }
}
