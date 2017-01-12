using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace RapidSoft.Etl.Runtime.DataSources.DB
{
    internal sealed class DBAccessor : IDisposable
    {
        #region Constructors

        public DBAccessor(string connectionString, string providerName)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            if (connectionString.Trim() == "")
            {
                throw new ArgumentException(string.Format("Parameter \"{0}\" cannot be empty", "connectionString"));
            }

            var factory = DbProviderFactories.GetFactory(providerName);
            _connection = factory.CreateConnection();
            _connection.ConnectionString = connectionString;
            _connection.Open();
        }

        #endregion

        #region Fields

        private readonly DbConnection _connection;

        #endregion

        #region Methods

        public long Insert(string tableName, IDataReader reader, int? timeoutMilliseconds, int batchSize)
        {
            if (tableName == null)
            {
                throw new ArgumentNullException("tableName");
            }

            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            var writer = new DBTableWriter(_connection, tableName);
            var counter = writer.Write(reader, timeoutMilliseconds, batchSize);
            return counter;
        }

        public int ExecuteNonQuery(string queryText, IDictionary<string, object> parameters, int? timeoutMilliseconds)
        {
            if (queryText == null)
            {
                throw new ArgumentNullException("queryText");
            }

            var cmd = CreateCommand(queryText, CommandType.Text, parameters, timeoutMilliseconds);
            var affectedRowCount = cmd.ExecuteNonQuery();
            return affectedRowCount;
        }

        public IDataReader ExecuteQuery(string queryText, IDictionary<string, object> parameters, int? timeoutMilliseconds)
        {
            if (queryText == null)
            {
                throw new ArgumentNullException("queryText");
            }

            var cmd = CreateCommand(queryText, CommandType.Text, parameters, timeoutMilliseconds);
            var reader = cmd.ExecuteReader();
            return reader;
        }

        public IDataReader ExecuteTableQuery(string tableName, int? timeoutMilliseconds)
        {
            if (tableName == null)
            {
                throw new ArgumentNullException("tableName");
            }

            var sql = string.Concat("SELECT * FROM ", tableName);
            return ExecuteQuery(sql, null, timeoutMilliseconds);
        }

        public int ExecuteProcedure(string procedureName, IDictionary<string, object> parameters, int? timeoutMilliseconds)
        {
            if (procedureName == null)
            {
                throw new ArgumentNullException("procedureName");
            }

            var cmd = CreateCommand(procedureName, CommandType.StoredProcedure, parameters, timeoutMilliseconds);
            var affectedRowCount = cmd.ExecuteNonQuery();
            return affectedRowCount;
        }

        public IDataReader ExecuteProcedureReader(string procedureName, IDictionary<string, object> parameters, int? timeoutMilliseconds)
        {
            if (procedureName == null)
            {
                throw new ArgumentNullException("procedureName");
            }

            var cmd = CreateCommand(procedureName, CommandType.StoredProcedure, parameters, timeoutMilliseconds);
            var reader = cmd.ExecuteReader();
            return reader;
        }

        public DbCommand CreateCommand(string commandText, CommandType commandType, IDictionary<string, object> parameters, int? timeoutMilliseconds)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            if (timeoutMilliseconds != null)
            {
                cmd.CommandTimeout = timeoutMilliseconds.Value;
            }

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    var dbparam = cmd.CreateParameter();
                    dbparam.ParameterName = param.Key;

                    if (param.Value == null)
                    {
                        dbparam.Value = DBNull.Value;
                    }
                    else
                    {
                        dbparam.Value = param.Value;
                    }

                    cmd.Parameters.Add(dbparam);
                }
            }

            return cmd;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _connection.Dispose();
        }

        #endregion
    }
}
