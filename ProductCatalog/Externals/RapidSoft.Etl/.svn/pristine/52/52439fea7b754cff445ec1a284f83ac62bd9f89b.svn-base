using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Data;
using System.Data.Common;
using System.Xml;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.DataSources.Xml;
using RapidSoft.Etl.Runtime.DataSources.DB;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    public sealed class EtlExecuteQueryStep : EtlStep
    {
        #region Constructors

        public EtlExecuteQueryStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlQuerySourceInfo Source
        {
            get;
            set;
        }

        [Category("3. Counters")]
        public EtlExecuteQueryCounterSet Counters
        {
            get;
            set;
        }

        [Category("4. Output")]
        public EtlExecuteQueryOutputVariableSet OutputVariables
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override EtlStepResult Invoke(EtlContext context, IEtlLogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (this.Source == null)
            {
                throw new InvalidOperationException("Source cannot be null");
            }

            if (string.IsNullOrEmpty(this.Source.ConnectionString))
            {
                throw new InvalidOperationException("Source.ConnectionString cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Source.ProviderName))
            {
                throw new InvalidOperationException("Source.ProviderName cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Source.Text))
            {
                throw new InvalidOperationException("Source.Text cannot be empty");
            }

            var result = new EtlStepResult(EtlStatus.Succeeded, null);

            var rowCounter = (this.Counters != null && this.Counters.RowCount != null && !string.IsNullOrEmpty(this.Counters.RowCount.CounterName))
                ? new EtlCounter 
                { 
                    EtlPackageId = context.EtlPackageId, 
                    EtlSessionId = context.EtlSessionId, 
                    EntityName = this.Counters.RowCount.EntityName,
                    CounterName = this.Counters.RowCount.CounterName,
                } 
                : null;

            var hasOutputVariables = this.OutputVariables != null && this.OutputVariables.FirstRow.Count > 0;
            var hasRowCounter = rowCounter != null;

            using (var dbAccessor = new DBAccessor(this.Source.ConnectionString, this.Source.ProviderName))
            {
                if (hasOutputVariables || hasRowCounter)
                {
                    using (var dbReader = dbAccessor.ExecuteQuery(this.Source.Text, EtlQueryParameter.ToDictionary(this.Source.Parameters), this.TimeoutMilliseconds))
                    {
                        if (dbReader.Read())
                        {
                            if (hasRowCounter)
                            {
                                rowCounter.CounterValue++;
                            }

                            if (hasOutputVariables)
                            {
                                foreach (var firstRowResult in this.OutputVariables.FirstRow)
                                {
                                    var firstRowResultValue = EtlValueTranslation.Evaluate(firstRowResult.SourceFieldName, firstRowResult.SourceFieldTranslation, dbReader, firstRowResult.DefaultValue);
                                    result.VariableAssignments.Add(new EtlVariableAssignment(firstRowResult.VariableName, EtlValueConverter.ToString(firstRowResultValue)));
                                }
                            }
                        }

                        if (hasRowCounter)
                        {
                            while (dbReader.Read())
                            {
                                rowCounter.CounterValue++;
                            }

                            rowCounter.DateTime = DateTime.Now;
                            rowCounter.UtcDateTime = rowCounter.DateTime.ToUniversalTime();
                            logger.LogEtlCounter(rowCounter);
                        }
                    }
                }
                else
                {
                    dbAccessor.ExecuteNonQuery(this.Source.Text, EtlQueryParameter.ToDictionary(this.Source.Parameters), this.TimeoutMilliseconds);
                }
            }

            return result;
        }

        private static DbConnection CreateConnection(string connectionString, string providerName)
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;

            return connection;
        }

        private static IDataReader CreateTableReader(DbConnection connection, string queryText, int timeout)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = queryText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = timeout;

            var reader = cmd.ExecuteReader();
            return reader;
        }

        #endregion
    }
}
